using Serilog;
using System.Data.SQLite;
using AnanaPhone.Extensions;
using System.Data;

namespace AnanaPhone.SettingsManager
{
	public partial class Manager : IDisposable
	{
		void PJSIPConfEnsureTable()
		{
			if (DB == null)
				throw new Exception("DB == null");

			if (false == DB.TableExists("pjsip.conf"))
			{
				string sql = @"
					CREATE TABLE ""pjsip.conf"" (
						id INTEGER PRIMARY KEY AUTOINCREMENT,
						""section"" TEXT,
						setting TEXT,
						value TEXT,
						comment TEXT,
						disabled INTEGER DEFAULT (0)
					);
					CREATE INDEX pjsip_conf_section_IDX ON ""pjsip.conf"" (""section"");
					CREATE INDEX pjsip_conf_setting_IDX ON ""pjsip.conf"" (setting);
					INSERT INTO ""pjsip.conf"" (""section"",setting,value,comment,disabled) VALUES
						 ('global','endpoint_identifier_order','auth_username,username,ip,anonymous',NULL,0),
						 ('transport-udp','type','transport',NULL,0),
						 ('transport-udp','protocol','udp',NULL,0),
						 ('transport-udp','bind','0.0.0.0:{{SIP_UDP_PORT}}',NULL,0),
						 ('transport-udp','external_media_address','{{SIP_EXTERNAL_IP}}',NULL,0),
						 ('transport-udp','external_signaling_address','{{SIP_EXTERNAL_IP}}',NULL,0),
						 ('transport-udp','allow_reload','yes',NULL,0),
						 ('transport-udp','tos','cs3',NULL,0),
						 ('transport-udp','cos','3',NULL,0),
						 ('transport-tcp','type','transport',NULL,0);
					INSERT INTO ""pjsip.conf"" (""section"",setting,value,comment,disabled) VALUES
						 ('transport-tcp','protocol','tcp',NULL,0),
						 ('transport-tcp','bind','0.0.0.0:{{SIP_TCP_PORT}}',NULL,0),
						 ('transport-tcp','external_media_address','{{SIP_EXTERNAL_IP}}',NULL,0),
						 ('transport-tcp','external_signaling_address','{{SIP_EXTERNAL_IP}}',NULL,0),
						 ('transport-tcp','allow_reload','yes',NULL,0),
						 ('transport-tcp','tos','cs3',NULL,0),
						 ('transport-tcp','cos','3',NULL,0);
				";

				using SQLiteCommand command = DB.CreateCommand();
				command.CommandText = sql;
				var name = command.ExecuteNonQuery();

				Log.Information("[{Class}.{Method}()] Created DB Table version {Version}",
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name,
					databaseFormatVersion);
			}
		}
	}
}
