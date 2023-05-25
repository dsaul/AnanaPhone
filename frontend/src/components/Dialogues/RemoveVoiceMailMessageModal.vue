<script setup lang="ts">
import isEmpty from '@/_Composables/Utility/isEmpty';
import { ref, computed } from 'vue';
import { useMutationRemove } from '@/_Composables/GraphQL/VoiceMailMessage/useMutationRemove';
import type { IVoiceMailMessage } from '@/_Composables/GraphQL/VoiceMailMessage/IVoiceMailMessage';
import { useModelValue } from '@/_Composables/Utility/useModelValue';
import { Notify } from 'quasar';
import MDIDelete from '../SVG/MDIDelete.vue';
import { useCallerIdName } from '@/_Composables/GraphQL/VoiceMailMessage/useCallerIdName';
import { useCallerIdNumber } from '@/_Composables/GraphQL/VoiceMailMessage/useCallerIdNumber';
import { useTimestampDateTime } from '@/_Composables/GraphQL/VoiceMailMessage/useTimestampDateTime';
import { DateTime } from 'luxon';

const { remove, removeLoading, removeError, removeDone } = useMutationRemove();

const props = defineProps<{
	modelValue?: IVoiceMailMessage | null,
	value?: IVoiceMailMessage | null,
}>()

const emit = defineEmits<{
	(e: 'update:modelValue', payload: IVoiceMailMessage | null): void,
	(e: 'on-value-changed', payload: IVoiceMailMessage | null): void,
}>();

const model = useModelValue(emit, props, null as IVoiceMailMessage | null);

const isOpen = ref<boolean>(false);

const resetAndOpen = () => {
	reset();
	open();
};

const resetAndClose = () => {
	reset();
	close();
};


const confirm = () => {
	remove(model.value);
};


const open = () => {
	isOpen.value = true;
};

const close = () => {
	isOpen.value = false;
};

const reset = () => {
	//
};

removeDone(() => {
	Notify.create({
		type: 'positive',
		message: 'Removed'
	});
	
	reset();
	close();
});

removeError(() => {
	Notify.create({
		type: 'negative',
		message: 'Removing Voice Mail Message Failed'
	});
});

const isAnyLoading = computed(() => {
	return removeLoading.value;
});

defineExpose({
	open,
	close,
	resetAndOpen,
	resetAndClose,
});

const callerIdName = useCallerIdName(model);
const callerIdNumber = useCallerIdNumber(model);
const timestamp = useTimestampDateTime(model);
const timestampDisplay = computed(() => {
	return timestamp.value.toLocaleString(DateTime.DATETIME_SHORT);
});





</script>
<template>
	<q-dialog v-model="isOpen" persistent>
		<q-card>
			<div class="flex flex-row p-4 gap-2 items-center">
				<q-avatar color="primary" text-color="white">
					<MDIDelete class="m-2.5" />
				</q-avatar>
				<div class="flex flex-col p-2">
					<div class="font-bold">Are you sure you want to remove this message?</div>
					<div v-if="!isEmpty(callerIdName)">{{ callerIdName }}</div>
					<div v-if="!isEmpty(callerIdNumber)">{{ callerIdNumber }}</div>
					<div v-if="!isEmpty(timestampDisplay)">{{ timestampDisplay }}</div>
				</div>
			</div>

			<q-card-actions align="right">
				<q-btn flat label="Cancel" color="green" @click="resetAndClose" :disable="isAnyLoading" />
				<q-btn flat label="Remove" color="red" @click="confirm" :loading="isAnyLoading" :disable="isAnyLoading" />
			</q-card-actions>
		</q-card>
	</q-dialog>
</template>