#nullable enable

using System.Data.SqlClient;

using Mikrotik.API.Model.IP.Firewall.AddressList;

using MikrotikDotNet;

using uom;

namespace Mikrotik.API
{

	public static class Extensions
	{
		public const string BOOL_YES = "yes", BOOL_NO = "no";

		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string e_ToMikrotikBoolString(this bool b) => b ? BOOL_YES : BOOL_NO;

		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static IEnumerable<AddressListItem> e_SortAddressList(this IEnumerable<AddressListItem> src) => src
			.OrderBy(r => r.List)
			.ThenBy(r => r.Address);


	}


	namespace Model
	{

		public sealed class MikrotikConnectionLostException : Exception
		{
			private const string ERROR_CONNECTION_SEEMS_LOST = "It's seems like mikrotik connection was lost!";

			public MikrotikConnectionLostException() : base(ERROR_CONNECTION_SEEMS_LOST) { }
		}


		public class ItemBase
		{

			public const string FIELD_ID = ".id";
			public const string FIELD_COMMENT = "comment";
			public const string FIELD_DISABLED = "disabled";


			protected MKConnection Connection { get; set; }


			/// <summary>Mikrotik item ID (.id)</summary>
			public string MKID { get; set; }  //MKID always referce to .id field in response.
											  // Use PascalCase naming style for properties. it will convert from/to kebab-case naming.

			public string Comment { get; set; }

			public bool @Dynamic { get; set; }

			public bool Disabled { get; set; }



			public ItemBase() : base() { }



			/// <summary>Set item property by it's name and new value</summary>
			public async Task SetPropertyAsync(string setCommand, string @field, string newValue)
			{
				if (field == FIELD_ID) throw new Exception("ID field can't be changed!");

				var mkRow = this;
				var cmd = Connection.CreateCommand(setCommand);
				cmd.Parameters.Add(FIELD_ID, mkRow.MKID);
				cmd.Parameters.Add(@field, newValue);

				using (Task tskGetMKData = new(() => cmd.ExecuteNonQuery(), TaskCreationOptions.LongRunning))
				{
					tskGetMKData.Start();
					await tskGetMKData;
				};
			}


			public override string ToString()
			{
				string[] allProps = this.GetType()
					.GetProperties()
					.Select(prop =>
					{
						object? val = prop.GetValue(this);
						string sVal = (null == val) ? "NULL" : val!.ToString()!;
						return $"{prop.Name}={sVal}";
					}).ToArray();
				return string.Join(", ", allProps);
			}
		}



		namespace IP
		{


			namespace Firewall.AddressList
			{

				public class AddressListItem : ItemBase
				{
					private const string COMMAND_LIST = @"/ip firewall address-list ";
					private const string COMMAND_LIST_PRINT = COMMAND_LIST + "print";
					private const string COMMAND_LIST_SET = COMMAND_LIST + "set";
					private const string COMMAND_LIST_ADD = COMMAND_LIST + "add";//add address=fra16s51-in-f14.1e100.net comment="PROPAGANDA_BLOCK russian.rt.com" disabled=yes list=PROPAGANDA_BLOCK


					public const string FIELD_ADDRESS = "address";
					public const string FIELD_LIST = "list";


					public string List { get; set; }

					public string Address { get; set; }

					public string CreationTime { get; set; }





					public AddressListItem() : base() { }


					internal DateTime CreationTime_AsDateTime
					{
						get
						{
							CultureInfo provider = CultureInfo.GetCultureInfoByIetfLanguageTag("En-us");// .InvariantCulture;
							return DateTime.Parse(CreationTime, provider);
						}
					}


					#region GetItems


					public static async Task<IEnumerable<AddressListItem>> GetItemsAsync(MKConnection mc)
					{
						var cmd = mc!.CreateCommand(COMMAND_LIST_PRINT);
						try
						{
							using (Task<IEnumerable<AddressListItem>> tskGetMKData = new(() => cmd.ExecuteReader<AddressListItem>(), TaskCreationOptions.LongRunning))
							{


								tskGetMKData.Start();
								var result = await tskGetMKData;

								foreach (var item in result) item.Connection = mc;
								return result.e_SortAddressList();
							};
						}
						catch (System.ArgumentOutOfRangeException) { throw new MikrotikConnectionLostException(); }
					}

					public static async Task<IEnumerable<AddressListItem>> GetItemsAsync(MKConnection mc, MKCommandParameterCollection condition, MKQueryLogicOperators logic)
					{
						var cmd = mc.CreateCommand(COMMAND_LIST_PRINT);
						try
						{
							using (Task<IEnumerable<AddressListItem>> tskGetMKData = new(() => cmd.ExecuteReader<AddressListItem>(condition, logic), TaskCreationOptions.LongRunning))
							{
								tskGetMKData.Start();
								var result = await tskGetMKData;

								foreach (var item in result) item.Connection = mc;
								return result.e_SortAddressList();
							};
						}
						catch (System.ArgumentOutOfRangeException) { throw new MikrotikConnectionLostException(); }
					}

