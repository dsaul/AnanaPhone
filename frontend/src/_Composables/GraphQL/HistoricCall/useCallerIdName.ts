import { computed, type Ref } from "vue"
import type { IHistoricCall } from "./IHistoricCall";

const useCallerIdName = (payload: Ref<IHistoricCall | null>) => {
	return computed(() => {
		return `${payload.value?.callerIdName || ''}`;
	});
}

export { useCallerIdName }