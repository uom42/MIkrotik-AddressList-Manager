#nullable enable

using System.CodeDom;
using System.Net.NetworkInformation;

using MikrotikDotNet;
using MikrotikDotNet.Converters;
using MikrotikDotNet.Model;

#if !WINDOWS

using uom.maui;

#endif

using uom.Network;

using static MALM.Localization.LStrings;


//using MikrotikDotNet;
namespace MikrotikDotNet.Model
{



	internal static class Helpers
	{

		public const int DEVICE_CONNECT_TIMEOUT_DEFAULT = 3_000;
		public const UInt16 DEFAULT_API_PORT = MikrotikDotNet.MKConnection.DEFAULT_API_PORT;


		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static MKConnection CreateConnection(string address, string user, string pwd, UInt16 port = DEFAULT_API_PORT)
		{
			if (address.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(address));
			if (user.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(user));
			if (pwd.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(pwd));

			return new(address, user, pwd, port);
		}


		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task<MKConnection> ConnectDeviceAsync(string address, string user, string pwd, UInt16 port = DEFAULT_API_PORT, int timeout = DEVICE_CONNECT_TIMEOUT_DEFAULT)
		{
			MKConnection con = CreateConnection(address, user, pwd, port);
			return await ConnectDeviceAsync(con, timeout);
		}


		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static async Task<MKConnection> ConnectDeviceAsync(MKConnection con, int timeout = DEVICE_CONNECT_TIMEOUT_DEFAULT)
		{
			CancellationTokenSource ct = new();
			Task tConnect = Task.Factory.StartNew(() => con.Open(), ct.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

			if (await Task.WhenAny(tConnect, Task.Delay(timeout, ct.Token)) == tConnect)
			{
				// Task completed within timeout.
				// Consider that the task may have faulted or been canceled.
				// We re-await the task so that any exceptions/cancellation is rethrown.
				await tConnect;
				if (!con.IsOpen) throw new Exception(E_MIKROTIK_CONNECTION_FAILED);
				return con;
			}
			else
			{
				// timeout/cancellation logic
				ct.Cancel();
				throw new TimeoutException(E_MIKROTIK_CONNECTION_FAILED);
			}
		}


	}

	internal static class Extensions
	{

		#region Parse...


		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uom.Network.IP4AddressWithMask? eParseIPMasked(this string? ipm)
		{
			try
			{
				if (ipm.IsNotNullOrWhiteSpace())
					return uom.Network.IP4AddressWithMask.ParseIPs(ipm!).FirstOrDefault();
			}
			catch { }
			return null;
		}


		#endregion






	}


	public interface IEnableItem
	{
		public Task EnableAsync(bool useOpenedConnection, bool enable);
	}

	namespace System
	{


		/// <summary>&#47;system&#47;health + &#47;system&#47;resource</summary>
		public class DeviceStatus
		{

			#region Sample Device Output


			/*
			 * 
				/system/health> print
				Columns: NAME, VALUE, TYPE
				#  NAME         VALUE  TYPE
				0  voltage      24.3   V   
				1  temperature  39     C   

				/system/resource> print                   
				uptime: 3w5d21h42m15s
				version: 6.48 (stable)
				build-time: Dec/22/2020 11:20:49
				factory-software: 6.39
				free-memory: 98.4MiB
				total-memory: 128.0MiB
				cpu: MIPS 74Kc V4.12
				cpu-count: 1
				cpu-frequency: 600MHz
				cpu-load: 25%
				free-hdd-space: 109.0MiB
				total-hdd-space: 128.0MiB
				write-sect-since-reboot: 55188
				write-sect-total: 75670
				bad-blocks: 0%
				architecture-name: mipsbe
				board-name: CRS109-8G-1S-2HnD
				platform: MikroTik
			*/

			#endregion


			#region /system/health


			private const string COMMAND_PRINT_HEALTH = "/system/health print";

