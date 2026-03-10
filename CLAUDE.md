# CLAUDE.md

> Context file for AI-assisted development. This file describes the project, its architecture, conventions, and constraints so that AI tools can assist effectively without introducing inconsistencies.

## Project Overview

**Farscry** is a private, cross-platform video calling app. No chat, no bloat, just calls. It is self-hostable via Docker and also offered as a managed service at farscry.app.

- **Company:** Loose Wire LLC (loosewire.dev)
- **GitHub Org:** LooseWireDev
- **License:** AGPL-3.0
- **Engineer:** Solo developer (Gav) with AI assistance

## Engineering Philosophy

- **No black boxes.** Every line of code must be understood and defensible.
- **Architecture first.** The system was fully designed before implementation began.
- **Monolith over microservices.** One server, one database, minimal ops.
- **Self-hosting is a feature.** Every choice must work in both managed and self-hosted contexts.
- **Privacy by default.** No phone numbers, no user search, friend codes only.
- **AI assists, not leads.** AI helps with unfamiliar APIs, code review, and boilerplate — not core architecture decisions.

## Tech Stack

| Layer | Technology | Version |
|-------|-----------|---------|
| Mobile | React Native (Expo) with development builds | Expo SDK 54+ |
| Server | Hono on Node.js | Latest |
| Database | SQLite via Drizzle ORM (`bun:sqlite`) | Latest |
| Auth | Better Auth (email/password) | Latest |
| Payments | Polar (merchant of record) | — |
| Package Manager | Bun (workspaces) | 1.2+ |
| Linting/Formatting | Biome | Latest |
| Hosting | Fly.io (prod) + Docker Compose (self-hosted) | `oven/bun` base image |

## Monorepo Structure

```
farscry/
├── apps/
│   ├── mobile/                 # Expo React Native app (iOS + Android)
│   │   ├── app/                # Expo Router file-based routes
│   │   │   ├── _layout.tsx     # Root layout
│   │   │   ├── (auth)/         # Unauthenticated screens
│   │   │   └── (app)/          # Authenticated screens
│   │   ├── src/
│   │   │   ├── hooks/          # Custom React hooks
│   │   │   ├── services/       # WebRTC, signaling, push
│   │   │   ├── lib/            # Auth client, API client
│   │   │   └── components/     # Shared UI components
│   │   ├── app.config.ts
│   │   └── package.json
│   └── server/                 # Hono HTTP + WebSocket server
│       ├── src/
│       │   ├── index.ts        # App entry, mounts routes + WebSocket
│       │   ├── routes/         # HTTP route handlers
│       │   │   ├── auth.ts     # Better Auth mount
│       │   │   ├── contacts.ts # Contact CRUD, friend code lookup
│       │   │   ├── families.ts # Family management, invitations
│       │   │   └── users.ts    # Profile, friend code regeneration
│       │   ├── signaling/      # WebSocket handlers
│       │   │   ├── handler.ts  # Connection management, message routing
│       │   │   └── calls.ts    # Call state machine
│       │   ├── push/           # Push notification senders
│       │   │   ├── apns.ts     # Apple Push Notification Service
│       │   │   └── fcm.ts      # Firebase Cloud Messaging
│       │   └── lib/            # Shared server utilities
│       │       ├── auth.ts     # Better Auth instance
│       │       ├── access.ts   # Subscription access checks
│       │       └── id.ts       # nanoid generation
│       ├── Dockerfile
│       └── package.json
├── packages/
│   ├── db/                     # Drizzle schema, migrations, DB helpers
│   │   ├── src/
│   │   │   ├── schema.ts       # All table definitions
│   │   │   ├── relations.ts    # Drizzle relation definitions
│   │   │   └── index.ts        # createDb() with bun:sqlite
│   │   ├── drizzle.config.ts
│   │   └── package.json
│   └── shared/                 # Types shared between server + mobile
│       ├── src/
│       │   ├── messages.ts     # WebSocket message type definitions
│       │   ├── api.ts          # HTTP request/response types
│       │   └── constants.ts    # Shared constants
│       └── package.json
├── docker/
│   └── docker-compose.yml
├── biome.json
├── tsconfig.json               # Root tsconfig, all packages extend this
├── package.json                # Bun workspace root
└── bun.lock
```

## Bun — Runtime, Package Manager, Everything

Bun is the runtime everywhere — local dev AND production Docker. Not just the package manager.

### Use Bun's built-in APIs

