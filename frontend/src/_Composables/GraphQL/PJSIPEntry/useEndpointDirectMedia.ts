import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useEndpointDirectMedia = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.endpointDirectMedia === undefined ? null : model.value?.endpointDirectMedia;
		},
		set(payload) {
			const mod = model.value || {};
			mod.endpointDirectMedia = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useEndpointDirectMedia }