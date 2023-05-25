import { computed, type Ref } from "vue"
import type { IHistoricCall } from "./IHistoricCall";

const useOriginalChannel = (payload: Ref<IHistoricCall | null>) => {
	return computed(() => {
		return `${payload.value?.originalChannel || ''}`;
	});
}

export { useOriginalChannel }