					/// <summary>Do not use Empty Comment field for search = don't working!</summary>
					public static async Task<IEnumerable<AddressListItem>> GetItemsAsync(MKConnection mc,
						string? id = null,
						string? list = null,
						string? address = null,
						bool? disabled = null,
						string? comment = null)
					{
						var condition = new MKCommandParameterCollection();
						if (id != null) condition.Add(new MKCommandParameter(FIELD_ID, id));
						if (list != null) condition.Add(new MKCommandParameter(FIELD_LIST, list));
						if (address != null) condition.Add(new MKCommandParameter(FIELD_ADDRESS, address));
						if (disabled.HasValue) condition.Add(new MKCommandParameter(FIELD_DISABLED, disabled!.Value.e_ToMikrotikBoolString()));
						if (comment != null) condition.Add(new MKCommandParameter(FIELD_COMMENT, comment));
						return Sort(await GetItemsAsync(mc, condition, MKQueryLogicOperators.And));
					}

					private static IEnumerable<AddressListItem> Sort(IEnumerable<AddressListItem> src) => src
						.OrderBy(r => r.List)
						.ThenBy(r => r.Address);

					#endregion



					/// <summary>Refresh item info from Mikrotik by item ID</summary>
					public async Task<AddressListItem> ReQueryAsync()
					{
						AddressListItem? newItem = (await GetItemsAsync(Connection, id: MKID)).FirstOrDefault();
						if (newItem == null) throw new Exception($"Item with ID = '{MKID}' was not found!");

						newItem.Connection = Connection;
						return newItem;
					}


					public async Task EnableAsync(bool enable) => await SetPropertyAsync(COMMAND_LIST_SET, FIELD_DISABLED, (!enable).e_ToMikrotikBoolString());



					/// <summary>Refresh item info from Mikrotik by item ID</summary>
					static public async Task<AddressListItem?> Add(MKConnection mc, string list, string address, bool disabled, string comment = "")
					{
						//Check tat this node is not exist in list				 
						AddressListItem? existingItem = (await GetItemsAsync(mc, address: address, list: list, disabled: disabled)).FirstOrDefault();

						if (existingItem != null) throw new Exception($"Item '{address}' already exist in list '{list}'!");

						//Trying to add
						var cmd = mc.CreateCommand(COMMAND_LIST_ADD);
						cmd.Parameters.Add(FIELD_LIST, list);
						cmd.Parameters.Add(FIELD_ADDRESS, address);
						cmd.Parameters.Add(FIELD_DISABLED, disabled.e_ToMikrotikBoolString());
						if (!string.IsNullOrWhiteSpace(comment)) cmd.Parameters.Add(FIELD_COMMENT, comment);

						using (Task tskGetMKData = new(() => cmd.ExecuteNonQuery(), TaskCreationOptions.LongRunning))
						{
							tskGetMKData.Start();
							await tskGetMKData;
						};

						existingItem = (await GetItemsAsync(mc, address: address, list: list, disabled: disabled)).FirstOrDefault();
						return existingItem;
					}
				}

			}


			namespace DHCPServer
			{
				public class LeaseListItem : ItemBase
				{

					private const string COMMAND_LIST = @"/ip dhcp-server lease ";
					private const string COMMAND_LIST_PRINT = COMMAND_LIST + "print";

					public string Address { get; set; }
					public string MacAddress { get; set; }
					public string ClientId { get; set; }
					public string AddressLists { get; set; }
					public string Server { get; set; }
					public string DhcpOption { get; set; }
					public string Status { get; set; }
					public string LastSeen { get; set; }
					public string HostName { get; set; }
					public bool Radius { get; set; }
					public bool Blocked { get; set; }


					public LeaseListItem() : base() { }

					#region GetItems

					public static async Task<IEnumerable<LeaseListItem>> GetItemsAsync(MKConnection mc)
					{
						var cmd = mc!.CreateCommand(COMMAND_LIST_PRINT);
						try
						{
							/*
							using (Task<IEnumerable<string>> tskGetMKData2 = new(() => cmd.ExecuteReader(), TaskCreationOptions.LongRunning))
							{
								tskGetMKData2.Start();
								var result = await tskGetMKData2;
								var ttt = string.Join("\n", result.ToArray());

								int yyy = 7;
							}
							 */

							using (Task<IEnumerable<LeaseListItem>> tskGetMKData = new(() => cmd.ExecuteReader<LeaseListItem>(), TaskCreationOptions.LongRunning))
							{


								tskGetMKData.Start();
								var result = await tskGetMKData;

								foreach (var item in result) item.Connection = mc;
								return result;
							};
						}
						catch (System.ArgumentOutOfRangeException) { throw new MikrotikConnectionLostException(); }
					}

					public static async Task<IEnumerable<LeaseListItem>> GetItemsAsync(MKConnection mc, MKCommandParameterCollection condition, MKQueryLogicOperators logic)
					{
						/*
						var condition2 = new MKCommandParameterCollection()
						{
							new MKCommandParameter("dynamic",false.e_ToMikrotikBoolString()),
							new MKCommandParameter("list","under_rsv")						
						};
						 */

						var cmd = mc.CreateCommand(COMMAND_LIST_PRINT);
						try
						{
							using (Task<IEnumerable<LeaseListItem>> tskGetMKData = new(() => cmd.ExecuteReader<LeaseListItem>(condition, logic), TaskCreationOptions.LongRunning))
							{
								tskGetMKData.Start();
								var result = await tskGetMKData;

								foreach (var item in result) item.Connection = mc;
								return result;
							};
						}
						catch (System.ArgumentOutOfRangeException) { throw new MikrotikConnectionLostException(); }
					}

					#endregion




				}
			}

		}
	}
}