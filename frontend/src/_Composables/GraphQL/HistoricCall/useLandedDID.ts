import { computed, type Ref } from "vue"
import type { IHistoricCall } from "./IHistoricCall";

const useLandedDID = (payload: Ref<IHistoricCall | null>) => {
	return computed(() => {
		return `${payload.value?.landedDID || ''}`;
	});
}

export { useLandedDID }