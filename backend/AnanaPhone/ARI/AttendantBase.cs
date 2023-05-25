using DanSaul.SharedCode.Asterisk;
using Serilog;

namespace AnanaPhone.ARI
{
	public abstract class AttendantBase : AGIScriptPlus
	{
		public const string kCallTargetHangup = "HangUp";
		public const string kCallTargetJoinConference = "JoinConference";
		public const string kCallTargetOfferConferenceReJoin = "OfferConferenceReJoin";
		public const string kCallTargetVoiceMail = "VoiceMail";
		public const string kCallTargetWaitForConferenceOwnerThenVoiceMail = "WaitForConferenceOwnerThenVoiceMail";
		public const string kCallTargetSeekOwnerOfCallRequest = "SeekOwnerOfCallRequest";

		static readonly IEnumerable<string> AllowedCallTargets = new string[]
		{
			kCallTargetHangup,
			kCallTargetJoinConference,
			kCallTargetOfferConferenceReJoin,
			kCallTargetVoiceMail,
			kCallTargetWaitForConferenceOwnerThenVoiceMail,
			kCallTargetSeekOwnerOfCallRequest,
			kCallTargetVoiceMail
		};



		public string? CallTarget
		{
			get
			{
				string? payload = GetVariable(Env.CALL_TARGET_VAR_NAME);
				if (string.IsNullOrWhiteSpace(payload))
					return null;
				else
					return payload;
			}
			set
			{
				if (!AllowedCallTargets.Contains(value))
				{
					Log.Warning("[{Class}.{Method}()] Invalid Call Target {RequestedTarget}",
						GetType().Name,
						System.Reflection.MethodBase.GetCurrentMethod()?.Name,
						value
					);
					return;
				}

				SetVariable(Env.CALL_TARGET_VAR_NAME, value ?? "");
			}
		}


		public string? FarCallId
		{
			get
			{
				string? payload = GetVariable(Env.FAR_CALL_ID_VAR_NAME);
				if (string.IsNullOrWhiteSpace(payload))
					return null;
				else
					return payload;
			}
			//set
			//{
			//	SetVariable(Env.FAR_CALL_ID_VAR_NAME, value ?? "");
			//}
		}

		public string? LandedConferenceName
		{
			get
			{
				string? payload = GetVariable(Env.LANDED_CONFERENCE_NAME_VAR_NAME);
				if (string.IsNullOrWhiteSpace(payload))
					return null;
				else
					return payload;
			}
			//set
			//{
			//	SetVariable(Env.LANDED_CONFERENCE_NAME_VAR_NAME, value ?? "");
			//}
		}


		public string? ConferenceId
		{
			get
			{
				string? payload = GetVariable(Env.CONFERENCE_ID_VAR_NAME);
				if (string.IsNullOrWhiteSpace(payload))
					return null;
				else
					return payload;
			}
			set
			{
				SetVariable(Env.CONFERENCE_ID_VAR_NAME, value ?? "");
			}
		}


		public string? OriginateExternalToConferenceDestination
		{
			get
			{
				string? payload = GetVariable(Env.ORIGINATE_EXTERNAL_TO_CONFERENCE_DESTINATION_VAR_NAME);
				if (string.IsNullOrWhiteSpace(payload))
					return null;
				else
					return payload;
			}
			set
			{
				SetVariable(Env.ORIGINATE_EXTERNAL_TO_CONFERENCE_DESTINATION_VAR_NAME, value ?? "");
			}
		}

	}
}
