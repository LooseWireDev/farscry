// ── WebRTC Types ────────────────────────────────────────────────────────────

export type IceCandidateInit = {
	candidate: string;
	sdpMid: string | null;
	sdpMLineIndex: number | null;
	usernameFragment?: string;
};

// ── Error Codes ─────────────────────────────────────────────────────────────

export type ErrorCode =
	| "not_authenticated"
	| "invalid_message"
	| "call_not_found"
	| "user_not_found"
	| "already_in_call"
	| "self_call"
	| "not_in_call"
	| "not_contacts";

// ── Client → Server ─────────────────────────────────────────────────────────
// No authenticate message — auth happens at WebSocket upgrade via ?token= query param.

export type CallInitiateMessage = { type: "call:initiate"; calleeId: string };
export type CallAcceptMessage = { type: "call:accept"; callId: string };
export type CallDeclineMessage = { type: "call:decline"; callId: string };
export type CallHangupMessage = { type: "call:hangup"; callId: string };
export type CallSdpMessage = { type: "call:sdp"; callId: string; sdp: string };
export type CallIceMessage = {
	type: "call:ice";
	callId: string;
	candidate: IceCandidateInit;
};

export type ClientMessage =
	| CallInitiateMessage
	| CallAcceptMessage
	| CallDeclineMessage
	| CallHangupMessage
	| CallSdpMessage
	| CallIceMessage;

// ── Server → Client ─────────────────────────────────────────────────────────

export type AuthenticatedMessage = { type: "authenticated"; userId: string };
export type CallIncomingMessage = {
	type: "call:incoming";
	callId: string;
	callerId: string;
	callerName: string;
};
export type CallAcceptedMessage = { type: "call:accepted"; callId: string };
export type CallDeclinedMessage = { type: "call:declined"; callId: string };
export type CallEndedMessage = { type: "call:ended"; callId: string };
export type CallBusyMessage = { type: "call:busy"; callId: string };
export type CallTimeoutMessage = { type: "call:timeout"; callId: string };
export type CallSdpRelayMessage = {
	type: "call:sdp";
	callId: string;
	sdp: string;
};
export type CallIceRelayMessage = {
	type: "call:ice";
	callId: string;
	candidate: IceCandidateInit;
};
export type ErrorMessage = { type: "error"; code: ErrorCode; message: string };

export type ServerMessage =
	| AuthenticatedMessage
	| CallIncomingMessage
	| CallAcceptedMessage
	| CallDeclinedMessage
	| CallEndedMessage
	| CallBusyMessage
	| CallTimeoutMessage
	| CallSdpRelayMessage
	| CallIceRelayMessage
	| ErrorMessage;
