using Square.Models;
using System.Text;

namespace DanSaul.SharedCode.Asterisk.AsteriskAEL.Statements
{
	public class DialStatement : Statement
	{
		public List<DialEndpoint> Endpoints { get; init; } = new();
		public int Timeout { get; init; } = 60 * 5;

		public bool IndicateRinging { get; init; } = false;

		public DialStatement(List<DialEndpoint> _Endpoints)
        {
			Endpoints = _Endpoints;
		}

		public override string? Content
		{
			get
			{
				StringBuilder opts = new();
				if (IndicateRinging)
					opts.Append('r');



				string endpoints = string.Join("&", Endpoints);

				return $"Dial({endpoints},{Timeout},{opts})";
			}
		}
	}
}
