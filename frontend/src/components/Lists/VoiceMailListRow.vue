<script setup lang="ts">
import { useModelValue } from '@/_Composables/Utility/useModelValue';
import { useCallerIdName } from '@/_Composables/GraphQL/VoiceMailMessage/useCallerIdName';
import { useCallerIdNumber } from '@/_Composables/GraphQL/VoiceMailMessage/useCallerIdNumber';
import { useTimestampDateTime } from '@/_Composables/GraphQL/VoiceMailMessage/useTimestampDateTime';
import MDIAccount from '../SVG/MDIAccount.vue';
import MDIDelete from '../SVG/MDIDelete.vue';
import isEmpty from '@/_Composables/Utility/isEmpty';
import { computed, ref } from 'vue';
import { DateTime } from 'luxon';
import type { IVoiceMailMessage } from '@/_Composables/GraphQL/VoiceMailMessage/IVoiceMailMessage';
import { useAudioURI } from '@/_Composables/GraphQL/VoiceMailMessage/useAudioURI';
import RemoveVoiceMailMessageModal from '../Dialogues/RemoveVoiceMailMessageModal.vue';

const props = defineProps<{
	modelValue?: IVoiceMailMessage,
	value?: IVoiceMailMessage,
}>();

const emit = defineEmits<{
	(e: 'update:modelValue', payload: IVoiceMailMessage | null): void,
	(e: 'on-value-changed', payload: IVoiceMailMessage | null): void,
}>();

const model = useModelValue(emit, props, null as IVoiceMailMessage | null);
const callerIdName = useCallerIdName(model);
const callerIdNumber = useCallerIdNumber(model);
const timestamp = useTimestampDateTime(model);
const timestampDisplay = computed(() => {
	return timestamp.value.toLocaleString(DateTime.DATETIME_SHORT);
});
const audioURI = useAudioURI(model);
const removeVoiceMailMessageModalEl = ref();

const onClickRemove = () => {
	if (removeVoiceMailMessageModalEl.value) {
		removeVoiceMailMessageModalEl.value.resetAndOpen();
	}
}

const displayName = computed(() => {
	if (!isEmpty(callerIdName.value) && !isEmpty(callerIdNumber.value)) {
		return `${callerIdName.value} (${callerIdNumber.value})`;
	}
	else if (!isEmpty(callerIdName.value) && isEmpty(callerIdNumber.value)) {
		return `${callerIdName.value}`;
	}
	else if (isEmpty(callerIdName.value) && !isEmpty(callerIdNumber.value)) {
		return `${callerIdNumber.value}`;
	}
	else {
		return 'Unknown';
	}
})

</script>
<template>
	<q-expansion-item class="shadow-2">
		<RemoveVoiceMailMessageModal ref="removeVoiceMailMessageModalEl" v-model="model" />
		<template v-slot:header>
			<q-item-section avatar>
				<q-avatar color="primary" text-color="white">
					<MDIAccount class="m-2" />
				</q-avatar>
			</q-item-section>

			<q-item-section class="flex flex-col gap-0.5">
				<div v-if="!isEmpty(displayName)" class="font-bold">
					{{ displayName }}
				</div>
				<div class="flex flex-row flex-wrap gap-2 items-center">
					<div>{{ timestampDisplay }}</div>
				</div>
			</q-item-section>
		</template>
		<audio controls preload="auto" class="w-full px-4 py-1" >
			<source :src="audioURI || undefined" type="audio/wav" />
			<a :href="audioURI || undefined">
				Download audio
			</a>
		</audio>
		<q-card>
			<q-item clickable v-ripple @click="onClickRemove">
				<q-item-section avatar>
					<MDIDelete class="w-6 h-6" />
				</q-item-section>
				<q-item-section>Remove&hellip;</q-item-section>
			</q-item>
		</q-card>
	</q-expansion-item>
</template>
