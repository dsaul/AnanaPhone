import { useQuery } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { provideApolloClient } from "@vue/apollo-composable";
import { computed, type Ref } from 'vue';
import type { IGraphQLResponse } from '../IGraphQLResponse';
import type { ApolloError } from '@apollo/client/errors';
import { useApolloClient } from '../useApolloClient';
import isEmpty from '@/_Composables/Utility/isEmpty';

const useQuerySpecific = (callName: Ref<string | null>) => {

	const apolloClient = useApolloClient();
	provideApolloClient(apolloClient);

	const channelNames = computed(() => {

		if (isEmpty(callName.value)) {
			return [];
		}

		return [callName.value];
	});

	const { result, onError, loading, onResult, error } = useQuery<IGraphQLResponse>(gql`
		query getCall($ids: [String!]!) {
			calls  {
				historic(ids: $ids) {
					id,
					callerIdName,
					callerIdNumber,
					duration,
					timestampISO8601,
					landedDID,
					originalChannel,
					callDirection,
				}
			}
		}
		`, () => ({
		ids: channelNames.value,
	}), {
		pollInterval: 1000 * 60 * 5,
	},);

	onError((error: ApolloError) => {
		console.error('useQuerySpecific error', error);
	});

	const prop = computed(() => {
		if (isEmpty(callName.value)) {
			return null;
		}

		if (!result.value) {
			return null;
		}
		return result.value?.calls?.historic?.[0] || null;
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