
module.exports = {
	client: {
	  service: {
		name: 'my-app',
		// URL to the GraphQL API
		url: 'https://localhost:7204/graphQL',
	  },
	  // Files processed by the extension
	  includes: [
		'src/**/*.vue',
		'src/**/*.js',
	  ],
	},
  }