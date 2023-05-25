import { computed, type Ref } from "vue"
import type { IConference } from "./IConference";

const useDisplayName = (payload: Ref<IConference | null>) => {
	return computed(() => {
		return `${payload.value?.displayName || ''}`;
	});
}

export { useDisplayName }