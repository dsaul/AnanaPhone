using DanSaul.SharedCode.Asterisk.AsteriskAEL.Statements;

namespace DanSaul.SharedCode.Asterisk.AsteriskAEL.Contexts
{
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
