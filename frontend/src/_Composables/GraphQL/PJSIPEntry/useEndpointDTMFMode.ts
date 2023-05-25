import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useEndpointDTMFMode = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.endpointDTMFMode === undefined ? null : model.value?.endpointDTMFMode;
		},
		set(payload) {
			const mod = model.value || {};
			mod.endpointDTMFMode = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useEndpointDTMFMode }