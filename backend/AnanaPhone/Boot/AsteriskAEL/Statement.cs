using System.Text;

namespace DanSaul.SharedCode.Asterisk.AsteriskAEL
{
	public class Statement : Block
	{
		public virtual string? Content { get; init; }

        protected Statement()
        {
            
        }
        public Statement(string? _Content)
        {
			Content = _Content;
		}

		public override string Generate(int indentLevel)
		{
			return $"{Tabs(indentLevel)}{Content};";
		}
	}
}
