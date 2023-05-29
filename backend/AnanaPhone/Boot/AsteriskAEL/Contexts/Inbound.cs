using AnanaPhone;
using AnanaPhone.SettingsManager;
using DanSaul.SharedCode.Asterisk.AsteriskAEL.Statements;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;

namespace DanSaul.SharedCode.Asterisk.AsteriskAEL.Contexts
{
	public class Inbound : ContextBlock
	{
		public Manager? SM = Program.Application?.Services.GetRequiredService<Manager>();

		public Inbound() : base("inbound")
		{
			if (SM == null)
				throw new InvalidOperationException("SM == null");

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

			IEnumerable<E164Row> rows = SM.E164sGetAll();
			foreach (E164Row row in rows)
			{
				if (string.IsNullOrWhiteSpace(row.E164))
					continue;

				Extensions.Add(
					new ExtensionBlock(row.E164)
						.Add(new GoToStatement("attendant-from-external", "${EXTEN}"))
				);
			}





		}
    }
}
