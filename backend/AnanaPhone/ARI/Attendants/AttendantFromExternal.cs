using AsterNET.FastAGI;
using Serilog;
using DanSaul.SharedCode.Asterisk;
using NodaTime;
using Newtonsoft.Json;
using Amazon.S3.Model;
using Amazon.S3;
using Renci.SshNet;
using AnanaPhone.Calls;
using AnanaPhone.AMI;
using AnanaPhone.VoiceMail;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;

namespace AnanaPhone.ARI.Attendants
{
	public partial class AttendantFromExternal : AttendantBase
	{

		public ActiveCallManager? ACM = Program.Application?.Services.GetRequiredService<ActiveCallManager>();
		public AMIManager? AMI = Program.Application?.Services.GetRequiredService<AMIManager>();
		public HistoricCallManager? CHM = Program.Application?.Services.GetRequiredService<HistoricCallManager>();
		public AmazonS3Client? S3Client = Program.Application?.Services.GetRequiredService<AmazonS3Client>();
		public VoiceMailManager? VMM = Program.Application?.Services.GetRequiredService<VoiceMailManager>();

		public ActiveCall? ActiveCall { get; set; } = null;

		public override void Service(AGIRequest request, AGIChannel channel)
		{
			if (ACM == null)
				throw new InvalidOperationException("ACM == null");
			if (CHM == null)
				throw new InvalidOperationException("CHM == null");

			try
			{
				if (AMI != null && false == AMI.IsConnected)
					AMI.Connect();

				string? landedDID = GetVariable(Env.LANDED_DID_VAR_NAME);

				ActiveCall = ACM.ForCallId(request.Channel);
				if (ActiveCall == null)
				{
					ActiveCall = new();
					ACM.Register(ActiveCall);
				}

				ActiveCall.AGIScript = this;
				ActiveCall.AGIRequest = request;
				ActiveCall.AGIChannel = channel;
				ActiveCall.LandedDID = landedDID;
				ActiveCall.CallerIdNumber = request.CallerId;
				ActiveCall.CallerIdName = request.CallerIdName;
				ActiveCall.ChannelName = request.Channel;
				ActiveCall.CallDirection = "Inbound";

				CHM.Upsert(ActiveCall.GenerateHistoricCall());

				Log.Information("[{CallId}][{Class}.{Method}()] New Call {CallerId} {CallerIdName}",
					ActiveCall.AGIRequest.UniqueId,
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name,
					ActiveCall.AGIRequest.CallerId,
					ActiveCall.AGIRequest.CallerIdName
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
				{
					ACM?.DeRegister(ActiveCall);
					CHM?.Upsert(ActiveCall.GenerateHistoricCall());
				}
				ActiveCall = null;
				Hangup();
			}
			catch (AGIHangupException)
			{
				Log.Information("[{CallId}][{Class}.{Method}()] EVT Call Ended AGIHangupException",
					ActiveCall?.AGIRequest?.UniqueId,
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name,
					ActiveCall?.AGIRequest?.CallerId,
					ActiveCall?.AGIRequest?.CallerIdName
				);
				if (ActiveCall != null)
				{
					ACM?.DeRegister(ActiveCall);
					CHM?.Upsert(ActiveCall.GenerateHistoricCall());
				}
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
				Log.Fatal(e, "[{CallId}][{Class}.{Method}()] {Message}",
					ActiveCall?.AGIRequest?.UniqueId,
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name,
					e.Message
				);
				if (ActiveCall != null)
				{
					ACM?.DeRegister(ActiveCall);
					CHM?.Upsert(ActiveCall.GenerateHistoricCall());
				}
				ActiveCall = null;
				Hangup();
			}
		}

		public void SeekOwnerInitiateOwnerCalls(ActiveCall activeCall)
		{
			try
			{
				if (AMI != null && false == AMI.IsConnected)
					AMI.Connect();
				if (AMI == null)
					throw new InvalidOperationException("AMI == null");
				if (ActiveCall == null)
					throw new InvalidOperationException("ActiveCall == null");

				IEnumerable<string> seekOwnerChannels = Env.SEEK_OWNER_CHANNELS;
				if (!seekOwnerChannels.Any())
					throw new InvalidOperationException("!seekOwnerChannels.Any()");

				foreach (string seekOwnerChannel in seekOwnerChannels)
				{
					try
					{

						List<KeyValuePair<string, string>> vars = new();
						if (!string.IsNullOrEmpty(activeCall.Id))
							vars.Add(new KeyValuePair<string, string>(Env.FAR_CALL_ID_VAR_NAME, activeCall.Id.ToString()));

						AMI.Originate(
							seekOwnerChannel,
							Env.SEEK_OWNER_ACCEPT_CALL_LAND_EXTEN,
							Env.SEEK_OWNER_ACCEPT_CALL_LAND_CONTEXT,
							$"Owner {seekOwnerChannel}",
							vars
							);
					}
					catch (UnauthorizedAccessException e)
					{
						Log.Error(e, "[{CallId}][{Class}.{Method}()] {Message}",
							ActiveCall?.AGIRequest?.UniqueId,
							GetType().Name,
							System.Reflection.MethodBase.GetCurrentMethod()?.Name,
							e.Message
						);
					}
				}
			}
			catch (Exception e)
			{
				Log.Error(e, "[{CallId}][{Class}.{Method}()] {Message}",
					ActiveCall?.AGIRequest?.UniqueId,
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name,
					e.Message
				);
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
					case kCallTargetOfferConferenceReJoin:
						OfferConferenceReJoin();
						break;
					case kCallTargetVoiceMail:
						VoiceMail();
						break;
					case kCallTargetWaitForConferenceOwnerThenVoiceMail:
						WaitForConferenceOwnerThenVoiceMail();
						break;
					case kCallTargetSeekOwnerOfCallRequest:
						SeekOwnerNotifyCallRequest();
						break;
				}

				Thread.Sleep(Env.CALL_ITERATE_SLEEP_MS);
			}


		}

