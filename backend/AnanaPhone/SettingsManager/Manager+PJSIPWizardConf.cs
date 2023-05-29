using Serilog;
using System.Data.SQLite;
using AnanaPhone.Extensions;
using System.Data;
using MongoDB.Driver;
using PasswordGenerator;

namespace AnanaPhone.SettingsManager
{
	public partial class Manager : IDisposable
	{
		public const string kClientDefaultName = "client_defaults";
		public const string kTrunkDefaultName = "trunk_defaults";


		void PJSIPWizardConfEnsureTable()
		{
			if (DB == null)
				throw new Exception("DB == null");

			if (false == DB.TableExists("pjsip_wizard.conf"))
			{
				string sql = @"
					CREATE TABLE ""pjsip_wizard.conf"" (
						id INTEGER PRIMARY KEY AUTOINCREMENT,
						name TEXT,
						setting TEXT,
						value TEXT,
						template INTEGER DEFAULT (0), 
						uses_template TEXT,
						comment TEXT,
						disabled INTEGER DEFAULT (0)
					);
					CREATE INDEX pjsip_wizard_conf_name_IDX ON ""pjsip_wizard.conf"" (name);
					CREATE INDEX pjsip_wizard_conf_setting_IDX ON ""pjsip_wizard.conf"" (setting);
					INSERT INTO ""pjsip_wizard.conf"" (name,setting,value,template,comment,disabled,uses_template) VALUES
						 ('trunk_defaults','type','wizard',1,NULL,0,NULL),
						 ('trunk_defaults','endpoint/allow','!all,ulaw',1,NULL,0,NULL),
						 ('trunk_defaults','endpoint/t38_udptl','no',1,NULL,0,NULL),
						 ('trunk_defaults','endpoint/t38_udptl_ec','none',1,NULL,0,NULL),
						 ('trunk_defaults','endpoint/fax_detect','no',1,NULL,0,NULL),
						 ('trunk_defaults','endpoint/trust_id_inbound','no',1,NULL,0,NULL),
						 ('trunk_defaults','endpoint/t38_udptl_nat','no',1,NULL,0,NULL),
						 ('trunk_defaults','endpoint/direct_media','no',1,NULL,0,NULL),
						 ('trunk_defaults','endpoint/rewrite_contact','yes',1,NULL,0,NULL),
						 ('trunk_defaults','endpoint/rtp_symmetric','yes',1,NULL,0,NULL);
					INSERT INTO ""pjsip_wizard.conf"" (name,setting,value,template,comment,disabled,uses_template) VALUES
						 ('trunk_defaults','endpoint/dtmf_mode','rfc4733',1,NULL,0,NULL),
						 ('trunk_defaults','endpoint/allow_subscribe','no',1,NULL,0,NULL),
						 ('trunk_defaults','endpoint/transport','transport-udp',1,NULL,0,NULL),
						 ('trunk_defaults','aor/qualify_frequency','15',1,NULL,0,NULL),
						 ('client_defaults','type','wizard',1,NULL,0,NULL),
						 ('client_defaults','accepts_auth','yes',1,NULL,0,NULL),
						 ('client_defaults','accepts_registrations','yes',1,NULL,0,NULL),
						 ('client_defaults','endpoint/allow','!all,ulaw',1,NULL,0,NULL),
						 ('client_defaults','endpoint/direct_media','no',1,NULL,0,NULL),
						 ('client_defaults','endpoint/force_rport','yes',1,NULL,0,NULL);
					INSERT INTO ""pjsip_wizard.conf"" (name,setting,value,template,comment,disabled,uses_template) VALUES
						 ('client_defaults','endpoint/rewrite_contact','yes',1,NULL,0,NULL),
						 ('client_defaults','endpoint/rtp_symmetric','yes',1,NULL,0,NULL),
						 ('client_defaults','aor/max_contacts','99',1,NULL,0,NULL),
						 ('client_defaults','aor/qualify_frequency','15',1,NULL,0,NULL),
						 ('client_defaults','endpoint/context','outbound',1,NULL,0,NULL),

						('client_defaults','endpoint/tos_audio','ef',1,NULL,0,NULL),
						('client_defaults','endpoint/tos_video','af41',1,NULL,0,NULL),
						('client_defaults','endpoint/cos_audio','5',1,NULL,0,NULL),
						('client_defaults','endpoint/cos_video','4',1,NULL,0,NULL),
						('client_defaults','endpoint/dtmf_mode','rfc4733',1,NULL,0,NULL),
						('client_defaults','endpoint/aggregate_mwi','yes',1,NULL,0,NULL),
						('client_defaults','endpoint/use_avpf','no',1,NULL,0,NULL),
						('client_defaults','endpoint/rtcp_mux','no',1,NULL,0,NULL),
						('client_defaults','endpoint/bundle','no',1,NULL,0,NULL),
						('client_defaults','endpoint/ice_support','no',1,NULL,0,NULL),
						('client_defaults','endpoint/media_use_received_transport','no',1,NULL,0,NULL),
						('client_defaults','endpoint/trust_id_inbound','yes',1,NULL,0,NULL),
						('client_defaults','endpoint/media_encryption','no',1,NULL,0,NULL),
						('client_defaults','endpoint/timers','yes',1,NULL,0,NULL),
						('client_defaults','endpoint/media_encryption_optimistic','no',1,NULL,0,NULL),
						('client_defaults','endpoint/send_pai','yes',1,NULL,0,NULL),
						('client_defaults','endpoint/language','en',1,NULL,0,NULL),

						 ('LocalTest1','has_hint','yes',0,NULL,0,'client_defaults'),
						 ('LocalTest1','hint_exten','100',0,NULL,0,'client_defaults'),
						 ('LocalTest1','hint_context','extensions',0,NULL,0,'client_defaults'),
						 ('LocalTest1','inbound_auth/username','LocalTest1',0,NULL,0,'client_defaults'),
						 ('LocalTest1','inbound_auth/password',@password1,0,NULL,0,'client_defaults');
					INSERT INTO ""pjsip_wizard.conf"" (name,setting,value,template,comment,disabled,uses_template) VALUES
						 ('LocalTest1','endpoint/callerid','"""" <>',0,NULL,1,'client_defaults'),
						 ('LocalTest1','aor/max_contacts','99',0,NULL,0,'client_defaults'),
						 ('LocalTest2','has_hint','yes',0,NULL,0,'client_defaults'),
						 ('LocalTest2','hint_exten','101',0,NULL,0,'client_defaults'),
						 ('LocalTest2','hint_context','extensions',0,NULL,0,'client_defaults'),
						 ('LocalTest2','inbound_auth/username','LocalTest2',0,NULL,0,'client_defaults'),
						 ('LocalTest2','inbound_auth/password',@password2,0,NULL,0,'client_defaults'),
						 ('LocalTest2','endpoint/callerid','"""" <>',0,NULL,1,'client_defaults'),
						 ('LocalTest2','aor/max_contacts','99',0,NULL,0,'client_defaults'),
						 ('LocalTest3','has_hint','yes',0,NULL,0,'client_defaults');
					INSERT INTO ""pjsip_wizard.conf"" (name,setting,value,template,comment,disabled,uses_template) VALUES
						 ('LocalTest3','hint_exten','102',0,NULL,0,'client_defaults'),
						 ('LocalTest3','hint_context','extensions',0,NULL,0,'client_defaults'),
						 ('LocalTest3','inbound_auth/username','LocalTest3',0,NULL,0,'client_defaults'),
						 ('LocalTest3','inbound_auth/password',@password3,0,NULL,0,'client_defaults'),
						 ('LocalTest3','endpoint/callerid','"""" <>',0,NULL,1,'client_defaults'),
						 ('LocalTest3','aor/max_contacts','99',0,NULL,0,'client_defaults'),
						 ('TrunkOutboundTwilio','sends_auth','no',0,NULL,0,'trunk_defaults');
					INSERT INTO ""pjsip_wizard.conf"" (name,setting,value,template,comment,disabled,uses_template) VALUES
						 ('TrunkOutboundTwilio','sends_registrations','no',0,NULL,0,'trunk_defaults'),
						 ('TrunkOutboundTwilio','remote_hosts','xxxxxx.pstn.ashburn.twilio.com',0,NULL,0,'trunk_defaults'),
						 ('TrunkOutboundTwilio','endpoint/context','inbound',0,NULL,0,'trunk_defaults'),
						 ('TrunkInboundTwilio','remote_hosts','168.86.128.0/18,54.172.60.0/30,54.172.60.0/23,34.203.250.0/23,54.244.51.0/30,54.244.51.0/24,54.171.127.192/30,54.171.127.192/26,52.215.127.0/24,35.156.191.128/30,35.156.191.128/25,3.122.181.0/24,54.65.63.192/30,54.65.63.192/26,3.112.80.0/24,54.169.127.128/30,54.169.127.128/26,3.1.77.0/24,54.252.254.64/30,54.252.254.64/26,3.104.90.0/24,177.71.206.192/30,177.71.206.192/26,18.228.249.0/24,168.86.128.0/18',0,NULL,0,'trunk_defaults'),
						 ('TrunkInboundTwilio','endpoint/context','inbound',0,NULL,0,'trunk_defaults'),
						 ('TrunkInboundTwilio','endpoint/allow','!all,ulaw',0,NULL,0,'trunk_defaults'),
						 ('TrunkInboundTwilio','sends_registrations','no',0,NULL,0,'trunk_defaults'),
						 ('TrunkInboundTwilio','accepts_registrations','no',0,NULL,0,'trunk_defaults'),
						 ('TrunkInboundTwilio','sends_auth','no',0,NULL,0,'trunk_defaults'),
						 ('TrunkInboundTwilio','accepts_auth','no',0,NULL,0,'trunk_defaults');
					INSERT INTO ""pjsip_wizard.conf"" (name,setting,value,template,comment,disabled,uses_template) VALUES
						 ('TrunkInboundTwilio','aor/qualify_frequency','0',0,NULL,0,'trunk_defaults');
				";

				using SQLiteCommand command = DB.CreateCommand();
				command.CommandText = sql;

				{
					SQLiteParameter param = new("@password1", System.Data.DbType.String)
					{
						Value = new Password(
							includeLowercase: true,
							includeUppercase: true,
							includeNumeric: true,
							includeSpecial: false,
							passwordLength: 20
						).Next()
					};
					command.Parameters.Add(param);
				}
				{
					SQLiteParameter param = new("@password2", System.Data.DbType.String)
					{
						Value = new Password(
							includeLowercase: true,
							includeUppercase: true,
							includeNumeric: true,
							includeSpecial: false,
							passwordLength: 20
						).Next()
					};
					command.Parameters.Add(param);
				}
				{
					SQLiteParameter param = new("@password3", System.Data.DbType.String)
					{
						Value = new Password(
							includeLowercase: true,
							includeUppercase: true,
							includeNumeric: true,
							includeSpecial: false,
							passwordLength: 20
						).Next()
					};
					command.Parameters.Add(param);
				}


				var name = command.ExecuteNonQuery();

				Log.Information("[{Class}.{Method}()] Created DB Table version {Version}",
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name,
					databaseFormatVersion);
			}
		}

		


