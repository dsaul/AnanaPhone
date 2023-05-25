<script setup lang="ts">
import MDIPlus from '../SVG/MDIPlus.vue';
import { useQueryAll } from '@/_Composables/GraphQL/E164Client/useQueryAll';
import isEmpty from '@/_Composables/Utility/isEmpty';
import { Notify } from 'quasar';
import { computed, ref } from 'vue';
import MDIAlert from '../SVG/MDIAlert.vue';
import MDIInformation from '../SVG/MDIInformation.vue';
import E164ClientEditor from '../Editors/E164ClientEditor.vue';
import AddE164ClientModal from '../Dialogues/AddE164ClientModal.vue';

const { all, allLoading, allOnError, allError } = useQueryAll();
const addE164ClientModalEl = ref();

allOnError(() => {
	Notify.create({
		type: 'negative',
		message: 'Loading Number Clients Failed'
	});
});

const any = computed<boolean>(() => {
	return all.value.length !== 0;
});

const onClickAddE164Client = () => {
	if (addE164ClientModalEl.value) {
		addE164ClientModalEl.value.resetAndOpen();
	}
}

</script>
<template>
	<div class="flex flex-col gap-2">
		<AddE164ClientModal ref="addE164ClientModalEl" />

		<div class="flex flex-row items-end">
			<div class="font-bold">Remote Number Client</div>
			<q-space />
			<q-btn flat color="primary" dense @click="onClickAddE164Client">
				<div class="flex flex-row gap-1 items-center">
					<MDIPlus class="w-4 h-4" />
					<div>Add</div>
				</div>
			</q-btn>
		</div>

		<div v-if="allError">
			<q-banner inline-actions class="text-white bg-red" rounded>
				<div class="flex flex-row gap-4 items-center">
					<MDIAlert class="w-6 h-6" />
					<div class="flex flex-col gap-0.5">
						<div class="font-bold">There was an error loading.</div>
						<div v-if="!isEmpty(allError?.message)">{{ allError?.message }}</div>
					</div>
				</div>
			</q-banner>
		</div>

		<div v-if="allLoading">
			<q-skeleton bordered type="rect" class="h-16" />
		</div>

		<div v-if="!any && !allLoading">
			<q-banner inline-actions class="text-black bg-grey-2" rounded>
				<div class="flex flex-row gap-4 items-center">
					<MDIInformation class="w-6 h-6 text-primary" />
					<div class="flex flex-col gap-0.5">
						<div class="font-bold">No Number Clients</div>
					</div>
				</div>
			</q-banner>
		</div>

		<E164ClientEditor v-for="(e164, e164Idx) of (all || [])" :key="e164?.e164 || `idx${e164Idx}`" :value="e164" />
	</div>
</template>