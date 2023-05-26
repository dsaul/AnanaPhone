using AnanaPhone.Calls;
using AnanaPhone.SettingsManager;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using Serilog;

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
