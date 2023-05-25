using AnanaPhone.Calls;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using NodaTime;
using Serilog;

namespace AnanaPhone.GraphQL
{
	public partial class Calls : GraphController
	{
		[Query("active")]
		public IEnumerable<ActiveCall> GetActiveCalls(IEnumerable<string>? ids = null)
		{
			ids ??= Array.Empty<string>();

			if (!ids.Any())
			{
				return ACM.ActiveCalls;
			}

			var e = from call in ACM.ActiveCalls
					where ids.Contains(call.Id)
					select call;

			return e;
		}

		[MutationRoot(template: "hangupChannel")]
		public GenericReturn HangupChannel(string channel)
		{
			string status = "unknown";

			try
			{

				if (AMI != null && false == AMI.IsConnected)
					AMI.Connect();
				if (AMI == null)
					throw new InvalidOperationException("AMI == null");

				AMI.Hangup(channel);

				status = "success";
			}
			catch (UnauthorizedAccessException e)
			{
				status = e.Message;
				goto done;
			}
			catch (Exception e)
			{
				Log.Error(e, "[{Class}.{Method}()] {Message}",
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name,
					e.Message
				);
			}

		done:
			return new GenericReturn()
			{
				Status = status,
			};
		}

		[MutationRoot(template: "performOwnerOutboundCall")]
		public GenericReturn PerformOwnerOutboundCall(string callDevice, string destination)
		{
			string status = "unknown";

			try
			{

				if (AMI != null && false == AMI.IsConnected)
					AMI.Connect();
				if (AMI == null)
					throw new InvalidOperationException("AMI == null");

				//AMI.Hangup(channel);

				//Dial(PJSIP/${EXTEN}@trunk-ob-twilio,40);

				Log.Information("[{Class}.{Method}()] PerformOwnerOutboundCall channel:{channel} destination:{destination} ctx:{ctx}",
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name,
					callDevice,
					destination,
					Env.OWNER_OUTBOUND_LAND_CONTEXT
				);

				Instant instantNow = SystemClock.Instance.GetCurrentInstant();
				string confRoomName = $"{Env.CONF_ROOM_NAME_PREFIX}{instantNow.ToUnixTimeTicks()}";



				List<KeyValuePair<string, string>> vars = new();
				if (!string.IsNullOrEmpty(confRoomName))
					vars.Add(new KeyValuePair<string, string>(Env.CONFERENCE_ID_VAR_NAME, confRoomName));
				if (!string.IsNullOrEmpty(destination))
					vars.Add(new KeyValuePair<string, string>(Env.ORIGINATE_EXTERNAL_TO_CONFERENCE_DESTINATION_VAR_NAME, destination));

				AMI.Originate(
					callDevice,
					confRoomName,
					Env.CONTEXT_ADMIN_DIRECT_TO_CONFERENCE,
					$"PBX To {destination}",
					vars
				);




				status = "success";
			}
			catch (UnauthorizedAccessException e)
			{
				status = e.Message;
				goto done;
			}
			catch (Exception e)
			{
				Log.Error(e, "[{Class}.{Method}()] {Message}",
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name,
					e.Message
				);
			}

		done:
			return new GenericReturn()
			{
				Status = status,
			};
		}
	}
}
