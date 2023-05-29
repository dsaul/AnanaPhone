import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useAorQualifyFrequency = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			const parsed = parseInt(`${model.value?.aorQualifyFrequency || ''}`, 10);
			if (isNaN(parsed)) {
				return null;
			}
			return parsed;
		},
		set(payload) {
			const parsed = parseInt(`${payload || ''}`, 10);
			const mod = model.value || {};
			mod.aorQualifyFrequency = isNaN(parsed) ? null : parsed;
			model.value = mod;
		}
	});
}

export { useAorQualifyFrequency }