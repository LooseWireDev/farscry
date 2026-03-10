// ── API Error ───────────────────────────────────────────────────────────────

export type ApiErrorCode =
	| "unauthorized"
	| "forbidden"
	| "not_found"
	| "conflict"
	| "validation_error"
	| "subscription_required";

export type ApiError = {
	error: string;
	code: ApiErrorCode;
};

// ── User Profile ────────────────────────────────────────────────────────────

export type UserProfile = {
	id: string;
	displayName: string;
	friendCode: string;
	hasActiveSubscription: boolean;
};

// ── Contacts ────────────────────────────────────────────────────────────────

export type Contact = {
	userId: string;
	displayName: string;
	friendCode: string;
};

// ── Families ────────────────────────────────────────────────────────────────

export type Family = {
	id: string;
	ownerId: string;
	members: FamilyMember[];
};

export type FamilyMember = {
	userId: string;
	displayName: string;
	role: "owner" | "member";
};

export type FamilyInvitation = {
	id: string;
	familyId: string;
	inviterId: string;
	inviterName: string;
	status: "pending" | "accepted" | "declined";
};
