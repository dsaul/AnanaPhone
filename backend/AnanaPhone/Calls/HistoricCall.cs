using GraphQL.AspNet.Attributes;
using System.Data;
using System.Data.SQLite;

namespace AnanaPhone.Calls
{
	public class HistoricCall
	{
		public string? Id { get; init; } = Guid.NewGuid().ToString();

		public string? CallerIdName { get; init; }

		public string? CallerIdNumber { get; init; }

		public string? Duration { get; init; }

		public string? TimestampISO8601 { get; init; }

		public string? LandedDID { get; init; }
		public string? OriginalChannel { get; init; }
		public string? CallDirection { get; init; }

		[GraphSkip]
		public static IEnumerable<HistoricCall> All(SQLiteConnection DB)
		{
			if (DB == null)
				throw new Exception("DB == null");

			using SQLiteCommand command = DB.CreateCommand();

			string sql = @"
				SELECT 
					id,callerIdName,callerIdNumber,duration,timestampISO8601,landedDID,originalChannel,callDirection 
				FROM 
					calls
				ORDER BY timestampISO8601 DESC
			;";
			command.CommandText = sql;

			using SQLiteDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
				string? id = reader.IsDBNull("id") ? null : reader.GetString("id");
				string? callerIdName = reader.IsDBNull("callerIdName") ? null : reader.GetString("callerIdName");
				string? callerIdNumber = reader.IsDBNull("callerIdNumber") ? null : reader.GetString("callerIdNumber");
				string? duration = reader.IsDBNull("duration") ? null : reader.GetString("duration");
				string? timestampISO8601 = reader.IsDBNull("timestampISO8601") ? null : reader.GetString("timestampISO8601");
				string? landedDID = reader.IsDBNull("landedDID") ? null : reader.GetString("landedDID");
				string? originalChannel = reader.IsDBNull("originalChannel") ? null : reader.GetString("originalChannel");
				string? callDirection = reader.IsDBNull("callDirection") ? null : reader.GetString("callDirection");

				yield return new()
				{
					Id = id,
					CallerIdName = callerIdName,
					CallerIdNumber = callerIdNumber,
					Duration = duration,
					TimestampISO8601 = timestampISO8601,
					LandedDID = landedDID,
					OriginalChannel = originalChannel,
					CallDirection = callDirection,
				};
			}

			yield break;
		}
		[GraphSkip]
		public static int Remove(SQLiteConnection DB, string id)
		{
			if (DB == null)
				throw new Exception("DB == null");

			using SQLiteCommand command = DB.CreateCommand();

			string sql = @"
				DELETE FROM calls
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
				DELETE FROM calls
			;";
			command.CommandText = sql;

			int rowsAffected = command.ExecuteNonQuery();
			return rowsAffected;
		}
		[GraphSkip]
		public static void Upsert(SQLiteConnection DB, HistoricCall call)
		{
			if (DB == null)
				throw new Exception("DB == null");

			using SQLiteCommand command = DB.CreateCommand();



			string sql = @"
				INSERT INTO calls
				(id, callerIdName, callerIdNumber, duration, timestampISO8601, landedDID, originalChannel, callDirection) 
				VALUES
				(@id, @callerIdName,@callerIdNumber,@duration,@timestampISO8601,@landedDID,@originalChannel,@callDirection)
				ON CONFLICT(id) DO UPDATE SET 
					callerIdName=excluded.callerIdName,
					callerIdNumber=excluded.callerIdNumber,
					duration=excluded.duration,
					timestampISO8601=excluded.timestampISO8601,
					landedDID=excluded.landedDID,
					originalChannel=excluded.originalChannel,
					callDirection=excluded.callDirection
			;";
			command.CommandText = sql;

			// Params
			{
				SQLiteParameter param = new("@id", DbType.String)
				{
					Value = call.Id.ToString()
				};
				command.Parameters.Add(param);
			}
			{
				SQLiteParameter param = new("@callerIdName", DbType.String)
				{
					Value = call.CallerIdName
				};
				command.Parameters.Add(param);
			}
			{
				SQLiteParameter param = new("@callerIdNumber", DbType.String)
				{
					Value = call.CallerIdNumber
				};
				command.Parameters.Add(param);
			}
			{
				SQLiteParameter param = new("@duration", DbType.String)
				{
					Value = call.Duration
				};
				command.Parameters.Add(param);
			}
			{
				SQLiteParameter param = new("@timestampISO8601", DbType.String)
				{
					Value = call.TimestampISO8601
				};
				command.Parameters.Add(param);
			}
			{
				SQLiteParameter param = new("@landedDID", DbType.String)
				{
					Value = call.LandedDID
				};
				command.Parameters.Add(param);
			}
			{
				SQLiteParameter param = new("@originalChannel", DbType.String)
				{
					Value = call.OriginalChannel
				};
				command.Parameters.Add(param);
			}
			{
				SQLiteParameter param = new("@callDirection", DbType.String)
				{
					Value = call.CallDirection
				};
				command.Parameters.Add(param);
			}



			var res = command.ExecuteScalar();

		}






	}
}
