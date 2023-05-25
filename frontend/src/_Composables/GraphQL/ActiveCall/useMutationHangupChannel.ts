import { useMutation } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { provideApolloClient } from "@vue/apollo-composable";
import { useApolloClient } from '../useApolloClient';
import type { ApolloError } from '@apollo/client/errors';

const useMutationHangupChannel = () => {

	const apolloClient = useApolloClient();
	provideApolloClient(apolloClient)

	const { mutate, onError, loading, onDone } = useMutation<{ status: string; }>(gql`
		mutation hangupChannel($channel: String!) {
			hangupChannel(channel: $channel) {
				status
			}
		}
	`);

	onError((error: ApolloError) => {
		console.error('useMutationHangupChannel error', error);
	});

	const fn = (channel: string) => {
		mutate({
			channel: channel,
		});
	};

	return {
		hangupChannel: fn,
		hangupChannelLoading: loading,
		hangupChannelError: onError,
		hangupChannelDone: onDone,
	}
}

export { useMutationHangupChannel }