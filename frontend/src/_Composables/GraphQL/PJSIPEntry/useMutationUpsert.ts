import { useMutation } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { provideApolloClient } from "@vue/apollo-composable";
import { useApolloClient } from '../useApolloClient';
import type { ApolloError } from '@apollo/client/errors';
import type { IPJSIPEntry } from './IPJSIPEntry';

const useMutationUpsert = () => {
	
	const apolloClient = useApolloClient();
	provideApolloClient(apolloClient)
	
	const { mutate, onError, loading, onDone } = useMutation<{ status: string; }>(gql`

	mutation e164Post($payload: Input_PJSIPEntry!, $name: String!, $templateName: String, $isTemplate: Boolean!) {
			settings {
				pjsipEntryUpsert(e164: $payload, name: $name, templateName: $templateName, isTemplate: $isTemplate) {
					status
				}
			}
		}
	`);
	
	onError((error: ApolloError) => {
		console.error('useMutationUpsert error', error);
	});
	
	const fn = (payload: IPJSIPEntry, name: string, templateName: string | null, isTemplate: boolean) => {
		mutate({
			payload: payload,
			name: name,
			templateName: templateName,
			isTemplate: isTemplate,
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