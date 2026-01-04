# Known Issues & Solutions

**Last Updated:** January 4, 2026  
**Parent:** [Editor System Overview](./editor-system-overview.md)

## 🐛 Hydration Mismatches

### Issue: Dynamic Icons Cause Hydration Errors

**Symptoms:**
```
[Vue warn]: Hydration node mismatch:
- rendered on server: <!--v-if-->
- expected on client: <svg>
```

**Root Cause:**
Cookie-based state (e.g., `showSideSheet`) differs between SSR and client hydration.

**Solution:**
Wrap dynamic components in `<ClientOnly>`

```vue
<template>
    <ClientOnly>
        <font-awesome-icon :icon="showSideSheet ? faChevronLeft : faChevronRight" />
        <template #fallback>
            <!-- Optional loading state -->
            <span>...</span>
        </template>
    </ClientOnly>
</template>
```

**Files Affected:**
- [components/header/HeaderMain.vue](../src/Frontend.Nuxt/components/header/HeaderMain.vue)

---

## 💾 Stale IndexedDB Cache

### Issue: Content Resets After Page Load

**Symptoms:**
1. Page loads with correct server content
2. Brief flash of correct content
3. Content resets to old cached version

**Root Cause:**
IndexedDB `onSynced()` event fires after server content is loaded, overwriting with stale cache.

**Solution:**
Hash-based content comparison with conflict resolution

```typescript
onSynced(_e) {
    const yDocContent = editor.value?.getHTML() || ''
    const serverContent = pageStore.initialContent || ''
    
    const yDocHash = hashContent(yDocContent)
    const serverHash = hashContent(serverContent)
    
    if (yDocHash !== serverHash) {
        // Conflict resolution (server wins)
        editor.value.commands.setContent(serverContent)
    }
}
```

**Files Affected:**
- [components/page/content/ContentEditor.vue](../src/Frontend.Nuxt/components/page/content/ContentEditor.vue)

**Implemented:** January 2026 (v2.0)

---

## ⚡ Race Conditions

### Issue: Grid Items Not Updating

**Symptoms:**
Child pages grid shows outdated data or empty

**Root Cause:**
SSR loads `pageStore.gridItems`, client expects `props.children`

**Solution:**
Always use props over store for SSR-compatible data

```typescript
// ❌ Before (race condition)
const pagesToFilter = computed(() => {
    if (pageStore.gridItems) {
        return pageStore.gridItems
    }
    return props.children
})

// ✅ After (always use props)
const pagesToFilter = computed(() => {
    return props.children
})
```

**Files Affected:**
- [components/page/content/grid/Grid.vue](../src/Frontend.Nuxt/components/page/content/grid/Grid.vue)

---

## 🔌 Provider Connection Timeout

### Issue: Editor Hangs on Slow Connections

**Symptoms:**
- Editor doesn't load for 30+ seconds
- No error message
- User waits indefinitely

**Root Cause:**
No timeout for HocuspocusProvider connection

**Solution:**
5-second timeout with fallback to offline mode

```typescript
const initProvider = () => {
    // Set timeout
    providerTimeout.value = setTimeout(() => {
        if (!providerLoaded.value) {
            providerLoaded.value = true
            loadCollab.value = false
            
            snackbarStore.showSnackbar({
                type: 'warning',
                text: { message: t('error.collaboration.timeout') }
            })
        }
    }, 5000)
    
    provider.value = new HocuspocusProvider({
        url: config.public.hocuspocusWebsocketUrl,
        name: `ydoc-${pageStore.id}`,
        token: userStore.collaborationToken,
        document: doc,
        onSynced() {
            // Clear timeout on success
            if (providerTimeout.value) {
                clearTimeout(providerTimeout.value)
            }
            // ... rest of logic
        }
    })
}
```

**Files Affected:**
- [components/page/content/ContentEditor.vue](../src/Frontend.Nuxt/components/page/content/ContentEditor.vue)
- [i18n/locales/de.json](../src/Frontend.Nuxt/i18n/locales/de.json)
- [i18n/locales/en.json](../src/Frontend.Nuxt/i18n/locales/en.json)

**Implemented:** January 2026 (v2.0)

---

## 🔄 Reconnection Issues

### Issue: Lost Connection Not Auto-Recovered

**Symptoms:**
- User loses network connection
- Editor shows "disconnected"
- Connection not restored automatically

**Current Behavior:**
```typescript
onClose(c) {
    isSynced.value = false
    
    // Try reconnect after 10 seconds
    reconnectTimer.value = setTimeout(() => {
        tryReconnect()
    }, 10000)
}

const tryReconnect = () => {
    if (provider.value) provider.value?.destroy()
    if (userStore.isLoggedIn && !isSynced.value) {
        initProvider()  // Reconnect
    }
}
```

**Known Limitation:**
Only one reconnection attempt, no exponential backoff

