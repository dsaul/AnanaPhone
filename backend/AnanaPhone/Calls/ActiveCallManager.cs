using AnanaPhone.AMI;
using AsterNET.Manager.Action;
using AsterNET.Manager.Event;
using Serilog;

namespace AnanaPhone.Calls
{
	public class ActiveCallManager : IDisposable
	{
		AMIManager AMI { get; init; }

		public List<ActiveCall> ActiveCalls { get; } = new List<ActiveCall>();

		public ActiveCallManager(AMIManager _AMI)
		{
			AMI = _AMI;

			AMI.Connection.Hangup += Hangup;
			AMI.Connection.NewChannel += NewChannel;

			ReSyncChannelsLoop();

		}


		#region Resync


		void ReSyncChannelsLoop()
		{
			TaskCompletionSource<bool>? isCommandCompleted = null;

			List<CoreShowChannelEvent> events = new();

			void CoreShowChannel(object? sender, CoreShowChannelEvent e)
			{
				if (isCommandCompleted == null)
					throw new InvalidOperationException("isCommandCompleted == null");
				if (e.Channel.StartsWith("Local/"))
					return;
				if (e.Channel.StartsWith("CBAnn/"))
					return;

				//Log.Information("[{Class}.{Method}()] EVT CoreShowChannel {ChannelName}",
				//	GetType().Name,
				//	System.Reflection.MethodBase.GetCurrentMethod()?.Name,
				//	e.Channel
				//);

				events.Add(e);
			}

			void CoreShowChannelsComplete(object? sender, CoreShowChannelsCompleteEvent e)
			{
				if (isCommandCompleted == null)
					throw new InvalidOperationException("isCommandCompleted == null");

				//Log.Information("[{Class}.{Method}()] EVT CoreShowChannelsComplete",
				//	GetType().Name,
				//	System.Reflection.MethodBase.GetCurrentMethod()?.Name,
				//	e.Channel
				//);

				isCommandCompleted.SetResult(true);
			}

			_ = Task.Run(async () =>
			{
				while (true)
				{
					// Get multi event information from Asterisk.
					isCommandCompleted = new TaskCompletionSource<bool>();
					events.Clear();

					AMI.Connection.CoreShowChannel += CoreShowChannel;
					AMI.Connection.CoreShowChannelsComplete += CoreShowChannelsComplete;

					CoreShowChannelsAction action = new();
					Task sendActionTask = AMI.SendActionAsync(action);

					await Task.WhenAll(new Task[] { sendActionTask, isCommandCompleted.Task });

					AMI.Connection.CoreShowChannel -= CoreShowChannel;
					AMI.Connection.CoreShowChannelsComplete -= CoreShowChannelsComplete;

					// Process Information
					List<string?> currentActiveChannels = new();

					// Add & Update Active Call Tracking
					foreach (CoreShowChannelEvent evt in events)
					{
						//Log.Information("evt {evt}", evt);

						currentActiveChannels.Add(evt.Channel);

						// First, see if the call is already tracked.
						ActiveCall? call = ForCallId(evt.Channel);
						if (call == null)
						{
							call = new()
							{
								AGIScript = null,
								AGIRequest = null,
								AGIChannel = null,
								FarCallId = null,
								CallDirection = null,
								ChannelName = evt.Channel,
							};
							Register(call);
						}

						call.CallerIdNumber = evt.CallerIdNum;
						call.CallerIdName = evt.CallerIdName;
						call.Language = evt.Language;
						call.Context = evt.Context;
						call.Exten = evt.Exten;
						call.Priority = evt.Priority;
						call.Uniqueid = evt.Uniqueid;
						call.Linkedid = evt.Linkedid;
						call.BridgeId = evt.BridgeId;
						call.Application = evt.Application;
						call.ApplicationData = evt.ApplicationData;
						call.Duration = evt.Duration;
						call.ChannelState = evt.ChannelState;
						call.ChannelStateDesc = evt.ChannelStateDesc;
						call.State = evt.State;
						call.AccountCode = evt.AccountCode;
					}

					// Go through the active calls, and remove those no longer active.
					DeRegisterAllNotInChannelList(currentActiveChannels);

					await Task.Delay(1000 * 10);
				}
			});







		}




		#endregion
		#region Registered
		public void Register(ActiveCall? call)
		{
			if (call == null)
			{
				return;
			}
			ActiveCalls.Add(call);
		}

		void DeRegisterAllNotInChannelList(IEnumerable<string?> channels)
		{
			ActiveCalls.RemoveAll((call) =>
			{
				return !channels.Contains(call.ChannelName);
			});
		}

		public void DeRegister(IEnumerable<string?> channels)
		{
			ActiveCalls.RemoveAll((call) =>
			{
				return channels.Contains(call.ChannelName);
			});
		}

		public void DeRegister(ActiveCall call)
		{
			if (!string.IsNullOrWhiteSpace(call.ChannelName))
				DeRegister(call.ChannelName);
		}
		public void DeRegister(string channelName)
		{
			DeRegister(new string[] { channelName });
		}

		void Hangup(object? sender, HangupEvent e)
		{
			Log.Information("[{Class}.{Method}()] EVT HangupEvent {ChannelName}",
				GetType().Name,
				System.Reflection.MethodBase.GetCurrentMethod()?.Name,
				e.Channel
			);

			if (!string.IsNullOrWhiteSpace(e.Channel))
				DeRegister(e.Channel);

		}

		void NewChannel(object? sender, NewChannelEvent evt)
		{
			Log.Information("[{Class}.{Method}()] EVT NewChannel {ChannelName}",
				GetType().Name,
				System.Reflection.MethodBase.GetCurrentMethod()?.Name,
				evt.Channel
			);

			ActiveCall? call = ForCallId(evt.Channel);
			if (call == null)
			{
				call = new()
				{
					AGIScript = null,
					AGIRequest = null,
					AGIChannel = null,
					FarCallId = null,
					CallDirection = null,
					ChannelName = evt.Channel,
				};
				Register(call);
			}

			call.CallerIdNumber = evt.CallerIdNum;
			call.CallerIdName = evt.CallerIdName;
			call.ChannelState = evt.ChannelState;
			call.ChannelStateDesc = evt.ChannelStateDesc;
			call.State = evt.State;
			call.AccountCode = evt.AccountCode;
		}


		#endregion

		public IEnumerable<ActiveCall> ForFarCallId(string? callId)
		{
			if (callId == null)
				yield break;

			foreach (ActiveCall call in ActiveCalls)
			{
				if (call.FarCallId == callId)
					yield return call;
			}
			yield break;
		}

		public ActiveCall? ForCallId(string? callId)
		{
			if (callId == null)
				return null;

			foreach (ActiveCall call in ActiveCalls)
			{
				if (call.Id == callId)
					return call;
			}
			return null;
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
		// ~ActiveCallManager()
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
