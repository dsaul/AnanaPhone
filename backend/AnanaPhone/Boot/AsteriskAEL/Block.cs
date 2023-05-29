using System.Text;

namespace DanSaul.SharedCode.Asterisk.AsteriskAEL
{
	[Block]
	public class Block
	{
		public virtual string Generate(int indentLevel)
		{
			StringBuilder sb = new();
			sb.AppendLine("{");
			sb.AppendLine(Tabs(indentLevel));
			sb.AppendLine("}");
			return sb.ToString();
		}

		protected string Tabs(int indentLevel) 
		{
			StringBuilder sb = new();
			for (int i = 0; i < indentLevel; i++)
				sb.Append('\t');

			return sb.ToString();
		}


	}
}
