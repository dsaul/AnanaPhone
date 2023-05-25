namespace AnanaPhone.SettingsManager
{
	public class E164ClientRow
	{
		public string? Id
		{
			get
			{
				return E164;
			}
		}

		public string? E164 { get; init; }
		public string? Comment { get; init; }
		public string? OutboundDevice { get; init; }
		public bool? Disabled { get; init; }
	}
}
