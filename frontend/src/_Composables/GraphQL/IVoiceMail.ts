import type { IVoiceMailMessage } from "./VoiceMailMessage/IVoiceMailMessage";

interface IVoiceMail {
	messages?: IVoiceMailMessage[];
}

export { type IVoiceMail }