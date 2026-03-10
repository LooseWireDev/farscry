// App entry point
// - Create Hono app instance
// - GET /health → { status: "ok", timestamp: ... }
// - WebSocket upgrade route at /ws using upgradeWebSocket from "hono/bun"
// - Port from Bun.env.PORT || 3000
// - Export { port, fetch: app.fetch, websocket } for Bun.serve()
