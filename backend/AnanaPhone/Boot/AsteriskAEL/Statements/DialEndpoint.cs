using System.Text;

namespace DanSaul.SharedCode.Asterisk.AsteriskAEL.Statements
{
	public record DialEndpoint
	{
		public string? Technology { get; init; }
		public string? Extension { get; init; }
		public string? Device { get; init; }

		public override string ToString()
		{
			StringBuilder sb = new();

			sb.Append(Technology);
			sb.Append('/');
			sb.Append(Extension);
			if (!string.IsNullOrWhiteSpace(Device))
			{
				sb.Append('@');
				sb.Append(Device);
			}

			return sb.ToString();
		}
	}
}
