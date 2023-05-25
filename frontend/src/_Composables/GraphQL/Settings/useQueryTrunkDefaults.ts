import { useQuery } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { provideApolloClient } from "@vue/apollo-composable";
import { useApolloClient } from '../useApolloClient';
import { computed } from 'vue';
import type { IGraphQLResponse } from '../IGraphQLResponse';
import type { ApolloError } from '@apollo/client/errors';
import type { IPJSIPEntry } from '../PJSIPEntry/IPJSIPEntry';

const useQueryTrunkDefaults = () => {

	const apolloClient = useApolloClient();
	provideApolloClient(apolloClient);

	const { result, onError, loading, onResult, error } = useQuery<IGraphQLResponse>(gql`
		query {
			settings {
				id,
				pjsipTrunkDefaults {
					id,
					name,
					hasHint,
					hintExten,
					hintContext,
					inboundAuthUsername,
					inboundAuthPassword,
					endpointCallerid,
					aorMaxContacts,
					remoteHosts,
					type,
					acceptsAuth,
					acceptsRegistrations,
					endpointAllow,
					endpointDirectMedia,
					endpointForceRport,
					endpointRewriteContact,
					endpointRTPSymmetric,
					aorQualifyFrequency,
					endpointContext,
					sendsAuth,
					sendsRegistrations,
					endpointT38Udptl,
					endpointT38UdptlEc,
					endpointFaxDetect,
					endpointTrustIdInbound,
					endpointT38UdptlNat,
					endpointDTMFMode,
					endpointAllowSubscribe,
					endpointTransport,
					xDummyPlaceholder,
				}
			}
		}
		`, null, {
		pollInterval: 1000,
	});

	onError((error: ApolloError) => {
		console.error('useQueryTrunkDefaults error', error);
	});

	const prop = computed<IPJSIPEntry | null>(() => {
		if (!result.value) {
			return null;
		}
		return result.value?.settings?.pjsipTrunkDefaults || null;
	});

	return {
		defaults: prop,
		defaultsLoading: loading,
		defaultsOnError: onError,
		defaultsError: error,
		defaultsOnResult: onResult,
	}
}

export { useQueryTrunkDefaults }