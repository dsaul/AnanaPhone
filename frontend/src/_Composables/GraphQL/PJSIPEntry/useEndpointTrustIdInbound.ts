import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useEndpointTrustIdInbound = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.endpointTrustIdInbound === undefined ? null : model.value?.endpointTrustIdInbound;
		},
		set(payload) {
			const mod = model.value || {};
			mod.endpointTrustIdInbound = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useEndpointTrustIdInbound }