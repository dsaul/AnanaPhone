<script setup lang="ts">
import type { IHistoricCall } from '@/_Composables/GraphQL/HistoricCall/IHistoricCall';
import { useModelValue } from '@/_Composables/Utility/useModelValue';
import { useCallerIdName } from '@/_Composables/GraphQL/HistoricCall/useCallerIdName';
import { useCallerIdNumber } from '@/_Composables/GraphQL/HistoricCall/useCallerIdNumber';
import { useCallDirection } from '@/_Composables/GraphQL/HistoricCall/useCallDirection';
import { useId } from '@/_Composables/GraphQL/HistoricCall/useId';
import MDIAccount from '../SVG/MDIAccount.vue';
import MDIPhone from '../SVG/MDIPhone.vue';
import MDIInformationOutline from '../SVG/MDIInformationOutline.vue';
import MDIPhoneOutgoing from '../SVG/MDIPhoneOutgoing.vue';
import MDIPhoneIncoming from '../SVG/MDIPhoneIncoming.vue';
import MDIDelete from '../SVG/MDIDelete.vue';

import isEmpty from '@/_Composables/Utility/isEmpty';
import { computed } from 'vue';
import { useTimestampDateTime } from '@/_Composables/GraphQL/HistoricCall/useTimestampDateTime';
import { DateTime } from 'luxon';
import { 
	trigger as onOpenOutboundCallModalTrigger
 } from '@/_Composables/Events/onOpenOutboundCallModal';
import { 
	trigger as onOpenViewHistoricCallDetailsModalTrigger
} from '@/_Composables/Events/onOpenViewHistoricCallDetailsModal';
import {
	trigger as onOpenRemoveHistoricCallModelTrigger
} from '@/_Composables/Events/onOpenRemoveHistoricCallModal';
const props = defineProps<{
	modelValue?: IHistoricCall,
	value?: IHistoricCall,
}>();

const emit = defineEmits<{
	(e: 'update:modelValue', payload: IHistoricCall | null): void,
	(e: 'on-value-changed', payload: IHistoricCall | null): void,
}>();

const model = useModelValue(emit, props, null as IHistoricCall | null);
const id = useId(model);
const callerIdName = useCallerIdName(model);
const callerIdNumber = useCallerIdNumber(model);
const callDirection = useCallDirection(model);
const callDirectionInbound = computed(() => {
	return callDirection.value === 'Inbound';
});
const callDirectionOutbound = computed(() => {
	return callDirection.value === 'Outbound';
});
const timestamp = useTimestampDateTime(model);
const timestampDisplay = computed(() => {
	return timestamp.value.toLocaleString(DateTime.DATETIME_SHORT);
});

const onClickViewCallDetails = () => {
	onOpenViewHistoricCallDetailsModalTrigger({
		id: id.value
	});
};

const onClickCall = () => {
	onOpenOutboundCallModalTrigger({
		e164: callerIdNumber.value,
	});
}

const onClickRemove = () => {
	onOpenRemoveHistoricCallModelTrigger({
		id: id.value,
	});
}

const displayName = computed(() => {
	if (!isEmpty(callerIdName.value) && !isEmpty(callerIdNumber.value)) 
	{
		return `${callerIdName.value} (${callerIdNumber.value})`;
	}
	else if (!isEmpty(callerIdName.value) && isEmpty(callerIdNumber.value))
	{
		return `${callerIdName.value}`;
	}
	else if (isEmpty(callerIdName.value) && !isEmpty(callerIdNumber.value))
	{
		return `${callerIdNumber.value}`;
	}
	else
	{
		return 'Unknown';
	}
})


</script>
<template>
	<q-expansion-item class="shadow-2">
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
					<MDIPhoneIncoming v-if="callDirectionInbound" class="w-4 h-4" style="color: blue;" />
					<MDIPhoneOutgoing v-if="callDirectionOutbound" class="w-4 h-4" style="color: green;" />
					<div>{{ timestampDisplay }}</div>
				</div>
			</q-item-section>

			<q-item-section side>
				<div class="row items-center">
					<q-btn @click.stop="onClickCall" flat rounded dense>
						<q-tooltip anchor="bottom middle" self="top middle">Call</q-tooltip>
						<MDIPhone class="w-6 h-6" />
					</q-btn>
				</div>
			</q-item-section>
		</template>

		<q-card class="border-t">
			<q-item clickable v-ripple @click="onClickCall">
				<q-item-section avatar>
					<MDIPhone class="w-6 h-6" />
				</q-item-section>
				<q-item-section>Call&hellip;</q-item-section>
			</q-item>
		</q-card>
		<q-card>
			<q-item clickable v-ripple @click="onClickViewCallDetails">
				<q-item-section avatar>
					<MDIInformationOutline class="w-6 h-6" />
				</q-item-section>
				<q-item-section>View details&hellip;</q-item-section>
			</q-item>
		</q-card>
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
