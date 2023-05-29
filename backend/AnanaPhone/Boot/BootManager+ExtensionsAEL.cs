using AnanaPhone.AsteriskContexts;
using AnanaPhone.SettingsManager;
using DanSaul.SharedCode.Asterisk.AsteriskAEL;
using Serilog;
using System.Reflection;

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

			IEnumerable<E164Row> e164s = SM.E164sGetAll();
			foreach (E164Row e164 in e164s)
			{
				Outbound? context = Outbound.ForE164(e164);
				if (context == null)
					continue;

				file.Contexts.Add(context);
			}




			if (Env.GENERATE_ASTERISK_CONFIG)
			{
				string contents = file.Generate();

				//Log.Information("AEL: {output}", contents);
				File.WriteAllText("/etc/asterisk/extensions.ael", contents);
			}
		}
	}
}
