<script setup lang="ts">
import type { IE164 } from '@/_Composables/GraphQL/E164/IE164';
import { useModelValue } from '@/_Composables/Utility/useModelValue';
import { cloneDeep } from 'lodash';
import { ref, computed } from 'vue';
import { useName } from '@/_Composables/GraphQL/E164/useName';
import { useE164 } from '@/_Composables/GraphQL/E164/useE164';
import { useComment } from '@/_Composables/GraphQL/E164/useComment';
import { useOutboundDevice } from '@/_Composables/GraphQL/E164/useOutboundDevice';
import { useDisabled } from '@/_Composables/GraphQL/E164/useDisabled';
import MDIDelete from '@/components/SVG/MDIDelete.vue';
import MDIContentSave from '@/components/SVG/MDIContentSave.vue';
import RemoveE164Modal from '@/components/Dialogues/RemoveE164Modal.vue';
import { useMutationUpsert } from '@/_Composables/GraphQL/E164/useMutationUpsert';
import { Notify } from 'quasar';

const { upsert, upsertLoading, upsertError, upsertDone } = useMutationUpsert();


const props = withDefaults(defineProps<{
	modelValue?: IE164 | null,
	value?: IE164 | null,

	showRemove?: boolean
}>(), {
	showRemove: true,
});

const emit = defineEmits<{
	(e: 'update:modelValue', payload: IE164 | null): void,
	(e: 'on-value-changed', payload: IE164 | null): void,
}>();

const editModel = ref<IE164 | null>(null);

const removeModalEl = ref();
const isExpansionOpen = ref<boolean>(false);

const model = useModelValue(emit, props, null as IE164 | null, () => {
	const clone = cloneDeep(model.value);
	editModel.value = clone;
});

const name = useName(editModel);
const comment = useComment(editModel);
const e164 = useE164(editModel);
const outboundDevice = useOutboundDevice(editModel);
const disabled = useDisabled(editModel);





const onClickSaveChanges = () => {
	if (null === editModel.value) {
		console.error('null === editModel.value');
		return;
	}
	upsert(editModel.value);
}

upsertDone(() => {
	Notify.create({
		type: 'positive',
		message: 'Saved'
	});
});

upsertError(() => {
	Notify.create({
		type: 'negative',
		message: 'Save Failed'
	});
});


const isAnyLoading = computed(() => {
	return upsertLoading.value;
});

const onClickOpenRemoveModal = () => {
	if (removeModalEl.value) {
		removeModalEl.value.resetAndOpen();
	}
};

</script>
<template>
	<q-card>
		<RemoveE164Modal ref="removeModalEl" v-model="editModel" />
		<q-expansion-item switch-toggle-side group="pksipentryeditor" :label="e164 || undefined" v-model="isExpansionOpen">
			<q-card-section v-if="isExpansionOpen">
				<div class="flex flex-col gap-8">
					<!-- <q-input dense v-model="e164" label="Name" readonly /> -->
					<div class="flex flex-col gap-2">
						<!-- <div class="font-bold">Endpoint</div> -->
						<div
							class="px-1 grid gap-2 items-end grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5">
							<div class="flex flex-col h-full gap-1">
								<q-input clearable dense v-model="name" label="Caller ID Name" :disable="isAnyLoading" />
								<div class="text-xs text-grey-8">This is the name that will be displayed on outbound calls.
									Don't expect anything past 10 letters to be seen.</div>
							</div>
							<div class="flex flex-col h-full gap-1">
								<q-input clearable dense v-model="outboundDevice" label="Outbound Device" :disable="isAnyLoading" />
								<div class="text-xs text-grey-8">When calls are placed with this caller id, which trunk
									(names listed below) should we use to place the call?</div>
							</div>
							<div class="flex flex-col h-full gap-1">
								<q-input clearable dense v-model="comment" label="Comment" :disable="isAnyLoading" />
								<div class="text-xs text-grey-8">For your reference only, doesn't affect anything.</div>
							</div>
							<div class="flex flex-col h-full gap-1">
								<q-toggle dense v-model="disabled" label="Disabled" :disable="isAnyLoading" />
								<div class="text-xs text-grey-8">Disables this entry.</div>
							</div>

						</div>
					</div>

					<div class="flex flex-row gap-2">
						<q-btn v-if="showRemove" color="red" @click="onClickOpenRemoveModal" :disable="isAnyLoading">
							<div class="flex flex-row items-center gap-2">
								<MDIDelete class="w-4 h-4" />
								<div>Remove</div>
							</div>
						</q-btn>
						<q-space />
						<q-btn color="primary" @click="onClickSaveChanges" :loading="isAnyLoading" :disable="isAnyLoading">
							<div class="flex flex-row items-center gap-2">
								<MDIContentSave class="w-4 h-4" />
								<div>Save Changes</div>
							</div>
						</q-btn>
					</div>
				</div>
			</q-card-section>
		</q-expansion-item>
	</q-card>
</template>