**Future Improvement:**
```typescript
let reconnectAttempts = 0
const maxReconnectAttempts = 5

const tryReconnect = () => {
    if (reconnectAttempts >= maxReconnectAttempts) {
        return
    }
    
    reconnectAttempts++
    const delay = Math.min(1000 * Math.pow(2, reconnectAttempts), 30000)
    
    reconnectTimer.value = setTimeout(() => {
        if (userStore.isLoggedIn && !isSynced.value) {
            initProvider()
        }
    }, delay)
}
```

---

## 📝 Content Normalization Issues

### Issue: Whitespace Differences Cause False Conflicts

**Historical Issue (Resolved):**
```typescript
// ❌ Old approach - fragile
const normalizeHtml = (html: string): string => {
    return html
        .replace(/\s+/g, ' ')  // Collapse whitespace
        .replace(/>\s+</g, '><')  // Remove between tags
        .trim()
}

if (normalizeHtml(yDocContent) !== normalizeHtml(serverContent)) {
    // False positives on code blocks, pre tags, etc.
}
```

**Solution:**
Hash-based comparison (v2.0)

```typescript
// ✅ Current approach - robust
const hashContent = (content: string): string => {
    let hash = 0
    for (let i = 0; i < content.length; i++) {
        const char = content.charCodeAt(i)
        hash = ((hash << 5) - hash) + char
        hash = hash & hash
    }
    return Math.abs(hash).toString(36)
}

if (hashContent(yDocContent) !== hashContent(serverContent)) {
    // Accurate detection
}
```

---

## 🖼️ Image Upload Race Conditions

### Issue: Multiple Simultaneous Uploads

**Symptoms:**
- Upload multiple images quickly
- Some fail silently
- Orphaned images in temp folder

**Current Mitigation:**
```typescript
// Upload queue with semaphore
const uploadQueue: Promise<any>[] = []
const maxConcurrentUploads = 3

const uploadImage = async (file: File) => {
    while (uploadQueue.length >= maxConcurrentUploads) {
        await Promise.race(uploadQueue)
    }
    
    const uploadPromise = $api('/apiVue/ImageUpload', {
        method: 'POST',
        body: formData
    })
    
    uploadQueue.push(uploadPromise)
    
    try {
        const result = await uploadPromise
        return result
    } finally {
        const index = uploadQueue.indexOf(uploadPromise)
        if (index > -1) uploadQueue.splice(index, 1)
    }
}
```

**Known Limitation:**
No retry mechanism for failed uploads

---

## 🧹 Orphaned Image Cleanup

### Issue: Deleted Images Not Removed from Server

**Current Behavior:**
Images are marked as orphaned but not immediately deleted

**Cleanup Process:**
```csharp
// Backend.Core/Domain/Images/ImageService.cs
public void CleanupOrphanedImages()
{
    var orphanedImages = _imageRepo.GetOrphanedImages(olderThanDays: 7);
    
    foreach (var image in orphanedImages)
    {
        _imageRepo.Delete(image);
        File.Delete(image.FilePath);
    }
}
```

**Scheduled Job:**
Runs daily at 3 AM (server time)

**Manual Trigger:**
```bash
# Via API (admin only)
POST /apiVue/ImageService/CleanupOrphanedImages
```

---

## 🔍 Debugging Tips

### Check IndexedDB

```javascript
// Browser DevTools > Application > IndexedDB
// Database name: 123|document-456
//   (userId|document-pageId)

// Inspect stored Y.Doc
const request = indexedDB.open('123|document-456')
request.onsuccess = (e) => {
    const db = e.target.result
    const transaction = db.transaction(['updates'], 'readonly')
    const store = transaction.objectStore('updates')
    const getAllRequest = store.getAll()
    
    getAllRequest.onsuccess = () => {
        console.log('Stored updates:', getAllRequest.result)
    }
}
```

### Monitor WebSocket

```javascript
// Browser DevTools > Network > WS filter
// Check messages:
// ← awareness (user cursor updates)
// ← sync (Y.js document updates)
// → sync-reply (acknowledgments)
```


---

## 📊 Performance Issues

### Issue: Slow Initial Load

**Symptoms:**
Editor takes 2-3 seconds to become interactive

**Profiling:**
```
Component Mount:        50ms
Provider Init:         500ms
IndexedDB Sync:        200ms
TipTap Init:           300ms
Extensions Load:       150ms
Total:                1200ms
```

**Optimization:**
```typescript
// Lazy load heavy extensions
const extensions = computed(() => [
    // Core (always loaded)
    Document, Paragraph, Text, Heading,
    
    // Conditional (load on demand)
    ...(needsCodeHighlight.value ? [CodeBlockLowlight] : []),
    ...(needsCollaboration.value ? [Collaboration, CollaborationCursor] : []),
])
```

---

## 🔗 Related Documentation

- [Editor System Overview](./editor-system-overview.md)
- [Collaboration System](./editor-collaboration-system.md)
- [Conflict Resolution](./editor-conflict-resolution.md)
