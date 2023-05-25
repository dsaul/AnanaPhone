using AnanaPhone.Calls;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using Serilog;

namespace AnanaPhone.GraphQL
{
	public partial class Calls : GraphController
	{
		[Query("historic")]
		public IEnumerable<HistoricCall> HistoricCallGet(IEnumerable<string>? ids = null)
		{

			IEnumerable<HistoricCall> ret = CHM.All();
			if (ids != null && ids.Any())
				ret = ret.Where(call => ids.Any(id => id == call.Id));

			return ret;
		}

		[Mutation("historic/remove")]
		public GenericReturn HistoricCallRemove(string id)
		{
			string status;

			try
			{
				CHM.Remove(id);

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

		[Mutation("historic/removeAll")]
		public GenericReturn HistoricCallRemoveAll(string payload)
		{
			string status;

			try
			{
				CHM.RemoveAll();

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
