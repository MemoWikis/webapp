# Real-Time Collaboration System

**Last Updated:** January 4, 2026  
**Parent:** [Editor System Overview](./editor-system-overview.md)

## 🎯 Purpose

Enables multiple users to edit the same page simultaneously using Y.js CRDT and HocuspocusProvider WebSocket connection.

## 🔄 Collaboration Flow

### Initial Connection

```typescript
onMounted(() => {
    recreate()  // Initialize editor + provider
})

const recreate = (login: boolean = false) => {
    provider.value?.destroy()
    editor.value?.destroy()
    
    doc = new Y.Doc()  // Fresh CRDT document
    
    if (userStore.isLoggedIn && loadCollab.value) {
        initProvider()  // Connect to WebSocket
    } else {
        providerLoaded.value = true  // Skip collaboration
    }
    
    initEditor()  // Initialize TipTap
}
```

### Provider Initialization

```typescript
const initProvider = () => {
    // 5-second timeout for connection
    providerTimeout.value = setTimeout(() => {
        if (!providerLoaded.value) {
            // Fallback to offline mode
            providerLoaded.value = true
            loadCollab.value = false
            snackbarStore.showSnackbar({
                type: 'warning',
                text: { message: t('error.collaboration.timeout') }
            })
        }
    }, 5000)
    
    const token = pageStore.shareToken
        ? `${userStore.collaborationToken}|accessToken=${pageStore.shareToken}`
        : userStore.collaborationToken
    
    provider.value = new HocuspocusProvider({
        url: config.public.hocuspocusWebsocketUrl,
        name: `ydoc-${pageStore.id}`,
        token: token,
        document: doc,
        onAuthenticated() { ... },
        onSynced() { ... },
        onClose() { ... }
    })
}
```

## 🔗 WebSocket Events

### onAuthenticated()
**Triggered:** After successful WebSocket authentication

```typescript
onAuthenticated() {
    const persistence = new IndexeddbPersistence(
        `${userStore.id}|document-${pageStore.id}`,
        doc
    )
}
```

**Purpose:**
- Initialize IndexedDB for offline caching
- Link Y.Doc to browser storage
- Enable offline editing

### onSynced()
**Triggered:** When Y.Doc is synced with server

```typescript
onSynced(_e) {
    // Clear connection timeout
    if (providerTimeout.value) {
        clearTimeout(providerTimeout.value)
    }
    
    const yDocContent = editor.value?.getHTML() || ''
    const serverContent = pageStore.initialContent || ''
    
    // Hash-based comparison
    const yDocHash = hashContent(yDocContent)
    const serverHash = hashContent(serverContent)

    // Conflict resolution (server wins)
    if (yDocHash !== serverHash && editor.value && serverContent) {
        editor.value.commands.setContent(serverContent)

        // Store current server version into Y.Doc config
        doc.getMap('config').set('version', {
            hash: serverHash,
            timestamp: Date.now(),
            userId: userStore.id,
            contentLength: serverContent.length
        })
    }
    
    providerContentLoaded.value = true
}
```

**Purpose:**
- Compare server vs cached content
- Resolve conflicts using strategy
- Mark content as loaded

### onClose()
**Triggered:** When WebSocket connection closes

```typescript
onClose(c) {
    isSynced.value = false
    
    if (c.event.code === 1006 || c.event.code === 1005 || !providerLoaded.value) {
        providerLoaded.value = true
        handleConnectionLost()
        
        if (!providerContentLoaded.value) {
            loadCollab.value = false
            recreate()  // Restart without collaboration
        }
    }
}
```

**Purpose:**
- Detect network issues
- Trigger reconnection
- Fallback to offline mode if needed

## 🔄 Reconnection Logic

```typescript
const handleConnectionLost = () => {
    if (connectionLostHandled.value) return
    
    snackbarStore.showSnackbar({
        type: 'error',
        text: { message: t('error.collaboration.connectionLost') },
        duration: 8000
    })
    
    reconnectTimer.value = setTimeout(() => {
        tryReconnect()
    }, 10000)  // Wait 10 seconds before retry
}

const tryReconnect = () => {
    if (reconnectTimer.value) clearTimeout(reconnectTimer.value)
    if (isReconnecting.value || isSynced.value) return
    
    isReconnecting.value = true
    
    if (provider.value) provider.value?.destroy()
    if (userStore.isLoggedIn && !isSynced.value) {
        initProvider()  // Reconnect
    }
    
    isReconnecting.value = false
}
```

## 👥 Collaborative Cursors

```typescript
CollaborationCursor.configure({
    provider: provider.value,
    user: {
        name: userStore.userName,
        color: getRandomColor(),
    },
    render: (user) => {
        // Custom cursor rendering
        const cursor = document.createElement('span')
        cursor.classList.add('collaboration-cursor__caret')
        cursor.setAttribute('style', `border-color: ${user.color}`)
        
        const label = document.createElement('div')
        label.classList.add('collaboration-cursor__label')
        label.insertBefore(document.createTextNode(user.name), null)
        label.setAttribute('style', `background-color: ${user.color}`)
        
        const labelContainer = document.createElement('div')
        labelContainer.classList.add('collaboration-cursor__label-container')
        labelContainer.insertBefore(label, null)
        
        cursor.insertBefore(labelContainer, null)
        return cursor
    },
    selectionRender: (user) => {
        return {
            nodeName: 'span',
            class: 'collaboration-cursor__selection',
            style: `background-color: ${user.color}33`,
            'data-user': user.name,
        }
    }
})
```

## 🔐 Authentication

### Collaboration Token
```typescript
// Generated on login
const collaborationToken = nanoid(32)

// Stored in user session
userStore.collaborationToken = collaborationToken

// Used for WebSocket authentication
provider.value = new HocuspocusProvider({
    token: collaborationToken,
    ...
})
```

### Share Token (Optional)
```typescript
// For shared pages with access control
const token = pageStore.shareToken
    ? `${userStore.collaborationToken}|accessToken=${pageStore.shareToken}`
    : userStore.collaborationToken
```

## 📊 Performance

### Connection Time
- **Typical:** 200-500ms
- **Timeout:** 5000ms (fallback to offline)
- **Retry:** 10 seconds after disconnect

### Data Sync
- **Initial Sync:** Y.Doc + IndexedDB (~100-300ms)
- **Live Updates:** Real-time via WebSocket
- **Conflict Resolution:** <10ms (hash comparison)

## 🧪 Testing Scenarios

### Scenario 1: Concurrent Editing
```
1. User A opens page → Editor loads
2. User B opens same page → Both connected
3. User A types → User B sees cursor + changes in real-time
4. User B types → User A sees cursor + changes in real-time
5. No conflicts (CRDT automatic merge)
```

### Scenario 2: Network Interruption
```
1. User edits page → Content in IndexedDB
2. Network disconnects → onClose() triggered
3. User continues editing → IndexedDB updates
4. Network reconnects → tryReconnect()
5. Content syncs to server → No data loss
```

### Scenario 3: Provider Timeout
```
1. User opens page → initProvider()
2. Network very slow → 5 seconds pass
3. Timeout triggered → Fallback to offline mode
4. Editor loaded → User can edit (without collaboration)
5. Network available → Save works normally
```

## 🔗 Related Documentation

- [Editor System Overview](./editor-system-overview.md)
- [Conflict Resolution](./editor-conflict-resolution.md)
- [Known Issues](./editor-issues.md)
