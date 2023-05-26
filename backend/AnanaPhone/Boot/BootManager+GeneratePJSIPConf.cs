using DanSaul.SharedCode.Asterisk.AsteriskINI;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;
using Serilog;

namespace AnanaPhone.Boot
{
	public partial class BootManager : IDisposable
	{
		[ConfGenerator]
		public void GeneratePJSIPConf()
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
						Name = "global",
						Entries = new Entry[]
						{
							new Entry()
							{
								Key = "endpoint_identifier_order",
								Value = "auth_username,username,ip,anonymous",
							},
						},
					},
					new Section()
					{
						Name = "transport-udp",
						Entries = new Entry[]
						{
							new Entry()
							{
								Key = "type",
								Value = "transport",
							},
							new Entry()
							{
								Key = "protocol",
								Value = "udp",
							},
							new Entry()
							{
								Key = "bind",
								Value = "0.0.0.0:5060",
							},
							new Entry()
							{
								Key = "external_media_address",
								Value = EnvAsterisk.ASTERISK_EXTERNAL_IP_ADDRESS,
							},
							new Entry()
							{
								Key = "external_signaling_address",
								Value = EnvAsterisk.ASTERISK_EXTERNAL_IP_ADDRESS,
							},
							new Entry()
							{
								Key = "allow_reload",
								Value = "no",
							},
							new Entry()
							{
								Key = "tos",
								Value = "cs3",
							},
							new Entry()
							{
								Key = "cos",
								Value = "4",
							},
						},
					},
					new Section()
					{
						Name = "transport-tcp",
						Entries = new Entry[]
						{
							new Entry()
							{
								Key = "type",
								Value = "transport",
							},
							new Entry()
							{
								Key = "protocol",
								Value = "tcp",
							},
							new Entry()
							{
								Key = "bind",
								Value = "0.0.0.0:5060",
							},
							new Entry()
							{
								Key = "external_media_address",
								Value = EnvAsterisk.ASTERISK_EXTERNAL_IP_ADDRESS,
							},
							new Entry()
							{
								Key = "external_signaling_address",
								Value = EnvAsterisk.ASTERISK_EXTERNAL_IP_ADDRESS,
							},
							new Entry()
							{
								Key = "allow_reload",
								Value = "no",
							},
							new Entry()
							{
								Key = "tos",
								Value = "cs3",
							},
							new Entry()
							{
								Key = "cos",
								Value = "4",
							},
						},
					}
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
				File.WriteAllText("/etc/asterisk/pjsip.conf", contents);
			}
		}
	}
}
