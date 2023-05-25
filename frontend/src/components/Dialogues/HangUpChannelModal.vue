<script setup lang="ts">

import { onMounted, onUnmounted, ref, computed } from 'vue';
import {
	type TCBParams as TOnOpenHangUpChannelModel,
	subscribe as onOpenHangUpChannelModelSubscribe,
	unsubscribe as onOpenHangUpChannelModelUnsubscribe,
} from '@/_Composables/Events/onOpenHangUpChannelModel';
import isEmpty from '@/_Composables/Utility/isEmpty';
import { useMutationHangupChannel } from '@/_Composables/GraphQL/ActiveCall/useMutationHangupChannel';
import { Notify } from 'quasar';
const { hangupChannel, hangupChannelLoading, hangupChannelError, hangupChannelDone } = useMutationHangupChannel();

const isOpen = ref<boolean>(false);
const channelName = ref<string | null>(null);
const callerIdName = ref<string | null>(null);
const callerIdNumber = ref<string | null>(null);


const resetAndOpen = (_channelName: string | null, _callerIdName: string | null, _callerIdNumber: string | null) => {
	reset();
	open(_channelName, _callerIdName, _callerIdNumber);
};

const resetAndClose = () => {
	reset();
	close();
};

const confirm = () => {
	if (isEmpty(channelName.value)) {
		console.error('isEmpty(channelName.value)');
		return;
	}
	hangupChannel(channelName.value);
};


const open = (_channelName?: string | null, _callerIdName?: string | null, _callerIdNumber?: string | null) => {
	channelName.value = _channelName || null;
	callerIdName.value = _callerIdName || null;
	callerIdNumber.value = _callerIdNumber || null;

	isOpen.value = true;
};

const close = () => {
	isOpen.value = false;
};

const reset = () => {
	channelName.value = null;
	callerIdName.value = null;
	callerIdNumber.value = null;
};

hangupChannelDone(() => {
	Notify.create({
		type: 'positive',
		message: 'Channel Hung Up'
	});

	reset();
	close();
});

hangupChannelError(() => {
	Notify.create({
		type: 'negative',
		message: 'Channel Hangup Failed'
	});
});


const isAnyLoading = computed(() => {
	return hangupChannelLoading.value;
});


defineExpose({
	open,
	close,
	resetAndOpen,
	resetAndClose,
});


const onOpenViewConferenceDetailsModal = (params: TOnOpenHangUpChannelModel) => {
	resetAndOpen(params.channelName, params.callerIdName, params.callerIdNumber);
};

onMounted(() => {
	onOpenHangUpChannelModelSubscribe(onOpenViewConferenceDetailsModal);
});

onUnmounted(() => {
	onOpenHangUpChannelModelUnsubscribe(onOpenViewConferenceDetailsModal);
});

</script>
<template>
	<q-dialog v-model="isOpen" persistent @hide="close" @show="open(channelName, callerIdName, callerIdNumber)">
		<q-card>
			<div class="flex flex-row p-4 gap-2">
				<q-avatar icon="signal_wifi_off" color="primary" text-color="white" />
				<div class="flex-1 flex flex-col gap-2">
					<span class="q-ml-sm font-bold">Are you sure you wish to hang up this channel?</span>
					<span class="q-ml-sm">{{ channelName }}</span>
					<span v-if="!isEmpty(callerIdName)" class="q-ml-sm">{{ callerIdName }}</span>
					<span v-if="!isEmpty(callerIdNumber)" class="q-ml-sm">{{ callerIdNumber }}</span>
				</div>
			</div>

			<q-card-actions align="right">
				<q-btn flat label="Cancel" color="green" @click="resetAndClose" :disable="isAnyLoading" />
				<q-btn flat label="Hang-Up" color="red" @click="confirm" :disable="isAnyLoading"
					:loading="hangupChannelLoading" />
			</q-card-actions>
		</q-card>
	</q-dialog>
</template>