import { computed, type Ref } from "vue"
import type { IActiveCall } from "./IActiveCall";

const useUniqueId = (payload: Ref<IActiveCall | null>) => {
	return computed(() => {
		return `${payload.value?.uniqueid || ''}`;
	});
}

export { useUniqueId }