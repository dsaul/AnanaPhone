import { computed, type Ref } from "vue"
import type { IActiveCall } from "./IActiveCall";

const useCallerIdNumber = (payload: Ref<IActiveCall | null>) => {
	return computed(() => {
		return `${payload.value?.callerIdNumber || ''}`;
	});
}

export { useCallerIdNumber }