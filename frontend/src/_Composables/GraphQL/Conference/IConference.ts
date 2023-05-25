import type { IConferenceParticipant } from "./IConferenceParticipant";

interface IConference {
	name?: string | null;
	displayName?: string | null;
	timestampISO8601?: string | null;
	participants?: IConferenceParticipant[];
}

export type { IConference }