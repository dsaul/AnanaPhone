using DanSaul.SharedCode.Asterisk.AsteriskINI;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;
using Serilog;

namespace AnanaPhone.Boot
{
	public partial class BootManager : IDisposable
	{
		[ConfGenerator]
		[RuntimeReloadable]
		public void GenerateManagerConf()
		{
			// manager.conf

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
							new Entry()
							{
								Key = "port",
								Value = "5038",
							},
							new Entry()
							{
								Key = "bindaddr",
								Value = "0.0.0.0",
								Comment = "Must be 0.0.0.0 in the docker container.",
							},
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
					new Section()
					{
						Name = "assistant",
						Entries = new Entry[]
						{
							new Entry()
							{
								Key = "secret",
								Value = PasswordForToday,
							},
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
							new Entry()
							{
								Key = "read",
								Value = "all",
							},
							new Entry()
							{
								Key = "write",
								Value = "all",
							},
							new Entry()
							{
								Key = "acl",
								Value = "assistant",
							},
						},
					},
				},
			};

			if (Env.GENERATE_ASTERISK_CONFIG)
			{
				string contents = Factory.Generate(file);
				File.WriteAllText("/etc/asterisk/manager.conf", contents);
			}
		}
	}
}
