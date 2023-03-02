using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if !WINDOWS

using uom.maui;

#endif

//using uom.maui;


namespace MALM.OUI
{
	internal class Manager
	{

		private static List<Action<OUIDatabase>> _dbReadyHandlers = [];

		private static EventArgs _lock = new();
		private static OUIDatabase? _base;



		public static OUIDatabase? Database { get { lock (_lock) return _base; } }


		public static void BeginInitialize(int delaySec = 3, Action<OUIDatabase>? baseReadyHandler = null)
		{
			if (baseReadyHandler != null)
			{
				if (AttachDatabaseReadyHandler(baseReadyHandler)) return;
			}

			if (Database != null) return;

			_ = Task.Factory.StartNew(delegate
			{
				//Make initial pause to allow main App load faster.
				Thread.Sleep(delaySec * 1000);

				try
				{

					var db = OUIDatabase.TryCreate();
					if (db == null) return;//Failed To Create Database!
					lock (_lock) _base = db;

					lock (_dbReadyHandlers)
					{
						_dbReadyHandlers.ForEach(h =>
							{
								try { h.Invoke(Database!); }
								catch { }
							}
						);
					}
				}
				catch (Exception ex)
				{

#if WINDOWS
					ex.eLogError(false);
#else
					ex.eLogErrorNoUI();
#endif
				}
			}, TaskCreationOptions.LongRunning);
		}


		public static bool AttachDatabaseReadyHandler(Action<OUIDatabase> a)
		{
			if (Database == null)
			{
				lock (_dbReadyHandlers) _dbReadyHandlers.Add(a);
				return false;
			}
			else
			{
				a.Invoke(Database!);
				return true;
			}
		}



	}


}
