<script setup lang="ts">

import { onMounted, onUnmounted, ref, computed } from 'vue';
import {
	type TCBParams as TonOpenViewActiveCallDetailsModalModal,
	subscribe as onOpenViewActiveCallDetailsModalSubscribe,
	unsubscribe as onOpenViewActiveCallDetailsModalUnsubscribe,
} from '@/_Composables/Events/onOpenViewActiveCallDetailsModal';
import { useQuerySpecific } from '@/_Composables/GraphQL/ActiveCall/useQuerySpecific';
import isEmpty from '@/_Composables/Utility/isEmpty';
import { useAccountCode } from '@/_Composables/GraphQL/ActiveCall/useAccountCode';
import { useLanguage } from '@/_Composables/GraphQL/ActiveCall/useLanguage';
import { useApplication } from '@/_Composables/GraphQL/ActiveCall/useApplication';
import { useApplicationData } from '@/_Composables/GraphQL/ActiveCall/useApplicationData';
import { useBridgeId } from '@/_Composables/GraphQL/ActiveCall/useBridgeId';
import { useCallerIdName } from '@/_Composables/GraphQL/ActiveCall/useCallerIdName';
import { useCallerIdNumber } from '@/_Composables/GraphQL/ActiveCall/useCallerIdNumber';
import { useChannelState } from '@/_Composables/GraphQL/ActiveCall/useChannelState';
import { useChannelStateDescription } from '@/_Composables/GraphQL/ActiveCall/useChannelStateDescription';
import { useContext } from '@/_Composables/GraphQL/ActiveCall/useContext';
import { useDuration } from '@/_Composables/GraphQL/ActiveCall/useDuration';
import { useExten } from '@/_Composables/GraphQL/ActiveCall/useExten';
import { useFarCallId } from '@/_Composables/GraphQL/ActiveCall/useFarCallId';
import { useLandedDID } from '@/_Composables/GraphQL/ActiveCall/useLandedDID';
import { useLinkedId } from '@/_Composables/GraphQL/ActiveCall/useLinkedId';
import { usePriority } from '@/_Composables/GraphQL/ActiveCall/usePriority';
import { useTimestampDateTime } from '@/_Composables/GraphQL/ActiveCall/useTimestampDateTime';
import { useState } from '@/_Composables/GraphQL/ActiveCall/useState';
import { useUniqueId } from '@/_Composables/GraphQL/ActiveCall/useUniqueId';
import { DateTime } from 'luxon';
import { Notify } from 'quasar';

const channelName = ref<string | null>(null);
const { specific, specificLoading, specificOnError, specificError } = useQuerySpecific(channelName);

const isOpen = ref<boolean>(false);
const accountCode = useAccountCode(specific);
const application = useApplication(specific);
const applicationData = useApplicationData(specific);
const bridgeId = useBridgeId(specific);
const callerIdName = useCallerIdName(specific);
const callerIdNumber = useCallerIdNumber(specific);
const channelState = useChannelState(specific);
const channelStateDescription = useChannelStateDescription(specific);
const context = useContext(specific);
const duration = useDuration(specific);
const durationDisplay = computed(() => {
	return duration.value.toHuman();
});
const exten = useExten(specific);
const farCallId = useFarCallId(specific);
const landedDID = useLandedDID(specific);
const language = useLanguage(specific);
const linkedId = useLinkedId(specific);
const priority = usePriority(specific);
const startDateTime = useTimestampDateTime(specific);
const startDateTimeLocalFormatted = computed(() => {
	const dt = startDateTime.value;
	const dtLocal = dt.toLocal();
	return dtLocal.toLocaleString(DateTime.DATETIME_MED);
})
const state = useState(specific);
const uniqueId = useUniqueId(specific);


const resetAndOpen = (_channelName?: string) => {
	reset();
	open(_channelName);
};

const resetAndClose = () => {
	reset();
	close();
};

const open = (_channelName?: string) => {
	channelName.value = _channelName || null;
	// console.log('channelName.value', channelName.value)
	isOpen.value = true;
};

const close = () => {
	isOpen.value = false;
};

const reset = () => {
	channelName.value = null;
};

