/* eslint max-params: 0 */

import type { Ref } from 'vue';

const usePropertyWatcherUpdate = <
	TDflt,
	TRef extends Ref<TDflt>,
	TNext extends Record<string, unknown>,
	TLast extends Record<string, unknown>>(
		ref: TRef,
		key: string,
		dflt: TDflt,
		next: TNext,
		last: TLast | undefined,
		addnlCB: ((pNext: TNext, pLast: TLast | undefined, pDflt: TDflt) => void) | null = null,
		validateCB: ((payload: TDflt) => TDflt) = ((noop) => { return noop; })
	) => {

	if (ref.value !== next[key] || next[key] !== last?.[key]) {
		ref.value = next[key] === undefined ? dflt : validateCB(next[key] as TDflt);
		if (addnlCB !== null) {
			addnlCB(next, last, dflt);
		}
	}

};

export { usePropertyWatcherUpdate };