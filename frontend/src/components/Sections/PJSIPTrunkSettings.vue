<script setup lang="ts">
import { useQueryTrunks } from '@/_Composables/GraphQL/Settings/useQueryTrunks';
import PJSIPEntryEditor from '@/components/Editors/PJSIPEntryEditor.vue';
import type { IPJSIPEntry } from '@/_Composables/GraphQL/PJSIPEntry/IPJSIPEntry';
import { useQueryTrunkDefaults } from '@/_Composables/GraphQL/Settings/useQueryTrunkDefaults';
import MDIPlus from '@/components/SVG/MDIPlus.vue';
import { computed, ref } from 'vue';
import AddPJSIPEntryModal from '../Dialogues/AddPJSIPEntryModal.vue';
import { Notify } from 'quasar';
import isEmpty from '@/_Composables/Utility/isEmpty';
import MDIInformation from '../SVG/MDIInformation.vue';
import MDIAlert from '../SVG/MDIAlert.vue';

const { defaults, defaultsLoading, defaultsOnError, defaultsError } = useQueryTrunkDefaults();
const { trunks, trunksLoading, trunksOnError, trunksError } = useQueryTrunks();

const anyTrunks = computed<boolean>(() => {
	return trunks.value.length !== 0;
});

defaultsOnError(() => {
	Notify.create({
		type: 'negative',
		message: 'Loading Defaults Failed'
	});
});

trunksOnError(() => {
	Notify.create({
		type: 'negative',
		message: 'Loading Trunks Failed'
	});
});

const addPJSIPEntryModalEl = ref();

const onTrunkChanged = (idx: number, payload: IPJSIPEntry | null) => {
	console.log('onTrunkChanged', idx, payload)
}

const onClickAdd = () => {
	if (addPJSIPEntryModalEl.value) {
		addPJSIPEntryModalEl.value.resetAndOpen();
	}
};

</script>
<template>
	<div class="flex flex-col gap-2">
		<AddPJSIPEntryModal ref="addPJSIPEntryModalEl" usesTemplate="trunk_defaults" :isTemplate="false" noun="trunk" />

		<div class="flex flex-row items-end">
			<div class="font-bold">PJSIP Trunks</div>
			<q-space />
			<q-btn flat color="primary" dense @click="onClickAdd">
				<div class="flex flex-row gap-1 items-center">
					<MDIPlus class="w-4 h-4" />
					<div>Add</div>
				</div>
			</q-btn>
		</div>
		<div v-if="trunksError">
			<q-banner inline-actions class="text-white bg-red" rounded>
				<div class="flex flex-row gap-4 items-center">
					<MDIAlert class="w-6 h-6" />
					<div class="flex flex-col gap-0.5">
						<div class="font-bold">There was an error loading.</div>
						<div v-if="!isEmpty(trunksError?.message)">{{ trunksError?.message }}</div>
					</div>
				</div>
			</q-banner>
		</div>
		<div v-if="trunksLoading">
			<q-skeleton bordered type="rect" class="h-16" />
		</div>
		<div v-if="!anyTrunks && !trunksLoading">
			<q-banner inline-actions class="text-black bg-grey-2" rounded>
				<div class="flex flex-row gap-4 items-center">
					<MDIInformation class="w-6 h-6 text-primary" />
					<div class="flex flex-col gap-0.5">
						<div class="font-bold">No Numbers</div>
					</div>
				</div>
			</q-banner>
		</div>
		
		<PJSIPEntryEditor v-for="(trunk, trunkIdx) of trunks" :key="trunkIdx" :value="trunk"
			@on-value-changed="onTrunkChanged(trunkIdx, $event)" usesTemplate="trunk_defaults" :isTemplate="false" />

		<div class="font-bold">PJSIP Trunk Template</div>
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
		<div v-if="!defaultsLoading && defaults === null">
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