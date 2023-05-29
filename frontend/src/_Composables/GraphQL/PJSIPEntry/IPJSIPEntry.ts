interface IPJSIPEntry {
	name?: string | null;
	hasHint?: boolean | null;
	hintExten?: string | null;
	hintContext?: string | null;
	inboundAuthUsername?: string | null;
	inboundAuthPassword?: string | null;
	outboundAuthUsername?: string | null;
	outboundAuthPassword?: string | null;
	endpointCallerid?: string | null;
	aorMaxContacts?: number | null;
	remoteHosts?: string[];
	type?: string | null;
	acceptsAuth?: boolean | null;
	acceptsRegistrations?: boolean | null;
	endpointAllow?: string[] | null;
	endpointDirectMedia?: boolean | null;
	endpointForceRport?: boolean | null;
	endpointRewriteContact?: boolean | null;
	endpointRTPSymmetric?: boolean | null;
	aorQualifyFrequency?: number | null;
	endpointContext?: string | null;
	sendsAuth?: boolean | null;
	sendsRegistrations?: boolean | null;
	endpointT38Udptl?: boolean | null;
	endpointT38UdptlEc?: string | null;
	endpointFaxDetect?: boolean | null;
	endpointTrustIdInbound?: boolean | null;
	endpointT38UdptlNat?: boolean | null;
	endpointDTMFMode?: string | null;
	endpointAllowSubscribe?: boolean;
	endpointTransport?: string | null;
	xDummyPlaceholder?: string | null;
}

const generateEmpty = (): IPJSIPEntry => {
	return {
		
	};
}


export { type IPJSIPEntry, generateEmpty }