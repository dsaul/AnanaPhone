import { computed, type Ref } from "vue"
import type { IVoiceMailMessage } from "./IVoiceMailMessage";
import { DateTime } from "luxon";

const useTimestampDateTime = (model: Ref<IVoiceMailMessage | null>) => {
	
	return computed({
		get() {
			return DateTime.fromISO(`${model.value?.timestampISO8601 || ''}`);
		},
		set(payload) {
			const mod = model.value || {};
			mod.timestampISO8601 = payload.toISO();
			model.value = mod;
		}
	});
}

export { useTimestampDateTime }