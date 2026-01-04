# Common code style guide for all languages

- Please always write code comments in English.
- Always spell out variable names; no abbreviations.
- After if statements and loops, never use single line statements.

# C#

- Do not use namespaces in C#.
- Prefer Verify() for Tests
- Use \_testHarness.ApiCall("apiVue/{controller}/{action}") to do apicalls, do not init/resolve controllers in tests

# Unit Tests

- For API calls use Testharness.ApiCall(..)

# Glossar

- `page`: a page
- `childpage`: a page that is a child of page or wiki
- `subpage`: subpage is a synonym for childpage and only used in user interface
- `wiki`: is a special kind of a page
- `orphaned page`: a page that does not have a parent page or wiki
- `wishknowledge`: knowledge that a user specifically wants to learn or has marked as desired to learn
- `wuwi`: abbreviation for "Wunschwissen" (wishknowledge), can be used in backend code, comments, and non-user-facing texts

# Frontend and Translations

- refer to the .copilotinstructions file in `../src/Frontend.Nuxt` for frontend specific instructions

# AI Token Usage System

Core components in `src/Backend.Core/Domain/AI/`:

- `Usage/TokenDeductionService.cs` - Token balance management, affordability checks, deduction with model-aware cost multipliers
- `Usage/AiUsageLogRepo.cs` - Usage logging with cost analytics, supports queries by model/date/user
- `Models/AiModelRegistry.cs` - Model whitelist & token cost multipliers, caching

Token types: `SubscriptionTokensBalance` (monthly, resets) and `PaidTokensBalance` (accumulates)
Deduction priority: Subscription tokens first, then paid tokens

For detailed documentation including integration points, database schema, and token flow examples, see `docs/ai-token-usage-system.md`

# Content Editor & Collaboration System

Core components in `src/Frontend.Nuxt/components/page/content/`:

- `ContentEditor.vue` - TipTap rich-text editor with real-time collaboration
- `pageStore.ts` - State management, content persistence, auto-save (3s debounce)
- Real-time collaboration via HocuspocusProvider & Y.js CRDT
- Offline editing with IndexedDB persistence
- Hash-based content versioning with configurable conflict resolution

Key Technologies: TipTap (editor), Y.js (CRDT), HocuspocusProvider (WebSocket), IndexedDB (offline cache)

Conflict Strategies: ServerWins (default), ClientWins, Timestamp, UserChoice (UI pending)

**📚 Documentation:**

- **[Editor System Overview](../docs/editor-system-overview.md)** - Architecture, quick start, file structure
- **[Collaboration System](../docs/editor-collaboration-system.md)** - Real-time editing, WebSocket events, reconnection
- **[Conflict Resolution](../docs/editor-conflict-resolution.md)** - Versioning, strategies, hash-based comparison
- **[Known Issues & Solutions](../docs/editor-issues.md)** - Hydration mismatches, stale cache, debugging tips

For optimal LLM performance, documentation is split into focused files. Start with the overview, then dive into specific topics as needed.

# Skills

Skills are domain-specific automation workflows that help with common development tasks.

**Important:** All skills, documentation, and descriptions must be written in **English**.

## App Management Skills

- **app-start** (aliases: start-app, startup, start): Starts Backend (port 5069) and Frontend (port 3000) in foreground terminals. Checks if services are already running before starting them.

## Database Skills

- **dev-database-create**: Create a fresh dev database with latest test data (runs ScenarioBuilder test, generates schema.sql, reinitializes MySQL)
- **dev-database-reset**: Reset the dev database from existing schema.sql (just reinitializes MySQL without updating schema.sql)
