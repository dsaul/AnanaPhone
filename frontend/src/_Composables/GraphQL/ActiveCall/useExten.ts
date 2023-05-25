
import { computed, type Ref } from "vue"
import type { IActiveCall } from "./IActiveCall";

const useExten = (payload: Ref<IActiveCall | null>) => {
	return computed(() => {
		return `${payload.value?.exten || ''}`;
	});
}

export { useExten }