import { computed, type Ref } from "vue"
import type { IActiveCall } from "./IActiveCall";

const useChannelState = (payload: Ref<IActiveCall | null>) => {
	return computed(() => {
		return `${payload.value?.channelState || ''}`;
	});
}

export { useChannelState }