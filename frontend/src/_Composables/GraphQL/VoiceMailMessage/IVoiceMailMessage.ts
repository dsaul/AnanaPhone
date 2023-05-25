interface IVoiceMailMessage {
	id?: string | null;
	callerIdNumber?: string | null;
	callerIdName?: string | null;
	timestampISO8601?: string | null;
	path?: string | null;
}

const generateEmpty = (): IVoiceMailMessage => {
	return {
		
	};
}

export {
	type IVoiceMailMessage,
	generateEmpty
}