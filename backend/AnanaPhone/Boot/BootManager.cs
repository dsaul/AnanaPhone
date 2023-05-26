using AnanaPhone.Calls;
using PasswordGenerator;
using Serilog;
using System.Reflection;

namespace AnanaPhone.Boot
{
	public partial class BootManager : IDisposable
	{
		[AttributeUsage(AttributeTargets.Method)]
		public class ConfGeneratorAttribute : Attribute { }

		SettingsManager.Manager SM { get; init; }

		static string PasswordForToday { get; } = new Password(
			includeLowercase: true, 
			includeUppercase: true, 
			includeNumeric: true, 
			includeSpecial: false, 
			passwordLength: 20
		).Next();

		public BootManager(SettingsManager.Manager _SM)
        {
			SM = _SM;
		}

		public void Run()
		{
			// Generate config files.
			Log.Information("[{Class}.{Method}()]",
				GetType().Name,
				System.Reflection.MethodBase.GetCurrentMethod()?.Name
			);

			// Pass AMI password onto Serve
			string amiPWPath = "/tmp/AMI_PW";
			if (File.Exists(amiPWPath))
				File.Delete(amiPWPath);

			if (File.Exists(amiPWPath))
				throw new Exception("Something odd is going on here");

			Log.Debug("Today's Password {PW}", PasswordForToday);
			File.WriteAllText(amiPWPath, PasswordForToday);

			Type type = GetType();
			MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			foreach (MethodInfo method in methods)
				if (method.GetCustomAttributes(typeof(ConfGeneratorAttribute), true).Length > 0)
					method.Invoke(this, null);


		}


		

        #region IDisposable

        private bool disposedValue;

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
		// ~BootManager()
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
