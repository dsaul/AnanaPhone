import { computed, type Ref } from "vue"
import type { IHistoricCall } from "./IHistoricCall";

const useCallerIdNumber = (payload: Ref<IHistoricCall | null>) => {
	return computed(() => {
		return `${payload.value?.callerIdNumber || ''}`;
	});
}

export { useCallerIdNumber }