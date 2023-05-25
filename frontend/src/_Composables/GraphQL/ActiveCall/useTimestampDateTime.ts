import { computed, type Ref } from "vue"
import type { IActiveCall } from "./IActiveCall";
import { DateTime } from "luxon";

const useTimestampDateTime = (payload: Ref<IActiveCall | null>) => {
	return computed(() => {
		return DateTime.fromISO(`${payload.value?.timestampISO8601 || ''}`);
	});
}

export { useTimestampDateTime }