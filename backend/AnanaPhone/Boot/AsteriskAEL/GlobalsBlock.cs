using System.Text;

namespace DanSaul.SharedCode.Asterisk.AsteriskAEL
{
	public class GlobalsBlock : Block
	{
		public override string Generate(int indentLevel)
		{
			StringBuilder sb = new();
			sb.AppendLine($"{Tabs(indentLevel)}globals {{");
			sb.AppendLine(Tabs(indentLevel));
			sb.AppendLine($"{Tabs(indentLevel)}}}");
			return sb.ToString();
		}
	}
}
