//allowedCallOutNumbers

import { useQuery } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { provideApolloClient } from "@vue/apollo-composable";
import { useApolloClient } from '../useApolloClient';
import { computed, type Ref } from 'vue';
import type { IGraphQLResponse } from '../IGraphQLResponse';
import isEmpty from '@/_Composables/Utility/isEmpty';
import type { ApolloError } from '@apollo/client/errors';

const useQueryJoinConferenceData = (conferenceName: Ref<string | null>) => {

	const apolloClient = useApolloClient();
	provideApolloClient(apolloClient)

	const conferenceNames = computed(() => {

		if (isEmpty(conferenceName.value)) {
			return [];
		}

		return [conferenceName.value];
	});

	const { result, onError, loading, onResult, error } = useQuery<IGraphQLResponse>(gql`
		query getRoom($names: [String!]!) {
			confBridge {
				id,
				rooms(names: $names) {
					id,
					name,
					displayName,
					participants {
						id,
						channel,
						callerIdNumber,
						callerIdName,
						conferenceName
					}
				}
			},
			settings {
				allowedCallOutNumbers
			}
		}
		`, () => ({
		names: conferenceNames.value,
	}), {
		pollInterval: 1000 * 60 * 5,
	},);

	onError((error: ApolloError) => {
		console.error('useQueryJoinConferenceData error', error, conferenceNames.value);
	});

	const allowedCallOutNumbers = computed(() => {
		if (!result.value) {
			return [];
		}
		return result.value?.settings?.allowedCallOutNumbers || [];
	});

	const room = computed(() => {
		if (isEmpty(conferenceName.value)) {
			return null;
		}

		if (!result.value) {
			return null;
		}
		return result.value?.confBridge?.rooms?.[0] || null;
	});

	return {
		allowedCallOutNumbers,
		room,
		conferenceNames,
		loading: loading,
		onError: onError,
		error: error,
		onResult: onResult,
	}
}

export { useQueryJoinConferenceData }