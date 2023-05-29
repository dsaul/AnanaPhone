using Serilog;
using System.Data.SQLite;
using AnanaPhone.Extensions;
using System.Data;
using AnanaPhone.SettingsManager;

namespace AnanaPhone.SettingsManager
{
	public partial class Manager : IDisposable
	{

		

		void E164ClientsEnsureTable()
		{
			if (DB == null)
				throw new Exception("DB == null");

			if (false == DB.TableExists("e164_clients"))
			{
				string sql = @"
					CREATE TABLE e164_clients (
						e164 TEXT PRIMARY KEY,
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

		public int E164ClientRemove(string e164)
		{
			if (DB == null)
				throw new Exception("DB == null");

			return E164ClientRow.Remove(DB, e164);
		}

		public int E164ClientUpsert(E164ClientRow number)
		{
			if (DB == null)
				throw new Exception("DB == null");

			return E164ClientRow.Upsert(DB, number);
		}


		public IEnumerable<E164ClientRow> E164ClientsGetAll()
		{
			if (DB == null)
				throw new Exception("DB == null");

			return E164ClientRow.GetAll(DB);
		}

		public IEnumerable<E164Row> ForE164(string e164)
		{
			if (DB == null)
				throw new Exception("DB == null");

			return E164Row.ForE164(DB, e164);
		}





















	}
}
