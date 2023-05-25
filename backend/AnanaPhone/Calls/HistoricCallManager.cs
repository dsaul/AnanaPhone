using System.Data;
using System.Data.SQLite;
using AnanaPhone.Extensions;
using Serilog;

namespace AnanaPhone.Calls
{
	public class HistoricCallManager : IDisposable
	{
		public HistoricCallManager()
		{
			OpenDB();
			CreateTableIfNotExists();
		}

		const int databaseFormatVersion = 1;

		static string DBPath
		{
			get
			{
				return Path.Join(Env.SQLITE_DATABASE_DIRECTORY, $"historicCalls.{databaseFormatVersion}.sqlite");
			}
		}

		static string ConnectionString
		{
			get
			{
				return $"URI=file:{DBPath}";
			}
		}

		SQLiteConnection? DB { get; set; } = null;

		SQLiteConnection OpenDB()
		{
			DB = new SQLiteConnection(ConnectionString);
			DB.Open();
			return DB;
		}


		void CreateTableIfNotExists()
		{
			if (DB == null)
				throw new Exception("DB == null");

			if (false == DB.TableExists("calls"))
			{
				string sql = @"
					CREATE TABLE calls (
					id TEXT PRIMARY KEY,
					callerIdName TEXT,
					callerIdNumber TEXT,
					duration TEXT,
					timestampISO8601 TEXT,
					landedDID TEXT,
					originalChannel TEXT,
					callDirection TEXT
				);";

				using SQLiteCommand command = DB.CreateCommand();
				command.CommandText = sql;
				var name = command.ExecuteNonQuery();

				Log.Information("[{Class}.{Method}()] Created DB Table version {Version}",
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name,
					databaseFormatVersion);
			}
		}

		public void Upsert(HistoricCall call)
		{
			if (DB == null)
				throw new Exception("DB == null");
			HistoricCall.Upsert(DB, call);
		}

		public int RemoveAll()
		{
			if (DB == null)
				throw new Exception("DB == null");
			return HistoricCall.RemoveAll(DB);
		}

		public int Remove(string id)
		{
			if (DB == null)
				throw new Exception("DB == null");
			return HistoricCall.Remove(DB, id);
		}

		public IEnumerable<HistoricCall> All()
		{
			if (DB == null)
				throw new Exception("DB == null");
			return HistoricCall.All(DB);
		}





		#region IDisposable
		private bool disposedValue;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// dispose managed state (managed objects)
					if (DB != null)
					{
						DB.Close();
						DB.Dispose();
						DB = null;
					}
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				disposedValue = true;
			}
		}

		// // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		// ~HistoricCallManager()
		// {
		//     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		//     Dispose(disposing: false);
		// }

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}
