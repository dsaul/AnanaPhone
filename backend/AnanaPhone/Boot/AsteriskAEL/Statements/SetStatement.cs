using Renci.SshNet.Security;
using System.Text;

namespace DanSaul.SharedCode.Asterisk.AsteriskAEL.Statements
{
	public class SetStatement : Statement
	{
		public string Key { get; init; }
		public string Value { get; init; }

        public SetStatement(string _Key, string _Value)
		{
			Key = _Key;
			Value = _Value;
		}

        public override string? Content
		{
			get
			{
				return $"Set({Key}={Value})";
			}
		}
	}
}
