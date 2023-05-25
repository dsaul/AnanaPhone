import { type IE164 } from '@/_Composables/GraphQL/E164/IE164'
import type { IPJSIPEntry } from './PJSIPEntry/IPJSIPEntry';
import type { IE164Client } from './E164Client/IE164Client';

interface ISettings {
	allowedCallOutNumbers?: string[];
	e164s?: IE164[];
	e164Clients?: IE164Client[];
	pjsipClientDefaults?: IPJSIPEntry;
	pjsipTrunkDefaults?: IPJSIPEntry;
	clientNames?: string[];
	clients?: IPJSIPEntry[];
	trunkNames?: string[];
	trunks?: IPJSIPEntry[];
}

export { type ISettings }