import { isEmpty } from "lodash";

export default (payload: string | null | undefined | ''): payload is null | undefined | '' => {
	
	if (isEmpty(payload)) {
		return true;
	}
	return false;
};