import { computed, type Ref } from "vue"
import type { IActiveCall } from "./IActiveCall";

const useLinkedId = (payload: Ref<IActiveCall | null>) => {
	return computed(() => {
		return `${payload.value?.linkedid || ''}`;
	});
}

export { useLinkedId }