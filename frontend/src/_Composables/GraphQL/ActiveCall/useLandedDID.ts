import { computed, type Ref } from "vue"
import type { IActiveCall } from "./IActiveCall";

const useLandedDID = (payload: Ref<IActiveCall | null>) => {
	return computed(() => {
		return `${payload.value?.landedDID || ''}`;
	});
}

export { useLandedDID }