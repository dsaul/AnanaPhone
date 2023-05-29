using DanSaul.SharedCode.Asterisk.AsteriskINI;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;
using Serilog;

namespace AnanaPhone.Boot
{
	public partial class BootManager : IDisposable
	{
		[ConfGenerator]
		[RuntimeReloadable]
		public void GenerateACLConf()
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
						Name = "assistant",
						Entries = new Entry[]
						{
							new Entry()
							{
								Key = "deny",
								Value = "0.0.0.0/0",
							},
							new Entry()
							{
								Key = "permit",
								Value = "10.0.0.0/8",
							},
							new Entry()
							{
								Key = "permit",
								Value = "172.16.0.0/12",
							},
							new Entry()
							{
								Key = "permit",
								Value = "192.168.0.0/24",
							},
							new Entry()
							{
								Key = "permit",
								Value = "127.0.0.0/8",
							},
						},
					},
				},
			};

			
			if (Env.GENERATE_ASTERISK_CONFIG)
			{
				string contents = Factory.Generate(file);
				File.WriteAllText("/etc/asterisk/acl.conf", contents);
			}
		}
	}
}
