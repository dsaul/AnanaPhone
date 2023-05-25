import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useEndpointCallerid = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.endpointCallerid === undefined ? null : model.value?.endpointCallerid;
		},
		set(payload) {
			const mod = model.value || {};
			mod.endpointCallerid = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useEndpointCallerid }