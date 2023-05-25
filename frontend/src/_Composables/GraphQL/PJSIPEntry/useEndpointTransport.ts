import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useEndpointTransport = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.endpointTransport === undefined ? null : model.value?.endpointTransport;
		},
		set(payload) {
			const mod = model.value || {};
			mod.endpointTransport = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useEndpointTransport }