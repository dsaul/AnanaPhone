import { computed, type Ref } from "vue"
import type { IActiveCall } from "./IActiveCall";

const useCallerIdName = (payload: Ref<IActiveCall | null>) => {
	return computed(() => {
		return `${payload.value?.callerIdName || ''}`;
	});
}

export { useCallerIdName }