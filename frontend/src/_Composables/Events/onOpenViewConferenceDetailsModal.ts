import { useConstructEvent } from './useConstructEvent';

type TCBParams = {
	conferenceName: string;
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