			public float Voltage { get; private set; } = 0f;
			public int Temperature { get; private set; } = 0;

			#endregion


			#region /system/resource

			private const string COMMAND_PRINT_RESOURCE = "/system/resource print";


			public TimeSpan? UpTime { get; private set; }
			public string UpTimeString => UpTime.HasValue
				? UpTime.Value.eFormatTimespan()
				: "-";


			public string Cpu { get; private set; } = string.Empty;
			public uint CpuCount { get; private set; } = 0;
			public uint CpuFrequency { get; private set; } = 0;
			public uint CpuLoad { get; private set; } = 0;


			public UInt64 FreeMemory { get; private set; } = 0;
			public UInt64 TotalMemory { get; private set; } = 0;


			public UInt64 FreeHddSpace { get; private set; } = 0;
			public UInt64 TotalHddSpace { get; private set; } = 0;
			public UInt64 WriteSectSinceReboot { get; private set; } = 0;
			public UInt64 WriteSectTotal { get; private set; } = 0;
			public UInt64? BadBlocks { get; private set; }


			public string ArchitectureName { get; private set; } = string.Empty;
			public string BoardName { get; private set; } = string.Empty;
			public string Platform { get; private set; } = string.Empty;


			public string Version { get; private set; } = string.Empty;
			public string BuildTime { get; private set; } = string.Empty;
			public string FactorySoftware { get; private set; } = string.Empty;


			public DateTime? BuildTime_Parsed
			{
				get
				{
					if (DateTime.TryParse(BuildTime, MikrotikDotNet.MKHelpers.CultureEnUs.Value, out var dt)) return dt;
					return null;
				}
			}

			public string BuildTime_Friendly
			{
				get
				{
					try
					{
						var dtfi = CultureInfo.CurrentCulture.DateTimeFormat;

						if (BuildTime_Parsed == null || !BuildTime_Parsed.HasValue) return string.Empty;
						return BuildTime_Parsed.Value.ToString(dtfi.MonthDayPattern + " yyyy");
					}
					catch { }

					return string.Empty;
				}
			}



			#endregion


#if !WINDOWS
			public float FreeMemory_Percent => (float)((double)FreeMemory / (double)TotalMemory);
			public string FreeMemory_String => FreeMemory.eFormatByteSize();
			public string TotalMemory_String => TotalMemory.eFormatByteSize();

			public float FreeHddSpace_Percent => (float)((double)FreeHddSpace / (double)TotalHddSpace);
			public string FreeHddSpace_String => FreeHddSpace.eFormatByteSize();
			public string TotalHddSpace_String => TotalHddSpace.eFormatByteSize();



			#region Error Stste


			public InvertableBool ErrorStste_BadBlocks => (BadBlocks != null && BadBlocks.HasValue && BadBlocks.Value > 0ul);

			public InvertableBool ErrorStste_HighTemp => (Temperature > 65);

			public InvertableBool ErrorStste_HighCPUUsage => (CpuLoad > 90u);

			public InvertableBool ErrorStste_LowVoltage => (Voltage < 12.0f);

			public InvertableBool ErrorStste_LowRAM => (FreeMemory_Percent < .1f);

			public InvertableBool ErrorStste_LowHddSpace => (FreeHddSpace_Percent < .1f);

			public InvertableBool ErrorStste_FWtooOld
				=> (BuildTime_Parsed == null || !BuildTime_Parsed.HasValue)
				? false
				: BuildTime_Parsed.Value < DateTime.Now.AddMonths(-3);

			/*
			public InvertableBool ErrorStste_AnyErrors				=> ErrorStste_BadBlocks || ErrorStste_HighTemp || ErrorStste_HighCPUUsage || ErrorStste_LowVoltage || ErrorStste_LowRAM || ErrorStste_LowHddSpace;
			 */

#if DEBUG
			internal void Test_SetErrorSate()
			{
				var i = new Random().Next(0, 3);
				BadBlocks = (i == 0) ? null : (ulong)i;

				Temperature = new Random().Next(60, 70);

				CpuLoad = (uint)new Random().Next(0, 100);

				Voltage = (float)new Random().Next(15, 25);

				//FreeMemory = new Random().Next(0, TotalMemory);
			}

#endif




