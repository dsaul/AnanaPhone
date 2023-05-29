using AsterNET.Manager;
using AsterNET.Manager.Action;
using AsterNET.Manager.Response;
using Serilog;
using AsterNET.Manager.Event;
using AnanaPhone.Calls;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;

namespace AnanaPhone.AMI
{
	public class AMIManager : IDisposable
	{
		public ManagerConnection Connection { get; } = new ManagerConnection(
			EnvAsterisk.ASTERISK_HOST, 
			EnvAsterisk.ASTERISK_AMI_PORT, 
			EnvAsterisk.ASTERISK_AMI_USER,
			Env.AMI_PW
		);

		public AMIManager()
		{
			Connect();
		}

		public bool IsConnected
		{
			get
			{
				return Connection.IsConnected();
			}
		}

		public void Connect()
		{
			try
			{
				Log.Debug("ASTERISK_HOST = {ASTERISK_HOST}", EnvAsterisk.ASTERISK_HOST);
				Log.Debug("ASTERISK_AMI_PORT = {ASTERISK_AMI_PORT}", EnvAsterisk.ASTERISK_AMI_PORT);
				Log.Debug("ASTERISK_AMI_USER = {ASTERISK_AMI_USER}", EnvAsterisk.ASTERISK_AMI_USER);
				Log.Debug("ASTERISK_AMI_PASS = {ASTERISK_AMI_PASS}", EnvAsterisk.ASTERISK_AMI_PASS);


				Connection.DefaultResponseTimeout = 0;
				Connection.DefaultEventTimeout = 0;
				//Connection.FireAllEvents = true;
				Connection.UnhandledEvent += new EventHandler<ManagerEvent>(UnhandledEvent);
				Connection.Login();

				Log.Information("[{Class}.{Method}()] Connected",
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name
				);
			}
			catch (Exception e)
			{
				Log.Error(e, "[{Class}.{Method}()] {Message}",
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name,
					e.Message
				);
			}
		}

		void UnhandledEvent(object? sender, ManagerEvent e)
		{
			Log.Warning("[{Class}.{Method}()] EVT UnhandledEvent {ManagerEvent}",
				GetType().Name,
				System.Reflection.MethodBase.GetCurrentMethod()?.Name,
				e
			);


		}

		public void Reload(string moduleName)
		{
			ReloadAction action = new(moduleName);
			Connection.SendActionAsync(action);
		}

		public void Originate(
			string channel,
			string exten,
			string context,
			string? CallerIdName,
			IEnumerable<KeyValuePair<string, string>>? variables = null
			)
		{
			OriginateAction action = new()
			{
				Channel = channel,
				Exten = exten,
				Context = context,
				Async = true,
			};

			if (!string.IsNullOrWhiteSpace(CallerIdName))
				action.CallerId = CallerIdName;

			if (variables != null)
			{
				foreach (KeyValuePair<string, string> variable in variables)
					action.SetVariable(variable.Key, variable.Value);
			}

			Connection.SendActionAsync(action);

			//Task.Run(() =>
			//{
			//	ResponseEvents re = AMI.SendEventGeneratingAction(action);
			//	foreach (ResponseEvent evt in re.Events)
			//	{
			//		if (evt is not OriginateResponseEvent or)
			//			continue;

			//		if (ActiveCall == null)
			//			return;

			//		Log.Information("or {channel}", or.Channel);

			//	}
			//});
		}
		public void BridgeCalls(ActiveCall call1, ActiveCall call2)
		{

			try
			{
				if (call1.AGIRequest == null || call2.AGIRequest == null)
					throw new Exception("call1.Request == null || call2.Request == null");


				BridgeAction action = new()
				{
					Channel1 = call1.AGIRequest.Channel,
					Channel2 = call2.AGIRequest.Channel,
					Tone = "no"
				};


				Connection.SendAction(action);
			}
			catch (Exception e)
			{
				Log.Error(e, "[{Class}.{Method}()] {Message}",
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name,
					e.Message
				);
			}
		}
		public void Hangup(ActiveCall call)
		{
			if (!string.IsNullOrWhiteSpace(call.ChannelName))
				Hangup(call.ChannelName);
		}
		public void Hangup(string channel)
		{
			try
			{
				HangupAction action = new()
				{
					Channel = channel
				};

				Connection.SendActionAsync(action);
			}
			catch (Exception e)
			{
				Log.Error(e, "[{Class}.{Method}()] {Message}",
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name,
					e.Message
				);
			}
		}
		public void BlindTransfer(ActiveCall call, string context, string dest)
		{
			try
			{
				if (call.AGIRequest == null)
				{
					Log.Warning("call.Request == null");
					return;
				}

				BlindTransferAction action = new()
				{
					Channel = call.AGIRequest.Channel,
					Context = context,
					Exten = dest
				};

				Connection.SendAction(action);
			}
			catch (Exception e)
			{
				Log.Error(e, "[{Class}.{Method}()] {Message}",
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name,
					e.Message
				);
			}

		}
		public Task<ManagerResponse> SendActionAsync(ManagerAction action)
		{
			if (IsConnected == false)
				Connect();
			return Connection.SendActionAsync(action);
		}
		public ResponseEvents SendEventGeneratingAction(ManagerActionEvent action)
		{
			if (IsConnected == false)
				Connect();
			return Connection.SendEventGeneratingAction(action);
		}
		public ResponseEvents SendEventGeneratingAction(ManagerActionEvent action, int timeout)
		{
			if (IsConnected == false)
				Connect();
			return Connection.SendEventGeneratingAction(action, timeout);
		}



		#region IDisposable
		private bool disposedValue;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				disposedValue = true;
			}
		}

		// // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		// ~AMIManager()
		// {
		//     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		//     Dispose(disposing: false);
		// }

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}
