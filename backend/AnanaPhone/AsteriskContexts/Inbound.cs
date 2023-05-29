using AnanaPhone.SettingsManager;
using DanSaul.SharedCode.Asterisk.AsteriskAEL;
using DanSaul.SharedCode.Asterisk.AsteriskAEL.Statements;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;

namespace AnanaPhone.AsteriskContexts
{
	[MarkContextIncluded]
	public class Inbound : ContextBlock
	{
		public Manager? SM = Program.Application?.Services.GetRequiredService<Manager>();

		public Inbound() : base("inbound")
		{
			if (Program.Application == null)
				throw new InvalidOperationException("Program.Application == null");
			if (SM == null)
				throw new InvalidOperationException("SM == null");

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

			IEnumerable<E164Row> rows = SM.E164sGetAll();
			foreach (E164Row row in rows)
			{
				if (string.IsNullOrWhiteSpace(row.E164))
					continue;

				Extensions.Add(
					new ExtensionBlock(row.E164)
						.Add(new GoToStatement(Program.Application.Services.GetRequiredService<AttendantFromExternal>(), "${EXTEN}"))
				);
			}





		}
	}
}
