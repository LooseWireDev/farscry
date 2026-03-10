<p align="center">
  <img src=".github/assets/farscry-logo.svg" alt="Farscry" width="80" />
</p>

<h1 align="center">Farscry</h1>

<p align="center">
  <strong>Private video calling. No chat. No bloat. Just calls.</strong>
</p>

<p align="center">
  <a href="https://github.com/LooseWireDev/farscry/blob/main/LICENSE"><img alt="License: AGPL-3.0" src="https://img.shields.io/badge/license-AGPL--3.0-blue.svg" /></a>
  <a href="https://ghcr.io/loosewiredev/farscry"><img alt="Docker" src="https://img.shields.io/badge/docker-ghcr.io-blue.svg" /></a>
  <a href="https://github.com/LooseWireDev/farscry/stargazers"><img alt="GitHub Stars" src="https://img.shields.io/github/stars/LooseWireDev/farscry?style=flat" /></a>
</p>

<p align="center">
  <a href="#quick-start">Quick Start</a> •
  <a href="#features">Features</a> •
  <a href="#self-hosting">Self-Hosting</a> •
  <a href="#how-it-works">How It Works</a> •
  <a href="#development">Development</a> •
  <a href="#support">Support</a>
</p>

---

## Quick Start

```bash
docker compose up
```

That's it. One container, one SQLite file, no external dependencies.

## What Is Farscry?

Farscry is a self-hostable, end-to-end encrypted video calling app for iOS and Android. It does one thing and does it well — private video calls between people you know.

No chat. No stories. No feed. No read receipts. Just tap a name and call.

## Why Farscry?

| Alternative | Problem |
|-------------|---------|
| Jitsi | Heavy, conference-focused, complex to self-host |
| Signal | Can't be self-hosted |
| Linphone | SIP-based, ugly, complex |
| Element/Matrix | Chat-first with calling bolted on |
| FaceTime | Apple-only, not self-hostable |
| **Farscry** | **Just calls. One container. Done.** |

## Features

- **End-to-end encrypted** — Video and audio are encrypted via WebRTC (DTLS-SRTP). The server never sees or hears your calls.
- **Self-hostable** — One Docker container + a SQLite file. Run it on your home server, NAS, or VPS.
- **No phone number required** — Sign up with email. Share a 6-character friend code to connect.
- **Native call integration** — Incoming calls use your phone's native call screen (CallKit on iOS, ConnectionService on Android).
- **Cross-platform** — iOS and Android from a single codebase.
- **Works without Google** — F-Droid / Obtainium builds have zero Google dependencies.
- **Family plans** — One subscription covers up to 5 family members.
- **Open source** — AGPL-3.0 licensed. Read every line, modify anything, host it yourself.

## Self-Hosting

### Docker Compose (recommended)

```yaml
# docker-compose.yml
services:
  farscry:
    image: ghcr.io/loosewiredev/farscry:latest
    ports:
      - "3000:3000"
    volumes:
      - farscry-data:/data
    environment:
      - DATABASE_PATH=/data/farscry.db
      - BETTER_AUTH_SECRET=generate-a-random-secret-here
    restart: unless-stopped

volumes:
  farscry-data:
```

```bash
docker compose up -d
```

### Environment Variables

| Variable | Required | Description |
|----------|----------|-------------|
| `DATABASE_PATH` | Yes | Path to SQLite database file |
| `BETTER_AUTH_SECRET` | Yes | Secret key for session signing (generate a random string) |
| `PORT` | No | Server port (default: 3000) |
| `APNS_KEY_ID` | No | Apple Push Notification Service key ID |
| `APNS_TEAM_ID` | No | Apple Developer Team ID |
| `APNS_KEY_PATH` | No | Path to APNs `.p8` key file |
| `FCM_SERVICE_ACCOUNT_KEY` | No | Firebase Cloud Messaging service account JSON |

> **Note:** Push notification credentials are optional for self-hosters. Without them, incoming calls work via a persistent WebSocket connection (requires the app to maintain a foreground service on Android).

### Unraid

Farscry is available in the Unraid Community Applications store. Search "Farscry" and click install.

### Reverse Proxy

Farscry uses both HTTP and WebSocket connections on the same port. Your reverse proxy must support WebSocket upgrades.

