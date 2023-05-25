using AnanaPhone.SettingsManager;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using Serilog;

namespace AnanaPhone.GraphQL
{
	public partial class Settings : GraphController
	{
		[Query("e164Clients")]
		public IEnumerable<E164ClientRow> E164Clients()
		{
			var e164s = SM.E164ClientsGetAll();

			return e164s;


		}

		[Mutation("e164Client/upsert")]
		public GenericReturn E164ClientUpsert(E164ClientRow e164)
		{
			string status;

			try
			{
				SM.E164ClientUpsert(e164);

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

		[Mutation("e164Client/remove")]
		public GenericReturn E164ClientRemove(string e164)
		{
			string status;

			try
			{
				SM.E164ClientRemove(e164);

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
