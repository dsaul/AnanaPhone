import { useQuery } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { provideApolloClient } from "@vue/apollo-composable";
import { useApolloClient } from '../useApolloClient';
import { computed } from 'vue';
import type { IGraphQLResponse } from '../IGraphQLResponse';
import type { ApolloError } from '@apollo/client/errors';

const useQueryAll = () => {

	const apolloClient = useApolloClient();
	provideApolloClient(apolloClient)

	const { result, onError, loading, onResult, error } = useQuery<IGraphQLResponse>(gql`
		query {
			settings {
				e164Clients {
					id,
					e164,
					comment,
					outboundDevice,
					disabled,
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
		return result.value?.settings?.e164Clients || [];
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