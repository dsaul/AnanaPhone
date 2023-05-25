using AnanaPhone.SettingsManager;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;

namespace AnanaPhone.GraphQL
{
	public partial class Settings : GraphController
	{
		[Query(template: "pjsipTrunkDefaults")]
		public PJSIPEntry TrunkDefaults()
		{
			PJSIPEntry ret = new()
			{
				Rows = SM.PJSIPWizardConfGetForName(SettingsManager.Manager.kTrunkDefaultName).ToList(),
			};

			return ret;
		}
	}
}
