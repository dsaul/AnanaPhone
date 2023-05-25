
import { computed, type Ref } from "vue"
import type { IActiveCall } from "./IActiveCall";

const useContext = (payload: Ref<IActiveCall | null>) => {
	return computed(() => {
		return `${payload.value?.context || ''}`;
	});
}

export { useContext }