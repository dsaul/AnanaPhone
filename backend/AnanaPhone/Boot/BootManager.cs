using AnanaPhone.Calls;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;
using PasswordGenerator;
using Serilog;
using System.Reflection;

namespace AnanaPhone.Boot
{
	public partial class BootManager : IDisposable
	{
		[AttributeUsage(AttributeTargets.Method)]
		public class ConfGeneratorAttribute : Attribute { }
		[AttributeUsage(AttributeTargets.Method)]
		public class RuntimeReloadableAttribute : Attribute { }

		SettingsManager.Manager SM { get; init; }

		private string _PasswordForToday = new Password(
					includeLowercase: true,
					includeUppercase: true,
					includeNumeric: true,
					includeSpecial: false,
					passwordLength: 20
				).Next();
		string PasswordForToday
		{
			get
			{
				if (EnvAsterisk.ASTERISK_DEBUG_SSH_ENABLE)
				{
					return Env.AMI_PW;
				}
				else
				{
					return _PasswordForToday;
				}
			}
		}

		public BootManager(SettingsManager.Manager _SM)
        {
			SM = _SM;
		}

		public void GenerateForStage1()
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

		/// <summary>
		/// Parts of asterisk can be reloaded during runtime, but not all.
		/// </summary>
		public void GenerateForStage2()
		{
			Type type = GetType();
			MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			foreach (MethodInfo method in methods)
				if (method.GetCustomAttributes(typeof(ConfGeneratorAttribute), true).Length > 0 &&
					method.GetCustomAttributes(typeof(RuntimeReloadableAttribute), true).Length > 0
					)
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
