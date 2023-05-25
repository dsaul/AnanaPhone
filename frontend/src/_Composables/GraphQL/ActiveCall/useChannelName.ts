import { computed, type Ref } from "vue"
import type { IActiveCall } from "./IActiveCall";

const useChannelName = (payload: Ref<IActiveCall | null>) => {
	return computed(() => {
		return `${payload.value?.channelName || ''}`;
	});
}

export { useChannelName }