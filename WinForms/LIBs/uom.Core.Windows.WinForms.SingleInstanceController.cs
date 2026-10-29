using System.ComponentModel.DataAnnotations;
using System.IO.Pipes;
using System.Security.AccessControl;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace uom;


internal static partial class AppTools
{


	/// <summary>
	/// Creates new (Default) instance of SingleInstanceController <br/>
	/// Used to allow only one running instance of app per user session <br/>
	/// When Second instance started, it checks that First instance is running and send to it Second Istance startup arguments 
	/// </summary>
	/// <param name="id">Some ID suffix for global mutex</param>
	/// <param name="setDefaultSingleton">Sets static 'Default' singleton property</param>
	internal static SingleInstanceController<SecondAppLaunchContext> CreateDefaultSingleInstanceController ( string id = "" , bool setDefaultSingleton = true )
		=> new(id: id , setDefaultSingleton: setDefaultSingleton);


	[Serializable]
	public class SecondAppLaunchContext ()
	{
		private const int MAX_STRING_LEN = 260;


		public Guid RequestId { get; set; } = Guid.NewGuid();

		public string CommandLine { get; set; } = string.Empty;
		public string FileName { get; set; } = string.Empty;
		public string ProcessName { get; set; } = string.Empty;

		public string[] Args { get; set; } = [];
		public System.Diagnostics.ProcessPriorityClass PriorityClass { get; set; }
		public DateTime StartTime { get; set; }

		public Int64 Id { get; set; }
		public Int64 SafeProcessHandle { get; set; }
		public Int64 SessionId { get; set; }



		private SecondAppLaunchContext (
			string commandLine ,
			string[] args ,
			string fileName ,
			string processName ,
			System.Diagnostics.ProcessPriorityClass priorityclass ,
			DateTime startTime ,
			Int64 id ,
			Int64 safeprocessHandle ,
			Int64 sessionId
			) : this()
		{

			CommandLine = commandLine;
			Args = args;
			FileName = fileName;
			ProcessName = processName;
			PriorityClass = priorityclass;
			StartTime = startTime;
			Id = id;
			SafeProcessHandle = safeprocessHandle;
			SessionId = sessionId;

		}


		public static SecondAppLaunchContext FromCurrentProcess ()
		{
			var p = Process.GetCurrentProcess();

			var currentInstanceContext = new SecondAppLaunchContext
			(
				Environment.CommandLine ,
				[ .. Environment.GetCommandLineArgs().Skip(1) ] ,
				p.MainModule?.FileName ?? string.Empty ,
				p.ProcessName ,
				p.PriorityClass ,
				p.StartTime ,
				p.Id ,
				p.SafeHandle.DangerousGetHandle() ,
				p.SessionId
			);

			return currentInstanceContext;
		}


		internal virtual bool Validate ()
		{
			if ( FileName.isNullOrWhiteSpace || FileName.Length > MAX_STRING_LEN ) return false;
			if ( ProcessName.isNullOrWhiteSpace || ProcessName.Length > MAX_STRING_LEN ) return false;
			if ( Args.Any(s => s.Length > MAX_STRING_LEN) ) return false;

			//
			return true;
		}


		public static SecondAppLaunchContext CreateDefault ()
			=> SecondAppLaunchContext.FromCurrentProcess();


	}


	internal interface ISecondInstanceHandler<TSecondAppContext> where TSecondAppContext : SecondAppLaunchContext
	{
		void OnSecondInstance ( TSecondAppContext? secondAppContext );

	}



	internal sealed class SingleInstanceController<TContext> : uom.AutoDisposable.AutoDisposable1T<Mutex>, ISecondInstanceHandler<TContext> where TContext : SecondAppLaunchContext
	{


		private static SingleInstanceController<TContext>? _default;


		public static SingleInstanceController<TContext>? Default => _default;


		// Событие, которое сработает в первом экземпляре при попытке запуска второго
		// Теперь событие передает массив строк (аргументы)
		//public event Action<TContext?>? SecondInstanceStarted;



		private readonly string
			_mutexName,
			_pipeName;

