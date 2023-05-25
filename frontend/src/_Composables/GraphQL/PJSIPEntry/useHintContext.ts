import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useHintContext = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.hintContext === undefined ? null : model.value?.hintContext;
		},
		set(payload) {
			const mod = model.value || {};
			mod.hintContext = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useHintContext }