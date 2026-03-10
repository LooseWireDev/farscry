// Call state machine
// - Initiate: caller sends call:initiate → server sends call:incoming to callee
// - Accept: callee sends call:accept → server sends call:accepted to caller
// - Decline: callee sends call:decline → server sends call:declined to caller
// - Hangup: either party sends call:hangup → server sends call:ended to other
// - SDP/ICE relay: forward call:sdp and call:ice between peers
// - Busy: if callee is already in a call, respond with call:busy
// - Track active calls in-memory (dropped on server restart)