		private Mutex? _mutex;
		private bool _isFirstInstance;
		private readonly CancellationTokenSource _cts = new();

		private Form? _ui;
		public ISecondInstanceHandler<TContext>? SecondInstanceStarHandler;

		/// <summary>Only apps from server path will be accepted to recive messages</summary>
		public bool StrictAppPath = false;


		public readonly JsonSerializerOptions JsonOptions = new()
		{
			MaxDepth = 8 , // Ограничиваем глубину вложенности JSON
			AllowTrailingCommas = false ,
			IgnoreReadOnlyFields = true ,
			AllowDuplicateProperties = false ,
			IncludeFields = true ,
			IndentCharacter = '\t' ,
			WriteIndented = true ,
		};



		/// <summary>
		/// Creates new instance
		/// </summary>
		/// <param name="id">Some ID suffix for global mutex</param>
		/// <param name="setDefaultSingleton">Sets static 'Default' singleton property</param>
		public SingleInstanceController ( string id = "" , bool setDefaultSingleton = true )
		{

			// Уникальность в рамках пользователя Windows
			string user = WindowsIdentity.GetCurrent().Name.Replace('\\' , '_');

			var appID = Application.ProductName;// Process.GetCurrentProcess().MainModule?.ModuleName;
			id = $"{appID}.{user}_{id}";
			_mutexName = $@"Global\{id}_mtx";
			_pipeName = $"{id}_Pipe";

			Debug.WriteLine($"Created SingleInstanceController with ID: '{id}'");
			if ( setDefaultSingleton ) _default = this;
		}



		// SecondInstanceStartedHandler не задан, поэтому сработал обработчик по умолчанию
		void ISecondInstanceHandler<TContext>.OnSecondInstance ( TContext? context )
		{
			// Если мы здесь, то SecondInstanceStarHandler задан по-умолчанию (не переопределён) и это мы сами!
			_ui ??= SecondInstanceStarHandler as Form;
			//SecondInstanceStarHandler ??= _ui as ISecondInstanceHandler<TContext>;
			if ( _ui is null ) throw new Exception("SecondInstanceStarHandler is not defined by user and SecondInstanceStarHandler is not Form!");

			_ui.BeginInvoke(_ui.expandAndActivate); // у нас есть форма, выполняем в её UI-потоке

			/*
		Action activateFirstInstanceCallback = (SecondInstanceStarHandler is null)
			? _ui!.expandAndActivate  // _ui НЕ поддерживает ISecondInstanceHandler<TContext> - просто разворачиваем форму
			//: new(() => SecondInstanceStarHandler.OnSecondInstance(context)); // _ui ПОДДЕРЖИВАЕТ ISecondInstanceHandler<TContext> - пусть сам и обрабатывает


		if ( _ui is null )
		{
			activateFirstInstanceCallback.Invoke(); // у нас нет формы, просто выполняем
		}
		else
		{
		}
			 */
		}





		/// <summary>
		/// If this app is first instance - run it via Application.Run()<br/>
		/// If this app is second instance - signal to the first instance and exit
		/// </summary>
		/// <param name="mainFormCreatorFunc">Main Form Creator</param>
		/// <param name="getSecondInstanceHandler"></param>
		/// <param name="secondInstanceStartContext">Some serializable data to send to the First istance from Second. App args or other info</param>
		/// <param name="createDefaultContext"></param>
		public void FirstInstanceRunOrSignal (
			Func<Form?> mainFormCreatorFunc ,
			Func<Form? , ISecondInstanceHandler<TContext>>? getSecondInstanceHandler = null ,
			TContext? secondInstanceStartContext = null
			)
		{
			ArgumentNullException.ThrowIfNull(mainFormCreatorFunc);

			if ( CheckIsFirstIsntance(secondInstanceStartContext) )
			{
				using var mainForm = mainFormCreatorFunc.Invoke();
				_ui = mainForm;
				SecondInstanceStarHandler = getSecondInstanceHandler?.Invoke(_ui) ?? this;
				if ( mainForm is not null ) Application.Run(mainForm);
			}
		}

