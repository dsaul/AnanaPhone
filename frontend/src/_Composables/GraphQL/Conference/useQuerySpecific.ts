import { useQuery } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { provideApolloClient } from "@vue/apollo-composable";
import { computed, type Ref } from 'vue';
import type { IGraphQLResponse } from '../IGraphQLResponse';
import type { ApolloError } from '@apollo/client/errors';
import { useApolloClient } from '../useApolloClient';
import isEmpty from '@/_Composables/Utility/isEmpty';

const useQuerySpecific = (conferenceName: Ref<string | null>) => {

	const apolloClient = useApolloClient();
	provideApolloClient(apolloClient);

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
					timestampISO8601,
					participants {
						id,
						channel,
						callerIdNumber,
						callerIdName,
						conferenceName
					}
				}
			}
		}
		`, () => ({
		names: conferenceNames.value,
	}), {
		pollInterval: 1000 * 60 * 5,
	},);

	onError((error: ApolloError) => {
		console.error('useQuerySpecific error', error);
	});

	const prop = computed(() => {
		if (isEmpty(conferenceName.value)) {
			return null;
		}

		if (!result.value) {
			return null;
		}
		return result.value?.confBridge?.rooms?.[0] || null;
	});

	return {
		specific: prop,
		specificLoading: loading,
		specificOnError: onError,
		specificError: error,
		specificOnResult: onResult,
	}
}

export { useQuerySpecific }