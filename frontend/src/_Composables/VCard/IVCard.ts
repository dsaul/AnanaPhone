export interface IVCard {
	Addresses: Array<{
		Type: string | null;
		Label: string | null;
		AddressParts: string[];
	}>;
	Birthday: string | null;
	EMails: Array<{
		Type: string | null;
		EMail: string | null;
	}>;
	FullName: string | null;
	Names: string[];
	PhotoURI: string | null;
	ProdId: string | null;
	RevisionTime: string | null;
	TelephoneNumbers: Array<{
		Type: string | null;
		E164: string | null;
	}>
	UID: string | null;
	Version: string | null;
}