		public void FirstInstanceRunOrSignal (
			Func<ApplicationContext> appContextCreatorFunc ,
			Func<ISecondInstanceHandler<TContext>>? getSecondInstanceHandler = null ,
			TContext? secondInstanceStartContext = null
			)
		{
			ArgumentNullException.ThrowIfNull(appContextCreatorFunc);

			if ( CheckIsFirstIsntance(secondInstanceStartContext) )
			{
				using var uiContext = appContextCreatorFunc.Invoke();


				SecondInstanceStarHandler = getSecondInstanceHandler?.Invoke()
					?? (uiContext as ISecondInstanceHandler<TContext>)
					?? throw new Exception("SecondInstanceStarHandler is NULL and ApplicationContext is not support ISecondInstanceHandler<TContext>!");

				if ( uiContext is not null ) Application.Run(uiContext);
			}
		}


		private bool CheckIsFirstIsntance ( TContext? secondInstanceContext )
		{
			try
			{
				// Пытаемся взять владение сразу (initiallyOwned: true)
				_mutex = new Mutex(true , _mutexName , out _isFirstInstance);
			}
			catch ( AbandonedMutexException )
			{
				// Предыдущий экземпляр аварийно завершился. Мы все равно теперь владеем этим мьютексом.
				_isFirstInstance = true;
			}

			Value = _mutex;

			if ( _isFirstInstance )
			{
				StartFirstInstanceServer();
				return _isFirstInstance;
			}

			secondInstanceContext ??= SecondAppLaunchContext.CreateDefault() as TContext;
			SendSignalToFirstInstance(secondInstanceContext);

			return _isFirstInstance;
		}


		private void StartFirstInstanceServer ()
		{
			// 1. Получаем SID текущего пользователя ПЕРЕД запуском сервера. Это наш "доверенный" идентификатор
			var userSid = WindowsIdentity.GetCurrent().User!;

			byte[] safeByteBuffer = new byte[ 1 ];

			Task.Run(async () =>
			{
				while ( !_cts.Token.IsCancellationRequested )
				{
					Debug.WriteLine("Pipe Server: begins waiting for new client connection...");

					try
					{
						// Создаем сервер (используя методы из предыдущих шагов)
						using var server = CreateSecurePipeServer();

						await server.WaitForConnectionAsync(_cts.Token);


						// Создаем отдельный токен на 2 секунды для конкретного сеанса связи
						using var cts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token);
						cts.CancelAfter(TimeSpan.FromSeconds(2));

						// 1. Читаем один "байт безопасности" (сигнал готовности)
						// Метод RunAsClient (имперсонация) опирается на токен безопасности, который передается вместе с сообщением.
						// Если клиент еще ничего не записал в поток, серверу просто некого «представлять», и вызов либо вернет данные самого сервера, либо завершится ошибкой.
						// Чтобы проверка безопасности заработала до обработки полезной нагрузки, используется стандартный прием «рукопожатия».

						// 1. Читаем байт безопасности асинхронно с таймаутом
						int read = await server.ReadAsync(safeByteBuffer , cts.Token);
						//await server.FlushAsync();
						if ( read == 0 ) continue; // Клиент закрыл поток

						// 2. Проверяем, кто подключился
						bool isTrusted = false;

						server.RunAsClient(() =>
						{
							var clientIdentity = WindowsIdentity.GetCurrent();
							// Сравниваем SID подключившегося клиента с нашим сохраненным SID
							isTrusted = clientIdentity.User!.Equals(userSid);
						});

						if ( !isTrusted )
						{
							Debug.WriteLine("Попытка взлома: подключился процесс другого пользователя!");
							server.Disconnect();
							continue;
						}

						if ( StrictAppPath )
						{
							if ( Kernel32.GetNamedPipeClientProcessId(server.SafePipeHandle.DangerousGetHandle() , out var clientPid) )
							{
								var clientProcess = Process.GetProcessById((int)clientPid);

								// Проверяем, что клиент — это наш же EXE файл
								// 'System.ComponentModel.Win32Exception' in System.Diagnostics.Process.dll происходит если клиент запущен с более высоким уровнем (например UAC или просто из под админа)
								string? clientPath = clientProcess?.MainModule?.FileName ?? Guid.NewGuid().ToString();

								string? currentPath = Process.GetCurrentProcess().MainModule?.FileName ?? Guid.NewGuid().ToString();
								isTrusted = clientPath.Equals(currentPath);
							}
							if ( !isTrusted )
							{
								Debug.WriteLine("Попытка взлома: подключился другой процесс!");
								server.Disconnect();
								continue;
							}
						}

						if ( server.IsConnected )
						{
							Debug.WriteLine("Pipe Server: client connected, reading second app context...");

							using StreamReader sr = new(server , Encoding.UTF8 , leaveOpen: true);
							var clientData = await sr.ReadToEndAsync(cts.Token);

							var context = clientData.isNullOrWhiteSpace
								? null
								: JsonSerializer.Deserialize<TContext>(clientData , JsonOptions);

							// Обязательная проверка полей!
							if ( !(context?.Validate() ?? true) ) throw new Exception("Попытка взлома?: context.Validate() FAILED!");

							Debug.WriteLine($"Pipe Server: second app context readed:\n{context?.dumpObjectToString()} ...");
							Debug.WriteLine($"Pipe Server: Notifyng OnSecondInstance...");
							SecondInstanceStarHandler?.OnSecondInstance(context); // context может быть NULL, если не совпадут версии или ещё чего не то в протоколе...							
						}

					}
					catch ( OperationCanceledException ) { }
					catch ( Exception ex ) { Debug.WriteLine(ex.Message); }
				}
			} , _cts.Token);
		}

