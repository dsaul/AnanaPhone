import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useEndpointAllow = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.endpointAllow === undefined ? null : model.value?.endpointAllow;
		},
		set(payload) {
			const mod = model.value || {};
			mod.endpointAllow = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useEndpointAllow }