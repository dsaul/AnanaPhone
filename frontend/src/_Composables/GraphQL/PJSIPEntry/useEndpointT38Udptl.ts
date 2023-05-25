import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useEndpointT38Udptl = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.endpointT38Udptl === undefined ? null : model.value?.endpointT38Udptl;
		},
		set(payload) {
			const mod = model.value || {};
			mod.endpointT38Udptl = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useEndpointT38Udptl }