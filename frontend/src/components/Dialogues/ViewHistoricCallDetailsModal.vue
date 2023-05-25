<script setup lang="ts">

import { onMounted, onUnmounted, ref, computed } from 'vue';
import {
	type TCBParams as TOnOpenViewHistoricCallDetailsModalModal,
	subscribe as onOpenViewHistoricCallDetailsModalSubscribe,
	unsubscribe as onOpenViewHistoricCallDetailsModalUnsubscribe,
} from '@/_Composables/Events/onOpenViewHistoricCallDetailsModal';
import { useQuerySpecific } from '@/_Composables/GraphQL/HistoricCall/useQuerySpecific';
import isEmpty from '@/_Composables/Utility/isEmpty';
import { useCallerIdName } from '@/_Composables/GraphQL/HistoricCall/useCallerIdName';
import { useCallerIdNumber } from '@/_Composables/GraphQL/HistoricCall/useCallerIdNumber';
import { useDuration } from '@/_Composables/GraphQL/HistoricCall/useDuration';
import { useLandedDID } from '@/_Composables/GraphQL/HistoricCall/useLandedDID';
import { useOriginalChannel } from '@/_Composables/GraphQL/HistoricCall/useOriginalChannel';
import { useTimestampDateTime } from '@/_Composables/GraphQL/HistoricCall/useTimestampDateTime';
import { DateTime } from 'luxon';
import { Notify } from 'quasar';

const id = ref<string | null>(null);
const { specific, specificLoading, specificOnError, specificError } = useQuerySpecific(id);
const isOpen = ref<boolean>(false);
const callerIdName = useCallerIdName(specific);
const callerIdNumber = useCallerIdNumber(specific);
const duration = useDuration(specific);
const durationDisplay = computed(() => {
	return duration.value.toHuman();
});
const landedDID = useLandedDID(specific);
const originalChannel = useOriginalChannel(specific);
const startDateTime = useTimestampDateTime(specific);
const startDateTimeLocalFormatted = computed(() => {
	const dt = startDateTime.value;
	const dtLocal = dt.toLocal();
	return dtLocal.toLocaleString(DateTime.DATETIME_MED);
})

const resetAndOpen = (_id?: string) => {
	reset();
	open(_id);
};

const resetAndClose = () => {
	reset();
	close();
};

const open = (_id?: string) => {
	id.value = _id || null;
	// console.log('open', id.value);
	isOpen.value = true;
};

const close = () => {
	isOpen.value = false;
};

const reset = () => {
	id.value = null;
};

specificOnError(() => {
	Notify.create({
		type: 'negative',
		message: 'Loading Historic Call Details Failed'
	});
});

defineExpose({
	open,
	close,
	resetAndOpen,
	resetAndClose,
});

const onOpenViewConferenceDetailsModal = (params: TOnOpenViewHistoricCallDetailsModalModal) => {
	// console.log('onOpenViewConferenceDetailsModal params', params);
	open(params.id);
};

onMounted(() => {
	onOpenViewHistoricCallDetailsModalSubscribe(onOpenViewConferenceDetailsModal);
});

onUnmounted(() => {
	onOpenViewHistoricCallDetailsModalUnsubscribe(onOpenViewConferenceDetailsModal);
});




</script>
<template>
	<q-dialog v-model="isOpen" @hide="close" @show="open(id || undefined)" :loading="specificLoading">
		<q-card class="p-4" :loading="specificLoading">
			<div class="flex flex-col gap-2">

				<div class="text-xl flex flex-row">
					<div class="px-2">
						Call Details
					</div>
					<q-space />
					<q-btn icon="close" flat round dense @click="close" />
				</div>
				<div class="flex flex-col gap-2 p-2">

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


					<div v-if="!isEmpty(callerIdName)" class="flex flex-row items-center">
						<div class="text-md font-semibold w-1/3">Caller Id Name</div>
						<div class="flex-1 break-all">{{ callerIdName }}</div>
					</div>

					<div v-if="!isEmpty(callerIdNumber)" class="flex flex-row items-center">
						<div class="text-md font-semibold w-1/3">Caller Id Number</div>
						<div class="flex-1 break-all">{{ callerIdNumber }}</div>
					</div>

					<div v-if="duration.isValid" class="flex flex-row items-center">
						<div class="text-md font-semibold w-1/3">Duration</div>
						<div class="flex-1 break-all">{{ durationDisplay }}</div>
					</div>

					<div v-if="!isEmpty(landedDID)" class="flex flex-row items-center">
						<div class="text-md font-semibold w-1/3">Landed DID</div>
						<div class="flex-1 break-all">{{ landedDID }}</div>
					</div>

					<div v-if="!isEmpty(originalChannel)" class="flex flex-row items-center">
						<div class="text-md font-semibold w-1/3">Original Channel</div>
						<div class="flex-1 break-all">{{ originalChannel }}</div>
					</div>

					<div v-if="startDateTime.isValid" class="flex flex-row items-center">
						<div class="text-md font-semibold w-1/3">Start Date &amp; Time</div>
						<div class="flex-1 break-all">{{ startDateTimeLocalFormatted }}</div>
					</div>

				</div>
			</div>

		</q-card>
	</q-dialog>
</template>