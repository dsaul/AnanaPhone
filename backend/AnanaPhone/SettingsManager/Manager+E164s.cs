using Serilog;
using System.Data.SQLite;
using AnanaPhone.Extensions;
using System.Data;
using AnanaPhone.SettingsManager;

namespace AnanaPhone.SettingsManager
{
	public partial class Manager : IDisposable
	{
		void E164sEnsureTable()
		{
			if (DB == null)
				throw new Exception("DB == null");

			if (false == DB.TableExists("e164s"))
			{
				string sql = @"
					CREATE TABLE e164s (
					e164 TEXT PRIMARY KEY,
					name TEXT,
					comment TEXT,
					outboundDevice TEXT,
					disabled INTEGER DEFAULT (0)
					);
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

		public int E164sRemove(string e164)
		{
			if (DB == null)
				throw new Exception("DB == null");

			return E164Row.Remove(DB, e164);
		}

		public int E164sUpsert(E164Row number)
		{
			if (DB == null)
				throw new Exception("DB == null");

			return E164Row.Upsert(DB, number);
		}

		public IEnumerable<E164Row> E164sGetAll()
		{
			if (DB == null)
				throw new Exception("DB == null");
			return E164Row.GetAll(DB);
		}

		public IEnumerable<E164Row> E164sGetForE164(string e164)
		{
			if (DB == null)
				throw new Exception("DB == null");
			return E164Row.ForE164(DB, e164);
		}
	}
}
