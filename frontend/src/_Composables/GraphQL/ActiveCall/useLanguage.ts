
import { computed, type Ref } from "vue"
import type { IActiveCall } from "./IActiveCall";

const useLanguage = (payload: Ref<IActiveCall | null>) => {
	return computed(() => {
		return `${payload.value?.language || ''}`;
	});
}

export { useLanguage }