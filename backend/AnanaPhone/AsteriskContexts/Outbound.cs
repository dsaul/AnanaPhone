using AnanaPhone.SettingsManager;
using DanSaul.SharedCode.Asterisk.AsteriskAEL;
using DanSaul.SharedCode.Asterisk.AsteriskAEL.Statements;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;

namespace AnanaPhone.AsteriskContexts
{
	public class Outbound : ContextBlock, IDisposable
	{
		private bool disposedValue;

		public static List<Outbound> Instances { get; } = new();

		public static Outbound? ForE164(string e164)
		{
			Outbound? instance = Instances.Find((el) => el?.E164?.E164 == e164);
			if (instance != null)
				return instance;

			SettingsManager.Manager? SM = Program.Application?.Services.GetRequiredService<SettingsManager.Manager>();
			if (SM == null)
				throw new InvalidOperationException("SM == null");

			E164Row? row = SM.ForE164(e164).FirstOrDefault();
			if (row == null)
				return null;

			instance = new(row);
			Instances.Add(instance);
			return instance;
		}

		public static Outbound? ForE164(E164Row e164)
		{
			if (string.IsNullOrWhiteSpace(e164.E164))
				throw new InvalidOperationException("string.IsNullOrWhiteSpace(e164.E164)");

			Outbound? instance = Instances.Find((el) => el?.E164?.E164 == e164?.E164);
			if (instance != null)
				return instance;

			instance = new(e164);
			Instances.Add(instance);
			return instance;
		}

		E164Row E164 { get; init; } = new E164Row();

		protected Outbound(E164Row e164) : base($"outbound_{e164.E164}")
		{
			if (Program.Application == null)
				throw new InvalidOperationException("Program.Application == null");
			if (string.IsNullOrWhiteSpace(e164.E164))
				throw new InvalidOperationException("string.IsNullOrWhiteSpace(e164.E164)");

			E164 = e164;

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
					.Add(
					  new MixMonitorStatement(EnvAsterisk.ASTERISK_RECORDINGS_DIRECTORY + @"/${EXTEN}/Outbound-${STRFTIME(${EPOCH},,%Y-%m-%d %H-%M-%S)}.wav")
					)
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

		#region IDisposable
		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)

					Instances.Remove(this);
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				disposedValue = true;
			}
		}

		// // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		// ~Outbound()
		// {
		//     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		//     Dispose(disposing: false);
		// }

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		#endregion

	}
}
