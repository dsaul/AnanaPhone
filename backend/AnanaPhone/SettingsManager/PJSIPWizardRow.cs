using Amazon.Runtime.Internal.Util;
using AnanaPhone.VoiceMail;
using GraphQL.AspNet.Attributes;
using Mono.Unix.Native;
using Polly;
using SharedCode.DatabaseSchemas;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace AnanaPhone.SettingsManager
{
	public class PJSIPWizardRow
	{
		public const string kSettingKeyType = "type";
		public const string kSettingKeyEndpointAllow = "endpoint/allow";
		public const string kSettingKeyEndpointT38Udptl = "endpoint/t38_udptl";
		public const string kSettingKeyEndpointT38UdptlEc = "endpoint/t38_udptl_ec";
		public const string kSettingKeyEndpointFaxDetect = "endpoint/fax_detect";
		public const string kSettingKeyEndpointTrustIdInbound = "endpoint/trust_id_inbound";
		public const string kSettingKeyEndpointT38UdptlNat = "endpoint/t38_udptl_nat";
		public const string kSettingKeyEndpointDirectMedia = "endpoint/direct_media";
		public const string kSettingKeyEndpointRewriteContact = "endpoint/rewrite_contact";
		public const string kSettingKeyEndpointRTPSymmetric = "endpoint/rtp_symmetric";
		public const string kSettingKeyEndpointDTMFMode = "endpoint/dtmf_mode";
		public const string kSettingKeyEndpointAllowSubscribe = "endpoint/allow_subscribe";
		public const string kSettingKeyEndpointTransport = "endpoint/transport";
		public const string kSettingKeyAorQualifyFrequency = "aor/qualify_frequency";
		public const string kSettingKeyAcceptsAuth = "accepts_auth";
		public const string kSettingKeyAcceptsRegistrations = "accepts_registrations";
		public const string kSettingKeyEndpointForceRport = "endpoint/force_rport";
		public const string kSettingKeyEndpointRtpSymmetric = "endpoint/rtp_symmetric";
		public const string kSettingKeyEndpointContext = "endpoint/context";
		public const string kSettingKeyHasHint = "has_hint";
		public const string kSettingKeyHintExten = "hint_exten";
		public const string kSettingKeyHintContext = "hint_context";
		public const string kSettingKeyInboundAuthUsername = "inbound_auth/username";
		public const string kSettingKeyInboundAuthPassword = "inbound_auth/password";
		public const string kSettingKeyOutboundAuthUsername = "outbound_auth/username";
		public const string kSettingKeyOutboundAuthPassword = "outbound_auth/password";
		public const string kSettingKeyEndpointCallerid = "endpoint/callerid";
		public const string kSettingKeyAorMaxContacts = "aor/max_contacts";
		public const string kSettingKeyRemoteHosts = "remote_hosts";
		public const string kSettingKeySendsAuth = "sends_auth";
		public const string kSettingKeySendsRegistrations = "sends_registrations";
		public const string kSettingKeyXDummyPlaceholder = "dummy_placeholder";

		public long? Id { get; init; }
		public string? Name { get; set; }
		public string? Setting { get; set; }
		public string? Value { get; set; }
		public bool? Template { get; set; }
		public string? Comment { get; set; }
		public bool? Disabled { get; set; }
		public string? UsesTemplate { get; set; }


		[GraphSkip]
		public static PJSIPWizardRow ForDataReader(DbDataReader reader)
		{
			return new()
			{
				Id = reader.IsDBNull("id") ? null : reader.GetInt64("id"),
				Name = reader.IsDBNull("name") ? null : reader.GetString("name"),
				Setting = reader.IsDBNull("setting") ? null : reader.GetString("setting"),
				Value = reader.IsDBNull("value") ? null : reader.GetString("value"),
				Template = reader.IsDBNull("template") ? null : reader.GetBoolean("template"),
				Comment = reader.IsDBNull("comment") ? null : reader.GetString("comment"),
				Disabled = reader.IsDBNull("disabled") ? null : reader.GetBoolean("disabled"),
				UsesTemplate = reader.IsDBNull("uses_template") ? null : reader.GetString("uses_template"),
			};
		}



		[GraphSkip]
		public static void DeleteAllForName(SQLiteConnection DB, string name)
		{
			if (DB == null)
				throw new Exception("DB == null");


			using SQLiteCommand command = DB.CreateCommand();

			string sql = $"DELETE FROM \"pjsip_wizard.conf\" WHERE name = @name";
			command.CommandText = sql;

			{
				SQLiteParameter param = new("@name", System.Data.DbType.String)
				{
					Value = name,
				};
				command.Parameters.Add(param);
			}

			
			int rowsAffected = command.ExecuteNonQuery();
			if (rowsAffected == 0)
				throw new Exception("No rows were deleted.");

		}
		[GraphSkip]
		static public IEnumerable<PJSIPWizardRow> ForName(SQLiteConnection DB, string name)
		{
			if (DB == null)
				throw new Exception("DB == null");

			using SQLiteCommand command = DB.CreateCommand();

			string sql = @"
				SELECT 
					id,name,setting,value,template,comment,disabled,uses_template
				FROM 
					""pjsip_wizard.conf""
				WHERE 
					name=@name
				ORDER BY setting ASC
			;";
			command.CommandText = sql;

			{
				SQLiteParameter param = new("@name", System.Data.DbType.String)
				{
					Value = name
				};
				command.Parameters.Add(param);
			}

			using SQLiteDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
				yield return ForDataReader(reader);
			}

			yield break;
		}
		[GraphSkip]
		static public IEnumerable<string?> NamesForTemplateName(SQLiteConnection DB, string name)
		{
			if (DB == null)
				throw new Exception("DB == null");

			using SQLiteCommand command = DB.CreateCommand();

			string sql = @"
				SELECT DISTINCT(name)
				FROM ""pjsip_wizard.conf"" 
				WHERE uses_template=@uses_template
				ORDER BY name ASC
			;";
			command.CommandText = sql;

			// Params
			{
				SQLiteParameter param = new("@uses_template", System.Data.DbType.String)
				{
					Value = name
				};
				command.Parameters.Add(param);
			}

			using SQLiteDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
				yield return reader.IsDBNull("name") ? null : reader.GetString("name");
			}

			yield break;
		}
		[GraphSkip]
		static public void DeleteNotIds(SQLiteConnection DB, string name, IEnumerable<long> _ids)
		{
			if (DB == null)
				throw new Exception("DB == null");

			long[] ids = _ids.ToArray();



			using SQLiteCommand command = DB.CreateCommand();

			string placeholders = string.Join(",", Enumerable.Range(0, ids.Length).Select(i => $"@param{i}"));
			string sql = $"DELETE FROM \"pjsip_wizard.conf\" WHERE name = @name AND id NOT IN ({placeholders})";
			command.CommandText = sql;

			{
				SQLiteParameter param = new("@name", System.Data.DbType.String)
				{
					Value = name,
				};
				command.Parameters.Add(param);
			}

			for (int i = 0; i < ids.Length; i++)
			{
				SQLiteParameter param = new($"@param{i}", System.Data.DbType.Int64)
				{
					Value = ids[i],
				};
				command.Parameters.Add(param);
			}

			int rowsAffected = command.ExecuteNonQuery();
			//if (rowsAffected == 0)
			//	throw new Exception("No rows were deleted.");


		}
		[GraphSkip]
		static public void DeleteIds(SQLiteConnection DB, string name, IEnumerable<long> _ids)
		{
			if (DB == null)
				throw new Exception("DB == null");

			long[] ids = _ids.ToArray();



			using SQLiteCommand command = DB.CreateCommand();

			string placeholders = string.Join(",", Enumerable.Range(0, ids.Length).Select(i => $"@param{i}"));
			string sql = $"DELETE FROM \"pjsip_wizard.conf\" WHERE name = @name AND (id IN ({placeholders}))";
			command.CommandText = sql;

			{
				SQLiteParameter param = new("@name", System.Data.DbType.String)
				{
					Value = name,
				};
				command.Parameters.Add(param);
			}

			for (int i = 0; i < ids.Length; i++)
			{
				SQLiteParameter param = new($"@param{i}", System.Data.DbType.Int64)
				{
					Value = ids[i],
				};
				command.Parameters.Add(param);
			}

			int rowsAffected = command.ExecuteNonQuery();
			if (rowsAffected == 0)
				throw new Exception("No rows were deleted.");
		}
		[GraphSkip]
		static public long Upsert(SQLiteConnection DB, PJSIPWizardRow row)
		{
			if (DB == null)
				throw new Exception("DB == null");

			using SQLiteCommand command = DB.CreateCommand();

			string sql = @"
				INSERT INTO ""pjsip_wizard.conf""
				(id,name,setting,value,template,comment,disabled,uses_template) 
				VALUES
				(@id,@name,@setting,@value,@template,@comment,@disabled,@uses_template)
				ON CONFLICT(id) DO UPDATE SET 
					name=excluded.name,
					setting=excluded.setting,
					value=excluded.value,
					template=excluded.template,
					comment=excluded.comment,
					disabled=excluded.disabled,
					uses_template=excluded.uses_template
				returning id;
			;";
			command.CommandText = sql;

			// Params
			{
				SQLiteParameter param = new("@id", System.Data.DbType.String)
				{
					Value = row.Id,
				};
				command.Parameters.Add(param);
			}
			{
				SQLiteParameter param = new("@name", System.Data.DbType.String)
				{
					Value = row.Name,
				};
				command.Parameters.Add(param);
			}
			{
				SQLiteParameter param = new("@setting", System.Data.DbType.String)
				{
					Value = row.Setting,
				};
				command.Parameters.Add(param);
			}
			{
				SQLiteParameter param = new("@value", System.Data.DbType.String)
				{
					Value = row.Value
				};
				command.Parameters.Add(param);
			}
			{
				SQLiteParameter param = new("@template", System.Data.DbType.Boolean)
				{
					Value = row.Template
				};
				command.Parameters.Add(param);
			}
			{
				SQLiteParameter param = new("@comment", System.Data.DbType.String)
				{
					Value = row.Comment,
				};
				command.Parameters.Add(param);
			}
			{
				SQLiteParameter param = new("@disabled", System.Data.DbType.Boolean)
				{
					Value = row.Disabled,
				};
				command.Parameters.Add(param);
			}
			{
				SQLiteParameter param = new("@uses_template", System.Data.DbType.String)
				{
					Value = row.UsesTemplate,
				};
				command.Parameters.Add(param);
			}

			long id = (long)command.ExecuteScalar();
			return id;
			
		}
		[GraphSkip]
		public long Upsert(SQLiteConnection DB)
		{
			if (DB == null)
				throw new Exception("DB == null");

			return Upsert(DB, this);
		}
		[GraphSkip]
		static public IEnumerable<PJSIPWizardRow> ForNameSetting(SQLiteConnection DB, string name, string setting)
		{
			if (DB == null)
				throw new Exception("DB == null");

			using SQLiteCommand command = DB.CreateCommand();

			string sql = @"
				SELECT 
					id,name,setting,value,template,comment,disabled,uses_template
				FROM 
					""pjsip_wizard.conf""
				WHERE 
					name=@name AND setting=@setting
				ORDER BY setting ASC
			;";

			command.CommandText = sql;

			// Params
			{
				SQLiteParameter param = new("@name", System.Data.DbType.String)
				{
					Value = name
				};
				command.Parameters.Add(param);
			}
			{
				SQLiteParameter param = new("@setting", System.Data.DbType.String)
				{
					Value = setting
				};
				command.Parameters.Add(param);
			}

			using SQLiteDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
				yield return ForDataReader(reader);
			}

			yield break;
		}
		[GraphSkip]
		static public IEnumerable<string?> TemplateNames(SQLiteConnection DB)
		{
			if (DB == null)
				throw new Exception("DB == null");

			using SQLiteCommand command = DB.CreateCommand();

			string sql = @"
				SELECT DISTINCT(name)
				FROM ""pjsip_wizard.conf"" 
				WHERE template=1
				ORDER BY name ASC
			;";
			command.CommandText = sql;

			using SQLiteDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
				yield return reader.IsDBNull("name") ? null : reader.GetString("name");
			}

			yield break;
		}
	}
}
