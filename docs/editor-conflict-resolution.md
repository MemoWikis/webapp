# Content Conflict Resolution

**Last Updated:** January 4, 2026  
**Parent:** [Editor System Overview](./editor-system-overview.md)

## 🎯 Purpose

Resolves conflicts between server content and cached IndexedDB content using hash-based versioning and configurable strategies.

## 🔐 Content Versioning

### ContentVersion Interface

```typescript
interface ContentVersion {
    hash: string          // Simple hash of content
    timestamp: number     // When content was saved
    userId: number        // Who saved it
    contentLength: number // Content size in bytes
}
```

### Hash Function

```typescript
const hashContent = (content: string): string => {
    let hash = 0
    for (let i = 0; i < content.length; i++) {
        const char = content.charCodeAt(i)
        hash = ((hash << 5) - hash) + char
        hash = hash & hash  // Convert to 32bit integer
    }
    return Math.abs(hash).toString(36)
}
```

**Characteristics:**
- ✅ Fast computation (~1ms for 10KB content)
- ✅ Deterministic (same content = same hash)
- ✅ Sufficient for change detection
- ❌ Not cryptographically secure (not needed)
- ❌ Collision possible (but rare for typical content)

**Example:**
```typescript
hashContent('<p>Hello World</p>')  // "abc123"
hashContent('<p>Hello World!</p>') // "def456" (different)
```

## ✅ Current Strategy (Server Wins)

If collaboration/IndexedDB content differs from server content, the editor applies **server content**.

**Rationale:**
- Server is the source of truth
- IndexedDB is a cache and can be stale
- Prevents the “flash correct → reset to old cache” bug

## 🔄 Conflict Resolution Flow

```typescript
onSynced(_e) {
    const yDocContent = editor.value?.getHTML() || ''
    const serverContent = pageStore.initialContent || ''
    
    // Calculate hashes
    const yDocHash = hashContent(yDocContent)
    const serverHash = hashContent(serverContent)

    // Conflict resolution (server wins)
    if (yDocHash !== serverHash && editor.value && serverContent) {
        editor.value.commands.setContent(serverContent)

        // Store new version
        doc.getMap('config').set('version', {
            hash: serverHash,
            timestamp: Date.now(),
            userId: userStore.id,
            contentLength: serverContent.length
        })
    }
}
```

## 🧪 Test Cases

### Test: Server Updated Content

```typescript
// Setup
server:     '<p>Admin updated content</p>'
indexedDB:  '<p>Old cached content</p>'

// Expected
result:     '<p>Admin updated content</p>'  // server wins
```

## 🔮 Future Improvements

### 1. Cryptographic Hash (Production)

```typescript
import { sha256 } from 'crypto-js'

const hashContent = (content: string): string => {
    return sha256(content).toString()
}
```

**Benefits:**
- Collision-resistant
- Industry standard
- More reliable

## 🔗 Related Documentation

- [Editor System Overview](./editor-system-overview.md)
- [Collaboration System](./editor-collaboration-system.md)
- [Known Issues](./editor-issues.md)