			#endregion

#endif


			private DeviceStatus() { }


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static async Task<DeviceStatus> GetDeviceStatusAsync(MKConnection mc)
			{
				bool useOpenedConnection = mc.IsOpen;
				if (!useOpenedConnection) mc = await Helpers.ConnectDeviceAsync(mc);
				try
				{



					var dicHealth = await mc
						.CreateCommand(COMMAND_PRINT_HEALTH)
						.ExecuteReaderStringsToDictionaryAsync(useOpenedConnection);

					var dicResource = await mc
						.CreateCommand(COMMAND_PRINT_RESOURCE)
						.ExecuteReaderTextToDictionaryAsync(useOpenedConnection);

					DeviceStatus ds = dicHealth.ApplyPropertiesTo(new DeviceStatus());
					_ = dicResource.ApplyPropertiesTo(ds);

					return ds;


				}
				finally
				{
					if (!useOpenedConnection) mc.Close();
				}

			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static async Task<DeviceStatus> GetDeviceStatusAsync(string address, string user, string pwd, UInt16 port = Helpers.DEFAULT_API_PORT, int timeout = 3_000)
			{
				using MKConnection con = new(address, user, pwd, port);
				_ = await Helpers.ConnectDeviceAsync(con, timeout);

				return await GetDeviceStatusAsync(con);
			}



		}

	}



	namespace IP
	{

		namespace Firewall.AddressList
		{

			public class AddressListItem : MKDataRowBase, IEnableItem
			{
				private const string COMMAND_LIST = @"/ip firewall address-list ";
				private const string COMMAND_LIST_PRINT = COMMAND_LIST + "print";
				private const string COMMAND_LIST_SET = COMMAND_LIST + "set";
				private const string COMMAND_LIST_ADD = COMMAND_LIST + "add";//add address=fra16s51-in-f14.1e100.net comment="PROPAGANDA_BLOCK russian.rt.com" disabled=yes list=PROPAGANDA_BLOCK


				public const string FIELD_ADDRESS = "address";
				public const string FIELD_LIST = "list";

				/// <summary>Mikrotik List Name</summary>
				public string List { get; set; }


				/// <summary>ip or url</summary>
				public string Address { get; set; }

				public DateTime CreationTime { get; set; }


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
				public AddressListItem() : base() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


				#region GetItems


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static async Task<IEnumerable<AddressListItem>> GetItemsAsync(
					MKConnection mc,
					bool useOpenedConnection)

					=> Sort(
						 await GetItemsAsync<AddressListItem>(
							 mc,
							 useOpenedConnection,
							 mc!.CreateCommand(COMMAND_LIST_PRINT)));


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static async Task<IEnumerable<AddressListItem>> GetItemsAsync(
					MKConnection mc,
					bool useOpenedConnection,
					MKCommandParameterCollection condition,
					MKQueryLogicOperators logic)

					=> Sort(
						 await GetItemsAsync<AddressListItem>(
							 mc,
							 useOpenedConnection,
							 mc.CreateCommand(COMMAND_LIST_PRINT), condition, logic));


				/// <summary>Do not use Empty Comment field for search = don't work!</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static async Task<IEnumerable<AddressListItem>> GetItemsAsync(
					MKConnection mc,
					bool useOpenedConnection,
					string? id = null,
					string? list = null,
					string? address = null,
					bool? disabled = null,
					string? comment = null)
				{
					var condition = new MKCommandParameterCollection();
					if (id != null) condition.Add(new(FIELD_ID, id));
					if (list != null) condition.Add(new(FIELD_LIST, list));
					if (address != null) condition.Add(new(FIELD_ADDRESS, address));
					if (disabled.HasValue) condition.Add(new(FIELD_DISABLED, disabled!.Value.ToDeviceBoolString()));
					if (comment != null) condition.Add(new(FIELD_COMMENT, comment));

					return await GetItemsAsync(
						mc,
						useOpenedConnection,
						condition,
						MKQueryLogicOperators.And);
				}


