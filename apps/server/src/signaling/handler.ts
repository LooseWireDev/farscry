// WebSocket connection management
// - Authenticate connection via session token on first message
// - Track connected users in-memory (Map<userId, WebSocket>)
// - Route incoming messages to appropriate handler (call actions, ICE/SDP relay)
// - Clean up on disconnect (end active calls, remove from connection map)
