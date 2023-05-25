<script setup lang="ts">

import { onMounted, onUnmounted, ref, nextTick, computed } from 'vue';
import {
	type TCBParams as TOnOpenOutboundCallModal,
	subscribe as onOpenOutboundCallModalSubscribe,
	unsubscribe as onOpenOutboundCallModalUnsubscribe,
} from '@/_Composables/Events/onOpenOutboundCallModal';

import { type IE164 } from '@/_Composables/GraphQL/E164/IE164'
import { useQueryDialerData } from '@/_Composables/GraphQL/Dialer/useQueryDialerData';
import MDIPhoneOutgoing from '@/components/SVG/MDIPhoneOutgoing.vue';
import MDIPhone from '@/components/SVG/MDIPhone.vue';
import MDIPhoneIncoming from '@/components/SVG/MDIPhoneIncoming.vue';
import isEmpty from '@/_Composables/Utility/isEmpty';
import { useMutationOutboundCall } from '@/_Composables/GraphQL/Dialer/useMutationOutboundCall';
import { Notify } from 'quasar';
const { allowedCallOutNumbers, e164s } = useQueryDialerData();
const { outboundCall, outboundCallLoading, outboundCallError, outboundCallDone } = useMutationOutboundCall();

const callDestinationEl = ref();

const isOpen = ref<boolean>(false);
const callDevice = ref<string | null>(null);
const callOutboundIdentityE164 = ref<IE164 | null>(null);
const callDestination = ref<string | null>(null);


const resetAndOpen = (_callDestination?: string | null) => {
	reset();
	open(_callDestination);
};

const resetAndClose = () => {
	reset();
	close();
};


const confirm = () => {
	if (isEmpty(callDevice.value)) {
		Notify.create({
			type: 'negative',
			message: 'No Call Device'
		});
		return;
	}
	if (isEmpty(callDestination.value)) {
		Notify.create({
			type: 'negative',
			message: 'No Call Destination'
		});
		return;
	}
	if (callOutboundIdentityE164.value === null) {
		Notify.create({
			type: 'negative',
			message: 'No Call Identity'
		});
		return;
	}
	outboundCall(
		callDevice.value,
		callDestination.value,
	);
};


const open = (_callDestination?: string | null) => {
	isOpen.value = true;
	selectDefaultCallOutIfEmpty();
	selectDefaultCallIdentityfEmpty();
	nextTick(() => {
		if (callDestinationEl.value) {
			callDestinationEl.value.focus();
		}
		if (!isEmpty(_callDestination)) {
			callDestination.value = _callDestination;
		}
	})
};

const close = () => {
	isOpen.value = false;
};

const reset = () => {
	callDevice.value = null;
	callOutboundIdentityE164.value = null;
	callDestination.value = null;
};


outboundCallDone(() => {
	Notify.create({
		type: 'positive',
		message: 'Call Originated'
	});

	reset();
	close();
});

outboundCallError(() => {
	Notify.create({
		type: 'negative',
		message: 'Call Origination Failed'
	});
});


const isAnyLoading = computed(() => {
	return outboundCallLoading.value;
});


defineExpose({
	open,
	close,
	resetAndOpen,
	resetAndClose,
});

const selectDefaultCallOutIfEmpty = () => {
	if (isEmpty(callDevice.value)) {
		callDevice.value = allowedCallOutNumbers.value?.[0] || null;
	}
};

const selectDefaultCallIdentityfEmpty = () => {
	if (callOutboundIdentityE164.value === null) {
		callOutboundIdentityE164.value = e164s.value?.[0] || null;
	}
};

onMounted(() => {
	onOpenOutboundCallModalSubscribe(onOpenJoinConferenceModal);
});

onUnmounted(() => {
	onOpenOutboundCallModalUnsubscribe(onOpenJoinConferenceModal);
})

const onOpenJoinConferenceModal = (params: TOnOpenOutboundCallModal) => {
	resetAndOpen(params.e164);
}

const onClickDialpad = (key: string) => {
	callDestination.value = `${callDestination.value || ''}${key}`;
	if (callDestinationEl.value) {
		callDestinationEl.value.focus();
	}
}

