import { useMutation } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { provideApolloClient } from "@vue/apollo-composable";
import { useApolloClient } from '../useApolloClient';
import type { ApolloError } from '@apollo/client/errors';
import type { IE164Client } from './IE164Client';
import isEmpty from '@/_Composables/Utility/isEmpty';

const useMutationRemove = () => {
	
	const apolloClient = useApolloClient();
	provideApolloClient(apolloClient)
	
	const { mutate, onError, loading, onDone } = useMutation<{ status: string; }>(gql`

	mutation post($payload: String!) {
			settings {
				e164Client {
					remove(e164: $payload) {
						status
					}
				}
			}
		}
	`);
	
	onError((error: ApolloError) => {
		console.error('useMutationRemove error', error);
	});
	
	const fn = (payload: IE164Client | null) => {
		
		if (payload === null) {
			console.warn('payload === null');
			return;
		}
		if (isEmpty(payload.e164)) {
			console.warn('isEmpty(payload.e164)');
			return;
		}
		
		mutate({
			payload: payload.e164,
		});
	};
	
	return {
		remove: fn,
		removeLoading: loading,
		removeError: onError,
		removeDone: onDone,
	}
}

export { useMutationRemove }