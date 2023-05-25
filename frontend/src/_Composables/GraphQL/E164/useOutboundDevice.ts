import { computed, type Ref } from "vue"
import type { IE164 } from "./IE164";

const useOutboundDevice = (model: Ref<IE164 | null>) => {
	return computed({
		get() {
			return `${model.value?.outboundDevice || ''}`;
		},
		set(payload) {
			const mod = model.value || {};
			mod.outboundDevice = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useOutboundDevice }