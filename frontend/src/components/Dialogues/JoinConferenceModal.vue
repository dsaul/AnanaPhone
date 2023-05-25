<script setup lang="ts">

import { computed, onMounted, onUnmounted, ref } from 'vue';
import MDIPhonePlus from '../SVG/MDIPhonePlus.vue';
import {
	type TCBParams as TOnOpenJoinConferenceModal,
	subscribe as onOpenJoinConferenceModalSubscribe,
	unsubscribe as onOpenJoinConferenceModalUnsubscribe,
} from '@/_Composables/Events/onOpenJoinConferenceModal';
import { useName } from '@/_Composables/GraphQL/Conference/useName';
import isEmpty from '@/_Composables/Utility/isEmpty';
import { useQueryJoinConferenceData } from '@/_Composables/GraphQL/Conference/useQueryJoinConferenceData';
import { useMutationJoinConfbridge } from '@/_Composables/GraphQL/Conference/useMutationJoinConfbridge';
import { Notify } from 'quasar';
const { joinConfbridge, joinConfbridgeLoading, joinConfbridgeError, joinConfbridgeDone } = useMutationJoinConfbridge();

const isOpen = ref<boolean>(false);
const conferenceName = ref<string | null>(null);

const {
	allowedCallOutNumbers,
	room,
} = useQueryJoinConferenceData(conferenceName);
const roomName = useName(room);

const resetAndOpen = (_conferenceName?: string | null) => {
	reset();
	open(_conferenceName);
};

const resetAndClose = () => {
	reset();
	close();
};


const confirm = (channel: string) => {
	if (isEmpty(conferenceName.value)) {
		console.error('isEmpty(conferenceName.value)');
		return;
	}

	joinConfbridge(
		conferenceName.value,
		channel,
	);

	close();
};


const open = (_conferenceName?: string | null) => {
	if (!isEmpty(_conferenceName)) {
		conferenceName.value = _conferenceName;
	}
	isOpen.value = true;
};

const close = () => {
	isOpen.value = false;
};

const reset = () => {
	conferenceName.value = null;
};

joinConfbridgeDone(() => {
	Notify.create({
		type: 'positive',
		message: 'Join Call Originated'
	});

	reset();
	close();
});

joinConfbridgeError(() => {
	Notify.create({
		type: 'negative',
		message: 'Join Call Failed'
	});
});

const isAnyLoading = computed(() => {
	return joinConfbridgeLoading.value;
});


defineExpose({
	open,
	close,
	resetAndOpen,
	resetAndClose,
});

onMounted(() => {
	onOpenJoinConferenceModalSubscribe(onOpenJoinConferenceModal);
});

onUnmounted(() => {
	onOpenJoinConferenceModalUnsubscribe(onOpenJoinConferenceModal);
})

const onOpenJoinConferenceModal = (params: TOnOpenJoinConferenceModal) => {
	open(params.conferenceName);
}





</script>
<template>
	<q-dialog v-model="isOpen" @hide="close" @show="open(conferenceName)">
		<q-card style="width: 300px" class="q-px-sm q-pb-md">
			<q-card-section>
				<div class="text-h6">Join Conference</div>
			</q-card-section>
			<q-item v-if="!isEmpty(roomName)" dense>
				<q-item-label caption>Room Name: {{ roomName }}</q-item-label>
			</q-item>

			<q-item v-for="(channel, channelIdx) of (allowedCallOutNumbers || [])" :disable="isAnyLoading"
				:loading="joinConfbridgeLoading" :key="channelIdx" clickable v-ripple @click="confirm(channel)">
				<q-item-section avatar>
					<MDIPhonePlus class="w-6 h-6" />
				</q-item-section>
				<q-item-section>Call {{ channel }}&hellip;</q-item-section>
			</q-item>
		</q-card>
	</q-dialog>
</template>