interface IE164Client {
	e164?: string | null;
	comment?: string | null;
	outboundDevice?: string | null;
	disabled?: boolean;
}

const generateEmpty = (): IE164Client => {
	return {
		e164: null,
		comment: null,
		outboundDevice: null,
		disabled: false,
	};
}

export { type IE164Client, generateEmpty }