<script setup lang="ts">
import { RouterView } from 'vue-router'
import JoinConferenceModal from './components/Dialogues/JoinConferenceModal.vue';
import ViewConferenceDetailsModal from './components/Dialogues/ViewConferenceDetailsModal.vue';
import HangUpChannelModal from './components/Dialogues/HangUpChannelModal.vue';
import ViewActiveCallDetailsModal from './components/Dialogues/ViewActiveCallDetailsModal.vue';
import ViewHistoricCallDetailsModal from './components/Dialogues/ViewHistoricCallDetailsModal.vue';
import OutboundCallModal from './components/Dialogues/OutboundCallModal.vue';
import RemoveHistoricCallModal from './components/Dialogues/RemoveHistoricCallModal.vue';
</script>

<template>
	<div>
		<RemoveHistoricCallModal />
		<OutboundCallModal />
		<ViewHistoricCallDetailsModal />
		<ViewActiveCallDetailsModal />
		<HangUpChannelModal />
		<JoinConferenceModal />
		<ViewConferenceDetailsModal />
		<RouterView />
	</div>
</template>