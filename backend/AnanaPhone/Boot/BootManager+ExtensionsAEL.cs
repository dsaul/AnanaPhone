using AnanaPhone.AsteriskContexts;
using DanSaul.SharedCode.Asterisk.AsteriskAEL;
using DanSaul.SharedCode.Asterisk.AsteriskINI;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;
using Serilog;
using System.Reflection;
using System.Runtime.InteropServices;

namespace AnanaPhone.Boot
{
    public partial class BootManager : IDisposable
	{
		[ConfGenerator]
		[RuntimeReloadable]
		public void GenerateExtensionsAEL()
		{
			if (Program.Application == null)
				throw new InvalidOperationException("Program.Application == null");

			// extensions.ael
			Log.Information("[{Class}.{Method}()]",
				GetType().Name,
				System.Reflection.MethodBase.GetCurrentMethod()?.Name
			);

			AsteriskAELFile file = new();

			// Get all the contexts in the main assembly that have been marked by attribute to be included.
			Assembly assembly = Assembly.GetExecutingAssembly();
			Type[] types = assembly.GetTypes();
			IEnumerable<Type> classesWithAttribute = types.Where(t => t.GetCustomAttributes(typeof(MarkContextIncludedAttribute), true).Any());
			foreach (Type c in classesWithAttribute)
			{
				if (Program.Application.Services.GetRequiredService(c) is not ContextBlock instance)
					continue;

				file.Contexts.Add(instance);
			}

			// MixMonitor("/etc/asterisk/_monitors/${EXTEN}/Outbound-${STRFTIME(${EPOCH},,%Y-%m-%d %H-%M-%S)}.wav",b);

			if (EnvAsterisk.ASTERISK_DEBUG_SSH_ENABLE)
			{
				Log.Information("[{Class}.{Method}()] Running remotely, skipping writing conf file.",
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name
				);
			}
			else
			{
				string contents = file.Generate();

				//Log.Information("AEL: {output}", contents);
				File.WriteAllText("/etc/asterisk/extensions.ael", contents);
			}
		}
	}
}
