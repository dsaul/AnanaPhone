namespace DanSaul.SharedCode.Asterisk.AsteriskAEL.Statements
{
	public class ConfBridgeStatement : Statement
	{
		public string RoomName { get; init; }
		public string BridgeSettingsName { get; init; } = "default_bridge";
		public string UserSettingsName { get; init; } = "default_user";

        public ConfBridgeStatement(string _RoomName, string _UserSettingsName)
        {
			RoomName = _RoomName;
			UserSettingsName = _UserSettingsName;
		}

		public override string? Content
		{
			get
			{
				return $"ConfBridge({RoomName},{BridgeSettingsName},{UserSettingsName})";
			}
		}
	}
}
