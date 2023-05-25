import { useQuery } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { provideApolloClient } from "@vue/apollo-composable";
import { useApolloClient } from '../useApolloClient';
import { computed } from 'vue';
import type { IGraphQLResponse } from '../IGraphQLResponse';
import type { ApolloError } from '@apollo/client/errors';

const useQueryTrunks = () => {

	const apolloClient = useApolloClient();
	provideApolloClient(apolloClient);

	const { result, onError, loading, onResult, error } = useQuery<IGraphQLResponse>(gql`
		query {
			settings {
				id,
				trunks {
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
		console.error('useQueryTrunks error', error);
	});

	const prop = computed(() => {
		if (!result.value) {
			return [];
		}
		return result.value?.settings?.trunks || [];
	});

	return {
		trunks: prop,
		trunksLoading: loading,
		trunksOnError: onError,
		trunksError: error,
		trunksOnResult: onResult,
	}
}

export { useQueryTrunks }