specificOnError(() => {
	Notify.create({
		type: 'negative',
		message: 'Loading Failed'
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


const onOpenViewConferenceDetailsModal = (params: TonOpenViewActiveCallDetailsModalModal) => {
	open(params.channelName);
}

onMounted(() => {
	onOpenViewActiveCallDetailsModalSubscribe(onOpenViewConferenceDetailsModal);
});

onUnmounted(() => {
	onOpenViewActiveCallDetailsModalUnsubscribe(onOpenViewConferenceDetailsModal);
})




</script>
<template>
	<q-dialog v-model="isOpen" @hide="close" @show="open(channelName || undefined)" :loading="isAnyLoading">
		<q-card class="p-4">
			<div class="flex flex-col gap-2">

				<div class="text-xl flex flex-row items-center">
					<div class="px-2">
						Call Details
					</div>
					<q-space />
					<q-btn icon="close" flat round dense @click="close" />
				</div>

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

				<div class="flex flex-col gap-2 p-2">
					<div v-if="!isEmpty(accountCode)" class="flex flex-row items-center">
						<div class="text-md font-semibold w-1/3">Account Code</div>
						<div class="flex-1 break-all">{{ accountCode }}</div>
					</div>

					<div v-if="!isEmpty(application)" class="flex flex-row items-center">
						<div class="text-md font-semibold w-1/3">Application</div>
						<div class="flex-1 break-all">{{ application }}</div>
					</div>

					<div v-if="!isEmpty(applicationData)" class="flex flex-row items-center">
						<div class="text-md font-semibold w-1/3">Application Data</div>
						<div class="flex-1 break-all">{{ applicationData }}</div>
					</div>

					<div v-if="!isEmpty(bridgeId)" class="flex flex-row items-center">
						<div class="text-md font-semibold w-1/3">Bridge Id</div>
						<div class="flex-1 break-all">{{ bridgeId }}</div>
					</div>

					<div v-if="!isEmpty(callerIdName)" class="flex flex-row items-center">
						<div class="text-md font-semibold w-1/3">Caller Id Name</div>
						<div class="flex-1 break-all">{{ callerIdName }}</div>
					</div>

					<div v-if="!isEmpty(callerIdNumber)" class="flex flex-row items-center">
						<div class="text-md font-semibold w-1/3">Caller Id Number</div>
						<div class="flex-1 break-all">{{ callerIdNumber }}</div>
					</div>

					<div v-if="!isEmpty(channelState)" class="flex flex-row items-center">
						<div class="text-md font-semibold w-1/3">Channel State</div>
						<div class="flex-1 break-all">{{ channelState }}</div>
					</div>

					<div v-if="!isEmpty(channelStateDescription)" class="flex flex-row items-center">
						<div class="text-md font-semibold w-1/3">Channel State Description</div>
						<div class="flex-1 break-all">{{ channelStateDescription }}</div>
					</div>

					<div v-if="!isEmpty(context)" class="flex flex-row items-center">
						<div class="text-md font-semibold w-1/3">Context</div>
						<div class="flex-1 break-all">{{ context }}</div>
					</div>

					<div v-if="duration.isValid" class="flex flex-row items-center">
						<div class="text-md font-semibold w-1/3">Duration</div>
						<div class="flex-1 break-all">{{ durationDisplay }}</div>
					</div>

					<div v-if="!isEmpty(exten)" class="flex flex-row items-center">
						<div class="text-md font-semibold w-1/3">Extension</div>
						<div class="flex-1 break-all">{{ exten }}</div>
					</div>

					<div v-if="!isEmpty(farCallId)" class="flex flex-row items-center">
						<div class="text-md font-semibold w-1/3">Far Call Id</div>
						<div class="flex-1 break-all">{{ farCallId }}</div>
					</div>

					<div v-if="!isEmpty(landedDID)" class="flex flex-row items-center">
						<div class="text-md font-semibold w-1/3">Landed DID</div>
						<div class="flex-1 break-all">{{ landedDID }}</div>
					</div>

					<div v-if="!isEmpty(language)" class="flex flex-row items-center">
						<div class="text-md font-semibold w-1/3">Language</div>
						<div class="flex-1 break-all">{{ language }}</div>
					</div>

					<div v-if="!isEmpty(linkedId)" class="flex flex-row items-center">
						<div class="text-md font-semibold w-1/3">Linked Id</div>
						<div class="flex-1 break-all">{{ linkedId }}</div>
					</div>

					<div v-if="!isEmpty(priority)" class="flex flex-row items-center">
						<div class="text-md font-semibold w-1/3">Priority</div>
						<div class="flex-1 break-all">{{ priority }}</div>
					</div>

					<div v-if="startDateTime.isValid" class="flex flex-row items-center">
						<div class="text-md font-semibold w-1/3">Start Date &amp; Time</div>
						<div class="flex-1 break-all">{{ startDateTimeLocalFormatted }}</div>
					</div>

					<div v-if="!isEmpty(state)" class="flex flex-row items-center">
						<div class="text-md font-semibold w-1/3">State</div>
						<div class="flex-1 break-all">{{ state }}</div>
					</div>

					<div v-if="!isEmpty(uniqueId)" class="flex flex-row items-center">
						<div class="text-md font-semibold w-1/3">Unique Id</div>
						<div class="flex-1 break-all">{{ uniqueId }}</div>
					</div>







				</div>
			</div>



		</q-card>
	</q-dialog>
</template>