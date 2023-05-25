import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useName = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.name === undefined ? null : model.value?.name;
		},
		set(payload) {
			const mod = model.value || {};
		mod.name = (payload === null || payload === undefined) ? undefined : payload;
		model.value = mod;
		}
	});
}

export { useName }