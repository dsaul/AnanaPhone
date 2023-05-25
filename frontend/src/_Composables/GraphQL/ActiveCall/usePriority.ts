
import { computed, type Ref } from "vue"
import type { IActiveCall } from "./IActiveCall";

const usePriority = (payload: Ref<IActiveCall | null>) => {
	return computed(() => {
		return `${payload.value?.priority || ''}`;
	});
}

export { usePriority }