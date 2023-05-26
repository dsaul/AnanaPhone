using DanSaul.SharedCode.Asterisk.AsteriskINI;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;
using Serilog;

namespace AnanaPhone.Boot
{
	public partial class BootManager : IDisposable
	{
		[ConfGenerator]
		public void GenerateAsteriskConf()
		{
			// asterisk.conf

			Log.Information("[{Class}.{Method}()]",
				GetType().Name,
				System.Reflection.MethodBase.GetCurrentMethod()?.Name
			);

			AsteriskINIFile file = new()
			{
				Sections = new Section[]
				{
					new Section()
					{
						Name = "directories",
						IsTemplate = true,
						Entries = new Entry[]
						{
							new Entry()
							{
								Key = "astcachedir",
								Value = "/var/cache/asterisk",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "astetcdir",
								Value = "/etc/asterisk",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "astmoddir",
								Value = "/usr/lib/x86_64-linux-gnu/asterisk/modules",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "astvarlibdir",
								Value = "/var/lib/asterisk",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "astdbdir",
								Value = "/var/lib/asterisk",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "astkeydir",
								Value = "/var/lib/asterisk",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "astdatadir",
								Value = "/usr/share/asterisk",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "astagidir",
								Value = "/usr/share/asterisk/agi-bin",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "astspooldir",
								Value = "/var/spool/asterisk",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "astrundir",
								Value = "/var/run/asterisk",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "astlogdir",
								Value = "/var/log/asterisk",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "astsbindir",
								Value = "/usr/sbin",
								Delimiter = " => "
							},
						},
					},
					new Section()
					{
						Name = "options",
						Entries = new Entry[]
						{
							new Entry()
							{
								Key = "verbose",
								Value = "5",
							},
							new Entry()
							{
								Key = "dumpcore",
								Value = "yes",
							},
							new Entry()
							{
								Key = "systemname",
								Value = "AnanaPhone",
							},
							new Entry()
							{
								Key = "maxcalls",
								Value = "10",
							},
							new Entry()
							{
								Key = "maxload",
								Value = "0.9",
							},
							new Entry()
							{
								Key = "minmemfree",
								Value = "1",
							},
							new Entry()
							{
								Key = "defaultlanguage",
								Value = "en",
							},
							new Entry()
							{
								Key = "documentation_language",
								Value = "en_US",
							},
						},
					},
				},
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
				File.WriteAllText("/etc/asterisk/asterisk.conf", contents);
			}
		}
	}
}
