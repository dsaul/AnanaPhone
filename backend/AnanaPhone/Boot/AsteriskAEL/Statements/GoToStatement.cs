namespace DanSaul.SharedCode.Asterisk.AsteriskAEL.Statements
{
	public class GoToStatement : Statement
	{
		public string Context { get; init; }
		public string Extension { get; init; }
		public string? Priority { get; init; } = "1";


		public override string? Content { 
			get {
				return $"goto {Context},{Extension},{Priority}";
			} 
		}

		public GoToStatement(string _Context, string _Extension)
        {
			Context = _Context;
			Extension = _Extension;
		}

		public GoToStatement(ContextBlock _Context, string _Extension)
		{
			Context = _Context.ContextName;
			Extension = _Extension;
		}
    }
}
