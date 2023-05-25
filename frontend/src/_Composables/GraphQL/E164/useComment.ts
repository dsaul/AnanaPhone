import { computed, type Ref } from "vue"
import type { IE164 } from "./IE164";

const useComment = (model: Ref<IE164 | null>) => {
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