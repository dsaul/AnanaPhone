
using System.Text;

namespace DanSaul.SharedCode.Asterisk.AsteriskAEL
{
	public class IfStatementBlock : StatementBlock
	{

        public string Predicate { get; init; }

        public IfStatementBlock(string _Predicate)
        {
			Predicate = _Predicate;

		}

		public override string Generate(int indentLevel)
		{
			StringBuilder sb = new();
			sb.AppendLine($"{Tabs(indentLevel)}if ({Predicate}) {{");
			indentLevel++;
			foreach (Statement statement in Statements)
				sb.AppendLine(statement.Generate(indentLevel));
			indentLevel--;
			sb.AppendLine($"{Tabs(indentLevel)}}}");
			return sb.ToString();
		}
	}
}
