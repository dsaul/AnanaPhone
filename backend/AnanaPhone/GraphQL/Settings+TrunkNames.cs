using AnanaPhone.SettingsManager;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;

namespace AnanaPhone.GraphQL
{
	public partial class Settings : GraphController
	{
		[Query(template: "trunkNames")]
		public IEnumerable<string?> TrunkNames()
		{
			return SM.PJSIPWizardConfGetNamesForTrunkDefaults();
		}
		
	}
}
