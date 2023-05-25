import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useEndpointT38UdptlNat = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.endpointT38UdptlNat === undefined ? null : model.value?.endpointT38UdptlNat;
		},
		set(payload) {
			const mod = model.value || {};
			mod.endpointT38UdptlNat = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useEndpointT38UdptlNat }