using Amazon.Runtime.Internal.Util;
using GraphQL.AspNet.Attributes;
using Mono.Unix.Native;
using System.Collections.Generic;
using System.Data.SQLite;

namespace AnanaPhone.SettingsManager
{
	public class PJSIPEntry
	{
		[GraphSkip]
		readonly SettingsManager.Manager? SM = Program.Application?.Services.GetRequiredService<SettingsManager.Manager>();

		public List<PJSIPWizardRow> Rows { get; init; } = new();
		//[GraphSkip]
		//public List<long> RowsToDeleteNextUpsert { get; init; } = new();
		[GraphSkip]
		public void ForceRowNamesTo(string name)
		{
			foreach (PJSIPWizardRow row in Rows)
				row.Name = name;
		}
		[GraphSkip]
		public void ForceUsesTemplateTo(string templateName)
		{
			foreach (PJSIPWizardRow row in Rows)
				row.UsesTemplate = templateName;
		}
		[GraphSkip]
		public void ForceRowTemplateTo(bool flag)
		{
			foreach (PJSIPWizardRow row in Rows)
				row.Template = flag;
		}

		[GraphSkip]
		public void Upsert(SQLiteConnection DB)
		{
			if (DB == null)
				throw new Exception("DB == null");

			using SQLiteCommand command = DB.CreateCommand();

			List<long> insertedRows = new();

			foreach (PJSIPWizardRow row in Rows)
			{
				if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(row.Setting))
					continue;

				var e = PJSIPWizardRow.ForNameSetting(DB, Name, row.Setting);
				if (e.Any())
				{
					// Update
					foreach (PJSIPWizardRow dbr in e)
					{
						dbr.Value = row.Value;
						dbr.Template = row.Template;
						dbr.Comment = row.Comment;
						dbr.Disabled = row.Disabled;
						dbr.UsesTemplate = row.UsesTemplate;
						long id = dbr.Upsert(DB);

						insertedRows.Add(id);
					}

				}
				else
				{
					// New
					long id = row.Upsert(DB);
					insertedRows.Add(id);
				}

				
					
			}

			if (!string.IsNullOrWhiteSpace(Name))
				PJSIPWizardRow.DeleteNotIds(DB, Name, insertedRows);
		}


		public string? Id
		{
			get
			{
				return Name;
			}
		}


		public string? Name
		{
			get
			{
				if (!Rows.Any())
					return null;

				return Rows.FirstOrDefault()?.Name;
			}
		}
		public bool? HasHint
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeyHasHint
						select row.Value;

