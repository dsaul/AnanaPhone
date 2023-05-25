import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useEndpointRewriteContact = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.endpointRewriteContact === undefined ? null : model.value?.endpointRewriteContact;
		},
		set(payload) {
			const mod = model.value || {};
			mod.endpointRewriteContact = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useEndpointRewriteContact }