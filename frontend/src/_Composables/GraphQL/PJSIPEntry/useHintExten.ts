import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useHintExten = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.hintExten === undefined ? null : model.value?.hintExten;
		},
		set(payload) {
			const mod = model.value || {};
			mod.hintExten = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useHintExten }