import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useOutboundAuthPassword = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.outboundAuthPassword === undefined ? null : model.value?.outboundAuthPassword;
		},
		set(payload) {
			const mod = model.value || {};
			mod.outboundAuthPassword = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useOutboundAuthPassword }