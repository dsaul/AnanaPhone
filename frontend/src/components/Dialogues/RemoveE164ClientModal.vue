<script setup lang="ts">
import isEmpty from '@/_Composables/Utility/isEmpty';
import { ref, computed } from 'vue';
import { useMutationRemove } from '@/_Composables/GraphQL/E164Client/useMutationRemove';
import type { IE164Client } from '@/_Composables/GraphQL/E164Client/IE164Client';
import { useModelValue } from '@/_Composables/Utility/useModelValue';
import { Notify } from 'quasar';
import MDIDelete from '../SVG/MDIDelete.vue';

const { remove, removeLoading, removeError, removeDone } = useMutationRemove();

const props = defineProps<{
	modelValue?: IE164Client | null,
	value?: IE164Client | null,
}>()

const emit = defineEmits<{
	(e: 'update:modelValue', payload: IE164Client | null): void,
	(e: 'on-value-changed', payload: IE164Client | null): void,
}>();

const model = useModelValue(emit, props, null as IE164Client | null);

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
		message: 'Removing Number Client Failed'
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







</script>
<template>
	<q-dialog v-model="isOpen" persistent>
		<q-card>
			<div class="flex flex-row p-4 gap-2 items-center">
				<q-avatar color="primary" text-color="white">
					<MDIDelete class="m-2.5" />
				</q-avatar>
				<div class="flex flex-col p-2">
					<div class="font-bold">Are you sure you want to remove this entry?</div>
					<div v-if="!isEmpty(model?.e164)">{{ model?.e164 || '' }}</div>
				</div>
			</div>

			<q-card-actions align="right">
				<q-btn flat label="Cancel" color="green" @click="resetAndClose" :disable="isAnyLoading" />
				<q-btn flat label="Remove" color="red" @click="confirm" :loading="isAnyLoading" :disable="isAnyLoading" />
			</q-card-actions>
		</q-card>
	</q-dialog>
</template>