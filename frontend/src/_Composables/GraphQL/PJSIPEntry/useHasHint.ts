import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useHasHint = (model: Ref<IPJSIPEntry | null>) => {
	
	
	return computed({
		get() {
			return model.value?.hasHint === undefined ? null : model.value?.hasHint;
		},
		set(payload) {
			const mod = model.value || {};
			mod.hasHint = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useHasHint }