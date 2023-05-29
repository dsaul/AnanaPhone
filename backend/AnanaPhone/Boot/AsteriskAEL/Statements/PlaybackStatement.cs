using System.Text;

namespace DanSaul.SharedCode.Asterisk.AsteriskAEL.Statements
{
	public class PlaybackStatement : Statement
	{
		public string File { get; init; }

        public PlaybackStatement(string _File)
        {
			File = _File;
		}

		public override string? Content
		{
			get
			{
				return $"Playback({File})";
			}
		}
	}
}
