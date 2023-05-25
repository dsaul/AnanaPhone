import { computed } from "vue"
import { useAPIBaseURI } from "./useAPIBaseURI";

const useGraphQLURI = () => {

	const apiBase = useAPIBaseURI();

	return computed(() => {
		return `${apiBase.value}graphQL`;
	})
}

export { useGraphQLURI }