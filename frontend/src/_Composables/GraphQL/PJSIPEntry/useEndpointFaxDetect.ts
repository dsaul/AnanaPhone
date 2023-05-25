import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useEndpointFaxDetect = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.endpointFaxDetect === undefined ? null : model.value?.endpointFaxDetect;
		},
		set(payload) {
			const mod = model.value || {};
			mod.endpointFaxDetect = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useEndpointFaxDetect }