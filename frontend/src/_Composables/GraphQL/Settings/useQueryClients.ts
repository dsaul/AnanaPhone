import { useQuery } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { provideApolloClient } from "@vue/apollo-composable";
import { useApolloClient } from '../useApolloClient';
import { computed } from 'vue';
import type { IGraphQLResponse } from '../IGraphQLResponse';
import type { ApolloError } from '@apollo/client/errors';
import type { IPJSIPEntry } from '../PJSIPEntry/IPJSIPEntry';

const useQueryClients = () => {

	const apolloClient = useApolloClient();
	provideApolloClient(apolloClient);

	const { result, onError, loading, onResult, error } = useQuery<IGraphQLResponse>(gql`
		query {
			settings {
				id,
				clients {
					id,
					name,
					hasHint,
					hintExten,
					hintContext,
					inboundAuthUsername,
					inboundAuthPassword,
					outboundAuthUsername,
					outboundAuthPassword,
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
		console.error('useQueryClients error', error);
	});

	const prop = computed<IPJSIPEntry[]>(() => {
		if (!result.value) {
			return [];
		}
		return result.value?.settings?.clients || [];
	});

	return {
		clients: prop,
		clientsLoading: loading,
		clientsOnError: onError,
		clientsError: error,
		clientsOnResult: onResult,
	}
}

export { useQueryClients }