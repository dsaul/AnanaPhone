import { computed, type Ref } from "vue"
import type { IHistoricCall } from "./IHistoricCall";

const useCallDirection = (payload: Ref<IHistoricCall | null>) => {
	return computed(() => {
		return `${payload.value?.callDirection || ''}`;
	});
}

export { useCallDirection }