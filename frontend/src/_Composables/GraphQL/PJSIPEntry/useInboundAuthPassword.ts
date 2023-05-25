import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useInboundAuthPassword = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.inboundAuthPassword === undefined ? null : model.value?.inboundAuthPassword;
		},
		set(payload) {
			const mod = model.value || {};
			mod.inboundAuthPassword = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useInboundAuthPassword }