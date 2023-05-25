import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useAorQualifyFrequency = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.aorQualifyFrequency === undefined ? null : model.value?.aorQualifyFrequency;
		},
		set(payload) {
			const mod = model.value || {};
			mod.aorQualifyFrequency = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useAorQualifyFrequency }