- **`bun:sqlite`** for SQLite — not `better-sqlite3`. Drizzle has a native `drizzle-orm/bun-sqlite` adapter. No C++ compilation, no node-gyp, faster.
- **`Bun.serve()`** as the underlying HTTP/WebSocket server — Hono mounts on top of it. Hono is runtime-agnostic and adapts automatically.
- **`Bun.file()` / `Bun.write()`** for file system operations — not `node:fs`.
- **`bun test`** with `bun:test` for all server-side tests — not jest or vitest.
- **`Bun.$`** for shell commands in scripts — not `execa`.
- Bun auto-loads `.env` files — never install or import `dotenv`.

### CLI commands

- `bun install` — never npm/yarn/pnpm.
- `bun run <script>` — never npm run.
- `bunx <pkg>` — never npx.
- `bun --watch` for dev server hot reload.

### What NOT to use

- **`Bun.serve()` HTML imports** — This is a React Native project. No web frontend, no HTML entry points.
- **`Bun.redis` / `Bun.sql` (Postgres)** — We don't use Redis or Postgres. SQLite via Drizzle is the only database.
- **`ws` npm package** — Bun's WebSocket is built-in via `Bun.serve()`. Hono's WebSocket helper wraps it.
- **`better-sqlite3`** — Use `bun:sqlite` instead.
- **`express`** — Use Hono.
- **`dotenv`** — Bun loads `.env` automatically.

### Runtime contexts

- **Server (dev + production):** Bun runs TypeScript directly. No build step needed. `bun run src/index.ts` in dev, same in Docker.
- **Docker:** Uses `FROM oven/bun` base image. Source copied directly, no compilation.
- **Mobile:** Metro bundler (via Expo) handles the React Native build. Bun is the package manager here only — Metro is the bundler/runtime.

## Code Conventions

### TypeScript
- **Strict mode** everywhere. No `any` unless absolutely unavoidable (and documented why).
- Functional components with hooks for React/React Native.
- Prefer explicit types over inference for function signatures and exports.
- Use `type` over `interface` unless extending is needed.

### Formatting (Biome)
- Tabs for indentation.
- Double quotes.
- Semicolons always.
- Line width: 100 characters.
- Run `bun run lint:fix` before committing.

### Naming
- Files: `kebab-case.ts`
- Components: `PascalCase.tsx`
- Functions/variables: `camelCase`
- Constants: `UPPER_SNAKE_CASE`
- Database tables: `snake_case` (Drizzle convention)
- Package names: `@farscry/` scope (e.g., `@farscry/db`, `@farscry/shared`)

### Imports
- Use `workspace:*` for internal package dependencies.
- Import from package names, not relative paths across package boundaries:
  ```typescript
  // Good
  import { schema } from "@farscry/db";
  import type { ClientMessage } from "@farscry/shared";

  // Bad — never cross package boundaries with relative imports
  import { schema } from "../../packages/db/src/schema";
  ```

## Architecture Decisions

### Server: Single Process, Four Responsibilities
The server is a monolith. One Hono process handles:
1. **Auth (HTTP)** — Better Auth middleware
2. **API (HTTP)** — Contacts, families, users, subscriptions
3. **Signaling (WebSocket)** — SDP/ICE relay for WebRTC call setup
4. **Push (triggered by signaling)** — APNs VoIP + FCM data messages

Do NOT split these into separate services or processes.

### Database: SQLite + Drizzle (bun:sqlite)
- `bun:sqlite` is the driver — built into Bun, no native compilation needed.
- Drizzle adapter: `drizzle-orm/bun-sqlite`.
- SQLite with WAL mode for concurrent reads.
- `foreign_keys = ON` and `busy_timeout = 5000` set per connection.
- All schema changes go through Drizzle migrations (`bunx drizzle-kit generate`).
- The schema is in `packages/db/src/schema.ts` — this is the source of truth.
- Better Auth has its own tables (user, session, account, verification) that coexist with ours.
- Our `userProfiles` table links to Better Auth's `user` table via `userId`.

### Identity & Contact Discovery
- **Primary identifier:** Email (via Better Auth)
- **Public identifier:** Friend code (6 uppercase alphanumeric, no 0/O/1/I)
- No phone numbers in v1.
- No user search by email or name. Privacy by design.
- Friend codes shared out-of-band → mutual contact created on entry.

### Signaling Protocol
All WebSocket messages are JSON with a `type` discriminator field. Types are defined in `packages/shared/src/messages.ts`.

**Client → Server:** `authenticate`, `call:initiate`, `call:accept`, `call:decline`, `call:hangup`, `call:sdp`, `call:ice`

**Server → Client:** `authenticated`, `call:incoming`, `call:accepted`, `call:declined`, `call:ended`, `call:sdp`, `call:ice`, `call:busy`, `error`