				#endregion


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				private static IEnumerable<AddressListItem> Sort(IEnumerable<AddressListItem> src) => src
					.OrderBy(r => r.List)
					.ThenBy(r => r.Address, uom.Network.IP4AddressWithMaskComparer.StaticInstance.Value);


				/// <summary>Refresh item info from Mikrotik by item ID</summary>
				public async Task<AddressListItem> ReQueryAsync(bool useOpenedConnection)
				{
					AddressListItem? newItem = (await GetItemsAsync(
						Connection,
						useOpenedConnection,
						id: MKID))
						.FirstOrDefault() ?? throw new Exception($"Item with ID = '{MKID}' was not found!");

					newItem.Connection = Connection;
					return newItem;
				}





				/// <summary>Refresh item info from Mikrotik by item ID</summary>
				static public async Task<AddressListItem?> Add(
					MKConnection mc,
					bool useOpenedConnection,
					string list, string address, bool disabled, string comment = "")
				{
					//Check tat this node is not exist in list				 
					AddressListItem? existingItem = (await GetItemsAsync(
						mc,
						useOpenedConnection,
						address: address,
						list: list,
						disabled: disabled))
						.FirstOrDefault();

					if (existingItem != null) throw new Exception($"Item '{address}' already exist in list '{list}'!");

					//Trying to add
					var cmd = mc.CreateCommand(COMMAND_LIST_ADD);
					cmd.Parameters
						.Add(FIELD_LIST, list)
						.Add(FIELD_ADDRESS, address)
						.Add(FIELD_DISABLED, disabled.ToDeviceBoolString());

					if (!string.IsNullOrWhiteSpace(comment)) cmd.Parameters.Add(FIELD_COMMENT, comment);


					try
					{
						//cmd!.Connection!.Open();
						await cmd.ExecuteNonQueryAsync(useOpenedConnection);
						existingItem = (await GetItemsAsync(
							mc,
							useOpenedConnection,
							address: address,
							list: list,
							disabled: disabled)).FirstOrDefault();

						return existingItem;
					}
					finally
					{
						//cmd!.Connection!.Close();
					}
				}


				public async Task EnableAsync(bool useOpenedConnection, bool enable)
					=> await SetPropertyAsync(
						useOpenedConnection,
						  COMMAND_LIST_SET,
						   FIELD_DISABLED,
						   (!enable).ToDeviceBoolString());

			}

		}

		namespace ARP
		{

			public class ARPItem : MKDataRowBase, IEnableItem
			{
				private const string COMMAND_LIST = @"/ip arp ";
				private const string COMMAND_LIST_PRINT = COMMAND_LIST + "print";
				private const string COMMAND_PARAM_DETAIL = "detail";


				public enum STATUSES : int
				{
					Unknown = 0,

					/// <summary>neighbor entry validation is currently delayed</summary>
					Delay,

					/// <summary>ARP resolution has failed, the system was not able to obtain the MAC address for the given IP address</summary>
					Failed,

					/// <summary>system does not have the MAC address information for the IP address, it has not yet been resolved</summary>
					Incomplete,

					/// <summary>ARP entry is considered permanent and will not be removed from the table, even if it is not actively being used.This is set for manually configured ARP entries</summary>
					Permanent,

					/// <summary>neighbor is being probed</summary>
					Probe,

					/// <summary>ARP resolution is successful, and the MAC address associated with the IP address is know, the entry is valid until the reachability timeout expires</summary>
					Reachable,

					/// <summary>entry is still valid, but it is aged. This means that the system has not recently communicated with the device associated with the IP address.</summary>
					Stale
				}



				public IPAddress? Address { get; set; }

