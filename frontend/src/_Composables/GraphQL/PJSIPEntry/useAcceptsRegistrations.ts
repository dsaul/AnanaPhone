import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useAcceptsRegistrations = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.acceptsRegistrations === undefined ? null : model.value?.acceptsRegistrations;
		},
		set(payload) {
			const mod = model.value || {};
			mod.acceptsRegistrations = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useAcceptsRegistrations }