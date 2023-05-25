<script setup lang="ts">

import { ref } from 'vue';

import ActiveCallList from '@/components/Lists/ActiveCallList.vue';
import ConferenceList from '@/components/Lists/ConferenceList.vue'
import HistoricCallList from '@/components/Lists/HistoricCallList.vue'
import MDIPhoneIncomingOutgoing from '@/components/SVG/MDIPhoneIncomingOutgoing.vue';
import MDIHistory from '@/components/SVG/MDIHistory.vue';
import MDICog from '@/components/SVG/MDICog.vue';
import MDIVoiceMail from '@/components/SVG/MDIVoiceMail.vue';
import MDIDialPad from '@/components/SVG/MDIDialPad.vue';
import PJSIPClientSettings from '@/components/Sections/PJSIPClientSettings.vue';
import PJSIPTrunkSettings from '@/components/Sections/PJSIPTrunkSettings.vue';
import E164Settings from '@/components/Sections/E164Settings.vue';
import E164ClientSettings from '@/components/Sections/E164ClientSettings.vue';

import {
	trigger as onOpenOutboundCallModalTrigger
} from '@/_Composables/Events/onOpenOutboundCallModal';
import VoiceMailList from '@/components/Lists/VoiceMailList.vue';

const tab = ref<'Active' | 'History' | 'VoiceMail' | 'Settings'>('Active');

const onClickDialpad = () => {
	onOpenOutboundCallModalTrigger({
		e164: '',
	});
}

</script>

<template>
	<main class="flex flex-col">

		<q-layout view="hHh lpR fFf">

			<q-header elevated class="bg-primary text-white" height-hint="98">

				<q-tabs v-model="tab" inline-label class="">

					<q-tab name="Active">
						<q-tooltip anchor="bottom middle" self="top middle">Active</q-tooltip>
						<div class="flex flex-row gap-2 items-center">
							<MDIPhoneIncomingOutgoing class="w-5 h-5" />
						</div>
					</q-tab>
					<q-tab name="History">
						<q-tooltip anchor="bottom middle" self="top middle">History</q-tooltip>
						<div class="flex flex-row gap-2 items-center">
							<MDIHistory class="w-5 h-5" />
						</div>
					</q-tab>
					<q-tab name="VoiceMail">
						<q-tooltip anchor="bottom middle" self="top middle">Voice Mail</q-tooltip>
						<div class="flex flex-row gap-2 items-center">
							<MDIVoiceMail class="w-5 h-5" />
						</div>
					</q-tab>
					<q-tab name="Settings">
						<q-tooltip anchor="bottom middle" self="top middle">Settings</q-tooltip>
						<div class="flex flex-row gap-2 items-center">
							<MDICog class="w-5 h-5" />
						</div>
					</q-tab>
				</q-tabs>
			</q-header>

			<q-page-container>

				<q-tab-panels v-model="tab" animated>
					<q-tab-panel name="Active">
						<ConferenceList />
						<ActiveCallList />
					</q-tab-panel>

					<q-tab-panel name="History">
						<HistoricCallList />
					</q-tab-panel>

					<q-tab-panel name="VoiceMail">
						<VoiceMailList />
						
						
					</q-tab-panel>

					<q-tab-panel name="Settings">
						<div class="flex flex-col gap-4">
							<E164Settings />
							<E164ClientSettings />
							<PJSIPClientSettings />
							<PJSIPTrunkSettings />
							
						</div>
					</q-tab-panel>
				</q-tab-panels>

			</q-page-container>

			<q-page-sticky position="bottom-right" :offset="[18, 18]">
				<q-btn fab color="accent" @click="onClickDialpad">
					<MDIDialPad />
				</q-btn>
			</q-page-sticky>


		</q-layout>
	</main>
</template>
