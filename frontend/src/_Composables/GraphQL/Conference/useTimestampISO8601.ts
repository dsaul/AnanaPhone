import { computed, type Ref } from "vue"
import type { IConference } from "./IConference";

const usetimestampISO8601 = (payload: Ref<IConference | null>) => {
	return computed(() => {
		return `${payload.value?.timestampISO8601 || ''}`;
	});
}

export { usetimestampISO8601 }