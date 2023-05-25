<script setup lang="ts">
import { onMounted, onUnmounted, ref, computed } from 'vue';
import {
	type TCBParams as TOnOpenRemoveHistoricCallModel,
	subscribe as onOpenRemoveHistoricCallModelSubscribe,
	unsubscribe as onOpenRemoveHistoricCallModelUnsubscribe,
} from '@/_Composables/Events/onOpenRemoveHistoricCallModal';
import isEmpty from '@/_Composables/Utility/isEmpty';
import { useMutationRemove } from '@/_Composables/GraphQL/HistoricCall/useMutationRemove';
import { Notify } from 'quasar';
const { remove, removeLoading, removeError, removeDone } = useMutationRemove();
import MDIDeleteClock from '../SVG/MDIDeleteClock.vue';

const isOpen = ref<boolean>(false);
const id = ref<string | null>(null);

const resetAndOpen = (_id?: string) => {
	reset();
	open(_id);
};

const resetAndClose = () => {
	reset();
	close();
};


const confirm = () => {
	if (isEmpty(id.value)) {
		console.error('isEmpty(id.value)');
		return;
	}
	remove(id.value);
	
	console.log('confirm remove historic');
	
};


const open = (_id?: string) => {
	id.value = _id || null;
	isOpen.value = true;
};

const close = () => {
	isOpen.value = false;
};

const reset = () => {
	id.value = null;
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
		message: 'Removal Failed'
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

const onOpenViewConferenceDetailsModal = (params: TOnOpenRemoveHistoricCallModel) => {
	open(params.id);
}

onMounted(() => {
	onOpenRemoveHistoricCallModelSubscribe(onOpenViewConferenceDetailsModal);
});

onUnmounted(() => {
	onOpenRemoveHistoricCallModelUnsubscribe(onOpenViewConferenceDetailsModal);
})


</script>
<template>
	<q-dialog v-model="isOpen" persistent @hide="close"
		@show="open(id || undefined)">
		<q-card>
			<div class="flex flex-row p-4 gap-2">
				<q-avatar color="primary" text-color="white">
					<MDIDeleteClock class="m-2.5" />
				</q-avatar>
				<div class="flex flex-col gap-2 px-2 py-1">
					<span class="q-ml-sm font-bold">Are you sure you wish to remove this call?</span>
				</div>
			</div>

			<q-card-actions align="right">
				<q-btn flat label="Cancel" color="green" @click="resetAndClose" :disable="isAnyLoading" />
				<q-btn flat label="Remove" color="red" @click="confirm" :disable="isAnyLoading"
					:loading="removeLoading" />
			</q-card-actions>
		</q-card>
	</q-dialog>
</template>