				public PhysicalAddress? MacAddress { get; set; }

				public string Interface { get; set; }

				public STATUSES Status { get; set; }


				/// <summary>
				/// Complete flag is included in ARP entries when the ARP status is permanent, reachable, stale, probe, or delay
				/// </summary>
				public bool Complete { get; set; }


				/// <summary>
				/// published(yes | no; Default: no)<br/>
				/// Static proxy-arp entry for individual IP address.<br/>
				/// When an ARP query is received for the specific IP address, the device will respond with its own MAC address.<br/>
				/// No need to set proxy-arp on the interface itself for all the MAC addresses to be proxied.<br/>
				/// The interface will respond to an ARP request only when the device has an active route towards the destination<br/>
				/// </summary>
				public bool Published { get; set; }


				#region Read only properties:

				public bool Invalid { get; set; }


				/*
				//public bool dhcp { get; set; }
				Read only properties:
	Property Description
	dhcp(yes | no) Whether ARP entry is added by DHCP server
	//dynamic(yes | no)  Whether entry is dynamically created
	invalid(yes | no)
				 */

				#endregion


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
				public ARPItem() : base() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


				#region GetItemsAsync


				public static async Task<IEnumerable<ARPItem>> GetItemsAsync(
					MKConnection mc,
					bool useOpenedConnection
					)

				{
					var cmd = mc!.CreateCommand(COMMAND_LIST_PRINT);
					cmd.Parameters.Add(COMMAND_PARAM_DETAIL, string.Empty);

					return Sort(await GetItemsAsync<ARPItem>(
						mc,
						useOpenedConnection,
						cmd));
				}


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				private static IEnumerable<ARPItem> Sort(IEnumerable<ARPItem> src)
					=> src
					.OrderBy(r => r.Interface)
					.ThenBy(r => r.Address!, uom.Network.IP4AddressComparer.StaticInstance.Value);


				#endregion


				public Task EnableAsync(bool useOpenedConnection, bool enable) => throw new NotImplementedException();

			}

		}


		namespace DHCPServer
		{

			/// <summary>
			/// https://help.mikrotik.com/docs/display/ROS/DHCP#DHCP-Leases
			/// </summary>
			public class LeaseItem : MKDataRowBase, IEnableItem
			{

				private const string COMMAND_LIST = @"/ip dhcp-server lease ";
				private const string COMMAND_LIST_PRINT = COMMAND_LIST + "print";

				public enum STATUSES : int
				{
					Unknown = 0,

					/// <summary>
					/// Shown for static bindings if it is not used.For dynamic bindings this status is shown if it was used previously, the server will wait 10 minutes to allow an old client to get this binding, otherwise binding will be cleared and prefix will be offered to other clients.
					/// </summary>
					Waiting,

					/// <summary>
					/// if solicit message was received, and the server responded with advertise a message, but the request was not received. During this state client have 2 minutes to get this binding, otherwise, it is freed or changed status to waiting for static bindings.
					/// </summary>
					Offered,


					/// <summary>currently bound</summary>
					Bound,

					/*
					error,
					rebinding,
					requesting,
					searching,
					stopped
					 */
				}


				public IPAddress? Address { get; set; }
				public PhysicalAddress? MacAddress { get; set; }
				public string ClientId { get; set; }
				public string AddressLists { get; set; }
				public string Server { get; set; }
				public string DhcpOption { get; set; }


				public string HostName { get; set; }
				public bool Radius { get; set; }
				public bool Blocked { get; set; }


				#region Read only properties

				/// <summary>The time period after which binding expires.</summary>
				public TimeSpan? ExpiresAfter { get; set; }
				/// <summary>Time period since the client was last seen.</summary>
				public TimeSpan? LastSeen { get; set; }
				public STATUSES Status { get; set; }


				public IPAddress? ActiveAddress { get; set; }
				public PhysicalAddress? ActiveMacAddress { get; set; }
				public string ActiveClientId { get; set; }
				public string ActiveServer { get; set; }


