// Firebase Cloud Messaging
// - Send high-priority data message for incoming calls → triggers ConnectionService on Android
// - Uses FCM HTTP v1 API with service account credentials
// - Only used when callee is offline (no active WebSocket)
// - Not used for F-Droid builds (persistent WebSocket fallback instead)
