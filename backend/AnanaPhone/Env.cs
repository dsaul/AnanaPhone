using DanSaul.SharedCode.StandardizedEnvironmentVariables;
using System.IO;

namespace AnanaPhone
{
	public static class Env
	{

		public static string VITE_API_ROOT
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("VITE_API_ROOT");
				if (string.IsNullOrWhiteSpace(payload))
					return "/";
				return payload;
			}
		}

		public static bool GENERATE_ASTERISK_CONFIG
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("GENERATE_ASTERISK_CONFIG");
				if (string.IsNullOrWhiteSpace(payload))
					return true;
				if (bool.TryParse(payload, out bool _payload))
					return _payload;
				return true;
			}

		}

		static string? _AMI_PW;
		public static string AMI_PW
		{
			get
			{
				if (EnvAsterisk.ASTERISK_DEBUG_SSH_ENABLE)
				{
					string? payload = Environment.GetEnvironmentVariable("AMI_PW");
					if (string.IsNullOrWhiteSpace(payload))
						throw new InvalidOperationException("AMI_PW empty or missing.");
					return payload;
				}
				else
				{
					if (!string.IsNullOrWhiteSpace(_AMI_PW))
						return _AMI_PW;

					string path = "/tmp/AMI_PW";
					if (!File.Exists(path))
						throw new InvalidOperationException("AMI_PW empty or missing.");
					_AMI_PW = File.ReadAllText(path);
					File.Delete(path);
					return _AMI_PW;
				}
			}
		}

		public static string SND_EXTERNAL_INTRO
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("SND_EXTERNAL_INTRO");
				if (string.IsNullOrWhiteSpace(payload))
					return "/etc/asterisk/_sounds/hi-there-im-the-robot-assistant-managing-this-line-please-wait-while-i-try-to-connect-you";
				return payload;
			}
		}

		public static string SND_CALL_DROPPED_SHOULD_WE_WAIT
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("SND_EXTERNAL_INTRO");
				if (string.IsNullOrWhiteSpace(payload))
					return "/etc/asterisk/_sounds/call-dropped-should-we-wait";
				return payload;
			}
		}

		public static string SND_INCOMING_CALL_FROM
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("SND_INCOMING_CALL_FROM");
				if (string.IsNullOrWhiteSpace(payload))
					return "/etc/asterisk/_sounds/incoming-call-from";
				return payload;
			}
		}

		public static string SND_PRESS_1_TO_ACCEPT_THIS_CALL
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("SND_PRESS_1_TO_ACCEPT_THIS_CALL");
				if (string.IsNullOrWhiteSpace(payload))
					return "/etc/asterisk/_sounds/press-1-to-accept-this-call";
				return payload;
			}
		}

		public static string SND_UNABLE_TO_REACH_LEAVE_VM
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("SND_UNABLE_TO_REACH_LEAVE_VM");
				if (string.IsNullOrWhiteSpace(payload))
					return "/etc/asterisk/_sounds/unable-to-reach-leave-vm";
				return payload;
			}
		}
		public static string SND_THANK_YOU_GOODBYE
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("SND_THANK_YOU_GOODBYE");
				if (string.IsNullOrWhiteSpace(payload))
					return "/etc/asterisk/_sounds/thank-you-goodbye";
				return payload;
			}
		}

		public static string SND_INCOMING_ALREADY_HUNG_UP
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("SND_INCOMING_ALREADY_HUNG_UP");
				if (string.IsNullOrWhiteSpace(payload))
					return "/etc/asterisk/_sounds/were-sorry-the-incoming-call-has-already-hung-up-goodbye";
				return payload;
			}
		}

		public static string CONF_ROOM_NAME_PREFIX
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("CONF_ROOM_NAME_PREFIX");
				if (string.IsNullOrWhiteSpace(payload))
					return "room";
				return payload;
			}
		}

		public static string CONF_CONTEXT_ADMIN
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("CONF_CONTEXT_ADMIN");
				if (string.IsNullOrWhiteSpace(payload))
					return "confbridge-admin";
				return payload;
			}
		}

		public static string CONF_CONTEXT_EXTERNAL
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("CONF_CONTEXT_EXTERNAL");
				if (string.IsNullOrWhiteSpace(payload))
					return "confbridge-external";
				return payload;
			}
		}
		public static string CALL_TARGET_VAR_NAME
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("CALL_TARGET_VAR_NAME");
				if (string.IsNullOrWhiteSpace(payload))
					return "CALL_TARGET";
				return payload;
			}
		}
		public static string LANDED_DID_VAR_NAME
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("LANDED_DID_VAR_NAME");
				if (string.IsNullOrWhiteSpace(payload))
					return "LANDED_DID";
				return payload;
			}
		}
		public static string LANDED_CONFERENCE_NAME_VAR_NAME
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("LANDED_CONFERENCE_NAME_VAR_NAME");
				if (string.IsNullOrWhiteSpace(payload))
					return "LANDED_CONFERENCE_NAME";
				return payload;
			}
		}
		public static string CONFERENCE_ID_VAR_NAME
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("CONFERENCE_ID_VAR_NAME");
				if (string.IsNullOrWhiteSpace(payload))
					return "CONFERENCE_ID_VAR_NAME";
				return payload;
			}
		}

		public static string ORIGINATE_EXTERNAL_TO_CONFERENCE_DESTINATION_VAR_NAME
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("ORIGINATE_EXTERNAL_TO_CONFERENCE_DESTINATION_VAR_NAME");
				if (string.IsNullOrWhiteSpace(payload))
					return "ORIGINATE_EXTERNAL_TO_CONFERENCE_DESTINATION";
				return payload;
			}
		}


		public static string FAR_CALL_ID_VAR_NAME
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("FAR_CALL_ID_VAR_NAME");
				if (string.IsNullOrWhiteSpace(payload))
					return "FAR_CALL_ID";
				return payload;
			}
		}
		public static string SEEK_OWNER_ACCEPT_CALL_LAND_CONTEXT
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("ASK_OWNER_ACCEPT_CALL_LAND_CONTEXT");
				if (string.IsNullOrWhiteSpace(payload))
					return "attendant-do-you-accept-the-call";
				return payload;
			}
		}
		public static string SEEK_OWNER_ACCEPT_CALL_LAND_EXTEN
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("ASK_OWNER_ACCEPT_CALL_LAND_EXTEN");
				if (string.IsNullOrWhiteSpace(payload))
					return "1234";
				return payload;
			}
		}
		public static string CONTEXT_ADMIN_DIRECT_TO_CONFERENCE
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("CONTEXT_ADMIN_DIRECT_TO_CONFERENCE");
				if (string.IsNullOrWhiteSpace(payload))
					return "attendant-tracked-admin-direct-to-conference";
				return payload;
			}
		}


		public static string CONTEXT_EXTERNAL_DIRECT_TO_CONFERENCE
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("CONTEXT_EXTERNAL_DIRECT_TO_CONFERENCE");
				if (string.IsNullOrWhiteSpace(payload))
					return "attendant-tracked-external-direct-to-conference";
				return payload;
			}
		}



		public static string VMAIL_S3_BUCKET
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("VMAIL_S3_BUCKET");
				if (string.IsNullOrWhiteSpace(payload))
					throw new InvalidOperationException("VMAIL_S3_BUCKET empty or missing.");
				return payload;
			}
		}


		public static int VMAIL_MAX_LENGTH
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("VMAIL_MAX_LENGTH");
				if (string.IsNullOrWhiteSpace(payload))
					return 1000 * 60 * 5;
				if (int.TryParse(payload, out int _payload))
					return _payload;
				return 1000 * 60 * 5;
			}
		}

		public static bool VMAIL_BEEP
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("VMAIL_BEEP");
				if (string.IsNullOrWhiteSpace(payload))
					return true;
				if (bool.TryParse(payload, out bool _payload))
					return _payload;
				return true;
			}

		}

		public static int VMAIL_MAX_SILENCE
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("VMAIL_MAX_SILENCE");
				if (string.IsNullOrWhiteSpace(payload))
					return 5;
				if (int.TryParse(payload, out int _payload))
					return _payload;
				return 5;
			}
		}

		public static int CALL_ITERATE_SLEEP_MS
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("CALL_ITERATE_SLEEP_MS");
				if (string.IsNullOrWhiteSpace(payload))
					return 250;
				if (int.TryParse(payload, out int _payload))
					return _payload;
				return 250;
			}
		}

		public static int CALL_FOLLOW_ME_TIMEOUT_SECTIONS
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("CALL_FOLLOW_ME_TIMEOUT_SECTIONS");
				if (string.IsNullOrWhiteSpace(payload))
					return 60;
				if (int.TryParse(payload, out int _payload))
					return _payload;
				return 60;
			}
		}

		public static int CALL_AUTO_HANGUP_TIMEOUT_WAITING_TO_CONNECT_SOMEONE
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("CALL_AUTO_HANGUP_TIMEOUT_WAITING_TO_CONNECT_SOMEONE");
				if (string.IsNullOrWhiteSpace(payload))
					return 60 * 10;
				if (int.TryParse(payload, out int _payload))
					return _payload;
				return 60 * 10;
			}
		}

		public static int CALL_AUTO_HANGUP_TIMEOUT_NORMAL_CALL
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("CALL_AUTO_HANGUP_TIMEOUT_NORMAL_CALL");
				if (string.IsNullOrWhiteSpace(payload))
					return 60 * 60 * 6;
				if (int.TryParse(payload, out int _payload))
					return _payload;
				return 60 * 60 * 6;
			}
		}


		public static string OWNER_OUTBOUND_LAND_CONTEXT
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("OWNER_OUTBOUND_LAND_CONTEXT");
				if (string.IsNullOrWhiteSpace(payload))
					return "ananaphone-outbound-owner";
				return payload;
			}
		}










		public static string? SQLITE_DATABASE_DIRECTORY
		{
			get
			{
				string? payload = Environment.GetEnvironmentVariable("SQLITE_DATABASE_DIRECTORY");
				if (string.IsNullOrWhiteSpace(payload))
				{
					throw new InvalidOperationException("SQLITE_DATABASE_DIRECTORY empty or missing.");
				}
				return payload;
			}
		}



	}
}
