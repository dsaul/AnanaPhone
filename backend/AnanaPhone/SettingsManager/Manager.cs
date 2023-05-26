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
			IEnumerable<E164ClientRow> e164Clients = E164ClientsGetAll();
			foreach (E164ClientRow client in e164Clients)
			{
				if (string.IsNullOrWhiteSpace(client.E164))
					continue;
				if (string.IsNullOrWhiteSpace(client.OutboundDevice))
				{
					Log.Warning("[{Class}.{Method}()] Number Client {e164} is missing outbound device.",
						GetType().Name,
						System.Reflection.MethodBase.GetCurrentMethod()?.Name,
						client.E164
					);
					continue;
				}
				if (!client.OutboundDevice.Contains('/'))
				{
					Log.Warning("[{Class}.{Method}()] Number Client {e164} is not formatted correctly #1 {outboundDevice}.",
						GetType().Name,
						System.Reflection.MethodBase.GetCurrentMethod()?.Name,
						client.E164,
						client.OutboundDevice
					);
					continue;
				}
				string[] parts = client.OutboundDevice.Split('/');
				if (parts.Length != 2)
				{
					Log.Warning("[{Class}.{Method}()] Number Client {e164} is not formatted correctly #2 {outboundDevice}.",
						GetType().Name,
						System.Reflection.MethodBase.GetCurrentMethod()?.Name,
						client.E164,
						client.OutboundDevice
					);
					continue;
				}

				string tech = parts[0];
				string name = parts[1];

				yield return $"{tech}/{client.E164.Trim()}@{name}";
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
