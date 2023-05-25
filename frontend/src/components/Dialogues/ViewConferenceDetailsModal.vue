<script setup lang="ts">

import { onMounted, onUnmounted, ref, computed } from 'vue';
import {
	type TCBParams as TOnOpenViewConferenceDetailsModal,
	subscribe as onOpenViewConferenceDetailsModalSubscribe,
	unsubscribe as onOpenViewConferenceDetailsModalUnsubscribe,
} from '@/_Composables/Events/onOpenViewConferenceDetailsModal';
import { useQuerySpecific } from '@/_Composables/GraphQL/Conference/useQuerySpecific';
import { useName } from '@/_Composables/GraphQL/Conference/useName';
import { useStartLocalFormatted } from '@/_Composables/GraphQL/Conference/useStartLocalFormatted';
import { useParticipants } from '@/_Composables/GraphQL/Conference/useParticipants';
import isEmpty from '@/_Composables/Utility/isEmpty';
import { Notify } from 'quasar';

const conferenceName = ref<string | null>(null);
const { specific, specificLoading, specificOnError, specificError } = useQuerySpecific(conferenceName);


const isOpen = ref<boolean>(false);
const roomName = useName(specific);
const startTime = useStartLocalFormatted(specific);
const participants = useParticipants(specific);
const hasParticipants = computed(() => {
	return participants.value.length !== 0;
});

const resetAndOpen = (_conferenceName?: string) => {
	reset();
	open(_conferenceName);
};

const resetAndClose = () => {
	reset();
	close();
};

const open = (_conferenceName?: string) => {
	conferenceName.value = _conferenceName || null;
	isOpen.value = true;
};

const close = () => {
	isOpen.value = false;
};

const reset = () => {
	conferenceName.value = null;
};

specificOnError(() => {
	Notify.create({
		type: 'negative',
		message: 'Loading Conference Details Failed'
	});
});

const isAnyLoading = computed(() => {
	return specificLoading.value;
});

defineExpose({
	open,
	close,
	resetAndOpen,
	resetAndClose,
});

const onOpenViewConferenceDetailsModal = (params: TOnOpenViewConferenceDetailsModal) => {
	open(params.conferenceName);
}

onMounted(() => {
	onOpenViewConferenceDetailsModalSubscribe(onOpenViewConferenceDetailsModal);
});

onUnmounted(() => {
	onOpenViewConferenceDetailsModalUnsubscribe(onOpenViewConferenceDetailsModal);
});

</script>
<template>
	<q-dialog v-model="isOpen" @hide="close" @show="open(conferenceName || undefined)" :loading="isAnyLoading">
		<q-card style="width: 350px" class="p-8">
			<div class="flex flex-col gap-2">

				<div v-if="specificError" class="px-3">
					<q-banner inline-actions class="text-white bg-red" rounded>
						<div class="flex flex-row gap-4 items-center">
							<MDIAlert class="w-6 h-6" />
							<div class="flex flex-col gap-0.5">
								<div class="font-bold">There was an error loading.</div>
								<div v-if="!isEmpty(specificError?.message)">{{ specificError?.message }}</div>
							</div>
						</div>
					</q-banner>
				</div>

				<div class="text-xl">Room Details</div>
				<div class="text-lg text-grey-33">General</div>
				<div class="flex flex-col">
					<div class="flex flex-row">
						<div class="text-md font-semibold w-1/3">Name</div>
						<div class="flex-1 break-all">{{ roomName }}</div>
					</div>
					<div class="flex flex-row">
						<div class="text-md font-semibold w-1/3">Started At</div>
						<div class="flex-1 break-all">{{ startTime }}</div>
					</div>
				</div>
				<div class="text-xl">Participants</div>
				<ul class="pl-4 flex flex-col gap-2">
					<li v-for="(participant, participantIdx) of (participants || [])" :key="participantIdx"
						class="list-disc">
						<div class="flex flex-col gap-1">
							<div class="flex flex-row">
								<div class="text-md font-semibold w-1/3">Channel</div>
								<div class="flex-1 break-all">{{ participant.channel }}</div>
							</div>
							<div v-if="!isEmpty(participant.callerIdName)" class="flex flex-row">
								<div class="text-md font-semibold w-1/3">Name</div>
								<div class="flex-1 break-all">{{ participant.callerIdName }}</div>
							</div>
							<div v-if="!isEmpty(participant.callerIdNumber)" class="flex flex-row">
								<div class="text-md font-semibold w-1/3">Number</div>
								<div class="flex-1 break-all">{{ participant.callerIdNumber }}</div>
							</div>
						</div>

					</li>
				</ul>
				<div v-if="!hasParticipants">
					No Participants
				</div>
			</div>



		</q-card>
	</q-dialog>
</template>