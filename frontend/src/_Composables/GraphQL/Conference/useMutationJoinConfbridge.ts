


import { useMutation } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { provideApolloClient } from "@vue/apollo-composable";
import { useApolloClient } from '../useApolloClient';
import type { ApolloError } from '@apollo/client/errors';


const useMutationJoinConfbridge = () => {
	
	
	const apolloClient = useApolloClient();
	provideApolloClient(apolloClient)

	const { mutate, onError, loading, onDone } = useMutation<{ status: string; }>(gql`
		mutation joinConfBridge($name: String!, $channel: String!) {
			joinConfBridge(name: $name, channel: $channel) {
				status
			}
		}
	`);

	onError((error: ApolloError) => {
		console.error('useMutationJoinConfbridge error', error);
	});
	
	const fn = (conferenceName: string, channel: string) => {
		mutate({
			name: conferenceName,
			channel: channel,
		});
	};
	
	return {
		joinConfbridge: fn,
		joinConfbridgeLoading: loading,
		joinConfbridgeError: onError,
		joinConfbridgeDone: onDone,
	}
	
}

export { useMutationJoinConfbridge }