**Nginx:**
```nginx
location / {
    proxy_pass http://localhost:3000;
    proxy_http_version 1.1;
    proxy_set_header Upgrade $http_upgrade;
    proxy_set_header Connection "upgrade";
    proxy_set_header Host $host;
    proxy_set_header X-Real-IP $remote_addr;
}
```

### TURN Servers

Most calls connect directly between devices using STUN (which Farscry includes by default). If you're behind a restrictive NAT, you may need a TURN relay server. Farscry does not bundle a TURN server — you can set up [coturn](https://github.com/coturn/coturn) or use a hosted TURN service.

## How It Works

```
┌──────────┐         ┌──────────────┐         ┌──────────┐
│  Phone A │◄───────►│ Farscry      │◄───────►│  Phone B │
│          │  signal  │ Server       │  signal  │          │
└────┬─────┘         │              │         └─────┬────┘
     │               │ • Auth       │               │
     │               │ • Contacts   │               │
     │               │ • Signaling  │               │
     │               │ • Push       │               │
     │               └──────────────┘               │
     │                                              │
     │◄────────────────────────────────────────────►│
     │           Direct P2P video/audio             │
     │          (E2E encrypted via WebRTC)          │
```

1. **You share your friend code** — a 6-character code like `A7K9X2`
2. **They enter it in the app** — mutual contact created instantly
3. **Tap their name to call** — the server relays the connection handshake
4. **Video flows directly between devices** — the server steps aside

The server handles authentication, contacts, and call signaling. It **never** touches your video or audio.

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Mobile | React Native (Expo) |
| Server | Hono on Node.js |
| Database | SQLite via Drizzle ORM (`bun:sqlite`) |
| Auth | Better Auth |
| Payments | Polar (managed hosting only) |
| Monorepo | Bun workspaces |

## Privacy Model

- **Video/audio:** End-to-end encrypted via WebRTC DTLS-SRTP. The server cannot see or hear calls.
- **Contacts:** Stored on the server as mutual friend-code-based connections. No phone number required.
- **Discovery:** No user search. No directory. Friend codes shared out-of-band.
- **Self-hosters:** You own all your data. The server stores it in a single SQLite file on your machine.

## Managed Hosting

Don't want to self-host? Use the managed server at [farscry.app](https://farscry.app).

| Plan | Monthly | Yearly |
|------|---------|--------|
| Individual | $3/mo | $30/yr |
| Family (up to 5) | $6/mo | $60/yr |

Subscriptions managed via web — no in-app purchase, no App Store markup.

## Distribution

| Platform | Download |
|----------|----------|
| iOS | [App Store](https://apps.apple.com/app/farscry) |
| Android | [Google Play](https://play.google.com/store/apps/details?id=dev.loosewire.farscry) |
| Android (no Google) | [F-Droid](https://f-droid.org) / [Obtainium](https://github.com/ImranR98/Obtainium) |
| Self-hosted | `docker compose up` |

## Development

Farscry is a Bun workspace monorepo.

```bash
# Clone
git clone https://github.com/LooseWireDev/farscry.git
cd farscry

# Install dependencies
bun install

# Start the server (with hot reload)
cd apps/server && bun run dev

# Start the mobile app
cd apps/mobile && bun start
```

See [CONTRIBUTING.md](CONTRIBUTING.md) for the full development setup guide.

### Project Structure

```
farscry/
├── apps/
│   ├── mobile/          # React Native (Expo) app
│   └── server/          # Hono server
├── packages/
│   ├── db/              # Drizzle schema + migrations
│   └── shared/          # Shared TypeScript types
├── docker/
│   └── docker-compose.yml
├── LICENSE              # AGPL-3.0
└── README.md
```

## Support

Farscry is built and maintained by [Loose Wire LLC](https://loosewire.dev).

- **GitHub Sponsors** — [Sponsor on GitHub](https://github.com/sponsors/LooseWireDev)
- **Ko-fi** — [Buy me a coffee](https://ko-fi.com/loosewire)
- **Bug reports** — [GitHub Issues](https://github.com/LooseWireDev/farscry/issues)

## License

[AGPL-3.0](LICENSE) — You can self-host, modify, and distribute Farscry. If you distribute a modified version, you must share your changes under the same license.
