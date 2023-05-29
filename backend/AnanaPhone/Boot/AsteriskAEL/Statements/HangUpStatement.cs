using System.Text;

namespace DanSaul.SharedCode.Asterisk.AsteriskAEL.Statements
{
	public class HangUpStatement : Statement
	{
		public override string? Content
		{
			get
			{
				return "Hangup()";
			}
		}
	}
}
