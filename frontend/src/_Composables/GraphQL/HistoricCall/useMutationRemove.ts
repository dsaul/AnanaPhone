import { useMutation } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { provideApolloClient } from "@vue/apollo-composable";
import { useApolloClient } from '../useApolloClient';
import type { ApolloError } from '@apollo/client/errors';
import isEmpty from '@/_Composables/Utility/isEmpty';

const useMutationRemove = () => {
	
	const apolloClient = useApolloClient();
	provideApolloClient(apolloClient)
	
	const { mutate, onError, loading, onDone } = useMutation<{ status: string; }>(gql`

	mutation post($payload: String!) {
			calls {
				historic {
					remove(id: $payload) {
						status
					}
				}
			}
		}
	`);
	
	onError((error: ApolloError) => {
		console.error('useMutationRemove error', error);
	});
	
	const fn = (id: string | null) => {
		
		if (isEmpty(id)) {
			console.warn('isEmpty(id)');
			return;
		}
		
		mutate({
			payload: id,
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