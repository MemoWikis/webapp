# Content Editor System - Overview

**Last Updated:** January 4, 2026

## 📋 Purpose

This document provides a high-level overview of the content editor system. For detailed information on specific topics, see the linked documents below.

## 🎯 Core Components

### ContentEditor.vue
📍 `src/Frontend.Nuxt/components/page/content/ContentEditor.vue`

**Purpose:** Rich-text editor with real-time collaboration support

**Key Technologies:**
- **TipTap** - Rich text editor framework (Vue 3)
- **Y.js** - CRDT for conflict-free data synchronization
- **HocuspocusProvider** - WebSocket provider for real-time collaboration
- **IndexedDB** - Offline persistence via `y-indexeddb`

**Main Features:**
- ✅ Real-time collaborative editing
- ✅ Offline editing capability
- ✅ Hash-based content versioning
- ✅ Server-wins conflict handling (prevents stale IndexedDB overwrites)
- ✅ Auto-save (3s debounce)
- ✅ Image upload & management
- ✅ Syntax highlighting for code
- ✅ Custom heading IDs for deep linking

## 📚 Detailed Documentation

### Core Docs
- **[Collaboration System](./editor-collaboration-system.md)** - Real-time editing, WebSocket events, reconnection
- **[Conflict Resolution](./editor-conflict-resolution.md)** - Versioning, hashing, server-wins behavior
- **[Known Issues & Solutions](./editor-issues.md)** - Hydration mismatches, stale cache, race conditions

## 🚀 Quick Start

### Using the Editor

```vue
<template>
    <ClientOnly>
        <PageTabsContent v-if="!pageStore.textIsHidden" />
        <template #fallback>
            <div id="PageContent">
                <div class="ProseMirror content-placeholder" v-html="pageStore.content" />
            </div>
        </template>
    </ClientOnly>
</template>
```

### State Management

```typescript
// pageStore.ts
const setPage = (page: Page) => {
    content.value = page.content
    initialContent.value = page.content  // Server content
}

const saveContent = async () => {
    await $api('/apiVue/PageStore/SaveContent', {
        method: 'POST',
        body: {
            id: id.value,
            content: content.value,
        }
    })
}
```

## 🔄 Architecture Flow

```
Server (MySQL)
    ↓ [SSR/Hydration]
pageStore.initialContent
    ↓ [onMounted]
ContentEditor
    ↓ [initProvider]
HocuspocusProvider + IndexedDB
    ↓ [onSynced]
Conflict Resolution (server wins)
    ↓ [Editor Ready]
User Editing + Auto-Save
```

## 📁 File Structure

```
src/Frontend.Nuxt/
└── components/
    └── page/
        ├── content/
        │   ├── ContentEditor.vue         [Main Editor]
        │   ├── TabsContent.vue           [Tab Wrapper]
        │   └── grid/Grid.vue             [Child Pages]
        ├── pageStore.ts                  [State Management]
        └── sidebar/outlineStore.ts       [Heading Outline]
```

## 🎛️ Configuration

### Conflict Resolution

If collaboration/IndexedDB content differs from server content, the editor applies **server content** (server-wins) to prevent stale caches from overwriting fresh data.

### Auto-Save
```typescript
// Debounced auto-save every 3 seconds
watch(() => pageStore.content, debounce(() => {
    if (pageStore.contentHasChanged) {
        pageStore.saveContent()
    }
}, 3000))
```

### Provider Timeout
```typescript
// 5-second timeout for provider connection
// Falls back to offline mode if connection fails
providerTimeout.value = setTimeout(() => {
    if (!providerLoaded.value) {
        // Fallback to non-collaborative mode
    }
}, 5000)
```

## 🔐 Security

- **Collaboration Tokens** - User authentication for WebSocket
- **Share Tokens** - Optional access tokens for shared pages
- **Content Validation** - Server-side HTML sanitization
- **Image Upload** - File type validation, size limits, orphan cleanup

## 📊 Key Metrics

- **Sync Time** - Time to sync after load (~500ms typical)
- **Auto-Save Frequency** - Every 3 seconds (debounced)
- **Provider Timeout** - 5 seconds before fallback
- **Content Hash** - Simple 32-bit integer hash
- **Conflict Detection** - Hash comparison on sync

## 🔗 Related Documentation

- [AI Token Usage System](./ai-token-usage-system.md)
- [Collaboration System Details](./editor-collaboration-system.md)
- [Conflict Resolution](./editor-conflict-resolution.md)

## 📝 Glossary

- **CRDT** - Conflict-free Replicated Data Type (Y.js)
- **Y.Doc** - Y.js document structure
- **HocuspocusProvider** - WebSocket collaboration provider
- **IndexedDB** - Browser storage for offline editing
- **Content Hash** - 32-bit integer hash for version comparison
- **Collaboration Token** - WebSocket authentication token
- **Share Token** - Optional token for shared page access
