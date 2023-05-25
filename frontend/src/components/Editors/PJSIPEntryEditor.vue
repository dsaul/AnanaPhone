<script setup lang="ts">
import { type IPJSIPEntry } from '@/_Composables/GraphQL/PJSIPEntry/IPJSIPEntry'
import { useModelValue } from '@/_Composables/Utility/useModelValue';
import { ref, computed } from 'vue';
import { cloneDeep } from 'lodash';
import MDIDelete from '@/components/SVG/MDIDelete.vue';
import MDIContentSave from '@/components/SVG/MDIContentSave.vue';
import { useHasHint } from '@/_Composables/GraphQL/PJSIPEntry/useHasHint';
import { useName } from '@/_Composables/GraphQL/PJSIPEntry/useName';
import { useHintExten } from '@/_Composables/GraphQL/PJSIPEntry/useHintExten';
import { useHintContext } from '@/_Composables/GraphQL/PJSIPEntry/useHintContext';
import { useInboundAuthUsername } from '@/_Composables/GraphQL/PJSIPEntry/useInboundAuthUsername';
import { useInboundAuthPassword } from '@/_Composables/GraphQL/PJSIPEntry/useInboundAuthPassword';
import { useEndpointCallerid } from '@/_Composables/GraphQL/PJSIPEntry/useEndpointCallerid';
import { useAorMaxContacts } from '@/_Composables/GraphQL/PJSIPEntry/useAorMaxContacts';
import { useRemoteHosts } from '@/_Composables/GraphQL/PJSIPEntry/useRemoteHosts';
import { useType } from '@/_Composables/GraphQL/PJSIPEntry/useType';
import { useAcceptsAuth } from '@/_Composables/GraphQL/PJSIPEntry/useAcceptsAuth';
import { useAcceptsRegistrations } from '@/_Composables/GraphQL/PJSIPEntry/useAcceptsRegistrations';
import { useEndpointAllow } from '@/_Composables/GraphQL/PJSIPEntry/useEndpointAllow';
import { useEndpointDirectMedia } from '@/_Composables/GraphQL/PJSIPEntry/useEndpointDirectMedia';
import { useEndpointForceRport } from '@/_Composables/GraphQL/PJSIPEntry/useEndpointForceRport';
import { useEndpointRewriteContact } from '@/_Composables/GraphQL/PJSIPEntry/useEndpointRewriteContact';
import { useEndpointRTPSymmetric } from '@/_Composables/GraphQL/PJSIPEntry/useEndpointRTPSymmetric';
import { useAorQualifyFrequency } from '@/_Composables/GraphQL/PJSIPEntry/useAorQualifyFrequency';
import { useEndpointContext } from '@/_Composables/GraphQL/PJSIPEntry/useEndpointContext';
import { useSendsAuth } from '@/_Composables/GraphQL/PJSIPEntry/useSendsAuth';
import { useSendsRegistrations } from '@/_Composables/GraphQL/PJSIPEntry/useSendsRegistrations';
import { useEndpointT38Udptl } from '@/_Composables/GraphQL/PJSIPEntry/useEndpointT38Udptl';
import { useEndpointT38UdptlEc } from '@/_Composables/GraphQL/PJSIPEntry/useEndpointT38UdptlEc';
import { useEndpointFaxDetect } from '@/_Composables/GraphQL/PJSIPEntry/useEndpointFaxDetect';
import { useEndpointTrustIdInbound } from '@/_Composables/GraphQL/PJSIPEntry/useEndpointTrustIdInbound';
import { useEndpointT38UdptlNat } from '@/_Composables/GraphQL/PJSIPEntry/useEndpointT38UdptlNat';
import { useEndpointDTMFMode } from '@/_Composables/GraphQL/PJSIPEntry/useEndpointDTMFMode';
import { useEndpointAllowSubscribe } from '@/_Composables/GraphQL/PJSIPEntry/useEndpointAllowSubscribe';
import { useEndpointTransport } from '@/_Composables/GraphQL/PJSIPEntry/useEndpointTransport';
import RemovePJSIPEntryModal from '@/components/Dialogues/RemovePJSIPEntryModal.vue';
import { useMutationUpsert } from '@/_Composables/GraphQL/PJSIPEntry/useMutationUpsert';
import { Notify } from 'quasar';

