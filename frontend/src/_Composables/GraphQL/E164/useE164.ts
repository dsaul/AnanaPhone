import { computed, type Ref } from "vue"
import type { IE164 } from "./IE164";

const useE164 = (payload: Ref<IE164 | null>) => {
	return computed(() => {
		return `${payload.value?.e164 || ''}`;
	});
}

export { useE164 }