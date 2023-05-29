
using System.Text;

namespace DanSaul.SharedCode.Asterisk.AsteriskAEL
{
	public class WhileStatement : StatementBlock
	{
		public string Predicate { get; init; }

		public WhileStatement(string _Predicate)
		{
			Predicate = _Predicate;

		}

		public override string Generate(int indentLevel)
		{
			StringBuilder sb = new();
			sb.AppendLine($"{Tabs(indentLevel)}while ({Predicate}) {{");
			indentLevel++;
			foreach (Statement statement in Statements)
				sb.AppendLine(statement.Generate(indentLevel));
			indentLevel--;
			sb.AppendLine($"{Tabs(indentLevel)}}}");
			return sb.ToString();
		}
	}
}
