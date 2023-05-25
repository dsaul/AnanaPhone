import { computed, type Ref } from "vue"
import type { IActiveCall } from "./IActiveCall";

const useAccountCode = (payload: Ref<IActiveCall | null>) => {
	return computed(() => {
		return `${payload.value?.accountCode || ''}`;
	});
}

export { useAccountCode }