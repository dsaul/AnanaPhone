import { type Ref, computed } from "vue";
import type { IConference } from "./IConference";
import { usetimestampISO8601 } from "./useTimestampISO8601";
import isEmpty from "@/_Composables/Utility/isEmpty";
import { DateTime } from 'luxon';

const useStartLocalFormatted = (payload: Ref<IConference | null>) => {
	
	const startISO = usetimestampISO8601(payload);
	
	return computed(() => {
		if (isEmpty(startISO.value)) {
			return null;
		}
		
		const dtISO = DateTime.fromISO(startISO.value);
		const dtLocal = dtISO.toLocal();
		
		return dtLocal.toLocaleString(DateTime.DATETIME_MED);
	});
};

export { useStartLocalFormatted }