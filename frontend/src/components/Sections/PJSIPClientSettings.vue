<script setup lang="ts">
import { useQueryClients } from '@/_Composables/GraphQL/Settings/useQueryClients';
import { useQueryClientDefaults } from '@/_Composables/GraphQL/Settings/useQueryClientDefaults';
import PJSIPEntryEditor from '@/components/Editors/PJSIPEntryEditor.vue';
import { type IPJSIPEntry } from '@/_Composables/GraphQL/PJSIPEntry/IPJSIPEntry';
import MDIPlus from '@/components/SVG/MDIPlus.vue';
import { computed, ref } from 'vue';
import { Notify } from 'quasar';

import AddPJSIPEntryModal from '../Dialogues/AddPJSIPEntryModal.vue';
import isEmpty from '@/_Composables/Utility/isEmpty';
import MDIAlert from '../SVG/MDIAlert.vue';
import MDIInformation from '../SVG/MDIInformation.vue';



const { defaults, defaultsLoading, defaultsOnError, defaultsError } = useQueryClientDefaults();
const { clients, clientsLoading, clientsOnError, clientsError } = useQueryClients();

const anyClients = computed<boolean>(() => {
	return clients.value.length !== 0;
});

defaultsOnError(() => {
	Notify.create({
		type: 'negative',
		message: 'Loading Defaults Failed'
	});
});

clientsOnError(() => {
	Notify.create({
		type: 'negative',
		message: 'Loading Clients Failed'
	});
});

const addPJSIPEntryModalEl = ref();

const onClientChanged = (idx: number, payload: IPJSIPEntry | null) => {
	console.log('onClientChanged', idx, payload)
}

const onClickAdd = () => {
	if (addPJSIPEntryModalEl.value) {
		addPJSIPEntryModalEl.value.resetAndOpen();
	}
};


</script>
<template>
	<div class="flex flex-col gap-2">
		<AddPJSIPEntryModal ref="addPJSIPEntryModalEl" usesTemplate="client_defaults" :isTemplate="false" noun="client" />

		<div class="flex flex-row items-end">
			<div class="font-bold">PJSIP Clients</div>
			<q-space />
			<q-btn flat color="primary" dense @click="onClickAdd">
				<div class="flex flex-row gap-1 items-center">
					<MDIPlus class="w-4 h-4" />
					<div>Add</div>
				</div>
			</q-btn>
		</div>
		<div v-if="clientsError">
			<q-banner inline-actions class="text-white bg-red" rounded>
				<div class="flex flex-row gap-4 items-center">
					<MDIAlert class="w-6 h-6" />
					<div class="flex flex-col gap-0.5">
						<div class="font-bold">There was an error loading.</div>
						<div v-if="!isEmpty(clientsError?.message)">{{ clientsError?.message }}</div>
					</div>
				</div>
			</q-banner>
		</div>
		<div v-if="clientsLoading">
			<q-skeleton bordered type="rect" class="h-16" />
		</div>
		<div v-if="!anyClients && !clientsLoading">
			<q-banner inline-actions class="text-black bg-grey-2" rounded>
				<div class="flex flex-row gap-4 items-center">
					<MDIInformation class="w-6 h-6 text-primary" />
					<div class="flex flex-col gap-0.5">
						<div class="font-bold">No Numbers</div>
					</div>
				</div>
			</q-banner>
		</div>
		
		<PJSIPEntryEditor v-for="(client, clientIdx) of clients" :key="clientIdx" :value="client"
			@on-value-changed="onClientChanged(clientIdx, $event)" usesTemplate="client_defaults" :isTemplate="false" />

			
			
			
			
		<div class="font-bold">PJSIP Client Template</div>
		<div v-if="defaultsError">
			<q-banner inline-actions class="text-white bg-red" rounded>
				<div class="flex flex-row gap-4 items-center">
					<MDIAlert class="w-6 h-6" />
					<div class="flex flex-col gap-0.5">
						<div class="font-bold">There was an error loading.</div>
						<div v-if="!isEmpty(defaultsError?.message)">{{ defaultsError?.message }}</div>
					</div>
				</div>
			</q-banner>
		</div>
		<div v-if="defaultsLoading">
			<q-skeleton bordered type="rect" class="h-16" />
		</div>
		<div v-if="!defaultsLoading &&  !defaultsLoading && defaults === null">
			<q-banner inline-actions class="text-black bg-grey-2" rounded>
				<div class="flex flex-row gap-4 items-center">
					<MDIInformation class="w-6 h-6 text-primary" />
					<div class="flex flex-col gap-0.5">
						<div class="font-bold">No Defaults</div>
					</div>
				</div>
			</q-banner>
		</div>
		
		<PJSIPEntryEditor v-if="defaults !== null" v-model="defaults" :showRemove="false" :isTemplate="true" />
	</div>
</template>