import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useEndpointForceRport = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.endpointForceRport === undefined ? null : model.value?.endpointForceRport;
		},
		set(payload) {
			const mod = model.value || {};
			mod.endpointForceRport = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useEndpointForceRport }