using DanSaul.SharedCode.Asterisk.AsteriskINI;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;
using Serilog;

namespace AnanaPhone.Boot
{
	public partial class BootManager : IDisposable
	{
		[ConfGenerator]
		[RuntimeReloadable]
		public void GenerateMusicOnHoldConf()
		{
			// acl.conf

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
						Name = "general",
					},
					new Section()
					{
						Name = "default",
						Entries = new Entry[]
						{
							new Entry()
							{
								Key = "mode",
								Value = "files",
							},
							new Entry()
							{
								Key = "directory",
								Value = "/holdmusic",
							},
							new Entry()
							{
								Key = "random",
								Value = "yes",
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
				File.WriteAllText("/etc/asterisk/musiconhold.conf", contents);
			}
		}
	}
}
