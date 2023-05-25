import { computed, type Ref } from "vue"
import type { IE164Client } from "./IE164Client";

const useOutboundDevice = (model: Ref<IE164Client | null>) => {
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