				#endregion

				#region Parsed Properties

				//internal IPAddress? Address_Parsed => Address.eParseIP();
				//internal PhysicalAddress? MacAddress_Parsed => MacAddress.eParseMAC();
				//internal IPAddress? ActiveAddress_Parsed => ActiveAddress.eParseIP();
				//internal PhysicalAddress? ActiveMacAddress_Parsed => ActiveMacAddress.eParseMAC();
				//internal TimeSpan? ExpiresAfter_Parsed => ExpiresAfter.eParseTimeSpan();
				//internal TimeSpan? LastSeen_Parsed => LastSeen.eParseTimeSpan();

				//internal STATUSES Status_Parsed => Status.eToEnumValue(STATUSES.unknown);


				#endregion





#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
				public LeaseItem() : base() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.



				#region GetItems


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static async Task<IEnumerable<LeaseItem>> GetItemsAsync(MKConnection mc, bool useOpenedConnection)
					=> Sort(
						 await GetItemsAsync<LeaseItem>(
							 mc,
							 useOpenedConnection,
							 mc!.CreateCommand(COMMAND_LIST_PRINT)));



				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static async Task<IEnumerable<LeaseItem>> GetItemsAsync(
					MKConnection mc,
					bool useOpenedConnection,
					MKCommandParameterCollection condition,
					MKQueryLogicOperators logic)

					=> Sort(
					 await GetItemsAsync<LeaseItem>(
						 mc,
						 useOpenedConnection,
						 mc.CreateCommand(COMMAND_LIST_PRINT),
						 condition, logic));



				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				private static IEnumerable<LeaseItem> Sort(IEnumerable<LeaseItem> src) => src
					.OrderBy(r => r.Address, uom.Network.IP4AddressComparer.StaticInstance.Value!)
					.ThenBy(r => r.MacAddress, uom.Network.MACComparer.StaticInstance.Value!);


				#endregion


				public Task EnableAsync(bool useOpenedConnection, bool enable) => throw new NotImplementedException();

			}
		}


		namespace AddressList
		{

			public class AddressItem : MKDataRowBase, IEnableItem
			{
				private const string COMMAND_LIST = @"/ip/address ";
				private const string COMMAND_LIST_PRINT = COMMAND_LIST + "print";

				/*
				 * 
	/ip/address> print
	Flags: X - DISABLED
	Columns: ADDRESS, NETWORK, INTERFACE
	#   ADDRESS            NETWORK         INTERFACE        
	;;; defconf
	0   192.168.5.10/24    192.168.5.0     bridge_1         
	;;; WAN Goodline
	1   95.181.81.146/26   95.181.81.128   ether1_wan       
	2   10.0.25.1/24       10.0.25.0       Wireguard_GHV    
	;;; RSV Netherlands
	3 X 10.200.162.225/32  10.200.162.225  RSV_Netherlands  
	4   10.0.26.1/24       10.0.26.0       Wireguard_Davt_49
	;;; RSV Finland 2023 with hidden IP
	5   10.202.131.223/32  10.202.131.223  RSV_Finland_23   
	6 X 10.200.196.199/32  10.200.196.199  RSV_Latvia       
	7 X 10.200.196.200/32  10.200.196.200  RSV_Poland       
	8 X 10.200.196.227/32  10.200.196.227  RSV_Frankfurt    



				public bool Running { get; set; }
				public string Name { get; set; }
				public string Mtu { get; set; }
				public string ActualMtu { get; set; }
				public string L2mtu { get; set; }
				public string Arp { get; set; }
				public string ArpTimeout { get; set; }
				public string MacAddress { get; set; }
				public string ProtocolMode { get; set; }
				public bool FastForward { get; set; }
				public bool IgmpSnooping { get; set; }

				public string MulticastRouter { get; set; }

				public bool MulticastQuerier { get; set; }
				public string StartupQueryCount { get; set; }
				public string LastMemberQueryCount { get; set; }




				#region Parsed Properties




				#endregion
	*/

