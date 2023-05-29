using DanSaul.SharedCode.Asterisk.AsteriskAEL;
using DanSaul.SharedCode.Asterisk.AsteriskAEL.Contexts;
using DanSaul.SharedCode.Asterisk.AsteriskINI;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;
using Serilog;

namespace AnanaPhone.Boot
{
    public partial class BootManager : IDisposable
	{
		[ConfGenerator]
		[RuntimeReloadable]
		public void GenerateExtensionsAEL()
		{
			// extensions.ael

			Log.Information("[{Class}.{Method}()]",
				GetType().Name,
				System.Reflection.MethodBase.GetCurrentMethod()?.Name
			);

			ConfBridgeAdmin confBridgeAdminCtx = new();
			ConfBridgeExternal confBridgeExternalCtx = new();
			Inbound inboundCtx = new();
			DanSaul.SharedCode.Asterisk.AsteriskAEL.Contexts.Extensions extensionsCtx = new();
			FAC facCtx = new();
			Conference conferenceCtx = new();
			AttendantFromExternal attendantFromExternalCtx = new();
			AttendantDoYouAcceptTheCall attendantDoYouAcceptTheCallCtx = new();
			AttendantTrackedAdminDirectToConference attendantTrackedAdminDirectToConferenceCtx = new();
			AttendantTrackedExternalDirectToConference attendantTrackedExternalDirectToConferenceCtx = new();
			OutboundMonitor outboundMonitorCtx = new();






			Outbound outboundCtx = new();
			outboundCtx.Includes.Add(inboundCtx);
			outboundCtx.Includes.Add(extensionsCtx);
			outboundCtx.Includes.Add(facCtx);
			outboundCtx.Includes.Add(conferenceCtx);

			ContextBlock bogusCtx = new("bogus");
			bogusCtx.Includes.Add(extensionsCtx);
			bogusCtx.Includes.Add(inboundCtx);
			bogusCtx.Includes.Add(outboundCtx);
			bogusCtx.Includes.Add(facCtx);
			bogusCtx.Includes.Add(conferenceCtx);
			bogusCtx.Includes.Add(attendantFromExternalCtx);
			bogusCtx.Includes.Add(attendantDoYouAcceptTheCallCtx);
			bogusCtx.Includes.Add(attendantTrackedAdminDirectToConferenceCtx);
			bogusCtx.Includes.Add(attendantTrackedExternalDirectToConferenceCtx);
			bogusCtx.Includes.Add(confBridgeAdminCtx);
			bogusCtx.Includes.Add(confBridgeExternalCtx);

			GlobalsBlock globals = new();
			AsteriskAELFile file = new()
			{
				Globals = globals,
			};

			file.Contexts.Add(confBridgeAdminCtx);
			file.Contexts.Add(confBridgeExternalCtx);
			file.Contexts.Add(inboundCtx);
			file.Contexts.Add(extensionsCtx);
			file.Contexts.Add(facCtx);
			file.Contexts.Add(conferenceCtx);
			file.Contexts.Add(attendantFromExternalCtx);
			file.Contexts.Add(attendantDoYouAcceptTheCallCtx);
			file.Contexts.Add(attendantTrackedAdminDirectToConferenceCtx);
			file.Contexts.Add(attendantTrackedExternalDirectToConferenceCtx);
			file.Contexts.Add(outboundMonitorCtx);
			file.Contexts.Add(outboundCtx);
			file.Contexts.Add(bogusCtx);


			


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
