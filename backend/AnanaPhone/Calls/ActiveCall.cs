using AsterNET.FastAGI;
using DanSaul.SharedCode.Asterisk;
using GraphQL.AspNet.Attributes;
using NodaTime;
using NodaTime.Text;

namespace AnanaPhone.Calls
{

	public class ActiveCall
	{
		#region Internal
		[GraphSkip]
		public AGIScriptPlus? AGIScript { get; set; } = null;
		[GraphSkip]
		public AGIRequest? AGIRequest { get; set; } = null;
		[GraphSkip]
		public AGIChannel? AGIChannel { get; set; } = null;
		[GraphSkip]
		public Instant TimestampInstant { get; set; } = SystemClock.Instance.GetCurrentInstant();
		#endregion
		#region Multi Threading Requests
		[GraphSkip]
		public object ThreadLock { get; } = new();
		[GraphSkip]
		public string? RequestNewCallTarget { get; set; } = null;
		[GraphSkip]
		public string? RequestConferenceId { get; set; } = null;
		#endregion

		#region Channel Information

		public string? CallerIdNumber { get; set; }
		public string? CallerIdName { get; set; }
		public string? ChannelName { get; set; }
		public string? Language { get; set; }
		public string? Context { get; set; }
		public string? Exten { get; set; }
		public string? Priority { get; set; }
		public string? Uniqueid { get; set; }
		public string? Linkedid { get; set; }
		public string? BridgeId { get; set; }
		public string? Application { get; set; }
		public string? ApplicationData { get; set; }
		public string? Duration { get; set; }
		public string? ChannelState { get; set; }
		public string? ChannelStateDesc { get; set; }
		public string? State { get; set; }
		public string? AccountCode { get; set; }
		public string? CallDirection { get; set; }

		#endregion

		#region Apollo
		public string? Id
		{
			get
			{
				return ChannelName;
			}
		}
		#endregion
		#region Historic Record

		[GraphSkip]
		private string HistoricId { get; } = Guid.NewGuid().ToString();
		[GraphSkip]
		public HistoricCall GenerateHistoricCall()
		{
			return new()
			{
				Id = HistoricId,
				CallerIdName = CallerIdName,
				CallerIdNumber = CallerIdNumber,
				Duration = Duration,
				TimestampISO8601 = TimestampISO8601,
				LandedDID = LandedDID,
				OriginalChannel = ChannelName,
				CallDirection = CallDirection,
			};
		}

		#endregion








		public string? LandedDID { get; set; } = null;
		public string? FarCallId { get; set; } = null;

		public string TimestampISO8601
		{
			get
			{
				return InstantPattern.ExtendedIso.Format(TimestampInstant);
			}
		}















	}
}
