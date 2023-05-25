import { ApolloClient, createHttpLink, InMemoryCache } from '@apollo/client/core'
import { useGraphQLURI } from '../Constants/useGraphQLURI';

const uri = useGraphQLURI();

// HTTP connection to the API
const httpLink = createHttpLink({
	uri: uri.value,
});

// Cache implementation
const cache = new InMemoryCache({
	typePolicies: {
		ActiveCall: {
			merge: true,
		},
		Conference: {
			merge: true,
			fields: {
				participants: {
					merge: false,
				},
			},
		},
		ConferenceParticipant: {
			merge: true,
		},
		E164Row: {
			merge: true,
		},
		GenericReturn: {
			merge: true,
		},
		HistoricCall: {
			merge: true,
		},
		PJSIPEntry: {
			merge: true,
		},
		PJSIPWizardRow: {
			merge: true,
		},
		Query: {
			merge: false,
		},
		Query_Calls: {
			merge: false,
			fields: {
				active: {
					merge: false,
				},
			},
		},
		Query_Confbridge: {
			merge: false,
			fields: {
				rooms: {
					merge: false
				}
			}
		},
		
		
		Query_Settings: {
			merge: false,
		},
		
	}
});

console.log('cache', cache)

// Create the apollo client
const apolloClient = new ApolloClient({
	link: httpLink,
	cache,
	connectToDevTools: true,
})

const useApolloClient = () => {
	return apolloClient;
}

export { useApolloClient }