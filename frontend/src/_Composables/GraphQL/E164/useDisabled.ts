import { computed, type Ref } from "vue"
import type { IE164 } from "./IE164";

const useDisabled = (model: Ref<IE164 | null>) => {
	
	return computed({
		get() {
			if (model.value?.disabled === undefined)
				return false;
			return model.value.disabled;
		},
		set(payload) {
			const mod = model.value || {};
			mod.disabled = (payload === null || payload === undefined) ? false : payload;
			model.value = mod;
		}
	});
}

export { useDisabled }