		void SeekOwnerNotifyCallRequest()
		{
			Log.Information("[{CallId}][{Class}.{Method}()]",
				ActiveCall?.AGIRequest?.UniqueId,
				GetType().Name,
				System.Reflection.MethodBase.GetCurrentMethod()?.Name
			);


			if (ActiveCall == null)
				throw new PerformHangupException();

			SeekOwnerInitiateOwnerCalls(ActiveCall);

			Log.Debug("[{CallId}][{Class}.{Method}()] notify play intro",
				ActiveCall?.AGIRequest?.UniqueId,
				GetType().Name,
				System.Reflection.MethodBase.GetCurrentMethod()?.Name
			);
			StreamFile(Env.SND_EXTERNAL_INTRO, kEscapeAllKeys);

			// The target could have already changed while the above stream is playing.
			if (ActiveCall != null)
				lock (ActiveCall.ThreadLock)
				{
					if (ActiveCall != null && ActiveCall.RequestNewCallTarget != kCallTargetJoinConference)
					{
						PlayMusicOnHold();
						CallTarget = kCallTargetWaitForConferenceOwnerThenVoiceMail;
					}
					else
					{
						CallTarget = kCallTargetJoinConference;
					}
				}
		}

		void WaitForConferenceOwnerThenVoiceMail()
		{
			Log.Information("[{CallId}][{Class}.{Method}()]",
				ActiveCall?.AGIRequest?.UniqueId,
				GetType().Name,
				System.Reflection.MethodBase.GetCurrentMethod()?.Name
			);

			if (ActiveCall == null)
				throw new PerformHangupException();

			Instant startWaitInstant = SystemClock.Instance.GetCurrentInstant();

			while (true)
			{
				// If the call target was changed elsewhere go back to the state machine loop.
				lock (ActiveCall.ThreadLock)
					if (
						ActiveCall.RequestNewCallTarget != null && ActiveCall.RequestNewCallTarget != kCallTargetWaitForConferenceOwnerThenVoiceMail ||
						CallTarget != kCallTargetWaitForConferenceOwnerThenVoiceMail
						)
						return;

				Instant instant = SystemClock.Instance.GetCurrentInstant();

				Duration diff = instant - startWaitInstant;
				if (diff.TotalSeconds > Env.CALL_FOLLOW_ME_TIMEOUT_SECTIONS)
				{
					CallTarget = kCallTargetVoiceMail;
					return;
				}
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

			// For whatever reason you can't just directly go to ConfBridge, you have to dial through a local channel.
			//Exec("ConfBridge", $"{conferenceName},default_bridge,default_user");

			// Once connected, raise the duration limit to something silly, but still less than a day.
			Exec("Dial", $"Local/{ConferenceId}@{Env.CONF_CONTEXT_EXTERNAL}");

			CallTarget = kCallTargetOfferConferenceReJoin;
		}

		void VoiceMail()
		{
			Log.Information("[{CallId}][{Class}.{Method}()]",
				ActiveCall?.AGIRequest?.UniqueId,
				GetType().Name,
				System.Reflection.MethodBase.GetCurrentMethod()?.Name
			);

			if (ActiveCall == null)
				throw new PerformHangupException();

			bool? leaveVM = null;

			for (int i = 0; i < 3; i++)
			{
				leaveVM = PromptBooleanQuestion(new List<AudioPlaybackEvent> {
						new AudioPlaybackEvent {
							Type = AudioPlaybackEventType.Stream,
							StreamFile = Env.SND_UNABLE_TO_REACH_LEAVE_VM,
						},
					});

				if (leaveVM != null)
					break;
			}

			if (leaveVM != null && leaveVM == true)
				LeaveVM();

			CallTarget = kCallTargetHangup;
		}

		void LeaveVM()
		{
			if (S3Client == null)
				throw new InvalidOperationException("S3Client == null");
			if (VMM == null)
				throw new InvalidOperationException("VMM == null");

			
			Log.Information("[{CallId}][{Class}.{Method}()] Start",
				ActiveCall?.AGIRequest?.UniqueId,
				GetType().Name,
				System.Reflection.MethodBase.GetCurrentMethod()?.Name
			);

			if (ActiveCall == null)
				throw new PerformHangupException();

			string id = Guid.NewGuid().ToString();
			string s3Key = $"{id}/recording.wav";
			VoiceMailMessageRow metadata = new()
			{
				Id = id,
				CallerIdNumber = ActiveCall.AGIRequest?.CallerId ?? null,
				CallerIdName = ActiveCall.AGIRequest?.CallerIdName ?? null,
				TimestampISO8601 = ActiveCall.TimestampISO8601,
			};

			RecordFile(
				$"/vmail/{id}",
				"wav",
				"",
				Env.VMAIL_MAX_LENGTH,
				0,
				Env.VMAIL_BEEP,
				Env.VMAIL_MAX_SILENCE
			);

			


			// Upload recording to S3

			// If we're in dev mode this is done by fetching the recording through
			// SSH, then uploading it. If this is production, it can be done by
			// fetching it through the local filesystem.

			if (EnvAsterisk.ASTERISK_DEBUG_SSH_ENABLE)
			{
				string pKey = Resources.private_key;
				var pkeyStream = new MemoryStream();
				var pkeyWriter = new StreamWriter(pkeyStream);
				pkeyWriter.Write(pKey);
				pkeyWriter.Flush();
				pkeyStream.Position = 0;

				PrivateKeyAuthenticationMethod auth = new(
					EnvAsterisk.ASTERISK_DEBUG_SSH_USER, 
					new PrivateKeyFile(pkeyStream)
				);

				var connectionInfo = new Renci.SshNet.ConnectionInfo(
					EnvAsterisk.ASTERISK_HOST, 
					EnvAsterisk.ASTERISK_DEBUG_SSH_PORT, 
					EnvAsterisk.ASTERISK_DEBUG_SSH_USER, 
					auth
				);
				using var client = new SftpClient(connectionInfo);
				client.Connect();

				string tempFilePath = Path.GetTempFileName();
				FileStream tmpFileStream = File.OpenWrite(tempFilePath);
				Task.Factory.FromAsync(client.BeginDownloadFile($"/vmail/{id}.wav", tmpFileStream), ar => { }).Wait();
				tmpFileStream.Flush();
				tmpFileStream.Close();

				Log.Debug("[{CallId}][{Class}.{Method}()] tmpfile {tempFilePath}",
					ActiveCall?.AGIRequest?.UniqueId,
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name,
					tempFilePath
				);

				client.DeleteFile($"/vmail/{id}.wav");

				PutObjectRequest request = new()
				{
					BucketName = Env.VMAIL_S3_BUCKET,
					Key = s3Key,
					ContentType = "audio/wav",
					FilePath = tempFilePath,
				};

				S3Client.PutObjectAsync(request).Wait();

				File.Delete(tempFilePath);
			}
			else // Local FS (Production)
			{
				string filePath = $"/vmail/{id}.wav";
				PutObjectRequest request = new()
				{
					BucketName = Env.VMAIL_S3_BUCKET,
					Key = s3Key,
					ContentType = "audio/wav",
					FilePath = filePath,
				};
				S3Client.PutObjectAsync(request).Wait();
				File.Delete(filePath);
			}

			// Send Metadata to S3
			VMM.Upsert(metadata);


			// Play a friendly message before hanging up.
			StreamFile(Env.SND_THANK_YOU_GOODBYE);

			CallTarget = kCallTargetHangup;

			Log.Information("[{CallId}][{Class}.{Method}()] End",
				ActiveCall?.AGIRequest?.UniqueId,
				GetType().Name,
				System.Reflection.MethodBase.GetCurrentMethod()?.Name
			);
		}



	}


}
