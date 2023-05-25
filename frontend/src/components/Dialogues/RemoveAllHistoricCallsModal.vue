<script setup lang="ts">
import { ref, computed } from 'vue';
import { useMutationRemoveAll } from '@/_Composables/GraphQL/HistoricCall/useMutationRemoveAll';
import { Notify } from 'quasar';
import MDIDeleteClock from '../SVG/MDIDeleteClock.vue';

const { removeAll, removeAllLoading, removeAllError, removeAllDone } = useMutationRemoveAll();

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
	removeAll();
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

removeAllDone(() => {
	Notify.create({
		type: 'positive',
		message: 'All Historic Calls Removed'
	});
	
	reset();
	close();
});

removeAllError(() => {
	Notify.create({
		type: 'negative',
		message: 'Failed Deleting All Historic Calls'
	});
});

const isAnyLoading = computed(() => {
	return removeAllLoading.value;
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
			<div class="flex flex-row p-4 gap-2">
				<q-avatar color="primary" text-color="white">
					<MDIDeleteClock class="m-2.5" />
				</q-avatar>
				<div class="flex flex-col gap-2 px-2 py-1">
					<div class="font-bold">Are you sure you want to remove all historic calls?</div>
				</div>
			</div>

			<q-card-actions align="right">
				<q-btn flat label="Cancel" color="green" @click="resetAndClose" :disable="isAnyLoading" />
				<q-btn flat label="Remove" color="red" @click="confirm" :loading="removeAllLoading" :disable="isAnyLoading" />
			</q-card-actions>
		</q-card>
	</q-dialog>
</template>