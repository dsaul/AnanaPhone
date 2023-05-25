using AnanaPhone.SettingsManager;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using Serilog;

namespace AnanaPhone.GraphQL
{
	public partial class Settings : GraphController
	{
		[Query("e164s")]
		public IEnumerable<E164Row> E164s()
		{
			var e164s = SM.E164sGetAll();

			return e164s;


		}

		[Mutation("e164/upsert")]
		public GenericReturn E164Upsert(E164Row e164)
		{
			string status;

			try
			{
				SM.E164sUpsert(e164);


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

		[Mutation("e164/remove")]
		public GenericReturn E164Remove(string e164)
		{
			string status;

			try
			{
				SM.E164sRemove(e164);


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
