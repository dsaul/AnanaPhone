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

		public void E164sRemove(string e164)
		{
			if (DB == null)
				throw new Exception("DB == null");

			using SQLiteCommand command = DB.CreateCommand();

			string sql = @"
				DELETE FROM e164s
				WHERE e164=@e164;
			;";
			command.CommandText = sql;

			// Params
			{
				SQLiteParameter param = new("@e164", System.Data.DbType.String)
				{
					Value = e164
				};
				command.Parameters.Add(param);
			}

			int rowsAffected = command.ExecuteNonQuery();
			if (rowsAffected == 0)
				throw new Exception("No numbers were deleted.");
		}

		public void E164sUpsert(E164Row number)
		{
			if (DB == null)
				throw new Exception("DB == null");

			using SQLiteCommand command = DB.CreateCommand();



			string sql = @"
				INSERT INTO e164s
				(e164, name, comment, outboundDevice, disabled) 
				VALUES
				(@e164, @name, @comment, @outboundDevice, @disabled)
				ON CONFLICT(e164) DO UPDATE SET 
					name=excluded.name,
					comment=excluded.comment,
					outboundDevice=excluded.outboundDevice,
					disabled=excluded.disabled
			;";
			command.CommandText = sql;

			// Params
			{
				SQLiteParameter param = new("@e164", System.Data.DbType.String)
				{
					Value = number.E164
				};
				command.Parameters.Add(param);
			}
			{
				SQLiteParameter param = new("@name", System.Data.DbType.String)
				{
					Value = number.Name
				};
				command.Parameters.Add(param);
			}
			{
				SQLiteParameter param = new("@comment", System.Data.DbType.String)
				{
					Value = number.Comment
				};
				command.Parameters.Add(param);
			}
			{
				SQLiteParameter param = new("@outboundDevice", System.Data.DbType.String)
				{
					Value = number.OutboundDevice
				};
				command.Parameters.Add(param);
			}
			{
				SQLiteParameter param = new("@disabled", System.Data.DbType.Boolean)
				{
					Value = number.Disabled
				};
				command.Parameters.Add(param);
			}


			int rowsAffected = command.ExecuteNonQuery();
			if (rowsAffected == 0)
				throw new Exception("No numbers were updated.");
		}

		public IEnumerable<E164Row> E164sGetAll()
		{
			if (DB == null)
				throw new Exception("DB == null");

			using SQLiteCommand command = DB.CreateCommand();

			string sql = @"
				SELECT 
					e164, name, comment, outboundDevice, disabled
				FROM 
					e164s
				ORDER BY e164 DESC
			;";
			command.CommandText = sql;

			using SQLiteDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
				E164Row row = new()
				{
					E164 = reader.IsDBNull("e164") ? null : reader.GetString("e164"),
					Name = reader.IsDBNull("name") ? null : reader.GetString("name"),
					Comment = reader.IsDBNull("comment") ? null : reader.GetString("comment"),
					OutboundDevice = reader.IsDBNull("outboundDevice") ? null : reader.GetString("outboundDevice"),
					Disabled = reader.IsDBNull("disabled") ? null : reader.GetBoolean("disabled"),
				};

				yield return row;
			}

			yield break;
		}
	}
}
