using System.Text;

namespace DanSaul.SharedCode.Asterisk.AsteriskAEL.Statements
{
	public class VerboseStatement : Statement
	{
		int VerbosityLevel { get; init; }
		string Message { get; init; }


		public VerboseStatement(string _Message, int _VerbosityLevel = 5)
        {
			Message = _Message;
			VerbosityLevel = _VerbosityLevel;
		}

		public override string? Content
		{
			get
			{
				return $"Verbose({VerbosityLevel},{Message})";
			}
		}
	}
}
