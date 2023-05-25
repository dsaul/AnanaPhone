import { computed, type Ref } from "vue";

const useUCWords = (payload: Ref<string>) => {
	return computed(() => {
		return payload.value.replace(/\b\w/g, (match) => match.toUpperCase());
	})
}

export { useUCWords }