				public string Address { get; set; }
				public string Network { get; set; }
				public string Interface { get; set; }


				internal uom.Network.IP4AddressWithMask? Address_Parsed => Address.eParseIPMasked();

				internal IPAddress? Network_Parsed => Network.ConvertToIPAddress();


				/*

				/// <summary>
				/// Complete flag is included in ARP entries when the ARP status is permanent, reachable, stale, probe, or delay
				/// </summary>
				//public bool Complete { get; set; }


				/// <summary>
				/// published(yes | no; Default: no)<br/>
				/// Static proxy-arp entry for individual IP address.<br/>
				/// When an ARP query is received for the specific IP address, the device will respond with its own MAC address.<br/>
				/// No need to set proxy-arp on the interface itself for all the MAC addresses to be proxied.<br/>
				/// The interface will respond to an ARP request only when the device has an active route towards the destination<br/>
				/// </summary>
				public bool Published { get; set; }


				#region Read only properties:

				public bool Invalid { get; set; }



				#endregion






				public enum STATUSES : int
				{
					unknown = 0,

					/// <summary>neighbor entry validation is currently delayed</summary>
					delay,

					/// <summary>ARP resolution has failed, the system was not able to obtain the MAC address for the given IP address</summary>
					failed,

					/// <summary>system does not have the MAC address information for the IP address, it has not yet been resolved</summary>
					incomplete,

					/// <summary>ARP entry is considered permanent and will not be removed from the table, even if it is not actively being used.This is set for manually configured ARP entries</summary>
					permanent,

					/// <summary>neighbor is being probed</summary>
					probe,

					/// <summary>ARP resolution is successful, and the MAC address associated with the IP address is know, the entry is valid until the reachability timeout expires</summary>
					reachable,

					/// <summary>entry is still valid, but it is aged.This means that the system has not recently communicated with the device associated with the IP address.</summary>
					stale
				}

				internal STATUSES Status_Parsed => Status.eToEnumValue(STATUSES.unknown);

				 */


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
				public AddressItem() : base() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.





				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static async Task<IEnumerable<AddressItem>> GetItemsAsync(
					MKConnection mc,
					bool useOpenedConnection
					)

					=> Sort(
						await GetItemsAsync<AddressItem>(
							mc,
							useOpenedConnection,
							mc!.CreateCommand(COMMAND_LIST_PRINT)));


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				private static IEnumerable<AddressItem> Sort(IEnumerable<AddressItem> src)
					=> src
					//.OrderBy(br => br.MacAddress_Parsed, uom.Network.MACComparer.StaticInstance.Value!)
					;


				public Task EnableAsync(bool useOpenedConnection, bool enable) => throw new NotImplementedException();

			}

		}

	}

	namespace Interface
	{

		namespace Bridge
		{


			public class BridgeItem : MKDataRowBase, IEnableItem
			{
				private const string COMMAND_LIST = @"/interface/bridge ";
				private const string COMMAND_LIST_PRINT = COMMAND_LIST + "print";
				/*

				/interface/bridge> print
				Flags: X - disabled, R - running 
				0 R ;;; defconf

				name="bridge_1" mtu=auto actual-mtu=1500 l2mtu=1596 arp=enabled arp-timeout=auto mac-address=DC:2C:6E:0E:9F:1B protocol-mode=rstp 
			 fast-forward=yes igmp-snooping=yes multicast-router=temporary-query multicast-querier=yes startup-query-count=2 last-member-query-count=2 
			 last-member-interval=1s membership-interval=4m20s querier-interval=4m15s query-interval=2m5s query-response-interval=10s 
			 startup-query-interval=31s250ms igmp-version=3 mld-version=2 auto-mac=no admin-mac=DC:2C:6E:0E:9F:1B ageing-time=5m priority=0x8000 
			 max-message-age=20s forward-delay=15s transmit-hold-count=6 vlan-filtering=no dhcp-snooping=yes add-dhcp-option82=yes port-cost-mode=short 

				 */

