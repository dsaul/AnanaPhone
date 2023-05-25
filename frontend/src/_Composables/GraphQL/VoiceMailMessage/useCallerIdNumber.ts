import { computed, type Ref } from "vue"
import type { IVoiceMailMessage } from "./IVoiceMailMessage";

const useCallerIdNumber = (model: Ref<IVoiceMailMessage | null>) => {
	
	return computed({
		get() {
			return `${model.value?.callerIdNumber || ''}`;
		},
		set(payload) {
			const mod = model.value || {};
			mod.callerIdNumber = (payload === null || payload === undefined) ? undefined : payload;
			model.value = mod;
		}
	});
}

export { useCallerIdNumber }