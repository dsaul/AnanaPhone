namespace DanSaul.SharedCode.Asterisk.AsteriskAEL.Statements
{
	public class NoOpStatement : Statement
	{
		string Message { get; init; }

		public NoOpStatement(string _Message, int _VerbosityLevel = 5)
		{
			Message = _Message;
		}

		public override string? Content
		{
			get
			{
				return $"NoOp({Message})";
			}
		}
	}
}
