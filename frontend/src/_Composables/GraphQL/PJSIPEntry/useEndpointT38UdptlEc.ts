import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useEndpointT38UdptlEc = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.endpointT38UdptlEc === undefined ? null : model.value?.endpointT38UdptlEc;
		},
		set(payload) {
			const mod = model.value || {};
			mod.endpointT38UdptlEc = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useEndpointT38UdptlEc }