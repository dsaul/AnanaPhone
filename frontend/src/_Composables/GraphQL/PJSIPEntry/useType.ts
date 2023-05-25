import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useType = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.type === undefined ? null : model.value?.type;
		},
		set(payload) {
			const mod = model.value || {};
			mod.type = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useType }