				public bool Running { get; set; }
				public string Name { get; set; }
				public string Mtu { get; set; }
				public string ActualMtu { get; set; }
				public string L2mtu { get; set; }
				public string Arp { get; set; }
				public string ArpTimeout { get; set; }
				public PhysicalAddress? MacAddress { get; set; }
				public string ProtocolMode { get; set; }
				public bool FastForward { get; set; }
				public bool IgmpSnooping { get; set; }

				public string MulticastRouter { get; set; }

				public bool MulticastQuerier { get; set; }
				public string StartupQueryCount { get; set; }
				public string LastMemberQueryCount { get; set; }

				/*
	last-member-interval=1s membership-interval=4m20s querier-interval=4m15s query-interval=2m5s query-response-interval=10s 
	startup-query-interval=31s250ms igmp-version=3 mld-version=2 auto-mac=no admin-mac=DC:2C:6E:0E:9F:1B ageing-time=5m priority=0x8000 
	max-message-age=20s forward-delay=15s transmit-hold-count=6 vlan-filtering=no dhcp-snooping=yes add-dhcp-option82=yes port-cost-mode=short 



	*/



				#region Parsed Properties


				//internal PhysicalAddress? MacAddress_Parsed => MacAddress.ConvertToPhysicalAddress();


				#endregion






				/*



				/// <summary>
				/// Complete flag is included in ARP entries when the ARP status is permanent, reachable, stale, probe, or delay
				/// </summary>
				//public bool Complete { get; set; }


				/// <summary>
				/// published(yes | no; Default: no)<br/>
				/// Static proxy-arp entry for individual IP address.<br/>
				/// When an ARP query is received for the specific IP address, the device will respond with its own MAC address.<br/>
				/// No need to set proxy-arp on the interface itself for all the MAC addresses to be proxied.<br/>
				/// The interface will respond to an ARP request only when the device has an active route towards the destination<br/>
				/// </summary>
				public bool Published { get; set; }


				#region Read only properties:

				public bool Invalid { get; set; }



				#endregion






				public enum STATUSES : int
				{
					unknown = 0,

					/// <summary>neighbor entry validation is currently delayed</summary>
					delay,

					/// <summary>ARP resolution has failed, the system was not able to obtain the MAC address for the given IP address</summary>
					failed,

					/// <summary>system does not have the MAC address information for the IP address, it has not yet been resolved</summary>
					incomplete,

					/// <summary>ARP entry is considered permanent and will not be removed from the table, even if it is not actively being used.This is set for manually configured ARP entries</summary>
					permanent,

					/// <summary>neighbor is being probed</summary>
					probe,

					/// <summary>ARP resolution is successful, and the MAC address associated with the IP address is know, the entry is valid until the reachability timeout expires</summary>
					reachable,

					/// <summary>entry is still valid, but it is aged.This means that the system has not recently communicated with the device associated with the IP address.</summary>
					stale
				}

				internal STATUSES Status_Parsed => Status.eToEnumValue(STATUSES.unknown);

				 */


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
				public BridgeItem() : base() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.





				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static async Task<IEnumerable<BridgeItem>> GetItemsAsync(
					MKConnection mc,
					bool useOpenedConnection
					)

					=> Sort(
						await GetItemsAsync<BridgeItem>(
							mc,
							useOpenedConnection,
							mc!.CreateCommand(COMMAND_LIST_PRINT)));


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				private static IEnumerable<BridgeItem> Sort(IEnumerable<BridgeItem> src)
					=> src
					.OrderBy(br => br.MacAddress!, uom.Network.MACComparer.StaticInstance.Value!)
					;






				public Task EnableAsync(bool useOpenedConnection, bool enable)
				{
					throw new NotImplementedException();
				}

			}



		}



	}

}