import { useQuery } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { provideApolloClient } from "@vue/apollo-composable";
import { computed } from 'vue';
import type { IGraphQLResponse } from '../IGraphQLResponse';
import type { ApolloError } from '@apollo/client/errors';
import { useApolloClient } from '../useApolloClient';

const useQueryAll = () => {

	const apolloClient = useApolloClient();
	provideApolloClient(apolloClient)

	const { result, onError, loading, onResult, error } = useQuery<IGraphQLResponse>(gql`
		query {
			confBridge {
				id,
				rooms {
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
			}
		}
		`, null, {
		pollInterval: 1000,
	});

	onError((error: ApolloError) => {
		console.error('useQueryAll error', error);
	});

	const prop = computed(() => {
		if (!result.value) {
			return [];
		}
		return result.value?.confBridge?.rooms || [];
	});

	return {
		all: prop,
		allLoading: loading,
		allOnError: onError,
		allError: error,
		allOnResult: onResult,
	}
}

export { useQueryAll }