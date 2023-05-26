using DanSaul.SharedCode.Asterisk.AsteriskINI;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;
using Serilog;

namespace AnanaPhone.Boot
{
	public partial class BootManager : IDisposable
	{
		[ConfGenerator]
		public void GenerateConfBridgeConf()
		{
			// confbridge.conf

			Log.Information("[{Class}.{Method}()]",
				GetType().Name,
				System.Reflection.MethodBase.GetCurrentMethod()?.Name
			);

			AsteriskINIFile file = new()
			{
				Sections = new Section[]
				{
					new Section()
					{
						Name = "general",
						Entries = Array.Empty<Entry>(),
					},
					new Section()
					{
						Name = "default_bridge",
						Entries = new Entry[]
						{
							new Entry()
							{
								Key = "type",
								Value = "bridge",
							},
							new Entry()
							{
								Key = "max_members",
								Value = "10",
							},
						},
					},
					new Section()
					{
						Name = "admin_user",
						Entries = new Entry[]
						{
							new Entry()
							{
								Key = "type",
								Value = "user",
							},
							new Entry()
							{
								Key = "marked",
								Value = "yes",
							},
							new Entry()
							{
								Key = "admin",
								Value = "yes",
							},
							new Entry()
							{
								Key = "music_on_hold_when_empty",
								Value = "yes",
							},
							new Entry()
							{
								Key = "quiet",
								Value = "yes",
							},
						},
					},
					new Section()
					{
						Name = "default_user",
						Entries = new Entry[]
						{
							new Entry()
							{
								Key = "type",
								Value = "user",
							},
							new Entry()
							{
								Key = "send_events",
								Value = "no",
							},
							new Entry()
							{
								Key = "echo_events",
								Value = "no",
							},
							new Entry()
							{
								Key = "startmuted",
								Value = "false",
							},
							new Entry()
							{
								Key = "announce_user_count",
								Value = "no",
							},
							new Entry()
							{
								Key = "quiet",
								Value = "yes",
							},
							new Entry()
							{
								Key = "hear_own_join_sound",
								Value = "no",
							},
							new Entry()
							{
								Key = "announce_user_count_all",
								Value = "no",
							},
							new Entry()
							{
								Key = "announce_only_user",
								Value = "no",
							},
							new Entry()
							{
								Key = "dsp_drop_silence",
								Value = "yes",
							},
							new Entry()
							{
								Key = "announce_join_leave",
								Value = "no",
							},
							new Entry()
							{
								Key = "wait_marked",
								Value = "yes",
							},
							new Entry()
							{
								Key = "end_marked",
								Value = "yes",
							},
							new Entry()
							{
								Key = "music_on_hold_when_empty",
								Value = "yes",
							},
						},
					},
				},
			};
			if (EnvAsterisk.ASTERISK_DEBUG_SSH_ENABLE)
			{
				Log.Information("[{Class}.{Method}()] Running remotely, skipping writing conf file.",
					GetType().Name,
					System.Reflection.MethodBase.GetCurrentMethod()?.Name
				);
			}
			else
			{
				string contents = Factory.Generate(file);
				File.WriteAllText("/etc/asterisk/confbridge.conf", contents);
			}

		}
	}
}