const { upsert, upsertLoading, upsertError, upsertDone } = useMutationUpsert();

const props = withDefaults(defineProps<{
	modelValue?: IPJSIPEntry | null,
	value?: IPJSIPEntry | null,

	showRemove?: boolean,
	usesTemplate?: string | null,
	isTemplate?: boolean;
}>(), {
	showRemove: true,
	usesTemplate: null,
	isTemplate: false,
});

const emit = defineEmits<{
	(e: 'update:modelValue', payload: IPJSIPEntry | null): void,
	(e: 'on-value-changed', payload: IPJSIPEntry | null): void,
}>();

const editModel = ref<IPJSIPEntry | null>(null);

const isExpansionOpen = ref<boolean>(false);

const model = useModelValue(emit, props, null as IPJSIPEntry | null, () => {
	const clone = cloneDeep(model.value);
	editModel.value = clone;
});
const name = useName(editModel);
const hasHint = useHasHint(editModel);
const hintExten = useHintExten(editModel);
const hintContext = useHintContext(editModel);
const inboundAuthUsername = useInboundAuthUsername(editModel);
const inboundAuthPassword = useInboundAuthPassword(editModel);
const endpointCallerid = useEndpointCallerid(editModel);
const aorMaxContacts = useAorMaxContacts(editModel);
const remoteHosts = useRemoteHosts(editModel);
const type = useType(editModel);
const acceptsAuth = useAcceptsAuth(editModel);
const acceptsRegistrations = useAcceptsRegistrations(editModel);
const endpointAllow = useEndpointAllow(editModel);
const endpointDirectMedia = useEndpointDirectMedia(editModel);
const endpointForceRport = useEndpointForceRport(editModel);
const endpointRewriteContact = useEndpointRewriteContact(editModel);
const endpointRTPSymmetric = useEndpointRTPSymmetric(editModel);
const aorQualifyFrequency = useAorQualifyFrequency(editModel);
const endpointContext = useEndpointContext(editModel);
const sendsAuth = useSendsAuth(editModel);
const sendsRegistrations = useSendsRegistrations(editModel);
const endpointT38Udptl = useEndpointT38Udptl(editModel);
const endpointT38UdptlEc = useEndpointT38UdptlEc(editModel);
const endpointFaxDetect = useEndpointFaxDetect(editModel);
const endpointTrustIdInbound = useEndpointTrustIdInbound(editModel);
const endpointT38UdptlNat = useEndpointT38UdptlNat(editModel);
const endpointDTMFMode = useEndpointDTMFMode(editModel);
const endpointAllowSubscribe = useEndpointAllowSubscribe(editModel);
const endpointTransport = useEndpointTransport(editModel);

const removeModalEl = ref();

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

const onClickRemove = () => {
	if (removeModalEl.value) {
		removeModalEl.value.resetAndOpen();
	}
};

const onClickSaveChanges = () => {
	if (editModel.value === null) {
		console.warn('editModel.value === null');
		return;
	}
	if (name.value === null) {
		console.warn('name.value === null');
		return;
	}
	console.log('onClickSaveChanges', editModel.value);
	
	upsert(editModel.value, name.value, props.usesTemplate, props.isTemplate);
}

