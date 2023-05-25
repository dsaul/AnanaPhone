using System.Xml.Linq;

namespace AnanaPhone.SettingsManager
{
	public class E164Row
	{
		public string? Id
		{
			get
			{
				return E164;
			}
		}

		public string? E164 { get; init; }
		public string? Name { get; init; }
		public string? Comment { get; init; }
		public string? OutboundDevice { get; init; }
		public bool? Disabled { get; init; }
	}
}
