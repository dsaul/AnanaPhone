using GraphQL.AspNet.Attributes;
using NodaTime;
using NodaTime.Text;

namespace AnanaPhone.Conferences
{
	public class Conference
	{
		#region Internal
		[GraphSkip]
		public Instant StartInstant { get; set; } = SystemClock.Instance.GetCurrentInstant();
		[GraphSkip]
		public void AddParticipant(ConferenceParticipant participant)
		{
			Participants.Add(participant);
		}
		[GraphSkip]
		public void RemoveForChannel(string channel)
		{
			Participants.RemoveAll((participant) =>
			{
				return participant.Channel == channel;
			});
		}
		#endregion
		public string? Id
		{
			get
			{
				return Name;
			}
		}

		public string? Name { get; init; } = null;

		public string? DisplayName
		{
			get
			{
				IEnumerable<string> displayNames = Participants.Select((participant) => participant.DisplayName);
				IEnumerable<string> noEmpty = displayNames.Where((displayName) => !string.IsNullOrWhiteSpace(displayName));
				return string.Join(", ", noEmpty);
			}
		}



		public List<ConferenceParticipant> Participants { get; init; } = new List<ConferenceParticipant>();
		public string TimestampISO8601
		{
			get
			{
				return InstantPattern.ExtendedIso.Format(StartInstant);
			}
		}


	}
}
