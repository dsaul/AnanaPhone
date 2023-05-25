namespace AnanaPhone.Conferences
{
	public class ConferenceParticipant
	{
		public Guid Id { get; } = Guid.NewGuid();
		public string? Channel { get; init; }
		public string? CallerIdNumber { get; init; }
		public string? CallerIdName { get; init; }
		public string? ConferenceName { get; init; }

		public string DisplayName
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(CallerIdName))
					return CallerIdName;
				if (!string.IsNullOrWhiteSpace(CallerIdNumber))
					return CallerIdNumber;
				return Channel ?? "Invalid";
			}
		}
	}
}
