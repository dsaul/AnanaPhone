import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useEndpointAllowSubscribe = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.endpointAllowSubscribe === undefined ? null : model.value?.endpointAllowSubscribe;
		},
		set(payload) {
			const mod = model.value || {};
			mod.endpointAllowSubscribe = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useEndpointAllowSubscribe }