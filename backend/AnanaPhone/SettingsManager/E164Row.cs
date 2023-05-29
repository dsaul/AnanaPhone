using GraphQL.AspNet.Attributes;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Xml.Linq;

namespace AnanaPhone.SettingsManager
{
	public class E164Row
	{
		public string? Id
		{
			get
			{
				return E164;
			}
		}

		public string? E164 { get; init; }
		public string? Name { get; init; }
		public string? Comment { get; init; }
		public string? OutboundDevice { get; init; }
		public bool? Disabled { get; init; }

		[GraphSkip]
		public static E164Row ForDataReader(DbDataReader reader)
		{
			return new()
			{
				E164 = reader.IsDBNull("e164") ? null : reader.GetString("e164"),
				Name = reader.IsDBNull("name") ? null : reader.GetString("name"),
				Comment = reader.IsDBNull("comment") ? null : reader.GetString("comment"),
				OutboundDevice = reader.IsDBNull("outboundDevice") ? null : reader.GetString("outboundDevice"),
				Disabled = reader.IsDBNull("disabled") ? null : reader.GetBoolean("disabled"),
			};
		}
		[GraphSkip]
		public int Remove(SQLiteConnection DB)
		{
			if (DB == null)
				throw new Exception("DB == null");
			if (string.IsNullOrWhiteSpace(E164))
				return 0;
			return Remove(DB, E164);
		}
		[GraphSkip]
		public static int Remove(SQLiteConnection DB, string e164)
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
			return rowsAffected;
		}
		[GraphSkip]
		public int Upsert(SQLiteConnection DB)
		{
			if (DB == null)
				throw new Exception("DB == null");
			return Upsert(DB, this);
		}
		[GraphSkip]
		public static int Upsert(SQLiteConnection DB, E164Row number)
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
			return rowsAffected;
		}

		[GraphSkip]
		public static IEnumerable<E164Row> GetAll(SQLiteConnection DB)
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
				yield return ForDataReader(reader);

			yield break;
		}

		[GraphSkip]
		public static IEnumerable<E164Row> ForE164(SQLiteConnection DB, string e164)
		{
			if (DB == null)
				throw new Exception("DB == null");

			using SQLiteCommand command = DB.CreateCommand();

			string sql = @"
				SELECT 
					e164, name, comment, outboundDevice, disabled
				FROM 
					e164s
				WHERE
					e164 = @e164
				ORDER BY e164 DESC
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

			using SQLiteDataReader reader = command.ExecuteReader();
			while (reader.Read())
				yield return ForDataReader(reader);

			yield break;
		}

		[GraphSkip]
		public string? OutboundDeviceTechnology
		{
			get
			{
				if (string.IsNullOrWhiteSpace(OutboundDevice))
					return null;
				string[] parts = OutboundDevice.Split('/');
				if (parts.Length != 2)
					return null;
				return parts[0];
			}
		}


		[GraphSkip]
		public string? OutboundDeviceName
		{
			get
			{
				if (string.IsNullOrWhiteSpace(OutboundDevice))
					return null;
				string[] parts = OutboundDevice.Split('/');
				if (parts.Length != 2)
					return null;
				return parts[1];
			}
		}





	}
}
