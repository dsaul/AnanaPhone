<script setup lang="ts">
import { ref } from 'vue';
import type { IE164 } from '@/_Composables/GraphQL/E164/IE164';
import { useQueryAll } from '@/_Composables/GraphQL/E164/useQueryAll';
import E164Editor from '@/components/Editors/E164Editor.vue';
import MDIPlus from '@/components/SVG/MDIPlus.vue';
import AddE164Modal from '../Dialogues/AddE164Modal.vue';
import { Notify } from 'quasar';
import { computed } from 'vue';
import isEmpty from '@/_Composables/Utility/isEmpty';
import MDIAlert from '../SVG/MDIAlert.vue';
import MDIInformation from '../SVG/MDIInformation.vue';

const { all, allLoading, allOnError, allError } = useQueryAll();
const addE164ModalEl = ref();

allOnError(() => {
	Notify.create({
		type: 'negative',
		message: 'Loading Numbers Failed'
	});
});

const any = computed<boolean>(() => {
	return all.value.length !== 0;
});

const onE164Changed = (idx: number, payload: IE164 | null) => {
	console.log('onE164Changed', idx, payload)
}

const onClickAddE164 = () => {
	if (addE164ModalEl.value) {
		addE164ModalEl.value.resetAndOpen();
	}
};

</script>
<template>
	<div class="flex flex-col gap-2">
		<AddE164Modal ref="addE164ModalEl" />

		<div class="flex flex-row items-end">
			<div class="font-bold">Numbers</div>
			<q-space />
			<q-btn flat color="primary" dense @click="onClickAddE164">
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
						<div class="font-bold">No Numbers</div>
					</div>
				</div>
			</q-banner>
		</div>
		
		<E164Editor v-for="(e164, e164Idx) of (all || [])" :key="e164?.id || `idx${e164Idx}`" :value="e164"
			@on-value-changed="onE164Changed(e164Idx, $event)" />
	</div>
</template>