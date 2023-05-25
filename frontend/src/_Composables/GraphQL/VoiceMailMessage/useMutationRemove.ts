import { useMutation } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { provideApolloClient } from "@vue/apollo-composable";
import { useApolloClient } from '../useApolloClient';
import type { ApolloError } from '@apollo/client/errors';
import type { IVoiceMailMessage } from './IVoiceMailMessage';
import isEmpty from '@/_Composables/Utility/isEmpty';

const useMutationRemove = () => {
	
	const apolloClient = useApolloClient();
	provideApolloClient(apolloClient)
	
	const { mutate, onError, loading, onDone } = useMutation<{ status: string; }>(gql`

	mutation post($payload: String!) {
		voiceMail {
			message {
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
	
	const fn = (payload: IVoiceMailMessage | null) => {
		
		if (payload === null) {
			console.warn('payload === null');
			return;
		}
		if (isEmpty(payload.id)) {
			console.warn('isEmpty(payload.id)');
			return;
		}
		
		mutate({
			payload: payload.id,
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