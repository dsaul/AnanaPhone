import { computed, type Ref } from 'vue';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useSendsAuth = (model: Ref<IPJSIPEntry | null>) => {
	return computed({
		get() {
			return model.value?.sendsAuth === undefined ? null : model.value?.sendsAuth;
		},
		set(payload) {
			const mod = model.value || {};
			mod.sendsAuth = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useSendsAuth }