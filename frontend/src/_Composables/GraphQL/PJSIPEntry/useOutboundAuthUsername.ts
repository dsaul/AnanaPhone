import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useOutboundAuthUsername = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.outboundAuthUsername === undefined ? null : model.value?.outboundAuthUsername;
		},
		set(payload) {
			const mod = model.value || {};
			mod.outboundAuthUsername = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useOutboundAuthUsername }