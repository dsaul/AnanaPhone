import { useMutation } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { provideApolloClient } from "@vue/apollo-composable";
import { useApolloClient } from '../useApolloClient';
import type { ApolloError } from '@apollo/client/errors';

const useMutationRemoveAll = () => {
	
	const apolloClient = useApolloClient();
	provideApolloClient(apolloClient)
	
	const { mutate, onError, loading, onDone } = useMutation<{ status: string; }>(gql`

	mutation post($payload: String!) {
			calls {
				historic {
					removeAll(payload: $payload) {
						status
					}
				}
			}
		}
	`);
	
	onError((error: ApolloError) => {
		console.error('useMutationRemoveAll error', error);
	});
	
	const fn = () => {
		
		mutate({
			payload: ''
		});
	};
	
	return {
		removeAll: fn,
		removeAllLoading: loading,
		removeAllError: onError,
		removeAllDone: onDone,
	}
}

export { useMutationRemoveAll }