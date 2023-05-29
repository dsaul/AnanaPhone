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

			
			if (Env.GENERATE_ASTERISK_CONFIG)
			{
				string contents = Factory.Generate(file);
				File.WriteAllText("/etc/asterisk/musiconhold.conf", contents);
			}
		}
	}
}
