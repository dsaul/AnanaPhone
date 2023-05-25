import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useAorMaxContacts = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.aorMaxContacts === undefined ? null : model.value?.aorMaxContacts;
		},
		set(payload) {
			const mod = model.value || {};
			mod.aorMaxContacts = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useAorMaxContacts }