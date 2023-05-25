import { computed, type Ref } from "vue"
import type { IActiveCall } from "./IActiveCall";

const useState = (payload: Ref<IActiveCall | null>) => {
	return computed(() => {
		return `${payload.value?.state || ''}`;
	});
}

export { useState }