import { computed, type Ref } from "vue"
import type { IActiveCall } from "./IActiveCall";

const useChannelStateDescription = (payload: Ref<IActiveCall | null>) => {
	return computed(() => {
		return `${payload.value?.channelStateDesc || ''}`;
	});
}

export { useChannelStateDescription }