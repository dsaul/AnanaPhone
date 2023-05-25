import { computed } from "vue"

const useAPIBaseURI = () => {
	return computed(() => {
		return 'http://localhost:5224/';
	})
}

export { useAPIBaseURI }