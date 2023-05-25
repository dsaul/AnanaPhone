import { computed, type Ref } from "vue"
import type { IConference } from "./IConference";

const useName = (payload: Ref<IConference | null>) => {
	return computed(() => {
		return `${payload.value?.name || ''}`;
	});
}

export { useName }