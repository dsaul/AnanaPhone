
import { computed, type Ref } from "vue"
import type { IActiveCall } from "./IActiveCall";
import { Duration } from 'luxon';

const asteriskParseRx = /^(?<hours>[0-9][0-9]):(?<minutes>[0-9][0-9]):(?<seconds>[0-9][0-9])/;

const useDuration = (payload: Ref<IActiveCall | null>) => {
	return computed(() => {
		
		const str = `${payload.value?.duration || ''}`;
		
		let hours: number | undefined = undefined;
		let minutes: number | undefined = undefined;
		let seconds: number | undefined = undefined;
		
		const match = str.match(asteriskParseRx);
		if (match && match.groups) {
			hours = parseInt(`${match.groups.hours || ''}`, 10);
			if (isNaN(hours)) {
				hours = undefined;
			}
			minutes = parseInt(`${match.groups.minutes || ''}`, 10);
			if (isNaN(minutes)) {
				minutes = undefined;
			}
			seconds = parseInt(`${match.groups.seconds || ''}`, 10);
			if (isNaN(seconds)) {
				seconds = undefined;
			}
		}
		
		const duration = Duration.fromObject({
			hours,
			minutes,
			seconds,
		});
		
		return duration;
	});
}

export { useDuration }