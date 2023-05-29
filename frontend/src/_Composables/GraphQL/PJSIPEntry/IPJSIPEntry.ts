interface IPJSIPEntry {
	name?: string | null;
	hasHint?: boolean;
	hintExten?: string | null;
	hintContext?: string | null;
	inboundAuthUsername?: string | null;
	inboundAuthPassword?: string | null;
	outboundAuthUsername?: string | null;
	outboundAuthPassword?: string | null;
	endpointCallerid?: string | null;
	aorMaxContacts?: number;
	remoteHosts?: string[];
	type?: string | null;
	acceptsAuth?: boolean;
	acceptsRegistrations?: boolean;
	endpointAllow?: string[] | null;
	endpointDirectMedia?: boolean;
	endpointForceRport?: boolean;
	endpointRewriteContact?: boolean;
	endpointRTPSymmetric?: boolean;
	aorQualifyFrequency?: number;
	endpointContext?: string | null;
	sendsAuth?: boolean;
	sendsRegistrations?: boolean;
	endpointT38Udptl?: boolean;
	endpointT38UdptlEc?: string | null;
	endpointFaxDetect?: boolean;
	endpointTrustIdInbound?: boolean;
	endpointT38UdptlNat?: boolean;
	endpointDTMFMode?: string | null;
	endpointAllowSubscribe?: boolean;
	endpointTransport?: string | null;
	xDummyPlaceholder?: string;
}

const generateEmpty = (): IPJSIPEntry => {
	return {
		
	};
}


export { type IPJSIPEntry, generateEmpty }