import { useConstructEvent } from './useConstructEvent';

type TCBParams = {
	channelName: string;
	callerIdName: string | null;
	callerIdNumber: string | null;
};

const {
	subscribe,
	unsubscribe,
	trigger
} = useConstructEvent<TCBParams, (params: TCBParams) => void>(() => {
	// Default Action
});

export {
	type TCBParams,
	subscribe,
	unsubscribe,
	trigger
};

