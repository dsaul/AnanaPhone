using DanSaul.SharedCode.Asterisk.AsteriskAEL;
using DanSaul.SharedCode.Asterisk.AsteriskAEL.Statements;

namespace AnanaPhone.AsteriskContexts
{
	[MarkContextIncluded]
	public class ConfBridgeExternal : ContextBlock
	{
		public ConfBridgeExternal() : base("confbridge-external")
		{
			Extensions.Add(
				new ExtensionBlock("_.")
					.Add(new ConfBridgeStatement("${EXTEN}", "default_user"))
					.Add(new HangUpStatement())
			);
		}
	}
}
