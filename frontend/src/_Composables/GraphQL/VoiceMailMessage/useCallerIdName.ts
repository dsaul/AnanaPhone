import { computed, type Ref } from "vue"
import type { IVoiceMailMessage } from "./IVoiceMailMessage";

const useCallerIdName = (model: Ref<IVoiceMailMessage | null>) => {
	
	return computed({
		get() {
			return `${model.value?.callerIdName || ''}`;
		},
		set(payload) {
			const mod = model.value || {};
			mod.callerIdName = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useCallerIdName }