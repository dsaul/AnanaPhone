import { computed, type Ref } from "vue"
import type { IActiveCall } from "./IActiveCall";

const useFarCallId = (payload: Ref<IActiveCall | null>) => {
	return computed(() => {
		return `${payload.value?.farCallId || ''}`;
	});
}

export { useFarCallId }