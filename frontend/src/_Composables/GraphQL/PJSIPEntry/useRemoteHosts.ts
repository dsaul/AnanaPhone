import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useRemoteHosts = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.remoteHosts === undefined ? null : model.value?.remoteHosts;
		},
		set(payload) {
			const mod = model.value || {};
			mod.remoteHosts = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useRemoteHosts }