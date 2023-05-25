import { computed, type Ref } from "vue"
import type { IActiveCall } from "./IActiveCall";

const useApplication = (payload: Ref<IActiveCall | null>) => {
	return computed(() => {
		return `${payload.value?.application || ''}`;
	});
}

export { useApplication }