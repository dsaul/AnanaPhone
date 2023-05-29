using System.Text;

namespace DanSaul.SharedCode.Asterisk.AsteriskAEL
{
	public class StatementBlock : Statement
	{
		public List<Statement> Statements { get; init; } = new();

		public StatementBlock() : base()
		{
			
		}

		public virtual StatementBlock Add(Statement statement)
		{
			Statements.Add(statement);
			return this;
		}

		public override string Generate(int indentLevel)
		{
			StringBuilder sb = new();

			foreach (Statement statement in Statements)
				sb.AppendLine(statement.Generate(indentLevel));
			return sb.ToString();
		}

		public override string? Content { 
			get
			{
				throw new NotImplementedException();
			}
		}
	}
}
