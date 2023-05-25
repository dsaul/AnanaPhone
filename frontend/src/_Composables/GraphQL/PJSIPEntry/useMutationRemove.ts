import { useMutation } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { provideApolloClient } from "@vue/apollo-composable";
import { useApolloClient } from '../useApolloClient';
import type { ApolloError } from '@apollo/client/errors';
import isEmpty from '@/_Composables/Utility/isEmpty';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useMutationRemove = () => {
	
	const apolloClient = useApolloClient();
	provideApolloClient(apolloClient)
	
	const { mutate, onError, loading, onDone } = useMutation<{ status: string; }>(gql`

	mutation pjsipEntryRemove($name: String!) {
			settings {
				pjsipEntryRemove(name: $name) {
					status
				}
			}
		}
	`);
	
	onError((error: ApolloError) => {
		console.error('useMutationRemove error', error);
	});
	
	const fn = (payload: IPJSIPEntry | null) => {
		
		if (payload === null) {
			console.warn('payload === null');
			return;
		}
		if (isEmpty(payload.name)) {
			console.warn('isEmpty(payload.name)');
			return;
		}
		
		mutate({
			name: payload.name,
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