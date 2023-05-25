<script setup lang="ts">
import type { IActiveCall } from '@/_Composables/GraphQL/ActiveCall/IActiveCall';
import { useModelValue } from '@/_Composables/Utility/useModelValue';
import { useCallerIdName } from '@/_Composables/GraphQL/ActiveCall/useCallerIdName';
import { useCallerIdNumber } from '@/_Composables/GraphQL/ActiveCall/useCallerIdNumber';
import MDIAccount from '../SVG/MDIAccount.vue';
import MDIPhoneHangup from '../SVG/MDIPhoneHangup.vue';
import MDIInformationOutline from '../SVG/MDIInformationOutline.vue';
import isEmpty from '@/_Composables/Utility/isEmpty';
import { useChannelName } from '@/_Composables/GraphQL/ActiveCall/useChannelName';
// import MDIPhonePlus from '../SVG/MDIPhonePlus.vue';
import { 
	trigger as onOpenHangUpChannelModelTrigger
 } from '@/_Composables/Events/onOpenHangUpChannelModel';
 import { 
	trigger as onOpenViewActiveCallDetailsModalTrigger
 } from '@/_Composables/Events/onOpenViewActiveCallDetailsModal';
// import MDIInformationOutline from '../SVG/MDIInformationOutline.vue';

const props = defineProps<{
	modelValue?: IActiveCall,
	value?: IActiveCall,
}>();

const emit = defineEmits<{
	(e: 'update:modelValue', payload: IActiveCall | null): void,
	(e: 'on-value-changed', payload: IActiveCall | null): void,
}>();

const model = useModelValue(emit, props, null as IActiveCall | null);

const callerIdName = useCallerIdName(model);
const callerIdNumber = useCallerIdNumber(model);
const channelName = useChannelName(model);

const onClickViewCallDetails = () => {
	onOpenViewActiveCallDetailsModalTrigger({
		channelName: channelName.value
	});
};

const onClickHangUp = () => {
	onOpenHangUpChannelModelTrigger({
		channelName: channelName.value,
		callerIdName: callerIdName.value,
		callerIdNumber: callerIdNumber.value,
	})
}

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
				<div v-if="!isEmpty(callerIdName)" class="font-bold">{{ callerIdName }}</div>
				<div v-if="!isEmpty(callerIdNumber)">{{ callerIdNumber }}</div>
			</q-item-section>

			<q-item-section side>
				<div class="row items-center">
					<q-btn @click.stop="onClickHangUp" flat rounded dense>
						<q-tooltip anchor="bottom middle" self="top middle">Force Call to Hang Up</q-tooltip>
						<MDIPhoneHangup class="w-6 h-6" style="color: red;" />
					</q-btn>
				</div>
			</q-item-section>
		</template>

		<q-card class="border-t">
			<q-item clickable v-ripple @click="onClickHangUp">
				<q-item-section avatar>
					<MDIPhoneHangup class="w-6 h-6" />
				</q-item-section>
				<q-item-section>Force call to hang up&hellip;</q-item-section>
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
	</q-expansion-item>
</template>
