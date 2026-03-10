// App entry point
// - Create Hono app instance
// - GET /health → { status: "ok", timestamp: ... }
// - WebSocket upgrade route at /ws using upgradeWebSocket from "hono/bun"
// - Port from Bun.env.PORT || 3000
// - Export { port, fetch: app.fetch, websocket } for Bun.serve()

import { Hono } from "hono";
import { upgradeWebSocket, websocket } from "hono/bun";
import { env } from "./env";

const app = new Hono();

const port = env.PORT || 3000;

// Health check
app.get("/health", (c) => {
	return c.json({ status: "ok", timestamp: new Date().toISOString() });
});

// WebSocket signaling endpoint
app.get(
	"/ws",
	upgradeWebSocket(() => {
		return {
			onOpen(_event, ws) {
				console.log("WebSocket connected");
			},
			onMessage(event, ws) {
				console.log(`WebSocket message: ${event.data}`);
			},
			onClose() {
				console.log("WebSocket disconnected");
			},
		};
	}),
);

console.log(`Farscry server listening on port ${port}`);

export default {
	port,
	fetch: app.fetch,
	websocket,
};
