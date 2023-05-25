import { computed, type Ref } from "vue"
import type { IHistoricCall } from "./IHistoricCall";

const useId = (payload: Ref<IHistoricCall | null>) => {
	return computed(() => {
		return `${payload.value?.id || ''}`;
	});
}

export { useId }