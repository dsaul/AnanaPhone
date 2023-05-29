using DanSaul.SharedCode.Asterisk.AsteriskAEL;
using DanSaul.SharedCode.Asterisk.AsteriskAEL.Statements;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;

namespace AnanaPhone.AsteriskContexts
{
	[MarkContextIncluded]
	public class Outbound : ContextBlock
	{
		public Outbound() : base("outbound")
		{
			if (Program.Application == null)
				throw new InvalidOperationException("Program.Application == null");

			Includes.Add(Program.Application.Services.GetRequiredService<Inbound>());
			Includes.Add(Program.Application.Services.GetRequiredService<Extensions>());
			Includes.Add(Program.Application.Services.GetRequiredService<FAC>());
			Includes.Add(Program.Application.Services.GetRequiredService<Conference>());


			Extensions.Add(
				new ExtensionBlock("_NXXXXXX")
					.Add(new GoToStatement(this, $"1{EnvAsterisk.ASTERISK_HOME_AREA_CODE}${{EXTEN}}"))
			);


			Extensions.Add(
				new ExtensionBlock("_NXXNXXXXXX")
					.Add(new GoToStatement(this, "1${EXTEN}"))
			);

			Extensions.Add(
				new ExtensionBlock("_1NXXNXXXXXX")
					.Add(new GoToStatement(this, "+${EXTEN}"))
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
					.Add(new GoToStatement(Program.Application.Services.GetRequiredService<AttendantFromExternal>(), "12049778449"))
			);
		}
	}
}
