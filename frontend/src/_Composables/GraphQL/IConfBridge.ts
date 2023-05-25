import type { IConference } from "./Conference/IConference"

interface IConfBridge {
	rooms?: IConference[];
}

export type { IConfBridge }