</script>
<template>
	<q-dialog persistent v-model="isOpen" @hide="close" @show="open(callDestination)">
		<q-card class="p-4">
			<div class="flex flex-col gap-2">
				<div class="text-xl flex flex-row items-center">
					<div class="px-2">
						New Call
					</div>
					<q-space />
					<q-btn icon="close" flat round dense @click="close" :disable="isAnyLoading" />
				</div>
				<div class="flex flex-row flex-wrap gap-2">
					<q-select dense label="Call Using:" v-model="callDevice" :options="allowedCallOutNumbers"
						:disable="isAnyLoading">
						<template #prepend>
							<MDIPhoneOutgoing class="w-6 h-6" />
						</template>
					</q-select>
					<q-select dense label="From Number:" v-model="callOutboundIdentityE164" :options="e164s"
						:disable="isAnyLoading" :option-label="(item) => item === null ? '(empty)' : item.e164">
						<template #prepend>
							<MDIPhoneOutgoing class="w-6 h-6" />
						</template>
					</q-select>
				</div>
				<q-input ref="callDestinationEl" v-model="callDestination" label="To Number:" @keyup.enter="confirm"
					:disable="isAnyLoading">
					<template #prepend>
						<MDIPhoneIncoming class="w-6 h-6" />
					</template>
				</q-input>
				<div class="grid grid-cols-3 gap-2">
					<q-btn flat color="primary" @click="onClickDialpad('1')" :disable="isAnyLoading">
						<div>
							<div class="text-xl">1</div>
							<div class="text-xs">&nbsp;</div>
						</div>
					</q-btn>
					<q-btn flat color="primary" @click="onClickDialpad('2')" :disable="isAnyLoading">
						<div>
							<div class="text-xl">2</div>
							<div class="text-xs">ABC</div>
						</div>
					</q-btn>
					<q-btn flat color="primary" @click="onClickDialpad('3')" :disable="isAnyLoading">
						<div>
							<div class="text-xl">3</div>
							<div class="text-xs">DEF</div>
						</div>
					</q-btn>
					<q-btn flat color="primary" @click="onClickDialpad('4')" :disable="isAnyLoading">
						<div>
							<div class="text-xl">4</div>
							<div class="text-xs">GHI</div>
						</div>
					</q-btn>
					<q-btn flat color="primary" @click="onClickDialpad('5')" :disable="isAnyLoading">
						<div>
							<div class="text-xl">5</div>
							<div class="text-xs">JKL</div>
						</div>
					</q-btn>
					<q-btn flat color="primary" @click="onClickDialpad('6')" :disable="isAnyLoading">
						<div>
							<div class="text-xl">6</div>
							<div class="text-xs">MNO</div>
						</div>
					</q-btn>
					<q-btn flat color="primary" @click="onClickDialpad('7')" :disable="isAnyLoading">
						<div>
							<div class="text-xl">7</div>
							<div class="text-xs">PQRS</div>
						</div>
					</q-btn>
					<q-btn flat color="primary" @click="onClickDialpad('8')" :disable="isAnyLoading">
						<div>
							<div class="text-xl">8</div>
							<div class="text-xs">TUV</div>
						</div>
					</q-btn>
					<q-btn flat color="primary" @click="onClickDialpad('9')" :disable="isAnyLoading">
						<div>
							<div class="text-xl">9</div>
							<div class="text-xs">WXYZ</div>
						</div>
					</q-btn>
					<q-btn flat color="primary" @click="onClickDialpad('*')" :disable="isAnyLoading">
						<div>
							<div class="text-xl">*</div>
							<div class="text-xs">&nbsp;</div>
						</div>
					</q-btn>
					<q-btn flat color="primary" @click="onClickDialpad('0')" :disable="isAnyLoading">
						<div>
							<div class="text-xl">0</div>
							<div class="text-xs">+</div>
						</div>
					</q-btn>
					<q-btn flat color="primary" @click="onClickDialpad('#')" :disable="isAnyLoading">
						<div>
							<div class="text-xl">#</div>
							<div class="text-xs">&nbsp;</div>
						</div>
					</q-btn>
				</div>
				<q-btn color="primary" @click="confirm" :disable="isAnyLoading" :loading="outboundCallLoading">
					<div class="flex flex-row items-center gap-2">
						<MDIPhone class="w-6 h-6" />
						<div>Call</div>
					</div>
				</q-btn>
			</div>

		</q-card>
	</q-dialog>
</template>