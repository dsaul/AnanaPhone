import { computed, type Ref } from "vue"
import type { IActiveCall } from "./IActiveCall";

const useBridgeId = (payload: Ref<IActiveCall | null>) => {
	return computed(() => {
		return `${payload.value?.bridgeId || ''}`;
	});
}

export { useBridgeId }