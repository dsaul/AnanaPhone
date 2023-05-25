import { computed, type Ref } from "vue"
import type { IConference } from "./IConference";

const useParticipants = (payload: Ref<IConference | null>) => {
	return computed(() => {
		
		const participants = payload.value?.participants || [];
		
		return participants;
	});
}

export { useParticipants }