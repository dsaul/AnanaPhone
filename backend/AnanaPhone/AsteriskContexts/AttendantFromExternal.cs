using DanSaul.SharedCode.Asterisk.AsteriskAEL;
using DanSaul.SharedCode.Asterisk.AsteriskAEL.Statements;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;

namespace AnanaPhone.AsteriskContexts
{
	[MarkContextIncluded]
	public class AttendantFromExternal : ContextBlock
	{
		public AttendantFromExternal() : base("attendant-from-external")
		{
			Extensions.Add(
				new ExtensionBlock("_.")
					.Add(new SetStatement("LANDED_DID", "${EXTEN}"))
					.Add(new SetStatement("CALL_TARGET", "SeekOwnerOfCallRequest"))
					.Add(
						new WhileStatement("true")
							.Add(new AGIStatement(EnvAsterisk.ASTERISK_AGI_HOST, $"/{typeof(AttendantFromExternal).Name}"))
							.Add(
								new IfStatementBlock(@"""${CALL_TARGET}"" = ""HangUp""")
									.Add(new HangUpStatement())
									.Add(new BreakStatement())
								)
						)
					.Add(new HangUpStatement())
			);

		}
	}
}
