using Serilog;
using System.Data.SQLite;
using AnanaPhone.Extensions;
using System.Data;

namespace AnanaPhone.SettingsManager
{
	public partial class Manager : IDisposable
	{
		void MusicOnHoldConfEnsureTable()
		{
			if (DB == null)
				throw new Exception("DB == null");

			if (false == DB.TableExists("musiconhold.conf"))
			{
				string sql = @"
					CREATE TABLE ""musiconhold.conf"" (
						id INTEGER PRIMARY KEY AUTOINCREMENT,
						""section"" TEXT,
						setting TEXT,
						value TEXT,
						comment TEXT,
						disabled INTEGER DEFAULT (0)
					);
					CREATE INDEX musiconhold_conf_section_IDX ON ""musiconhold.conf"" (""section"");
					CREATE INDEX musiconhold_conf_setting_IDX ON ""musiconhold.conf"" (setting);
					INSERT INTO ""musiconhold.conf"" (""section"",setting,value,comment,disabled) VALUES
						 ('general','empty section',NULL,NULL,1),
						 ('default','mode','files',NULL,0),
						 ('default','directory','/etc/asterisk/_holdmusic',NULL,0),
						 ('default','random','yes',NULL,0);
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
