using DanSaul.SharedCode.Asterisk.AsteriskINI;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;
using Serilog;

namespace AnanaPhone.Boot
{
	public partial class BootManager : IDisposable
	{
		[ConfGenerator]
		[RuntimeReloadable]
		public void GenerateModulesConf()
		{
			// modules.conf

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
						Name = "modules",
						Entries = new Entry[]
						{
							new Entry()
							{
								Key = "autoload",
								Value = "yes",
							},
							new Entry()
							{
								Key = "load",
								Value = "pbx_ael.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "pbx_gtkconsole.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "pbx_kdeconsole.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "app_intercom.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "chan_modem.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "chan_modem_aopen.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "chan_modem_bestdata.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "chan_modem_i4l.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "chan_capi.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "load",
								Value = "res_musiconhold.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "chan_alsa.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "chan_console.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "chan_oss.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "cdr_sqlite.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "app_directory_odbc.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "res_config_odbc.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "res_config_pgsql.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "res_config_ldap.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "res_config_curl.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "res_config_sqlite3.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "res_statsd.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "res_geolocation.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "res_resolver_unbound.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "res_smdi.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "res_fax.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "res_parking.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "cel_custom.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "cdr_custom.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "app_voicemail.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "res_http_post.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "app_minivm.so",
								Delimiter = " => "
							},





							new Entry()
							{
								Key = "noload",
								Value = "app_adsiprog.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "app_agent_pool.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "app_alarmreceiver.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "app_amd.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "app_dictate.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "app_directory.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "app_disa.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "app_externalivr.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "app_festival.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "app_followme.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "app_ices.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "app_image.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "app_jack.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "app_milliwatt.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "app_morsecode.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "app_mp3.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "app_nbscat.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "app_page.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "app_privacy.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "app_queue.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "app_sayunixtime.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "app_url.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "app_zapateller.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "chan_iax2.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "chan_mgcp.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "chan_motif.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "chan_sip.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "chan_skinny.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "chan_unistim.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "pbx_dundi.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "pbx_lua.so",
								Delimiter = " => "
							},
							new Entry()
							{
								Key = "noload",
								Value = "res_xmpp.so",
								Delimiter = " => "
							},
							



							
							// Do not disable these below:
							//new Entry()
							//{
							//	Key = "noload",
							//	Value = "cdr",
							//	Delimiter = " => "
							//},
							//new Entry()
							//{
							//	Key = "noload",
							//	Value = "cel",
							//	Delimiter = " => "
							//},
							
						},
					},
					new Section()
					{
						Name = "global",
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
				File.WriteAllText("/etc/asterisk/modules.conf", contents);
			}
		}
	}
}
