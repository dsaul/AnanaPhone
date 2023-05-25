import { ref } from 'vue';

const useConstructEvent = <TPrams, TCallback extends (params: TPrams) => void>(
	firstCB: TCallback
) => {

	const cbList = ref<TCallback[]>([firstCB]);

	const subscribe = (cb: TCallback): void => {
		(cbList.value as TCallback[]).push(cb);
	}

	const unsubscribe = (cb: TCallback): void => {
		cbList.value = cbList.value.filter((e) => e !== cb);
	}

	const trigger = (params: TPrams): void => {
		for (const cbFn of cbList.value) {
			if (!cbFn) {
				continue;
			}
			cbFn(params);
		}
	}

	return {
		subscribe,
		unsubscribe,
		trigger,
	}
}

export { useConstructEvent }