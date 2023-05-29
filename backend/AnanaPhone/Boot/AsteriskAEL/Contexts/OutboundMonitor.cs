using DanSaul.SharedCode.Asterisk.AsteriskAEL.Statements;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;

namespace DanSaul.SharedCode.Asterisk.AsteriskAEL.Contexts
{
	public class OutboundMonitor : ContextBlock
	{
		public OutboundMonitor() : base("outbound-monitor")
		{
			Extensions.Add(
				new ExtensionBlock("_NXXXXXX")
					.Add(
						new GoToStatement(ContextName, $"1{EnvAsterisk.ASTERISK_HOME_AREA_CODE}${{EXTEN}}")
					)
			);

			Extensions.Add(
				new ExtensionBlock("_NXXNXXXXXX")
					.Add(
					  new GoToStatement(ContextName, "1${EXTEN}")
				)
			);

			Extensions.Add(
				new ExtensionBlock("_1NXXNXXXXXX")
					.Add(new GoToStatement(ContextName, "+${EXTEN}")
				)
			);

			Extensions.Add(
				new ExtensionBlock("_.X")
					.Add(
						new IfStatementBlock(@"""${EXTEN}"" = """"")
							.Add(new Statement(@"Set(EXTEN=""Unknown"")"))
					)
					.Add(
					  new MixMonitorStatement(EnvAsterisk.ASTERISK_RECORDINGS_DIRECTORY + @"/${EXTEN}/Outbound-${STRFTIME(${EPOCH},,%Y-%m-%d %H-%M-%S)}.wav")
					)
					.Add(
						new GoToStatement("outbound", "${EXTEN}")
					)
			);
		}
	}
}
