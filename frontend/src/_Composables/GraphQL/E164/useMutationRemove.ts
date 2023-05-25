import { useMutation } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { provideApolloClient } from "@vue/apollo-composable";
import { useApolloClient } from '../useApolloClient';
import type { ApolloError } from '@apollo/client/errors';
import type { IE164 } from './IE164';
import isEmpty from '@/_Composables/Utility/isEmpty';

const useMutationRemove = () => {
	
	const apolloClient = useApolloClient();
	provideApolloClient(apolloClient)
	
	const { mutate, onError, loading, onDone } = useMutation<{ status: string; }>(gql`

	mutation e164Post($payload: String!) {
			settings {
				e164 {
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
	
	const fn = (payload: IE164 | null) => {
		
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