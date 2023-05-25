import { computed, type Ref } from "vue"
import type { IVoiceMailMessage } from "./IVoiceMailMessage";

const useId = (model: Ref<IVoiceMailMessage | null>) => {
	
	return computed({
		get() {
			return `${model.value?.id || ''}`;
		},
		set(payload) {
			const mod = model.value || {};
			mod.id = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useId }