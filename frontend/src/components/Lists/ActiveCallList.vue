<script setup lang="ts">
import { useQueryAll } from '@/_Composables/GraphQL/ActiveCall/useQueryAll';
import ActiveCallRow from '@/components/Lists/ActiveCallRow.vue';
import { computed } from 'vue';
import { Notify } from 'quasar';
import isEmpty from '@/_Composables/Utility/isEmpty';
import MDIAlert from '@/components/SVG/MDIAlert.vue';
import MDIInformation from '@/components/SVG/MDIInformation.vue';

const { all, allLoading, allOnError, allError } = useQueryAll();

allOnError(() => {
	Notify.create({
		type: 'negative',
		message: 'Loading Calls Failed'
	});
});

const any = computed<boolean>(() => {
	return all.value.length !== 0;
});

</script>
<template>
	<q-list class="p-2 flex flex-col gap-2">
		<q-item-label header class="font-semibold px-1">Active Calls</q-item-label>
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
		<div v-if="allLoading" class="px-3"><q-skeleton bordered type="rect" class="h-16" /></div>
		<div v-else class="flex flex-col gap-2">
			<ActiveCallRow class="mx-3" v-for="(row, rowIdx) in all" :key="rowIdx" :value="row || undefined" />

			<div v-if="!any && !allLoading" class="px-3">
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
	