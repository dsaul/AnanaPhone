// import { computed, type Ref } from 'vue';
// import { useAttachmentURIPrefix } from '@/_Compostables/Konstants/useAttachmentURIPrefix';
// import type { IVCard } from './IVCard';

// const attachmentURIPrefix = useAttachmentURIPrefix();

// const useVCardPhotoURI = (model: Ref<IVCard | null>) => {
// 	return computed(() => {
//         return `${attachmentURIPrefix.value || ''}${model.value?.PhotoURI || ''}`;
//     });
// }

// export { useVCardPhotoURI }