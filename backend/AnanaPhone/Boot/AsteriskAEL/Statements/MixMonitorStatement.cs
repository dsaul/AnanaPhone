using System.Text;

namespace DanSaul.SharedCode.Asterisk.AsteriskAEL.Statements
{
	public class MixMonitorStatement : Statement
	{
		public string FileName { get; init; }
		public bool OnlySaveAudioWhileChannelIsBridged { get; init; } = true;

		public MixMonitorStatement(string _FileName)
		{
			FileName = _FileName;
		}

		public override string? Content
		{
			get
			{
				StringBuilder opts = new();
				if (OnlySaveAudioWhileChannelIsBridged)
					opts.Append('b');
				return $"MixMonitor({FileName},{opts})";
			}
		}
	}
}
