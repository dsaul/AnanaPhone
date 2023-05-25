using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;

namespace AnanaPhone.GraphQL
{
	public partial class Settings : GraphController
	{
		[Query("allowedCallOutNumbers")]
		public IEnumerable<string> GetAllowedCallOutNumbers()
		{
			return Env.ALLOWED_CALL_OUT_E164S;
		}
	}
}
