<script setup lang="ts">
import { computed, ref } from 'vue';
import { useMutationUpsert } from '@/_Composables/GraphQL/PJSIPEntry/useMutationUpsert';
import { Notify } from 'quasar';
import { generateEmpty as generateEmptyIPJSIPEntry } from '@/_Composables/GraphQL/PJSIPEntry/IPJSIPEntry';
import isEmpty from '@/_Composables/Utility/isEmpty';
import { useUCWords } from '@/_Composables/Formatters/useUCWords';

const props = withDefaults(defineProps<{
	usesTemplate?: string | null
	isTemplate?: boolean;
	noun?: string;
}>(), {
	usesTemplate: null,
	isTemplate: false,
	noun: 'noun',
});

const isOpen = ref<boolean>(false);
const newName = ref<string | null>(null);
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
	
	if (isEmpty(newName.value)) {
		console.warn('isEmpty(newName.value)');
		return;
	}

	const payload = {
		...generateEmptyIPJSIPEntry(),
		name: newName.value,
		xDummyPlaceholder: "asdasd",
	}
	
	upsert(payload, newName.value, props.usesTemplate, props.isTemplate);

	newName.value = null;
	isOpen.value = false;
};

const open = () => {
	isOpen.value = true;
};

const close = () => {
	isOpen.value = false;
};

const reset = () => {
	newName.value = null;
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

const noun = computed(() => {
	return props.noun;
})
const ucNoun = useUCWords(noun);

defineExpose({
	open,
	close,
	resetAndOpen,
	resetAndClose,
});

</script>
<template>
	<q-dialog v-model="isOpen" persistent>
		<q-card>
			<div class="flex flex-col p-4 gap-1">
				<div class="font-bold">Add {{ ucNoun }}:</div>
				<q-input label="Name:" placeholder="SomeName" dense v-model="newName" autofocus @keyup.enter="save">
				</q-input>
				<div class="text-xs text-grey-8">This is the name that will be used to refer to this {{noun}}, it cannot be
					changed without being recreated.</div>
				<div class="flex flex-row justify-end">
					<q-btn flat color="red" label="Cancel" @click="resetAndClose" :disable="isAnyLoading" />
					<q-btn flat color="green" label="Add" @click="save" :disable="isAnyLoading" :loading="upsertLoading" />
				</div>
			</div>
		</q-card>
	</q-dialog>
</template>