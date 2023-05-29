using AnanaPhone.AsteriskContexts;
using Serilog;
using System.Data.SQLite;

namespace AnanaPhone.SettingsManager
{
	public partial class Manager : IDisposable
	{
		public Manager()
		{
			OpenDB();
			E164sEnsureTable();
			PJSIPWizardConfEnsureTable();
			ARIConfEnsureTable();
			MusicOnHoldConfEnsureTable();
			RTPConfEnsureTable();
			E164ClientsEnsureTable();
		}

		const int databaseFormatVersion = 1;

		static string DBPath
		{
			get
			{
				return Path.Join(Env.SQLITE_DATABASE_DIRECTORY, $"settings.{databaseFormatVersion}.sqlite");
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

		public IEnumerable<string> GetClientChannels()
		{



			string callOutE164 = Env.SEEK_OWNER_FROM_E164;
			E164Row? row = E164sGetForE164(callOutE164).FirstOrDefault();
			if (row == null)
				throw new InvalidOperationException("row == null");

			Outbound? obCtx = Outbound.ForE164(row);
			if (obCtx == null)
				throw new InvalidOperationException("obCtx == null");




			IEnumerable<E164ClientRow> e164Clients = E164ClientsGetAll();
			foreach (E164ClientRow client in e164Clients)
			{
				if (string.IsNullOrWhiteSpace(client.E164))
					continue;
				
				yield return $"Local/{client.E164.Trim()}@{obCtx.ContextName}";
			}

			IEnumerable<string?> pjsipClients = PJSIPWizardConfGetNamesForClientDefaults();
			foreach (string? pjsipClient in pjsipClients)
			{
				yield return $"PJSIP/{pjsipClient}";
			}

			yield break;
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
		// ~SettingsManager()
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