				// should be yes/no
				return e.FirstOrDefault("").ToLowerInvariant() switch
				{
					"yes" or "true" or "1" => true,
					"no" or "false" or "0" => false,
					_ => null,
				};
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeyHasHint
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}


				row ??= new()
				{
					Setting = PJSIPWizardRow.kSettingKeyHasHint,
				};

				row.Value = value.Value ? "yes" : "no";

				
			}
		}
		public string? HintExten
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeyHintExten
						select row.Value;

				return e.FirstOrDefault();
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeyHintExten
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}


				row ??= new()
				{
					Setting = PJSIPWizardRow.kSettingKeyHintExten,
				};

				row.Value = value;

				
			}
		}
		public string? HintContext
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeyHintContext
						select row.Value;

				return e.FirstOrDefault();
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeyHintContext
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}


				row ??= new()
				{
					Setting = PJSIPWizardRow.kSettingKeyHintContext,
				};

				row.Value = value;

				
			}
		}
		public string? InboundAuthUsername
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeyInboundAuthUsername
						select row.Value;

				return e.FirstOrDefault();
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeyInboundAuthUsername
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}


				row ??= new()
				{
					Setting = PJSIPWizardRow.kSettingKeyInboundAuthUsername,
				};

				row.Value = value;

				
			}
		}
		public string? InboundAuthPassword
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeyInboundAuthPassword
						select row.Value;

				return e.FirstOrDefault();
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeyInboundAuthPassword
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}


				row ??= new()
				{
					Setting = PJSIPWizardRow.kSettingKeyInboundAuthPassword,
				};

				row.Value = value;

				
			}
		}
		public string? EndpointCallerid
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeyEndpointCallerid
						select row.Value;

				return e.FirstOrDefault();
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeyEndpointCallerid
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}


				row ??= new()
				{
					Setting = PJSIPWizardRow.kSettingKeyEndpointCallerid,
				};

				row.Value = value;

				
			}
		}
		public int? AorMaxContacts
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeyAorMaxContacts
						select row.Value;

				if (int.TryParse(e.FirstOrDefault(), out int _parsed))
					return _parsed;

				return null;
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeyAorMaxContacts
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}


				if (row == null)
				{
					row = new()
					{
						Setting = PJSIPWizardRow.kSettingKeyAorMaxContacts,
					};
					Rows.Add(row);
				}

				row.Value = $"{value}";

				
			}
		}
		public IEnumerable<string> RemoteHosts
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeyRemoteHosts
						select row.Value;

				string? allow = e.FirstOrDefault();
				if (string.IsNullOrWhiteSpace(allow))
					yield break;

				string[] parts = allow.Split(',');
				foreach (string part in parts)
				{
					if (string.IsNullOrWhiteSpace(part))
						continue;
					yield return part;
				}

				yield break;
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeyRemoteHosts
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}

				string joined = string.Join(",", value);
				if (string.IsNullOrEmpty(joined))
					return;


				if (row == null)
				{
					row = new()
					{
						Setting = PJSIPWizardRow.kSettingKeyRemoteHosts,
					};
					Rows.Add(row);
				}

				row.Value = string.IsNullOrEmpty(joined) ? null : joined;
			}
		}
		public string? Type
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeyType
						select row.Value;

				return e.FirstOrDefault();
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeyType
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}


				if (row == null)
				{
					row = new()
					{
						Setting = PJSIPWizardRow.kSettingKeyType,
					};
					Rows.Add(row);
				}

				row.Value = value;

				
			}
		}
		public bool? AcceptsAuth
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeyAcceptsAuth
						select row.Value;

				// should be yes/no
				return e.FirstOrDefault("").ToLowerInvariant() switch
				{
					"yes" or "true" or "1" => true,
					"no" or "false" or "0" => false,
					_ => null,
				};
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeyAcceptsAuth
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}


				if (row == null)
				{
					row = new()
					{
						Setting = PJSIPWizardRow.kSettingKeyAcceptsAuth,
					};
					Rows.Add(row);
				}

				row.Value = value.Value ? "yes" : "no";

				
			}
		}
		public bool? AcceptsRegistrations
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeyAcceptsRegistrations
						select row.Value;

				// should be yes/no
				return e.FirstOrDefault("").ToLowerInvariant() switch
				{
					"yes" or "true" or "1" => true,
					"no" or "false" or "0" => false,
					_ => null,
				};
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeyAcceptsRegistrations
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}


				if (row == null)
				{
					row = new()
					{
						Setting = PJSIPWizardRow.kSettingKeyAcceptsRegistrations,
					};
					Rows.Add(row);
				}

				row.Value = value.Value ? "yes" : "no";

				
			}
		}
		public IEnumerable<string> EndpointAllow
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeyEndpointAllow
						select row.Value;

				string? allow = e.FirstOrDefault();
				if (string.IsNullOrWhiteSpace(allow))
					yield break;

				string[] parts = allow.Split(',');
				foreach (string part in parts)
				{
					if (string.IsNullOrWhiteSpace(part))
						continue;
					yield return part;
				}

				yield break;
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeyEndpointAllow
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}

				string joined = string.Join(",", value);
				if (string.IsNullOrEmpty(joined))
					return;

				if (row == null)
				{
					row = new()
					{
						Setting = PJSIPWizardRow.kSettingKeyEndpointAllow,
					};
					Rows.Add(row);
				}

				row.Value = string.IsNullOrEmpty(joined) ? null : joined;

				
			}
		}
		public bool? EndpointDirectMedia
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeyEndpointDirectMedia
						select row.Value;

				// should be yes/no
				return e.FirstOrDefault("").ToLowerInvariant() switch
				{
					"yes" or "true" or "1" => true,
					"no" or "false" or "0" => false,
					_ => null,
				};
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeyEndpointDirectMedia
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}


				if (row == null)
				{
					row = new()
					{
						Setting = PJSIPWizardRow.kSettingKeyEndpointDirectMedia,
					};
					Rows.Add(row);
				}

				row.Value = value.Value ? "yes" : "no";

				
			}
		}
		public bool? EndpointForceRport
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeyEndpointForceRport
						select row.Value;

				// should be yes/no
				return e.FirstOrDefault("").ToLowerInvariant() switch
				{
					"yes" or "true" or "1" => true,
					"no" or "false" or "0" => false,
					_ => null,
				};
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeyEndpointForceRport
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}


				if (row == null)
				{
					row = new()
					{
						Setting = PJSIPWizardRow.kSettingKeyEndpointForceRport,
					};
					Rows.Add(row);
				}

				row.Value = value.Value ? "yes" : "no";

				
			}
		}
		public bool? EndpointRewriteContact
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeyEndpointRewriteContact
						select row.Value;

				// should be yes/no
				return e.FirstOrDefault("").ToLowerInvariant() switch
				{
					"yes" or "true" or "1" => true,
					"no" or "false" or "0" => false,
					_ => null,
				};
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeyEndpointRewriteContact
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}


				if (row == null)
				{
					row = new()
					{
						Setting = PJSIPWizardRow.kSettingKeyEndpointRewriteContact,
					};
					Rows.Add(row);
				}

				row.Value = value.Value ? "yes" : "no";

				
			}
		}
		public bool? EndpointRTPSymmetric
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeyEndpointRTPSymmetric
						select row.Value;

				// should be yes/no
				return e.FirstOrDefault("").ToLowerInvariant() switch
				{
					"yes" or "true" or "1" => true,
					"no" or "false" or "0" => false,
					_ => null,
				};
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeyEndpointRTPSymmetric
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}


				if (row == null)
				{
					row = new()
					{
						Setting = PJSIPWizardRow.kSettingKeyEndpointRTPSymmetric,
					};
					Rows.Add(row);
				}

				row.Value = value.Value ? "yes" : "no";

				
			}
		}
		public int? AorQualifyFrequency
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeyAorQualifyFrequency
						select row.Value;

				if (int.TryParse(e.FirstOrDefault(), out int _parsed))
					return _parsed;

				return null;
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeyAorQualifyFrequency
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}


				if (row == null)
				{
					row = new()
					{
						Setting = PJSIPWizardRow.kSettingKeyAorQualifyFrequency,
					};
					Rows.Add(row);
				}

				row.Value = $"{value}";

				
			}
		}
		public string? EndpointContext
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeyEndpointContext
						select row.Value;

				return e.FirstOrDefault();
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeyEndpointContext
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}


				if (row == null)
				{
					row = new()
					{
						Setting = PJSIPWizardRow.kSettingKeyEndpointContext,
					};
					Rows.Add(row);
				}

				row.Value = value;

				
			}
		}
		public bool? SendsAuth
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeySendsAuth
						select row.Value;

				// should be yes/no
				return e.FirstOrDefault("").ToLowerInvariant() switch
				{
					"yes" or "true" or "1" => true,
					"no" or "false" or "0" => false,
					_ => null,
				};
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeySendsAuth
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}


				if (row == null)
				{
					row = new()
					{
						Setting = PJSIPWizardRow.kSettingKeySendsAuth,
					};
					Rows.Add(row);
				}

				row.Value = value.Value ? "yes" : "no";

				
			}
		}
		public bool? SendsRegistrations
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeySendsRegistrations
						select row.Value;

				// should be yes/no
				return e.FirstOrDefault("").ToLowerInvariant() switch
				{
					"yes" or "true" or "1" => true,
					"no" or "false" or "0" => false,
					_ => null,
				};
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeySendsRegistrations
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}


				if (row == null)
				{
					row = new()
					{
						Setting = PJSIPWizardRow.kSettingKeySendsRegistrations,
					};
					Rows.Add(row);
				}

				row.Value = value.Value ? "yes" : "no";

				
			}
		}
		public bool? EndpointT38Udptl
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeyEndpointT38Udptl
						select row.Value;

				// should be yes/no
				return e.FirstOrDefault("").ToLowerInvariant() switch
				{
					"yes" or "true" or "1" => true,
					"no" or "false" or "0" => false,
					_ => null,
				};
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeyEndpointT38Udptl
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}


				if (row == null)
				{
					row = new()
					{
						Setting = PJSIPWizardRow.kSettingKeyEndpointT38Udptl,
					};
					Rows.Add(row);
				}

				row.Value = value.Value ? "yes" : "no";

				
			}
		}
		public string? EndpointT38UdptlEc
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeyEndpointT38UdptlEc
						select row.Value;

				return e.FirstOrDefault();
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeyEndpointT38UdptlEc
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}


				if (row == null)
				{
					row = new()
					{
						Setting = PJSIPWizardRow.kSettingKeyEndpointT38UdptlEc,
					};
					Rows.Add(row);
				}

				row.Value = value;

				
			}
		}
		public bool? EndpointFaxDetect
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeyEndpointFaxDetect
						select row.Value;

				// should be yes/no
				return e.FirstOrDefault("").ToLowerInvariant() switch
				{
					"yes" or "true" or "1" => true,
					"no" or "false" or "0" => false,
					_ => null,
				};
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeyEndpointFaxDetect
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}


				if (row == null)
				{
					row = new()
					{
						Setting = PJSIPWizardRow.kSettingKeyEndpointFaxDetect,
					};
					Rows.Add(row);
				}

				row.Value = value.Value ? "yes" : "no";

				
			}
		}
		public bool? EndpointTrustIdInbound
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeyEndpointTrustIdInbound
						select row.Value;

				// should be yes/no
				return e.FirstOrDefault("").ToLowerInvariant() switch
				{
					"yes" or "true" or "1" => true,
					"no" or "false" or "0" => false,
					_ => null,
				};
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeyEndpointTrustIdInbound
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}


				if (row == null)
				{
					row = new()
					{
						Setting = PJSIPWizardRow.kSettingKeyEndpointTrustIdInbound,
					};
					Rows.Add(row);
				}

				row.Value = value.Value ? "yes" : "no";

				
			}
		}
		public bool? EndpointT38UdptlNat
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeyEndpointT38UdptlNat
						select row.Value;

				// should be yes/no
				return e.FirstOrDefault("").ToLowerInvariant() switch
				{
					"yes" or "true" or "1" => true,
					"no" or "false" or "0" => false,
					_ => null,
				};
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeyEndpointT38UdptlNat
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}


				if (row == null)
				{
					row = new()
					{
						Setting = PJSIPWizardRow.kSettingKeyEndpointT38UdptlNat,
					};
					Rows.Add(row);
				}

				row.Value = value.Value ? "yes" : "no";

				
			}
		}
		public string? EndpointDTMFMode
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeyEndpointDTMFMode
						select row.Value;

				return e.FirstOrDefault();
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeyEndpointDTMFMode
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}


				if (row == null)
				{
					row = new()
					{
						Setting = PJSIPWizardRow.kSettingKeyEndpointDTMFMode,
					};
					Rows.Add(row);
				}

				row.Value = value;

				
			}
		}
		public bool? EndpointAllowSubscribe
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeyEndpointAllowSubscribe
						select row.Value;

				// should be yes/no
				return e.FirstOrDefault("").ToLowerInvariant() switch
				{
					"yes" or "true" or "1" => true,
					"no" or "false" or "0" => false,
					_ => null,
				};
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeyEndpointAllowSubscribe
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}


				if (row == null)
				{
					row = new()
					{
						Setting = PJSIPWizardRow.kSettingKeyEndpointAllowSubscribe,
					};
					Rows.Add(row);
				}

				row.Value = value.Value ? "yes" : "no";

				
			}
		}
		public string? EndpointTransport
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeyEndpointTransport
						select row.Value;

				return e.FirstOrDefault();
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeyEndpointTransport
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}


				if (row == null)
				{
					row = new()
					{
						Setting = PJSIPWizardRow.kSettingKeyEndpointTransport,
					};
					Rows.Add(row);
				}

				row.Value = value;

				
			}
		}
		public string? XDummyPlaceholder
		{
			get
			{
				var e = from row in Rows
						where row.Setting == PJSIPWizardRow.kSettingKeyXDummyPlaceholder
						select row.Value;

				return e.FirstOrDefault();
			}
			set
			{
				if (SM == null)
					throw new Exception("SM == null");

				var e = from r in Rows
						where r.Setting == PJSIPWizardRow.kSettingKeyXDummyPlaceholder
						select r;
				PJSIPWizardRow? row = e.FirstOrDefault();

				if (value == null)
				{
					if (row != null)
						Rows.Remove(row);
					return;
				}


				if (row == null)
				{
					row = new()
					{
						Setting = PJSIPWizardRow.kSettingKeyXDummyPlaceholder,
					};
					Rows.Add(row);

				}

				row.Value = "this row allows the code generator to ensure the section stays here until explicitly deleted.";
				row.Disabled = true;
			}
		}

	}
}
