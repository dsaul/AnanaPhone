using AsterNET.FastAGI;
using Serilog;
using DanSaul.SharedCode.Asterisk;
using AnanaPhone.Calls;
using AnanaPhone.AMI;

namespace AnanaPhone.ARI.Attendants
{
	public class AttendantTrackedExternalDirectToConference : AttendantBase
	{
		public ActiveCall? ActiveCall { get; set; } = null;
		public ActiveCallManager? ACM = Program.Application?.Services.GetRequiredService<ActiveCallManager>();
		public AMIManager? AMI = Program.Application?.Services.GetRequiredService<AMIManager>();

		public override void Service(AGIRequest request, AGIChannel channel)
		{
			try
			{
				if (ACM == null)
					throw new InvalidOperationException("ACM == null");
				if (AMI != null && false == AMI.IsConnected)
					AMI.Connect();

				if (request.Channel == "OutgoingSpoolFailed")
					throw new Exception("request.Channel == \"OutgoingSpoolFailed\"");

				ActiveCall = ACM.ForCallId(request.Channel);
				if (ActiveCall == null)
				{
					ActiveCall = new();
					ACM.Register(ActiveCall);
				}

				ActiveCall.AGIScript = this;
				ActiveCall.AGIRequest = request;
				ActiveCall.AGIChannel = channel;
				ActiveCall.FarCallId = FarCallId;
				ActiveCall.RequestConferenceId = LandedConferenceName;
				ActiveCall.CallerIdNumber = request.CallerId;
				ActiveCall.CallerIdName = request.CallerIdName;
				ActiveCall.ChannelName = request.Channel;
				ActiveCall.CallDirection = "Internal";

				Log.Information("[{CallId}][{Class}.{Method}()] New Call {CallerId} {CallerIdName}",
					ActiveCall?.AGIRequest?.UniqueId,
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name,
					ActiveCall?.AGIRequest.CallerId,
					ActiveCall?.AGIRequest.CallerIdName
				);

				Answer();

				StateMachineLoop();
			}
			catch (AGINetworkException)
			{
				Log.Information("[{CallId}][{Class}.{Method}()] EVT Call Ended AGINetworkException",
					ActiveCall?.AGIRequest?.UniqueId,
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name,
					ActiveCall?.AGIRequest?.CallerId,
					ActiveCall?.AGIRequest?.CallerIdName
				);
				if (ActiveCall != null)
					ACM?.DeRegister(ActiveCall);
				ActiveCall = null;
				Hangup();
			}
			catch (AGIHangupException)
			{
				Log.Information("[{CallId}][{Class}.{Method}()] EVT Call Ended PerformHangupException",
					ActiveCall?.AGIRequest?.UniqueId,
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name,
					ActiveCall?.AGIRequest?.CallerId,
					ActiveCall?.AGIRequest?.CallerIdName
				);
				if (ActiveCall != null)
					ACM?.DeRegister(ActiveCall);
				ActiveCall = null;
				Hangup();
			}
			catch (PerformHangupException)
			{
				Log.Information("[{CallId}][{Class}.{Method}()] EVT Call Ended PerformHangupException",
					ActiveCall?.AGIRequest?.UniqueId,
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name,
					ActiveCall?.AGIRequest?.CallerId,
					ActiveCall?.AGIRequest?.CallerIdName
				);
				if (ActiveCall != null)
					ACM?.DeRegister(ActiveCall);
				ActiveCall = null;
				Hangup();
			}
			catch (Exception e)
			{
				Log.Error(e, "[{CallId}][{Class}.{Method}()] {Message}",
					ActiveCall?.AGIRequest?.UniqueId,
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name,
					e.Message
				);
				if (ActiveCall != null)
					ACM?.DeRegister(ActiveCall);
				ActiveCall = null;
				Hangup();
			}
		}


		void StateMachineLoop()
		{
			while (true)
			{
				if (AMI != null && false == AMI.IsConnected)
					AMI.Connect();
				if (ActiveCall == null)
					throw new PerformHangupException();

				// Update Channel Variables
				lock (ActiveCall.ThreadLock)
				{
					if (!string.IsNullOrWhiteSpace(ActiveCall.RequestNewCallTarget))
					{
						CallTarget = ActiveCall.RequestNewCallTarget;
						ActiveCall.RequestNewCallTarget = null;
					}

					if (!string.IsNullOrWhiteSpace(ActiveCall.RequestConferenceId))
					{
						ConferenceId = ActiveCall.RequestConferenceId;
						ActiveCall.RequestConferenceId = null;
					}
				}

				string? targetForThisIteration = CallTarget;

				switch (targetForThisIteration)
				{
					default:
						Log.Warning("[{CallId}][{Class}.{Method}()] State Machine Unhandled Target {Target}",
							ActiveCall?.AGIRequest?.UniqueId,
							GetType().Name,
							System.Reflection.MethodBase.GetCurrentMethod()?.Name,
							targetForThisIteration
						);
						throw new PerformHangupException();
					case kCallTargetOfferConferenceReJoin:
						OfferConferenceReJoin();
						break;
					case kCallTargetHangup:
						throw new PerformHangupException();
					case kCallTargetJoinConference:
						JoinConference();
						break;
				}

				Thread.Sleep(Env.CALL_ITERATE_SLEEP_MS);
			}
		}

		void OfferConferenceReJoin()
		{
			Log.Information("[{CallId}][{Class}.{Method}()]",
				ActiveCall?.AGIRequest?.UniqueId,
				GetType().Name,
				System.Reflection.MethodBase.GetCurrentMethod()?.Name
			);

			if (ActiveCall == null)
				throw new PerformHangupException();

			bool? wait = null;

			for (int i = 0; i < 3; i++)
			{
				wait = PromptBooleanQuestion(new List<AudioPlaybackEvent> {
						new AudioPlaybackEvent {
							Type = AudioPlaybackEventType.Stream,
							StreamFile = Env.SND_CALL_DROPPED_SHOULD_WE_WAIT,
						},
					});

				if (wait != null)
					break;
			}

			if (wait == null || wait != null && wait == false)
			{
				CallTarget = kCallTargetHangup;
				return;
			}

			CallTarget = kCallTargetJoinConference;
		}

		void JoinConference()
		{
			Log.Information("[{CallId}][{Class}.{Method}()]",
				ActiveCall?.AGIRequest?.UniqueId,
				GetType().Name,
				System.Reflection.MethodBase.GetCurrentMethod()?.Name
			);

			if (ActiveCall == null)
				throw new PerformHangupException();

			// HACK: For some reason in this one we CAN call conf bridge directly! Seems
			// to be regular users must go directly, but admins can't.
			Exec("ConfBridge", $"{ConferenceId},default_bridge,default_user");
			//Exec("Dial", $"Local/{ConferenceId}@{Env.CONF_CONTEXT_EXTERNAL}");

			CallTarget = kCallTargetOfferConferenceReJoin;
		}
	}
}
