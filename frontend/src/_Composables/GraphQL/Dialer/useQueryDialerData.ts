import { useQuery } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { provideApolloClient } from "@vue/apollo-composable";
import { useApolloClient } from '../useApolloClient';
import { computed } from 'vue';
import type { IGraphQLResponse } from '../IGraphQLResponse';
import type { ApolloError } from '@apollo/client/errors';

const useQueryDialerData = () => {

	const apolloClient = useApolloClient();
	provideApolloClient(apolloClient);

	const { result, onError, loading, onResult, error } = useQuery<IGraphQLResponse>(gql`
		query {
			settings {
				allowedCallOutNumbers,
				e164s {
					id,
					e164,
					name,
					comment,
				}
			}
		}
		`, () => ({

	}), {
		pollInterval: 1000 * 60 * 5,
	},);

	onError((error: ApolloError) => {
		console.error('useJoinConferenceData error', error);
	});

	const allowedCallOutNumbers = computed(() => {
		if (!result.value) {
			return [];
		}
		return result.value?.settings?.allowedCallOutNumbers || [];
	});

	const e164s = computed(() => {
		return result.value?.settings?.e164s || [];
	});


	return {
		allowedCallOutNumbers,
		e164s,
		loading: loading,
		onError: onError,
		error: error,
		onResult: onResult,
	}
}

export { useQueryDialerData }