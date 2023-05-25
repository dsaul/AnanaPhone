import { useMutation } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { provideApolloClient } from "@vue/apollo-composable";
import { useApolloClient } from '../useApolloClient';
import type { ApolloError } from '@apollo/client/errors';



const useMutationOutboundCall = () => {
	
	const apolloClient = useApolloClient();
	provideApolloClient(apolloClient)

	const { mutate, onError, loading, onDone } = useMutation<{ status: string; }>(gql`
		mutation performOwnerOutboundCall($callDevice: String!, $destination: String!) {
			performOwnerOutboundCall(callDevice: $callDevice, destination: $destination) {
				status
			}
		}
	`);

	onError((error: ApolloError) => {
		console.error('useMutationOutboundCall error', error);
	});
	
	const fn = (callDevice: string, destination: string) => {
		mutate({
			callDevice: callDevice,
			destination: destination,
		});
	}
	
	return {
		outboundCall: fn,
		outboundCallLoading: loading,
		outboundCallError: onError,
		outboundCallDone: onDone,
	}
}

export { useMutationOutboundCall }