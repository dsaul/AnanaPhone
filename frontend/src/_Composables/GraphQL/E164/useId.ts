import { computed, type Ref } from "vue"
import type { IE164 } from "./IE164";

const useId = (model: Ref<IE164 | null>) => {
	
	return computed({
		get() {
			return `${model.value?.id || ''}`;
		},
		set(payload) {
			const mod = model.value || {};
			mod.id = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useId }