import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useSendsRegistrations = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.sendsRegistrations === undefined ? null : model.value?.sendsRegistrations;
		},
		set(payload) {
			const mod = model.value || {};
			mod.sendsRegistrations = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useSendsRegistrations }