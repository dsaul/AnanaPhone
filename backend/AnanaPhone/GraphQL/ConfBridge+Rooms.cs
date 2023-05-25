using AnanaPhone.Conferences;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using Serilog;

namespace AnanaPhone.GraphQL
{
	public partial class ConfBridge : GraphController
	{
		[Query("rooms")]
		public IEnumerable<Conference> GetRooms(IEnumerable<string>? names = null)
		{
			names ??= Array.Empty<string>();

			if (!names.Any())
			{
				return CBM.Conferences;
			}

			var e = from conf in CBM.Conferences
					where names.Contains(conf.Name)
					select conf;

			return e;
		}

		[MutationRoot("joinConfBridge")]
		public GenericReturn JoinConfbridgeOwner(string name, string channel)
		{
			string status = "unknown";

			try
			{

				if (AMI != null && false == AMI.IsConnected)
					AMI.Connect();
				if (AMI == null)
					throw new InvalidOperationException("AMI == null");
				if (CBM == null)
					throw new InvalidOperationException("CBM == null");

				Conference? conference = CBM.ForName(name);
				if (conference == null)
				{
					status = "room-missing";
					goto done;
				}

				AMI.Originate(
					channel,
					name,
					Env.CONTEXT_ADMIN_DIRECT_TO_CONFERENCE,
					$"Owner {channel}"
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
