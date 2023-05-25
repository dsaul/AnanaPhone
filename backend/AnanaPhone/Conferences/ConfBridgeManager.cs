using AnanaPhone.AMI;
using AsterNET.Manager.Event;
using Serilog;

namespace AnanaPhone.Conferences
{
	public class ConfBridgeManager : IDisposable
	{
		public AMIManager AMI { get; init; }
		public List<Conference> Conferences { get; } = new List<Conference>();

		public ConfBridgeManager(AMIManager _AMI)
		{
			AMI = _AMI;

			AMI.Connection.ConfbridgeStart += new EventHandler<ConfbridgeStartEvent>(ConfbridgeStart);
			AMI.Connection.ConfbridgeEnd += new EventHandler<ConfbridgeEndEvent>(ConfbridgeEnd);
			AMI.Connection.ConfbridgeJoin += new EventHandler<ConfbridgeJoinEvent>(ConfbridgeJoin);
			AMI.Connection.ConfbridgeLeave += new EventHandler<ConfbridgeLeaveEvent>(ConfbridgeLeave);
			//AMI.Connection.ConfbridgeList += new EventHandler<ConfbridgeListEvent>(ConfbridgeList);
			AMI.Connection.ConfbridgeMute += new EventHandler<ConfbridgeMuteEvent>(ConfbridgeMute);
			//AMI.Connection.ConfbridgeRecord += new EventHandler<ConfbridgeRecordEvent>(ConfbridgeRecord);
			//AMI.Connection.ConfbridgeStopRecord += new EventHandler<ConfbridgeStopRecordEvent>(ConfbridgeStopRecord);
			AMI.Connection.ConfbridgeTalking += new EventHandler<ConfbridgeTalkingEvent>(ConfbridgeTalking);
			AMI.Connection.ConfbridgeUnmute += new EventHandler<ConfbridgeUnmuteEvent>(ConfbridgeUnmute);
		}


		public Conference? ForName(string name)
		{
			return Conferences.Find((conference) =>
			{
				return conference.Name == name;
			});
		}

		private Conference AddConference(string name)
		{
			Conference conference = new()
			{
				Name = name,
			};
			Conferences.Add(conference);
			return conference;
		}

		private Conference EnsureConference(string name)
		{
			Conference? conference = ForName(name);
			conference ??= AddConference(name);
			return conference;
		}

		private void RemoveConference(string name)
		{
			Conferences.RemoveAll((conference) =>
			{
				return conference.Name == name;
			});
		}


		void ConfbridgeStart(object? sender, ConfbridgeStartEvent e)
		{
			Log.Information("[{Class}.{Method}()] EVT ConfbridgeStart {ConferenceName}",
				GetType().Name,
				System.Reflection.MethodBase.GetCurrentMethod()?.Name,
				e.Conference
			);

			AddConference(e.Conference);

		}

		void ConfbridgeEnd(object? sender, ConfbridgeEndEvent e)
		{
			Log.Information("[{Class}.{Method}()] EVT ConfbridgeEnd {ConferenceName}",
				GetType().Name,
				System.Reflection.MethodBase.GetCurrentMethod()?.Name,
				e.Conference
			);

			RemoveConference(e.Conference);

		}

		void ConfbridgeJoin(object? sender, ConfbridgeJoinEvent e)
		{
			Log.Information("[{Class}.{Method}()] EVT ConfbridgeJoin {ConferenceName} {Channel} {CallerIdNumber} {CallerIdName}",
				GetType().Name,
				System.Reflection.MethodBase.GetCurrentMethod()?.Name,
				e.Conference,
				e.Channel,
				e.CallerIDnum,
				e.CallerIDname
			);

			Conference conference = EnsureConference(e.Conference);

			ConferenceParticipant conferenceParticipant = new()
			{
				Channel = e.Channel,
				CallerIdNumber = e.CallerIDnum,
				CallerIdName = e.CallerIDname,
				ConferenceName = e.Conference,
			};

			conference.AddParticipant(conferenceParticipant);

		}

		void ConfbridgeLeave(object? sender, ConfbridgeLeaveEvent e)
		{
			Log.Information("[{Class}.{Method}()] EVT ConfbridgeLeave {ConferenceName} {Channel} {CallerIdNumber} {CallerIdName}",
				GetType().Name,
				System.Reflection.MethodBase.GetCurrentMethod()?.Name,
				e.Conference,
				e.Channel,
				e.CallerIDnum,
				e.CallerIDname
			);

			Conference conference = EnsureConference(e.Conference);

			conference.RemoveForChannel(e.Conference);
		}

		//void ConfbridgeList(object? sender, ConfbridgeListEvent e)
		//{
		//	Log.Information("[{Class}.{Method}()] EVT ConfbridgeList {ConferenceName}",
		//		GetType().Name,
		//		System.Reflection.MethodBase.GetCurrentMethod()?.Name,
		//		e.Conference
		//	);
		//}

		void ConfbridgeMute(object? sender, ConfbridgeMuteEvent e)
		{
			Log.Information("[{Class}.{Method}()] EVT ConfbridgeMute {ConferenceName}",
				GetType().Name,
				System.Reflection.MethodBase.GetCurrentMethod()?.Name,
				e.Conference
			);
		}

		//void ConfbridgeRecord(object? sender, ConfbridgeRecordEvent e)
		//{
		//	Log.Information("ConfbridgeRecord");
		//}

		//void ConfbridgeStopRecord(object? sender, ConfbridgeStopRecordEvent e)
		//{
		//	Log.Information("ConfbridgeStopRecord");
		//}

		void ConfbridgeTalking(object? sender, ConfbridgeTalkingEvent e)
		{
			Log.Information("[{Class}.{Method}()] EVT ConfbridgeTalking {ConferenceName}",
				GetType().Name,
				System.Reflection.MethodBase.GetCurrentMethod()?.Name,
				e.Conference
			);
		}

		void ConfbridgeUnmute(object? sender, ConfbridgeUnmuteEvent e)
		{
			Log.Information("[{Class}.{Method}()] EVT ConfbridgeUnmute {ConferenceName}",
				GetType().Name,
				System.Reflection.MethodBase.GetCurrentMethod()?.Name,
				e.Conference
			);
		}


		#region IDisposable
		private bool disposedValue;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				disposedValue = true;
			}
		}

		// // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		// ~ConfBridgeManager()
		// {
		//     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		//     Dispose(disposing: false);
		// }

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}
