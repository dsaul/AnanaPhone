interface IActiveCall {
	// Channel Information
	callerIdNumber?: string;
	callerIdName?: string;
	channelName?: string;
	language?: string;
	context?: string;
	exten?: string;
	priority?: string;
	uniqueid?: string;
	linkedid?: string;
	bridgeId?: string;
	application?: string;
	applicationData?: string;
	duration?: string;
	channelState?: string;
	channelStateDesc?: string;
	state?: string;
	accountCode?: string;

	id?: string; // For apollo, channel is actual primary key.

	landedDID?: string;
	farCallId?: string;
	timestampISO8601?: string;
}
export { type IActiveCall }