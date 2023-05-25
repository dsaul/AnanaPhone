import { type Ref, computed } from 'vue';
import type { IVoiceMailMessage } from './IVoiceMailMessage';
import { useAPIBaseURI } from '@/_Composables/Constants/useAPIBaseURI';
import isEmpty from '@/_Composables/Utility/isEmpty';

const useAudioURI = (model: Ref<IVoiceMailMessage | null>) => {
	const apiBase = useAPIBaseURI();
	
	return computed(() => {
		if (model.value === null) {
			return null;
		}
		if (isEmpty(model.value.id)) {
			return null;
		}
		
		return `${apiBase.value}api/MessageAudio/${model.value.id}`;
	});
};

export { useAudioURI }