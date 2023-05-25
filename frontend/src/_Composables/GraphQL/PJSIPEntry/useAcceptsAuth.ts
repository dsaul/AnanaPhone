import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useAcceptsAuth = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.acceptsAuth === undefined ? null : model.value?.acceptsAuth;
		},
		set(payload) {
			const mod = model.value || {};
			mod.acceptsAuth = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useAcceptsAuth }