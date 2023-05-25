import { computed, type Ref } from "vue"
import type { IActiveCall } from "./IActiveCall";

const useApplicationData = (payload: Ref<IActiveCall | null>) => {
	return computed(() => {
		return `${payload.value?.applicationData || ''}`;
	});
}

export { useApplicationData }