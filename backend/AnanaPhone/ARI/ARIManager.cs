using Serilog;
using AsterNET.FastAGI;
using AsterNET.FastAGI.MappingStrategies;
using AnanaPhone.ARI.Attendants;

namespace AnanaPhone.ARI
{
	public class ARIManager : IDisposable
	{
		
		private bool disposedValue;

		private AsteriskFastAGI AGI { get; init; } = new();
		
		public ARIManager()
		{
			AGI.MappingStrategy = new GeneralMappingStrategy(new List<ScriptMapping>()
			{
				new ScriptMapping() {
					ScriptClass = typeof(AttendantFromExternal).FullName ?? "",
					ScriptName = "AttendantFromExternal",
				},
				new ScriptMapping()
				{
					ScriptClass =  typeof(AttendantDoYouAcceptTheCall).FullName ?? "",
					ScriptName = "AttendantDoYouAcceptTheCall",
				},
				new ScriptMapping() {
					ScriptClass = typeof(AttendantTrackedAdminDirectToConference).FullName ?? "",
					ScriptName = "AttendantTrackedAdminDirectToConference",
				},
				new ScriptMapping() {
					ScriptClass = typeof(AttendantTrackedExternalDirectToConference).FullName ?? "",
					ScriptName = "AttendantTrackedExternalDirectToConference",
				},
			});

			AGI.SC511_CAUSES_EXCEPTION = true;
			AGI.SCHANGUP_CAUSES_EXCEPTION = true;


			Task.Run(() =>
			{
				while (true)
				{
					try
					{
						Log.Information("[{Class}.{Method}()] Ready for calls.",
							GetType().Name,
							System.Reflection.MethodBase.GetCurrentMethod()?.Name
						);
						AGI.Start();
					}
					catch (Exception e)
					{
						Log.Error(e, "[{Class}.{Method}()] {Message}",
							GetType().Name,
							System.Reflection.MethodBase.GetCurrentMethod()?.Name,
							e.Message
						);
					}
				}
				
				
			});

		}

		#region IDisposable
		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				disposedValue = true;
			}
		}

		// // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		// ~ARI()
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
