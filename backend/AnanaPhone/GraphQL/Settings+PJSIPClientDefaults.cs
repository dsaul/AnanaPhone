using AnanaPhone.SettingsManager;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;

namespace AnanaPhone.GraphQL
{
	public partial class Settings : GraphController
	{
		[Query(template: "pjsipClientDefaults")]
		public PJSIPEntry ClientDefaults()
		{
			PJSIPEntry ret = new()
			{
				Rows = SM.PJSIPWizardConfGetForName(SettingsManager.Manager.kClientDefaultName).ToList(),
			};

			return ret;
		}
	}
}
