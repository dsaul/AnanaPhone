import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useAorMaxContacts = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			const parsed = parseInt(`${model.value?.aorMaxContacts || ''}`, 10);
			if (isNaN(parsed)) {
				return null;
			}
			return parsed;
		},
		set(payload) {
			
			const parsed = parseInt(`${payload || ''}`, 10);
			
			const mod = model.value || {};
			mod.aorMaxContacts = isNaN(parsed) ? null : parsed;
			model.value = mod;
		}
	});
}

export { useAorMaxContacts }