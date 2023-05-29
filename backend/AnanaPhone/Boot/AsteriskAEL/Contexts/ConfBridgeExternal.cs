using DanSaul.SharedCode.Asterisk.AsteriskAEL.Statements;

namespace DanSaul.SharedCode.Asterisk.AsteriskAEL.Contexts
{
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
