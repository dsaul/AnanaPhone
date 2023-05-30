using AnanaPhone.Calls;
using GraphQL.AspNet.Attributes;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Data.Common;
using Serilog;

namespace AnanaPhone.VoiceMail
{
	public class VoiceMailMessageRow
	{
		public string? Id { get; init; } = Guid.NewGuid().ToString();
		public string? CallerIdNumber { get; init; } = null;
		public string? CallerIdName { get; init; } = null;
		public string? TimestampISO8601 { get; init; } = null;


		[GraphSkip]
		public static VoiceMailMessageRow ForDataReader(DbDataReader reader)
		{
			string? id = reader.IsDBNull("id") ? null : reader.GetString("id");
			string? callerIdNumber = reader.IsDBNull("callerIdNumber") ? null : reader.GetString("callerIdNumber");
			string? callerIdName = reader.IsDBNull("callerIdName") ? null : reader.GetString("callerIdName");
			string? timestampISO8601 = reader.IsDBNull("timestampISO8601") ? null : reader.GetString("timestampISO8601");

			return new()
			{
				Id = id,
				CallerIdNumber = callerIdNumber,
				CallerIdName = callerIdName,
				TimestampISO8601 = timestampISO8601,
			};
		}


		[GraphSkip]
		public static IEnumerable<VoiceMailMessageRow> All(SQLiteConnection DB)
		{
			if (DB == null)
				throw new Exception("DB == null");

			using SQLiteCommand command = DB.CreateCommand();

			string sql = @"
				SELECT 
					id,callerIdNumber,callerIdName,timestampISO8601
				FROM 
					messages
				ORDER BY timestampISO8601 DESC
			;";
			command.CommandText = sql;

			using SQLiteDataReader reader = command.ExecuteReader();
			while (reader.Read())
				yield return ForDataReader(reader);

			yield break;
		}
		[GraphSkip]
		public static List<VoiceMailMessageRow> ForIds(SQLiteConnection DB, IEnumerable<string> _ids)
		{
			List<VoiceMailMessageRow> res = new();

			if (DB == null)
				throw new Exception("DB == null");
			if (!_ids.Any())
				return res;

			string[] ids = _ids.ToArray();

			using SQLiteCommand command = DB.CreateCommand();

			List<string> paramNames = new();
			for (int i = 0; i < ids.Length; i++)
				paramNames.Add($"@param{i}");

			string sql = @"
				SELECT 
					id,callerIdNumber,callerIdName,timestampISO8601
				FROM 
					messages
				WHERE 
					id IN (" + string.Join(',', paramNames) + @")
				ORDER BY timestampISO8601 DESC
			;";
			command.CommandText = sql;


			for (int j = 0; j < ids.Length; j++)
			{
				string name = paramNames[j];
				string id = ids[j];

				SQLiteParameter param = new(name, System.Data.DbType.String)
				{
					Value = id
				};
				command.Parameters.Add(param);
			}

			using SQLiteDataReader reader = command.ExecuteReader();
			while (reader.Read())
				res.Add(ForDataReader(reader));


			return res;
		}

		[GraphSkip]
		public static int Remove(SQLiteConnection DB, string id)
		{
			if (DB == null)
				throw new Exception("DB == null");

			using SQLiteCommand command = DB.CreateCommand();

			string sql = @"
				DELETE FROM messages
				WHERE id=@id;
			;";
			command.CommandText = sql;

			// Params
			{
				SQLiteParameter param = new("@id", System.Data.DbType.String)
				{
					Value = id
				};
				command.Parameters.Add(param);
			}

			int rowsAffected = command.ExecuteNonQuery();
			if (rowsAffected == 0)
				throw new Exception("No calls were deleted.");
			return rowsAffected;
		}
		[GraphSkip]
		public static int RemoveAll(SQLiteConnection DB)
		{
			if (DB == null)
				throw new Exception("DB == null");

			using SQLiteCommand command = DB.CreateCommand();

			string sql = @"
				DELETE FROM messages
			;";
			command.CommandText = sql;

			int rowsAffected = command.ExecuteNonQuery();
			return rowsAffected;
		}
		[GraphSkip]
		public static void Upsert(SQLiteConnection DB, VoiceMailMessageRow row)
		{
			if (DB == null)
				throw new Exception("DB == null");

			using SQLiteCommand command = DB.CreateCommand();



			string sql = @"
				INSERT INTO messages
				(id,callerIdNumber,callerIdName,timestampISO8601,path) 
				VALUES
				(@id,@callerIdNumber,@callerIdName,@timestampISO8601)
				ON CONFLICT(id) DO UPDATE SET 
					callerIdNumber=excluded.callerIdNumber,
					callerIdName=excluded.callerIdName,
					timestampISO8601=excluded.timestampISO8601
			;";
			command.CommandText = sql;

			// Params
			{
				SQLiteParameter param = new("@id", DbType.String)
				{
					Value = row.Id
				};
				command.Parameters.Add(param);
			}
			{
				SQLiteParameter param = new("@callerIdNumber", DbType.String)
				{
					Value = row.CallerIdNumber
				};
				command.Parameters.Add(param);
			}
			{
				SQLiteParameter param = new("@callerIdName", DbType.String)
				{
					Value = row.CallerIdName
				};
				command.Parameters.Add(param);
			}
			{
				SQLiteParameter param = new("@timestampISO8601", DbType.String)
				{
					Value = row.TimestampISO8601
				};
				command.Parameters.Add(param);
			}
			



			var res = command.ExecuteScalar();

		}
	}
}
