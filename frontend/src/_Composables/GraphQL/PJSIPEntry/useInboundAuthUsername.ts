import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useInboundAuthUsername = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.inboundAuthUsername === undefined ? null : model.value?.inboundAuthUsername;
		},
		set(payload) {
			const mod = model.value || {};
			mod.inboundAuthUsername = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useInboundAuthUsername }