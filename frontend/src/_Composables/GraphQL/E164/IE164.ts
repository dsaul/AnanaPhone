import { v4 as uuidv4 } from 'uuid';

interface IE164 {
	id?: string | null;
	e164?: string | null;
	name?: string | null;
	comment?: string | null;
	outboundDevice?: string | null;
	disabled?: boolean;
}

const generateEmpty = (): IE164 => {
	return {
		id: uuidv4(),
		e164: null,
		name: null,
		comment: null,
		outboundDevice: null,
		disabled: false,
	};
}

export { type IE164, generateEmpty }