using DanSaul.SharedCode.Asterisk.AsteriskAEL;
using DanSaul.SharedCode.Asterisk.AsteriskAEL.Statements;

namespace AnanaPhone.AsteriskContexts
{
	[MarkContextIncluded]
	public class ConfBridgeAdmin : ContextBlock
	{
		public ConfBridgeAdmin() : base("confbridge-admin")
		{
			Extensions.Add(
				new ExtensionBlock("_.")
					.Add(new ConfBridgeStatement("${EXTEN}", "admin_user"))
					.Add(new HangUpStatement())
			);
		}
	}
}
