# WebSocket Signaling Design

## Connection Layer

**Authentication:** Session token passed as `?token=xxx` query parameter on WebSocket URL. Validated at upgrade time using Better Auth session lookup. Bad token = connection rejected with HTTP 401 before upgrade completes.

**Connection tracking:** In-memory `Map<userId, WebSocket>`. One connection per user. If a user connects while already connected, the old connection is closed with close code 4001 ("replaced") and the new one takes over.

**Lifecycle:**

1. Client opens `ws://host/ws?token=sessionToken`
2. Server validates token, extracts userId, stores in connection map
3. Server sends `{ type: "authenticated", userId }` to confirm
4. Client sends typed JSON messages, server routes them
5. On disconnect: remove from map, clean up any active call

## Call State Machine

**Active calls:** In-memory `Map<callId, { callerId, calleeId, status }>`. Lost on server restart.

**Flow:**

| Step | Client sends | Server does | Server sends |
|------|-------------|-------------|-------------|
| 1. Initiate | `call:initiate { calleeId }` | Check callee online + not busy. Generate callId. | `call:incoming { callId, callerId }` to callee |
| 2a. Accept | `call:accept { callId }` | Mark call active | `call:accepted { callId }` to caller |
| 2b. Decline | `call:decline { callId }` | Remove call | `call:declined { callId }` to caller |
| 2c. Busy | — | Callee already in active call | `call:busy { callId }` to caller |
| 2d. Offline | — | No WebSocket for callee | Send push notification, start 30s timeout |
| 2e. Timeout | — | 30s elapsed, callee never connected | `call:timeout { callId }` to caller, clean up |
| 3. Media | `call:sdp { callId, sdp }` | Relay to peer | `call:sdp { callId, sdp }` to other party |
| 3. Media | `call:ice { callId, candidate }` | Relay to peer | `call:ice { callId, candidate }` to other party |
| 4. Hangup | `call:hangup { callId }` | Remove call | `call:ended { callId }` to other party |

**Edge cases:**

- Caller disconnects while ringing: server sends `call:ended` to callee, cleans up
- Callee disconnects during active call: server sends `call:ended` to caller, cleans up
- Invalid callId or unauthorized action: `error` message back to sender

**Errors:** Malformed or unknown messages get `{ type: "error", message: "reason" }` back. Connection stays open.

## Message Types & File Structure

**Shared types** (`packages/shared/src/messages.ts`): All WebSocket messages defined as discriminated unions with `type` field. Two unions — `ClientMessage` (client to server) and `ServerMessage` (server to client). Single source of truth for both server and mobile.

**Server files:**

- `signaling/handler.ts` — Connection map, auth at upgrade, message routing, disconnect cleanup
- `signaling/calls.ts` — Call map, state transitions, timeout logic. Exports pure functions that handler calls (e.g., `initiateCall`, `acceptCall`, `relayMedia`). No direct WebSocket access — handler passes the relevant sockets in.

**Separation rationale:** Handler owns WebSocket concerns (connections, routing, sending). Calls owns business logic (state machine, validation). Calls is testable without WebSocket mocks.
