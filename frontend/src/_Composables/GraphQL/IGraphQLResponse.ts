import type { ICalls } from "./ICalls";
import type { IConfBridge } from "./IConfBridge";
import type { ISettings } from "./ISettings";
import type { IVoiceMail } from "./IVoiceMail";

interface IGraphQLResponse
{
	confBridge?: IConfBridge;
	calls?: ICalls;
	settings?: ISettings;
	voiceMail?: IVoiceMail;
}

export type { IGraphQLResponse }