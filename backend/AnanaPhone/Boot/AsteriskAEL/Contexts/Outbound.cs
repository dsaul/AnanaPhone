using DanSaul.SharedCode.Asterisk.AsteriskAEL.Statements;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;
using Square.Models;

namespace DanSaul.SharedCode.Asterisk.AsteriskAEL.Contexts
{
	public class Outbound : ContextBlock
	{
        public Outbound() : base("outbound")
		{
			Extensions.Add(
				new ExtensionBlock("_NXXXXXX")
					.Add(new GoToStatement(ContextName, $"1{EnvAsterisk.ASTERISK_HOME_AREA_CODE}${{EXTEN}}"))
			);


			Extensions.Add(
				new ExtensionBlock("_NXXNXXXXXX")
					.Add(new GoToStatement(ContextName, "1${EXTEN}"))
			);

			Extensions.Add(
				new ExtensionBlock("_1NXXNXXXXXX")
					.Add(new GoToStatement(ContextName, "+${EXTEN}"))
			);

			Extensions.Add(
				new ExtensionBlock("_+1NXXNXXXXXX")
					.Add(new VerboseStatement("Dialed ${EXTEN}"))
					.Add(new NoOpStatement(@"Outbound Caller id name ${CALLERID(name)}"))
					.Add(new NoOpStatement(@"Outbound Caller id number ${CALLERID(num)}"))
					.Add(new DialStatement(new List<DialEndpoint>()
					{
						new DialEndpoint()
						{
							Technology = "PJSIP",
							Extension = "${EXTEN}",
							Device = "trunk-ob-twilio",
						}
					})
					{
						Timeout = 40,
					})
			);

			Extensions.Add(
				new ExtensionBlock("111")
					.Add(new GoToStatement("attendant-from-external", "12049778449"))
			);
		}
    }
}
