import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useEndpointRTPSymmetric = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.endpointRTPSymmetric === undefined ? null : model.value?.endpointRTPSymmetric;
		},
		set(payload) {
			const mod = model.value || {};
			mod.endpointRTPSymmetric = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useEndpointRTPSymmetric }