</script>
<template>
	<q-card>
		<RemovePJSIPEntryModal ref="removeModalEl" v-model="editModel" />
		<q-expansion-item switch-toggle-side group="pksipentryeditor" :label="name || undefined" v-model="isExpansionOpen">
			<q-card-section v-if="isExpansionOpen" :disable="isAnyLoading">
				<div class="flex flex-col gap-8">
					<!-- <q-input dense v-model="name" label="Name" readonly /> -->
					<div class="flex flex-col gap-2">
						<div class="font-bold">Endpoint</div>
						<div
							class="px-1 grid gap-2 items-end grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5">
							<div class="flex flex-col h-full gap-1">
								<q-toggle toggle-indeterminate dense v-model="endpointDirectMedia" label="Direct Media" :disable="isAnyLoading" />
								<div class="text-xs text-grey-8">
									Determines whether media may flow directly between endpoints. <a
										class="text-blue underline"
										href="https://wiki.asterisk.org/wiki/display/AST/Asterisk+20+Configuration_res_pjsip"
										target="_blank">See asterisk.org reference.</a>
								</div>
							</div>
							<div class="flex flex-col h-full gap-1">
								<q-toggle toggle-indeterminate dense v-model="endpointForceRport" label="Force Rport" :disable="isAnyLoading" />
								<div class="text-xs text-grey-8">
									Force use of return port. <a class="text-blue underline"
										href="https://wiki.asterisk.org/wiki/display/AST/Asterisk+20+Configuration_res_pjsip"
										target="_blank">See asterisk.org reference.</a>
								</div>
							</div>
							<div class="flex flex-col h-full gap-1">
								<q-toggle toggle-indeterminate dense v-model="endpointRewriteContact" :disable="isAnyLoading"
									label="Rewrite Contact" />
								<div class="text-xs text-grey-8">
									Allow Contact header to be rewritten with the source IP address-port. <a
										class="text-blue underline"
										href="https://wiki.asterisk.org/wiki/display/AST/Asterisk+20+Configuration_res_pjsip#Asterisk20Configuration_res_pjsip-endpoint_rewrite_contact"
										target="_blank">See asterisk.org reference.</a>
								</div>
							</div>
							<div class="flex flex-col h-full gap-1">
								<q-toggle toggle-indeterminate dense v-model="endpointRTPSymmetric" label="RTP Symmetric" :disable="isAnyLoading" />
								<div class="text-xs text-grey-8">
									Enforce that RTP must be symmetric. <a class="text-blue underline"
										href="https://wiki.asterisk.org/wiki/display/AST/Asterisk+20+Configuration_res_pjsip"
										target="_blank">See asterisk.org reference.</a>
								</div>
							</div>
							<div class="flex flex-col h-full gap-1">
								<q-toggle toggle-indeterminate dense v-model="endpointTrustIdInbound" :disable="isAnyLoading"
									label="Trust Id Inbound" />
								<div class="text-xs text-grey-8">
									Accept identification information received from this endpoint. <a
										class="text-blue underline"
										href="https://wiki.asterisk.org/wiki/display/AST/Asterisk+20+Configuration_res_pjsip#Asterisk20Configuration_res_pjsip-endpoint_trust_id_inbound"
										target="_blank">See asterisk.org reference.</a>
								</div>
							</div>
							<div class="flex flex-col h-full gap-1">
								<q-toggle toggle-indeterminate dense v-model="endpointAllowSubscribe" :disable="isAnyLoading"
									label="Allow Subscribe" />
								<div class="text-xs text-grey-8">
									Determines if endpoint is allowed to initiate subscriptions with Asterisk. <a
										class="text-blue underline"
										href="https://wiki.asterisk.org/wiki/display/AST/Asterisk+20+Configuration_res_pjsip"
										target="_blank">See asterisk.org reference.</a>
								</div>
							</div>
							<div class="flex flex-col h-full gap-1">
								<q-input clearable dense v-model="endpointDTMFMode" label="DTMF Mode" :disable="isAnyLoading" />
								<div class="text-xs text-grey-8">
									This setting allows to choose the DTMF mode for endpoint communication. <a
										class="text-blue underline"
										href="https://wiki.asterisk.org/wiki/display/AST/Asterisk+20+Configuration_res_pjsip#Asterisk20Configuration_res_pjsip-endpoint_dtmf_mode"
										target="_blank">See asterisk.org reference.</a>
								</div>
							</div>
							<div class="flex flex-col h-full gap-1">
								<q-input clearable dense v-model="endpointTransport" label="Transport" :disable="isAnyLoading" />
								<div class="text-xs text-grey-8">
									Explicit transport configuration to use. <a class="text-blue underline"
										href="https://wiki.asterisk.org/wiki/display/AST/Asterisk+20+Configuration_res_pjsip#Asterisk20Configuration_res_pjsip-endpoint_transport"
										target="_blank">See asterisk.org reference.</a>
								</div>
							</div>
							<div class="flex flex-col h-full gap-1">
								<q-input clearable dense v-model="endpointContext" label="Context" :disable="isAnyLoading" />
								<div class="text-xs text-grey-8">
									Dialplan context for inbound sessions. <a class="text-blue underline"
										href="https://wiki.asterisk.org/wiki/display/AST/Asterisk+20+Configuration_res_pjsip"
										target="_blank">See asterisk.org reference.</a>
								</div>
							</div>
							<div class="flex flex-col h-full gap-1">
								<q-input clearable dense v-model="endpointCallerid" label="Endpoint Caller Id" :disable="isAnyLoading" />
								<div class="text-xs text-grey-8">
									Must be in the format "Name &lt;Number&gt;", or only "&lt;Number&gt;". <a
										class="text-blue underline"
										href="https://wiki.asterisk.org/wiki/display/AST/Asterisk+20+Configuration_res_pjsip"
										target="_blank">See asterisk.org reference.</a>
								</div>
							</div>
							<div class="flex flex-col h-full gap-1">
								<q-input clearable dense v-model="aorMaxContacts" label="AoR Max Contacts" type="number" :disable="isAnyLoading"
									step="1" />
								<div class="text-xs text-grey-8">
									Maximum number of contacts that can bind to an AoR. <a class="text-blue underline"
										href="https://wiki.asterisk.org/wiki/display/AST/Asterisk+20+Configuration_res_pjsip#Asterisk20Configuration_res_pjsip-aor_max_contacts"
										target="_blank">See asterisk.org reference.</a>
								</div>
							</div>
							<div class="flex flex-col h-full gap-1">
								<q-select label="Allow Codecs" v-model="endpointAllow" use-input use-chips multiple
									hide-dropdown-icon input-debounce="0" new-value-mode="toggle" dense clearable :disable="isAnyLoading" />
								<div class="text-xs text-grey-8">
									Media Codec(s) to allow. <a class="text-blue underline"
										href="https://wiki.asterisk.org/wiki/display/AST/Asterisk+20+Configuration_res_pjsip"
										target="_blank">See asterisk.org reference.</a>
								</div>
							</div>
						</div>
					</div>
					<div class="flex flex-col gap-2">
						<div class="font-bold">Fax (T38)</div>
						<div
							class="px-1 grid gap-2 grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5">
							<div class="flex flex-col h-full gap-1">
								<q-toggle toggle-indeterminate dense v-model="endpointFaxDetect" label="Fax Detect" :disable="isAnyLoading" />
								<div class="text-xs text-grey-8">
									Whether CNG tone detection is enabled. <a class="text-blue underline"
										href="https://wiki.asterisk.org/wiki/display/AST/Asterisk+20+Configuration_res_pjsip#Asterisk20Configuration_res_pjsip-endpoint_fax_detect"
										target="_blank">See asterisk.org reference.</a>
								</div>
							</div>
							<div class="flex flex-col h-full gap-1">
								<q-toggle toggle-indeterminate dense v-model="endpointT38Udptl" label="T38 Udptl" :disable="isAnyLoading" />
								<div class="text-xs text-grey-8">
									Whether T.38 UDPTL support is enabled or not. <a class="text-blue underline"
										href="https://wiki.asterisk.org/wiki/display/AST/Asterisk+20+Configuration_res_pjsip"
										target="_blank">See asterisk.org reference.</a>
								</div>
							</div>
							<div class="flex flex-col h-full gap-1">
								<q-input clearable dense v-model="endpointT38UdptlEc" label="Endpoint T38 Udptl Ec" :disable="isAnyLoading" />
								<div class="text-xs text-grey-8">
									T.38 UDPTL error correction method. <a class="text-blue underline"
										href="https://wiki.asterisk.org/wiki/display/AST/Asterisk+20+Configuration_res_pjsip"
										target="_blank">See asterisk.org reference.</a>
								</div>
							</div>
							<div class="flex flex-col h-full gap-1">
								<q-toggle toggle-indeterminate dense v-model="endpointT38UdptlNat" label="T38 Udptl Nat" :disable="isAnyLoading" />
								<div class="text-xs text-grey-8">
									Whether T.38 UDPTL support is enabled or not. <a class="text-blue underline"
										href="https://wiki.asterisk.org/wiki/display/AST/Asterisk+20+Configuration_res_pjsip#Asterisk20Configuration_res_pjsip-endpoint_t38_udptl_nat"
										target="_blank">See asterisk.org reference.</a>
								</div>
							</div>
						</div>
					</div>
					<div class="flex flex-col gap-2">
						<div class="font-bold">Auth</div>
						<div class="flex flex-col h-full gap-1">
							<q-select class="px-1" label="Remote Hosts" v-model="remoteHosts" use-input use-chips multiple
								hide-dropdown-icon input-debounce="0" new-value-mode="toggle" dense clearable :disable="isAnyLoading" />
							<div class="text-xs text-grey-8">
								A list of remote hosts in the form of
								&lt;ipaddress | hostname&gt;[:port]
								If specified, a static contact for each host will be created
								in the aor. If accepts_registrations is no, an identify
								object is also created with a match line for each remote host.
								Hostnames must resolve to A, AAAA or CNAME records.
								SRV records are not currently supported. Press enter to confirm the host. <a
									class="text-blue underline"
									href="https://wiki.asterisk.org/wiki/display/AST/PJSIP+Configuration+Wizard"
									target="_blank">See asterisk.org reference.</a>
							</div>
						</div>

						<div
							class="px-1 grid gap-2 grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5">
							<div class="flex flex-col h-full gap-1">
								<q-toggle toggle-indeterminate dense v-model="sendsAuth" label="Sends Auth" :disable="isAnyLoading" />
								<div class="text-xs text-grey-8">
									Will create an outbound auth object for the endpoint and
									registration. At least outbound/username must be specified. <a
										class="text-blue underline"
										href="https://wiki.asterisk.org/wiki/display/AST/PJSIP+Configuration+Wizard"
										target="_blank">See asterisk.org reference.</a>
								</div>
							</div>
							<div class="flex flex-col h-full gap-1">
								<q-toggle toggle-indeterminate dense v-model="sendsRegistrations" :disable="isAnyLoading"
									label="Sends Registrations" />
								<div class="text-xs text-grey-8">
									Will create an outbound registration object for each
									host in remote_hosts. <a class="text-blue underline"
										href="https://wiki.asterisk.org/wiki/display/AST/PJSIP+Configuration+Wizard"
										target="_blank">See asterisk.org reference.</a>
								</div>
							</div>
							<div class="flex flex-col h-full gap-1">
								<q-toggle toggle-indeterminate dense v-model="acceptsAuth" label="Accepts Auth" :disable="isAnyLoading" />
								<div class="text-xs text-grey-8">
									Will create an inbound auth object for the endpoint.
									At least 'inbound/username' must be specified. <a class="text-blue underline"
										href="https://wiki.asterisk.org/wiki/display/AST/PJSIP+Configuration+Wizard"
										target="_blank">See asterisk.org reference.</a>
								</div>
							</div>
							<div class="flex flex-col h-full gap-1">
								<q-toggle toggle-indeterminate dense v-model="acceptsRegistrations" :disable="isAnyLoading"
									label="Accepts Registrations" />
								<div class="text-xs text-grey-8">
									Will create an inbound registration object for each
									host in remote_hosts. <a class="text-blue underline"
										href="https://wiki.asterisk.org/wiki/display/AST/PJSIP+Configuration+Wizard"
										target="_blank">See asterisk.org reference.</a>
								</div>
							</div>
							<div class="flex flex-col h-full gap-1">
								<q-input clearable dense v-model="inboundAuthUsername" label="Inbound Auth Username" :disable="isAnyLoading" />
							</div>
							<div class="text-xs text-grey-8">
								<q-input clearable dense v-model="inboundAuthPassword" label="Inbound Auth Password" :disable="isAnyLoading" />
							</div>
						</div>
					</div>
					<div class="flex flex-col gap-2">
						<div class="font-bold">Hints</div>
						<div
							class="px-1 grid gap-2 grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5">
							<div class="flex flex-col h-full gap-1">
								<q-toggle toggle-indeterminate dense v-model="hasHint" label="Has Hint" :disable="isAnyLoading" />
								<div class="text-xs text-grey-8">
									Causes hints to be automatically created. <a class="text-blue underline"
										href="https://wiki.asterisk.org/wiki/display/AST/PJSIP+Configuration+Wizard"
										target="_blank">See asterisk.org reference.</a>
								</div>
							</div>
							<div class="flex flex-col h-full gap-1">
								<q-input clearable dense v-model="hintExten" label="Hint Exten" :disable="isAnyLoading" />
								<div class="text-xs text-grey-8">
									The extension this hint will be registered with. <a class="text-blue underline"
										href="https://wiki.asterisk.org/wiki/display/AST/PJSIP+Configuration+Wizard"
										target="_blank">See asterisk.org reference.</a>
								</div>
							</div>
							<div class="flex flex-col h-full gap-1">
								<q-input clearable dense v-model="hintContext" label="Hint Context" :disable="isAnyLoading" />
								<div class="text-xs text-grey-8">
									The context into which hints are placed. <a class="text-blue underline"
										href="https://wiki.asterisk.org/wiki/display/AST/PJSIP+Configuration+Wizard"
										target="_blank">See asterisk.org reference.</a>
								</div>
							</div>
						</div>
					</div>
					<div class="flex flex-col gap-2">
						<div class="font-bold">Keep Alive / Status</div>
						<div
							class="px-1 grid gap-2 grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5">
							<div class="flex flex-col h-full gap-1">
								<q-input dense v-model="aorQualifyFrequency" label="AoR Qualify Frequency" type="number"
									step="1" clearable :disable="isAnyLoading" />
								<div class="text-xs text-grey-8">
									Interval at which to qualify a contact. <a class="text-blue underline"
										href="https://wiki.asterisk.org/wiki/display/AST/Asterisk+20+Configuration_res_pjsip"
										target="_blank">See asterisk.org reference.</a>
								</div>
							</div>

						</div>
					</div>
					<div class="flex flex-col gap-2">
						<div class="font-bold">Other</div>
						<div
							class="px-1 grid gap-2 grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5">
							<div class="flex flex-col h-full gap-1">
								<q-input dense v-model="type" label="type" clearable :disable="isAnyLoading" />
								<div class="text-xs text-grey-8">
									The type of config entry, leave this unless you know better. <a
										class="text-blue underline"
										href="https://wiki.asterisk.org/wiki/display/AST/Asterisk+20+Configuration_res_pjsip"
										target="_blank">See asterisk.org reference.</a>
								</div>
							</div>

						</div>
					</div>

					<div class="flex flex-row gap-2">
						<q-btn color="primary" @click="onClickSaveChanges" :disable="isAnyLoading" :loading="upsertLoading">
							<div class="flex flex-row items-center gap-2">
								<MDIContentSave class="w-4 h-4" />
								<div>Save Changes</div>
							</div>
						</q-btn>
						<q-space />
						<q-btn v-if="showRemove" color="red" @click="onClickRemove" :disable="isAnyLoading">
							<div class="flex flex-row items-center gap-2">
								<MDIDelete class="w-4 h-4" />
								<div>Remove</div>
							</div>
						</q-btn>
					</div>
				</div>


			</q-card-section>
		</q-expansion-item>
	</q-card>
</template>