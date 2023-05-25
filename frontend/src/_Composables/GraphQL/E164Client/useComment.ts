import { computed, type Ref } from "vue"
import type { IE164Client } from "./IE164Client";

const useComment = (model: Ref<IE164Client | null>) => {
	return computed({
		get() {
			return `${model.value?.comment || ''}`;
		},
		set(payload) {
			const mod = model.value || {};
			mod.comment = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useComment }