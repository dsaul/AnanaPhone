import { useMutation } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { provideApolloClient } from "@vue/apollo-composable";
import { useApolloClient } from '../useApolloClient';
import type { ApolloError } from '@apollo/client/errors';
import type { IE164Client } from './IE164Client';

const useMutationUpsert = () => {
	
	const apolloClient = useApolloClient();
	provideApolloClient(apolloClient)
	
	const { mutate, onError, loading, onDone } = useMutation<{ status: string; }>(gql`

	mutation post($payload: Input_E164ClientRow!) {
			settings {
				e164Client {
					upsert(e164: $payload) {
						status
					}
				}
			}
		}
	`);
	
	onError((error: ApolloError) => {
		console.error('useMutationUpsert error', error);
	});
	
	const fn = (payload: IE164Client) => {
		mutate({
			payload: payload,
		});
	};
	
	return {
		upsert: fn,
		upsertLoading: loading,
		upsertError: onError,
		upsertDone: onDone,
	}
}

export { useMutationUpsert }