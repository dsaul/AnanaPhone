import { computed, type Ref } from "vue"
import type { IE164Client } from "./IE164Client";

const useE164 = (payload: Ref<IE164Client | null>) => {
	return computed(() => {
		return `${payload.value?.e164 || ''}`;
	});
}

export { useE164 }