using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;

namespace AnanaPhone.GraphQL
{
	public partial class Settings : GraphController
	{
		[Query(template: "clientNames")]
		public IEnumerable<string?> ClientNames()
		{
			return SM.PJSIPWizardConfGetNamesForClientDefaults();
		}
	}
}
