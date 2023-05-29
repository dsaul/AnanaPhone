using DanSaul.SharedCode.Asterisk.AsteriskAEL.Statements;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;

namespace DanSaul.SharedCode.Asterisk.AsteriskAEL.Contexts
{
	public class AttendantDoYouAcceptTheCall : ContextBlock
	{
        public AttendantDoYouAcceptTheCall() : base("attendant-do-you-accept-the-call")
		{
			Extensions.Add(
				new ExtensionBlock("_.")
					.Add(new SetStatement("CALL_TARGET", "SeekOwnerOfCallRequest"))
					.Add(
						new WhileStatement("true")
							.Add(new AGIStatement(EnvAsterisk.ASTERISK_AGI_HOST, $"/{typeof(AttendantDoYouAcceptTheCall).Name}"))
							.Add(
								new IfStatementBlock(@"""${CALL_TARGET}"" = ""HangUp""")
									.Add(new HangUpStatement())
									.Add(new BreakStatement())
								)
						)
					.Add(new HangUpStatement())
			);


			Extensions.Add(
				new ExtensionBlock("abandoned")
					.Add(new PlaybackStatement("/etc/asterisk/_sounds/were-sorry-the-incoming-call-has-already-hung-up-goodbye"))
					.Add(new HangUpStatement())
			);
		}
    }
}