Signaling state (connections, active calls) is **in-memory only**. Server restart drops active calls.

### WebRTC
- STUN: Cloudflare free STUN server for v1.
- TURN: Not bundled. Self-hosters set up their own if needed.
- Media: DTLS-SRTP (E2E encrypted by WebRTC spec).
- Speakerphone is default audio route (this is video calling, not phone calling).

### Auth Flow (Mobile → Server)
- Better Auth with email/password strategy.
- Sessions stored in SQLite.
- Mobile stores session token in `expo-secure-store`.
- If cookies don't work in React Native, use Bearer token auth instead.
- 30-day session expiry, daily refresh.

### Push Notifications
- **iOS:** APNs VoIP push → CallKit native incoming call UI
- **Android (Google Play):** FCM high-priority data message → ConnectionService
- **Android (F-Droid):** Persistent WebSocket foreground service (no Google dependencies)
- Self-hosters without push credentials → WebSocket-only fallback

### Subscriptions & Access
- Polar is merchant of record (handles global VAT/sales tax).
- Subscriptions sold via web (farscry.app) — no in-app purchase.
- Access check: user has active subscription OR user is in a family whose owner has active family subscription.
- Self-hosted mode: no Polar config → access checks bypassed (everyone has access).

## Database Schema

Tables (defined in `packages/db/src/schema.ts`):

| Table | Purpose |
|-------|---------|
| `user` | Better Auth managed — email, password hash, sessions |
| `session` | Better Auth managed — active sessions |
| `account` | Better Auth managed — auth providers |
| `verification` | Better Auth managed — email verification |
| `userProfiles` | App-specific: displayName, friendCode, publicKey |
| `families` | Family groups (owner pays) |
| `familyMembers` | Who belongs to which family (one family per user) |
| `familyInvitations` | Pending/accepted/declined family invitations |
| `subscriptions` | Polar subscription state (source of truth for access) |
| `contacts` | Mutual contact pairs (userId ↔ contactUserId) |
| `pushTokens` | Device push tokens (APNs, FCM, or WebSocket) |
| `callHistory` | Call metadata: who, when, how long, status |

## Testing

Use `bun:test` for all server-side tests. Tests live next to the code they test.

```typescript
import { test, expect, describe } from "bun:test";

describe("friend code", () => {
  test("generates 6 character code", () => {
    const code = generateFriendCode();
    expect(code).toHaveLength(6);
    expect(code).toMatch(/^[A-Z2-9]+$/);
  });
});
```

- Test files: `*.test.ts` next to source files
- No separate test directories — colocate tests with the code they test
- Mobile tests use Expo's testing setup (if needed)

## Common Tasks

```bash
# Install all dependencies
bun install

# Start server in dev mode
cd apps/server && bun run dev

# Start mobile app
cd apps/mobile && bun start

# Run tests
bun test

# Run tests for a specific package
cd apps/server && bun test

# Run linter
bun run lint

# Auto-fix lint issues
bun run lint:fix

# Type check all packages
bun run typecheck

# Generate Drizzle migration after schema change
cd packages/db && bunx drizzle-kit generate

# Open Drizzle Studio (visual DB browser)
cd packages/db && bunx drizzle-kit studio

# Build Docker image
docker build -f apps/server/Dockerfile -t farscry .

# Run with Docker Compose
docker compose -f docker/docker-compose.yml up
```

## What NOT to Do

- **Don't add a chat feature.** Farscry is calls only. This is a product decision, not a limitation.
- **Don't add microservices.** The monolith is intentional.
- **Don't use Supabase, Firebase, or any managed backend.** The server must be fully self-hostable with zero external dependencies.
- **Don't use in-app purchases.** Subscriptions go through Polar via web to avoid platform fees.
- **Don't add phone number verification.** Friend codes are the identity mechanism.
- **Don't store call content.** No recordings, no transcripts, no media on the server. Ever.
- **Don't use Turborepo or Nx.** Bun workspaces handle everything we need.
- **Don't use ESLint/Prettier.** Biome is the single linting/formatting tool.
- **Don't install dotenv.** Bun auto-loads `.env` files.
- **Don't use `better-sqlite3`, `express`, `ws`, or `node:fs`.** Use Bun-native equivalents. See "Bun" section.
- **Don't add a Node.js build step.** Bun runs TypeScript directly. No `tsc`, no `tsup`, no bundler for the server.

## Links

- **Architecture Doc:** See Linear project → Farscry → Documents
- **Issue Tracker:** Linear (workspace: loosewiredev, project: Farscry)
- **GitHub:** github.com/LooseWireDev/farscry
- **Production:** farscry.app
- **Company:** loosewire.dev
