<script setup lang="ts">
import { computed, ref } from 'vue';
import { generateEmpty as generateEmptyE164 } from '@/_Composables/GraphQL/E164/IE164';
import { Notify } from 'quasar';
import { useMutationUpsert } from '@/_Composables/GraphQL/E164/useMutationUpsert';

const isOpen = ref<boolean>(false);
const newE164Value = ref<string | null>(null);
const { upsert, upsertLoading, upsertError, upsertDone } = useMutationUpsert();

const resetAndOpen = () => {
	reset();
	open();
};

const resetAndClose = () => {
	reset();
	close();
};

const save = () => {
	
	const payload = {
		...generateEmptyE164(),
		e164: newE164Value.value,
	}
	
	upsert(payload);
};

const open = () => {
	isOpen.value = true;
};

const close = () => {
	isOpen.value = false;
};

const reset = () => {
	newE164Value.value = null;
};

upsertDone(() => {
	Notify.create({
		type: 'positive',
		message: 'Created'
	});
	
	reset();
	close();
});

upsertError(() => {
	Notify.create({
		type: 'negative',
		message: 'Creation Failed'
	});
});

const isAnyLoading = computed(() => {
	return upsertLoading.value;
});

defineExpose({
	open,
	close,
	resetAndOpen,
	resetAndClose,
});

</script>
<template>
	<q-dialog @show="open" @hide="close" v-model="isOpen" persistent>
		<q-card>
			<div class="flex flex-col p-4 gap-1">
				<div class="font-bold">Add Number:</div>
				<q-input label="E164:" placeholder="+12045551234" dense v-model="newE164Value" autofocus
					@keyup.enter="save">
				</q-input>
				<div class="text-xs text-grey-8">This is the number that the system will respond to, as well as allow
					calling out from.</div>
				<div class="flex flex-row justify-end">
					<q-btn flat color="red" label="Cancel" @click="resetAndClose" :loading="isAnyLoading" :disable="isAnyLoading" />
					<q-btn flat color="green" label="Add" @click="save" :loading="isAnyLoading" :disable="isAnyLoading" />
				</div>
			</div>
		</q-card>
	</q-dialog>
</template>