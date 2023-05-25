import { computed, type Ref } from "vue"
import type { IE164 } from "./IE164";

const useName = (model: Ref<IE164 | null>) => {
	return computed({
		get() {
			return `${model.value?.name || ''}`;
		},
		set(payload) {
			const mod = model.value || {};
			mod.name = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useName }