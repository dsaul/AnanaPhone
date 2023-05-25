<script setup lang="ts">
import { useQueryAll } from '@/_Composables/GraphQL/HistoricCall/useQueryAll';
import HistoricCallRow from '@/components/Lists/HistoricCallRow.vue';
import { computed, ref } from 'vue';
import MDIDeleteClock from '@/components/SVG/MDIDeleteClock.vue';
import { Notify } from 'quasar';
import isEmpty from '@/_Composables/Utility/isEmpty';
import RemoveAllHistoricCallsModal from '../Dialogues/RemoveAllHistoricCallsModal.vue';
import MDIAlert from '../SVG/MDIAlert.vue';
import MDIInformation from '../SVG/MDIInformation.vue';

const { all, allLoading, allOnError, allError } = useQueryAll();
const any = computed<boolean>(() => {
	return all.value.length !== 0;
})

allOnError(() => {
	Notify.create({
		type: 'negative',
		message: 'Loading Historic Calls Failed'
	});
});

const removeAllHistoricCallsModalEl = ref();

const onClickRemoveAll = () => {
	if (removeAllHistoricCallsModalEl.value) {
		removeAllHistoricCallsModalEl.value.resetAndOpen();
	}
}

</script>
<template>
	<q-list class="p-2">
		
		<RemoveAllHistoricCallsModal ref="removeAllHistoricCallsModalEl" />
		
		<div class="flex flex-row items-center">
			<q-item-label header class="font-semibold px-1">History</q-item-label>
			<q-space />
			<q-btn flat @click="onClickRemoveAll" :disable="!any">
				<div class="flex flex-row gap-2 items-center">
					<MDIDeleteClock class="w-4 h-4" />
					<div class="text-xs">Remove All</div>
				</div>
			</q-btn>
		</div>
		
		<div v-if="allLoading" class="px-3"><q-skeleton bordered type="rect" class="h-16" /></div>
		<div class="flex flex-col gap-2">
			<div v-if="allError" class="px-3">
				<q-banner inline-actions class="text-white bg-red" rounded>
					<div class="flex flex-row gap-4 items-center">
						<MDIAlert class="w-6 h-6" />
						<div class="flex flex-col gap-0.5">
							<div class="font-bold">There was an error loading active calls.</div>
							<div v-if="!isEmpty(allError?.message)">{{ allError?.message }}</div>
						</div>
					</div>
				</q-banner>
			</div>
			
			<HistoricCallRow
				class="mx-3"
				v-for="(row, rowIdx) in all"
				:key="rowIdx" 
				:value="row || undefined"
				/>
				
			<div v-if="!allLoading && !any" class="px-3">
				<q-banner inline-actions class="text-black bg-grey-2" rounded>
					<div class="flex flex-row gap-4 items-center">
						<MDIInformation class="w-6 h-6 text-primary" />
						<div class="flex flex-col gap-0.5">
							<div class="font-bold">No Calls</div>
						</div>
					</div>
				</q-banner>
			</div>
			
		</div>
	</q-list>
</template>
	