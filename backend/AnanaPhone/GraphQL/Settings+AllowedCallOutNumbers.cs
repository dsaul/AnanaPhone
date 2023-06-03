using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;

namespace AnanaPhone.GraphQL
{
	public partial class Settings : GraphController
	{
		[Query("allowedCallOutNumbers")]
		public IEnumerable<string> GetAllowedCallOutNumbers()
		{
			return SM.GetClientChannels();
		}
	}
}
