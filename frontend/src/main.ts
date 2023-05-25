import './assets/index.css'
import { Quasar, Notify } from 'quasar'
import { createApp, provide, h } from 'vue'
import App from './App.vue'
import router from './router'
import { DefaultApolloClient, provideApolloClient } from '@vue/apollo-composable'

// Import icon libraries
import '@quasar/extras/material-icons/material-icons.css'

// Import Quasar css
import 'quasar/dist/quasar.css'

import { useApolloClient } from './_Composables/GraphQL/useApolloClient'


const apolloClient = useApolloClient();
provideApolloClient(apolloClient)

const app = createApp({
	setup() {
		provide(DefaultApolloClient, apolloClient)
	},
	render: () => h(App),
})

app.use(router);
app.use(Quasar, {
	plugins: {
		Notify
	},
	config: {
		//     brand: {
		//       // primary: '#e46262',
		//       // ... or all other brand colors
		//     },
		notify: {}, // default set of options for Notify Quasar plugin
		//     loading: {...}, // default set of options for Loading Quasar plugin
		//     loadingBar: { ... }, // settings for LoadingBar Quasar plugin
		//     // ..and many more (check Installation card on each Quasar component/directive/plugin)
	}
})

app.mount('#app')


