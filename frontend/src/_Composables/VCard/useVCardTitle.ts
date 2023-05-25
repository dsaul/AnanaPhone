import { computed, type Ref } from 'vue';
import type { IVCard } from './IVCard';

const useVCardTitle = (model: Ref<IVCard | null>, fallback: Ref<string | null>) => {
	return computed(() => {
		return model.value?.FullName || fallback.value || '?';
	})
}

export { useVCardTitle }