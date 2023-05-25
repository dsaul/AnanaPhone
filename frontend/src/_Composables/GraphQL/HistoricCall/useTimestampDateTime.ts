import { computed, type Ref } from "vue"
import type { IHistoricCall } from "./IHistoricCall";
import { DateTime } from "luxon";

const useTimestampDateTime = (payload: Ref<IHistoricCall | null>) => {
	return computed(() => {
		return DateTime.fromISO(`${payload.value?.timestampISO8601 || ''}`);
	});
}

export { useTimestampDateTime }