		public void PJSIPWizardRowDeleteAllForName(string name)
		{
			if (DB == null)
				throw new Exception("DB == null");
			PJSIPWizardRow.DeleteAllForName(DB, name);
		}

		public void PJSIPEntryUpsert(PJSIPEntry entry)
		{
			if (DB == null)
				throw new Exception("DB == null");
			entry.Upsert(DB);
		}

		public IEnumerable<string?> PJSIPWizardConfGetNamesForClientDefaults()
		{
			return PJSIPWizardNamesForTemplateName(kClientDefaultName);
		}

		public IEnumerable<string?> PJSIPWizardConfGetNamesForTrunkDefaults()
		{
			return PJSIPWizardNamesForTemplateName(kTrunkDefaultName);
		}

		public IEnumerable<string?> PJSIPWizardNamesForTemplateName(string name)
		{
			if (DB == null)
				throw new Exception("DB == null");

			return PJSIPWizardRow.NamesForTemplateName(DB, name);
		}
		public IEnumerable<string?> TemplateNames()
		{
			if (DB == null)
				throw new Exception("DB == null");

			return PJSIPWizardRow.TemplateNames(DB);
		}

		public IEnumerable<PJSIPWizardRow> PJSIPWizardConfGetForName(string name)
		{
			if (DB == null)
				throw new Exception("DB == null");

			return PJSIPWizardRow.ForName(DB, name);
		}



	}
}
