import type { IActiveCall } from "./ActiveCall/IActiveCall";
import type { IHistoricCall } from "./HistoricCall/IHistoricCall";

interface ICalls {
	active?: IActiveCall[];
	historic?: IHistoricCall[];
}

export { type ICalls }