using Serilog;
using System.Data.SQLite;
using AnanaPhone.Extensions;
using System.Data;

namespace AnanaPhone.SettingsManager
{
	public partial class Manager : IDisposable
	{
		void RTPConfEnsureTable()
		{
			if (DB == null)
				throw new Exception("DB == null");

			if (false == DB.TableExists("rtp.conf"))
			{
				string sql = @"
					CREATE TABLE ""rtp.conf"" (
						id INTEGER PRIMARY KEY AUTOINCREMENT,
						""section"" TEXT,
						setting TEXT,
						value TEXT, 
						comment TEXT, 
						disabled INTEGER DEFAULT (0));

					CREATE INDEX rtp_conf_section_IDX ON ""rtp.conf"" (""section"");
					CREATE INDEX rtp_conf_setting_IDX ON ""rtp.conf"" (setting);
					
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