		private NamedPipeServerStream CreateSecurePipeServer ()
		{

			// Это работает во всех версиях .NET Core / .NET 5+
			// Ограничивает доступ к Pipe ТОЛЬКО тем же самым пользователем, 
			// от имени которого запущен сервер.
			NamedPipeServerStream? server = new(
				_pipeName ,
				PipeDirection.In ,
				1 ,
				PipeTransmissionMode.Byte ,
				PipeOptions.Asynchronous | PipeOptions.CurrentUserOnly);

			return server;


			/*
			/// <summary>Использовать старый добрый PipeSecurity, но разрешить доступ конкретному SID пользователя без учета его уровня элевации.</summary>
			private NamedPipeServerStream CreateSecurePipeServer ()
			{
			1. Подключите NuGet пакет System.IO.Pipes.AccessControl

				var pipeSecurity = new PipeSecurity();

				// Получаем SID пользователя (он одинаков и у админа, и у обычного юзера в рамках одной сессии)
				var userSid = WindowsIdentity.GetCurrent().User;

				// Разрешаем полный доступ этому SID
				pipeSecurity.AddAccessRule(new PipeAccessRule(
					userSid ,
					PipeAccessRights.ReadWrite ,
					AccessControlType.Allow));

				// Создаем сервер (используя пакет AccessControl, о котором говорили выше)
				return NamedPipeServerStreamConstructors.New(
					_pipeName ,
					PipeDirection.In ,
					1 ,
					PipeTransmissionMode.Byte ,
					PipeOptions.Asynchronous ,
					0 , 0 ,
					pipeSecurity);
			}
			 */

		}


		private void SendSignalToFirstInstance ( TContext? secondInstanceContext )
		{
			try
			{
				var secondInstanceContextJson = JsonSerializer.Serialize(secondInstanceContext , JsonOptions);

				using var client = new NamedPipeClientStream("." , _pipeName , PipeDirection.Out);
				client.Connect(500);

				// 1. Отправляем "байт безопасности", чтобы сервер мог нас идентифицировать
				client.WriteByte(1);
				// Важно сделать Flush, чтобы байт ушел немедленно, не дожидаясь заполнения буфера
				client.Flush();

				using StreamWriter sw = new(client , Encoding.UTF8) { AutoFlush = true };
				sw.Write(secondInstanceContextJson);
				sw.Flush();
			}
			catch ( Exception ex ) { Debug.WriteLine(ex.Message); }
		}


		protected override void FreeManagedObjects ()
		{
			_cts.Cancel();

			if ( _isFirstInstance ) _mutex?.ReleaseMutex();
			base.FreeManagedObjects();
		}




	}



}