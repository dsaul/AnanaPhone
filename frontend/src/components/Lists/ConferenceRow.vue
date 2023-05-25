<script setup lang="ts">
import type { IConference } from '@/_Composables/GraphQL/Conference/IConference';
import { useModelValue } from '@/_Composables/Utility/useModelValue';
import { useDisplayName } from '@/_Composables/GraphQL/Conference/useDisplayName';
import MDIAccount from '../SVG/MDIAccount.vue';
import MDIPhonePlus from '../SVG/MDIPhonePlus.vue';
import { 
	trigger as onOpenViewConferenceDetailsModalTrigger
 } from '@/_Composables/Events/onOpenViewConferenceDetailsModal';
import MDIInformationOutline from '../SVG/MDIInformationOutline.vue';
import { 
	trigger as onOpenJoinConferenceModalTrigger
 } from '@/_Composables/Events/onOpenJoinConferenceModal';
import { useParticipants } from '@/_Composables/GraphQL/Conference/useParticipants';
import { computed } from 'vue';
import isEmpty from '@/_Composables/Utility/isEmpty';

const props = defineProps<{
	modelValue?: IConference,
	value?: IConference,
}>();

const emit = defineEmits<{
	(e: 'update:modelValue', payload: IConference | null): void,
	(e: 'on-value-changed', payload: IConference | null): void,
}>();

const model = useModelValue(emit, props, null as IConference | null);

const displayName = useDisplayName(model);

const onClickViewConferenceDetails = () => {
	
	if (!model.value?.name) {
		return;
	}
	
	onOpenViewConferenceDetailsModalTrigger({
		conferenceName: model.value?.name
	});
};

const onClickJoinConference = () => {
	
	if (!model.value?.name) {
		return;
	}
	
	onOpenJoinConferenceModalTrigger({
		conferenceName: model.value?.name
	});
};

const participants = useParticipants(model);
const participantCountDisplay = computed(() => {
	
	const count = participants.value.length;
	
	switch (count) {
		case 0: {
			return 'No participants';
		}
		case 1: {
			return '1 participant';
		}
		default: {
			return `${count} participants`;
		}
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
				<div v-if="!isEmpty(displayName)" class="font-bold">{{ displayName }}</div>
				<div v-if="!isEmpty(participantCountDisplay)">{{ participantCountDisplay }}</div>
			</q-item-section>
			
			<q-item-section side>
				<div class="row items-center">
					<q-btn @click.stop="onClickJoinConference" flat rounded>
						<q-tooltip anchor="bottom middle" self="top middle">Join Conference</q-tooltip>
						<MDIPhonePlus class="w-6 h-6" />
					</q-btn>
				</div>
			</q-item-section>
		</template>

		<q-card class="border-t">
			<q-item clickable v-ripple @click="onClickJoinConference">
				<q-item-section avatar>
					<MDIPhonePlus class="w-6 h-6" />
				</q-item-section>
				<q-item-section>Join&hellip;</q-item-section>
			</q-item>
			<!-- <q-item clickable v-ripple>
				<q-item-section avatar>
					<q-icon color="primary" name="bluetooth" />
				</q-item-section>
				<q-item-section>Block a number&hellip;</q-item-section>
			</q-item> -->
			<q-item clickable v-ripple @click="onClickViewConferenceDetails">
				<q-item-section avatar>
					<MDIInformationOutline class="w-6 h-6" />
				</q-item-section>
				<q-item-section>View details&hellip;</q-item-section>
			</q-item>
		</q-card>
	</q-expansion-item>
</template>
