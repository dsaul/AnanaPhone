using AnanaPhone.SettingsManager;
using DanSaul.SharedCode.Asterisk.AsteriskINI;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;
using Serilog;

namespace AnanaPhone.Boot
{
	public partial class BootManager : IDisposable
	{
		[ConfGenerator]
		[RuntimeReloadable]
		public void GeneratePJSIPWizardConf()
		{
			// acl.conf

			Log.Information("[{Class}.{Method}()]",
				GetType().Name,
				System.Reflection.MethodBase.GetCurrentMethod()?.Name
			);


			List<Section> sections = new();

			// Create the templates.
			IEnumerable<string?> templateNames = SM.TemplateNames();

			foreach (string? templateName in templateNames)
			{
				if (string.IsNullOrWhiteSpace(templateName))
					continue;

				IEnumerable<PJSIPWizardRow> dbRows = SM.PJSIPWizardConfGetForName(templateName);
				IEnumerable<Entry> entries = dbRows.Select<PJSIPWizardRow, Entry>((PJSIPWizardRow row) =>
				{
					return new()
					{
						Key = row.Setting,
						Value = row.Value,
						Disabled = row.Disabled ?? false,
						Comment = row.Comment,
						Delimiter = " = ",

					};
				});

				Section section = new()
				{
					Name = templateName,
					Entries = entries,
					IsTemplate = true,
				};
				sections.Add(section);
			}

			// Create the non templates.
			foreach (string? templateName in templateNames)
			{
				if (string.IsNullOrWhiteSpace(templateName))
					continue;

				IEnumerable<string?> regularNames = SM.PJSIPWizardNamesForTemplateName(templateName);

				foreach (string? regularName in regularNames)
				{
					if (string.IsNullOrWhiteSpace(regularName))
						continue;

					IEnumerable<PJSIPWizardRow> dbRows = SM.PJSIPWizardConfGetForName(regularName);
					IEnumerable<Entry> entries = dbRows.Select<PJSIPWizardRow, Entry>((PJSIPWizardRow row) =>
					{
						return new()
						{
							Key = row.Setting,
							Value = row.Value,
							Disabled = row.Disabled ?? false,
							Comment = row.Comment,
							Delimiter = " = ",

						};
					});

					Section section = new()
					{
						Name = regularName,
						Entries = entries,
						UsesTemplate = templateName,
						IsTemplate = false,
					};
					sections.Add(section);
				}



				

			}









			AsteriskINIFile file = new()
			{
				Sections = sections,
			};

			if (EnvAsterisk.ASTERISK_DEBUG_SSH_ENABLE)
			{
				Log.Information("[{Class}.{Method}()] Running remotely, skipping writing conf file.",
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name
				);
			}
			else
			{
				string contents = Factory.Generate(file);
				File.WriteAllText("/etc/asterisk/pjsip_wizard.conf", contents);
			}
		}
	}
}
