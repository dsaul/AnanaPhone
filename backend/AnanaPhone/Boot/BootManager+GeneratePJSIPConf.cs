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
								Value = $"0.0.0.0:{EnvAsterisk.ASTERISK_SIP_PORT}",
							},
							new Entry()
							{
								Key = "external_media_address",
								Value = NotStun?.CurrentWANIP?.ToString(),
							},
							new Entry()
							{
								Key = "external_signaling_address",
								Value = NotStun?.CurrentWANIP?.ToString(),
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
								Value = $"0.0.0.0:{EnvAsterisk.ASTERISK_SIP_PORT}",
							},
							new Entry()
							{
								Key = "external_media_address",
								Value = NotStun?.CurrentWANIP?.ToString(),
							},
							new Entry()
							{
								Key = "external_signaling_address",
								Value = NotStun?.CurrentWANIP?.ToString(),
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

			
			if (Env.GENERATE_ASTERISK_CONFIG)
			{
				string contents = Factory.Generate(file);
				File.WriteAllText("/etc/asterisk/pjsip.conf", contents);
			}
		}
	}
}
