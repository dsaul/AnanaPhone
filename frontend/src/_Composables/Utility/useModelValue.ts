/* eslint max-params: 0 */

import { computed, watch, nextTick, ref } from 'vue';

export interface IModelValueProps extends Object {
	modelValue?: unknown;
	value?: unknown;
}

export interface IModelValueEmits<TVal> {
	(e: 'update:modelValue', payload: TVal | null): void,
	(e: 'on-value-changed', payload: TVal | null): void,
}


const useModelValue = <TVal>(
	emit: IModelValueEmits<TVal>,
	props: IModelValueProps,
	valueDefault: TVal | null,
	afterUpdateCB: (() => void) | null = null,
	deepWatch = false,
) => {
	const modelValue = ref<TVal | null>(null);
	const value = ref<TVal | null>(null);

	watch(props, (next, last) => {
		let callCB = false;
		if (modelValue.value !== next.modelValue || next.modelValue !== last?.modelValue) {
			(modelValue.value as TVal | null) = next.modelValue === undefined ? valueDefault : next.modelValue as TVal | null;
			callCB = true;
		}
		if (value.value !== next.value || next.value !== last?.value) {
			(value.value as TVal | null) = next.value === undefined ? valueDefault : next.value as TVal | null;
			callCB = true;
		}
		if (callCB && afterUpdateCB) {
			nextTick(() => {
				afterUpdateCB();
			});
		}
	}, { immediate: true, deep: deepWatch });

	return computed<TVal | null>({
		get() {
			if (modelValue.value !== null && modelValue.value !== undefined) {
				return (modelValue.value as TVal | null);
			}
			if (value.value !== null && value.value !== undefined) {
				return (value.value as TVal | null);
			}
			return valueDefault;
		},
		set(payload) {
			// We could only send it where appropriate, but this is better 
			// in case someone overlooks the right one.
			emit('on-value-changed', payload);
			emit('update:modelValue', payload);
		}
	});
};

export {
	useModelValue,
};