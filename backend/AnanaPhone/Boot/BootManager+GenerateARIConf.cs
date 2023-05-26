using DanSaul.SharedCode.Asterisk.AsteriskINI;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;
using Serilog;

namespace AnanaPhone.Boot
{
	public partial class BootManager : IDisposable
	{
		[ConfGenerator]
		public void GenerateARIConf()
		{
			// ari.conf

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
						Entries = new Entry[]
						{
							new Entry()
							{
								Key = "enabled",
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
				File.WriteAllText("/etc/asterisk/ari.conf", contents);
			}

		}
	}
}
