using AnanaPhone.Calls;
using AnanaPhone.VoiceMail;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using Serilog;

namespace AnanaPhone.GraphQL
{
	public partial class VoiceMail : GraphController
	{
		[Query("messages")]
		public IEnumerable<VoiceMailMessageRow> GetMessages(IEnumerable<string>? ids = null)
		{
			ids ??= Array.Empty<string>();

			if (!ids.Any())
			{
				return VMM.GetAll();
			}

			var e = from message in VMM.GetAll()
					where ids.Contains(message.Id)
					select message;

			return e;
		}

		[Mutation("message/remove")]
		public GenericReturn E164Remove(string id)
		{
			string status;

			try
			{
				VMM.Remove(id);


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
				throw;
			}

		done:
			return new GenericReturn()
			{
				Status = status,
			};
		}
	}
}
