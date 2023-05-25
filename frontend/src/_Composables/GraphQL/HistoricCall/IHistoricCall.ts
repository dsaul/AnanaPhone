interface IHistoricCall {
	id?: string;
	callerIdName?: string;
	callerIdNumber?: string;
	duration?: string;
	timestampISO8601?: string;
	landedDID?: string;
	originalChannel?: string;
	callDirection?: string;
}

export { type IHistoricCall }