using Serilog;
using System.Data.SQLite;
using AnanaPhone.Extensions;
using System.Data;

namespace AnanaPhone.SettingsManager
{
	public partial class Manager : IDisposable
	{
		void ARIConfEnsureTable()
		{
			if (DB == null)
				throw new Exception("DB == null");

			if (false == DB.TableExists("ari.conf"))
			{
				string sql = @"
					CREATE TABLE ""ari.conf"" (
						id INTEGER PRIMARY KEY AUTOINCREMENT,
						""section"" TEXT,
						setting TEXT,
						value TEXT, 
						comment TEXT, 
						disabled INTEGER DEFAULT (0));

					CREATE INDEX ari_conf_section_IDX ON ""ari.conf"" (""section"");
					CREATE INDEX ari_conf_setting_IDX ON ""ari.conf"" (setting);
					INSERT INTO ""ari.conf"" (""section"",setting,value,comment) VALUES
						 ('general','enabled','yes',NULL);
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
