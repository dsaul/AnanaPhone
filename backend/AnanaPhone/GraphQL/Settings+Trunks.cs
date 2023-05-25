using AnanaPhone.SettingsManager;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;

namespace AnanaPhone.GraphQL
{
	public partial class Settings : GraphController
	{
		[Query(template: "trunks")]
		public IEnumerable<PJSIPEntry> Trunks()
		{
			IEnumerable<string?> names = SM.PJSIPWizardConfGetNamesForTrunkDefaults();

			foreach (string? name in names)
			{
				if (string.IsNullOrWhiteSpace(name))
					continue;

				IEnumerable<PJSIPWizardRow> rows = SM.PJSIPWizardConfGetForName(name);
				yield return new PJSIPEntry()
				{
					Rows = rows.ToList()
				};
			}

			yield break;
		}
	}
}
