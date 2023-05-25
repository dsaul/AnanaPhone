using AsterNET.FastAGI;
using Serilog;
using DanSaul.SharedCode.Asterisk;
using AnanaPhone.AMI;
using AnanaPhone.Calls;
using NodaTime;

namespace AnanaPhone.ARI.Attendants
{
	public partial class AttendantDoYouAcceptTheCall : AttendantBase
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
					case kCallTargetHangup:
						throw new PerformHangupException();
					case kCallTargetJoinConference:
						JoinConference();
						break;
					case kCallTargetSeekOwnerOfCallRequest:
						NotifyOwnerOfCallRequest();
						break;
				}

				Thread.Sleep(Env.CALL_ITERATE_SLEEP_MS);
			}
		}



		void NotifyOwnerOfCallRequest()
		{
			Log.Information("[{CallId}][{Class}.{Method}()]",
				ActiveCall?.AGIRequest?.UniqueId,
				GetType().Name,
				System.Reflection.MethodBase.GetCurrentMethod()?.Name
			);

			if (ACM == null)
				throw new InvalidOperationException("ACM == null");
			if (ActiveCall == null)
				throw new PerformHangupException();

			ActiveCall? farCall;
			bool? acceptCall = null;

			for (int i = 0; i < 3; i++)
			{
				// We check this every loop, that way if the far call disconnects, we can abort this call.
				farCall = ACM.ForCallId(ActiveCall.FarCallId);
				if (farCall == null)
				{
					StreamFile(Env.SND_INCOMING_ALREADY_HUNG_UP, "");
					throw new PerformHangupException();
				}

				acceptCall = PromptBooleanQuestion(new List<AudioPlaybackEvent> {
						new AudioPlaybackEvent {
							Type = AudioPlaybackEventType.Stream,
							StreamFile = Env.SND_INCOMING_CALL_FROM,
						},
						new AudioPlaybackEvent
						{
							Type = AudioPlaybackEventType.SayAlpha,
							Alpha = $"{farCall.AGIRequest?.CallerId ?? "?"}",
						},
						new AudioPlaybackEvent {
							Type = AudioPlaybackEventType.Stream,
							StreamFile = Env.SND_PRESS_1_TO_ACCEPT_THIS_CALL,
						},
					});

				if (acceptCall != null)
					break;
			}

			// One last alive check before we transfer
			farCall = ACM.ForCallId(ActiveCall.FarCallId);
			if (farCall == null)
			{
				StreamFile(Env.SND_INCOMING_ALREADY_HUNG_UP, "");
				throw new PerformHangupException();
			}

			// Tell the calls when in the call loop to transfer to the conference room.
			if (acceptCall != null && acceptCall == true)
			{
				Instant instantNow = SystemClock.Instance.GetCurrentInstant();
				string confRoomName = $"{Env.CONF_ROOM_NAME_PREFIX}{instantNow.ToUnixTimeTicks()}";

				lock (ActiveCall.ThreadLock)
				{
					ActiveCall.RequestConferenceId = confRoomName;
					CallTarget = kCallTargetJoinConference;
				}

				lock (farCall.ThreadLock)
				{
					farCall.RequestConferenceId = confRoomName;
					farCall.RequestNewCallTarget = kCallTargetJoinConference;
				}

			}

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

			//Exec("ConfBridge", $"{conferenceName},default_bridge,default_user");

			Exec("Dial", $"Local/{ConferenceId}@{Env.CONF_CONTEXT_ADMIN}");
			CallTarget = kCallTargetOfferConferenceReJoin;
		}


	}
}
