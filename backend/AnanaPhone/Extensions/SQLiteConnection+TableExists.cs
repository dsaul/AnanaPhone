using System.Data.SQLite;

namespace AnanaPhone.Extensions
{
	public static class SQLiteConnection_TableExists
	{
		public static bool TableExists(this SQLiteConnection connection, string name)
		{
			using SQLiteCommand command = connection.CreateCommand();
			command.CommandText = "SELECT name FROM sqlite_master WHERE name=@name";

			SQLiteParameter nameParam = new("@name", System.Data.DbType.String)
			{
				Value = name
			};
			command.Parameters.Add(nameParam);

			var res = command.ExecuteScalar();
			return res != null && res.ToString() == name;
		}
	}
}
