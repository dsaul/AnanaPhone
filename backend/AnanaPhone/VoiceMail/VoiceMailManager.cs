using Amazon.S3;
using Amazon.S3.Model;
using AnanaPhone.Calls;
using AnanaPhone.Extensions;
using Renci.SshNet.Security;
using Serilog;
using System.Collections.Generic;
using System.Data.SQLite;

namespace AnanaPhone.VoiceMail
{
	public class VoiceMailManager : IDisposable
	{
		AmazonS3Client S3Client;

		public VoiceMailManager(AmazonS3Client _S3Client)
        {
			S3Client = _S3Client;

			OpenDB();
			CreateTableIfNotExists();
		}

		const int databaseFormatVersion = 1;

		static string DBPath
		{
			get
			{
				return Path.Join(Env.SQLITE_DATABASE_DIRECTORY, $"voiceMail.{databaseFormatVersion}.sqlite");
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

			if (false == DB.TableExists("messages"))
			{
				string sql = @"
					CREATE TABLE messages (
						id TEXT, callerIdNumber TEXT, callerIdName TEXT, timestampISO8601 TEXT,
						CONSTRAINT messages_PK PRIMARY KEY (id)
					);
					CREATE UNIQUE INDEX messages_id_IDX ON messages (id);
					CREATE INDEX messages_callerIdNumber_IDX ON messages (callerIdNumber);
					CREATE INDEX messages_callerIdName_IDX ON messages (callerIdName);
					CREATE INDEX messages_timestampISO8601_IDX ON messages (timestampISO8601);
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


		public List<VoiceMailMessageRow> ForIds(IEnumerable<string> ids)
		{
			if (DB == null)
				throw new Exception("DB == null");
			List<VoiceMailMessageRow> res = VoiceMailMessageRow.ForIds(DB, ids);
			return res;
		}

		public void Upsert(VoiceMailMessageRow call)
		{
			if (DB == null)
				throw new Exception("DB == null");
			VoiceMailMessageRow.Upsert(DB, call);
		}

		public void Remove(string id)
		{
			if (DB == null)
				throw new Exception("DB == null");

			// first remove the database row
			VoiceMailMessageRow.Remove(DB, id);

			// delete the audio from s3
			DeleteObjectRequest request = new()
			{
				BucketName = Env.VMAIL_S3_BUCKET,
				Key = $"{id}/message.wav"
			};
			S3Client.DeleteObjectAsync(request).Wait();
		}

		public IEnumerable<VoiceMailMessageRow> GetAll()
		{
			if (DB == null)
				throw new Exception("DB == null");
			return VoiceMailMessageRow.All(DB);
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
		// ~VoiceMailManager()
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
