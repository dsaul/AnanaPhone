using System.Text;

namespace DanSaul.SharedCode.Asterisk.AsteriskAEL.Statements
{
	public class AGIStatement : Statement
	{
		public string Host { get; init; }
		public string Path { get; init; }

        public AGIStatement(string _Host, string _Path)
        {
			Host = _Host;
			Path = _Path;

		}

		public override string? Content
		{
			get
			{
				return $"AGI(agi://{Host}{Path})";
			}
		}
	}
}
