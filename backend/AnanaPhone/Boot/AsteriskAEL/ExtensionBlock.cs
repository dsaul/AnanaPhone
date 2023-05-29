using DanSaul.SharedCode.Extensions;
using System.Text;

namespace DanSaul.SharedCode.Asterisk.AsteriskAEL
{
	public class ExtensionBlock : StatementBlock
	{
		public string Extension { get; init; }
		
		public ExtensionBlock(string _Extension)
        {
			Extension = _Extension;

		}

		public override ExtensionBlock Add(Statement statement)
		{
			base.Add(statement);
			return this;
		}

		public override string Generate(int indentLevel)
		{
			StringBuilder sb = new();
			sb.AppendLine($"{Tabs(indentLevel)}{Extension} => {{");
			indentLevel++;

			sb.AppendLine(base.Generate(indentLevel));

			indentLevel--;
			sb.AppendLine($"{Tabs(indentLevel)}}}");
			return sb.ToString();
		}

	}
}
