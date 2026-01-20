# EntityCache Pattern

## Overview

The EntityCache is a centralized in-memory cache that provides fast access to frequently used entities (Users, Pages, Questions, Relations, Shares). It is backed by a `ConcurrentDictionary` and persisted in `MemoCache`.

**Key Principle:** Always read from cache, write to both DB and cache.

## Architecture

```
┌─────────────────┐    ┌──────────────────┐    ┌──────────────┐
│   Controller    │───▶│   EntityCache    │◀───│   MemoCache  │
│   / Service     │    │   (static)       │    │ (persistent) │
└─────────────────┘    └──────────────────┘    └──────────────┘
         │                      ▲
         │                      │
         ▼                      │
┌─────────────────┐    ┌──────────────────┐
│   Repository    │───▶│     Database     │
│  (DB writes)    │    │     (MySQL)      │
└─────────────────┘    └──────────────────┘
```

## Core Files

- **EntityCache.cs**: `src/Backend.Core/Infrastructure/Cache/EntityCache.cs`
- **UserCacheItem.cs**: `src/Backend.Core/Infrastructure/Cache/EntityCache/UserCacheItem.cs`
- **ExtendedUserCacheItem.cs**: `src/Backend.Core/Infrastructure/Cache/SlidingCache/ExtendedUserCacheItem.cs`
- **ExtendedUserCache.cs**: `src/Backend.Core/Infrastructure/Cache/SlidingCache/ExtendedUserCache.cs`
- **PageCacheItem.cs**: `src/Backend.Core/Infrastructure/Cache/EntityCache/PageCacheItem.cs`
- **QuestionCacheItem.cs**: `src/Backend.Core/Infrastructure/Cache/EntityCache/QuestionCacheItem.cs`
- **EntityCacheInitializer.cs**: `src/Backend.Core/Infrastructure/Cache/EntityCache/EntityCacheInitializer.cs`

## Two-Tier User Cache Pattern

The user cache is split into two tiers:

### UserCacheItem (Tier 1: Always loaded)
- Loaded at **application startup** for ALL users
- Contains basic user data (name, email, subscription status, etc.)
- Low memory footprint per user
- Access via `EntityCache.GetUserById(userId)`

### ExtendedUserCacheItem (Tier 2: Loaded on login)
- **Extends** `UserCacheItem`
- Loaded **only when user logs in** (`SessionUser.Login()`)
- Contains "expensive" user-specific data:
  - `PageValuations` - User's page ratings
  - `QuestionValuations` - User's question answers/ratings
  - `AnswerCounter` - Answer history
  - `Skills` - User skill evaluations
  - `CurrentWeekTokenUsage` - AI token usage this week
- Access via `ExtendedUserCache.GetUser(userId)` or `EntityCache.GetExtendedUserByIdNullable(userId)`

### When to use which tier:

```csharp
// Tier 1: Basic user data (always available)
var user = EntityCache.GetUserById(userId);
var userName = user.Name;
var hasSubscription = user.SubscriptionStartDate.HasValue;

// Tier 2: User-specific data (only after login)
var extendedUser = _extendedUserCache.GetUser(userId);
var skills = extendedUser.GetAllSkills();
var tokenUsage = extendedUser.CurrentWeekTokenUsage;
```

### Adding data to ExtendedUserCacheItem:

When adding new user-specific data that should only be loaded at login:

1. Add property to `ExtendedUserCacheItem`:
```csharp
public class ExtendedUserCacheItem : UserCacheItem
{
    public long CurrentWeekTokenUsage { get; set; } = 0;
}
```

2. Add populate method in `ExtendedUserCache`:
```csharp
private void PopulateTokenUsage(ExtendedUserCacheItem cacheItem, AiUsageLogRepo repo)
{
    var usage = repo.GetCurrentWeekTokenUsage(cacheItem.Id);
    cacheItem.CurrentWeekTokenUsage = usage.TotalTokens;
}
```

3. Call in `CreateExtendedUserCacheItem()`:
```csharp
public ExtendedUserCacheItem CreateExtendedUserCacheItem(int userId, ...)
{
    var cacheItem = CreateCacheItem(EntityCache.GetUserById(userId));
    PopulatePageValuations(cacheItem);
    PopulateTokenUsage(cacheItem, _aiUsageLogRepo);  // New
    // ...
    return cacheItem;
}
```

## Reading from Cache

Always use `EntityCache.GetXxx()` methods for reading:

```csharp
// Get user by ID (returns empty item if not found)
var user = EntityCache.GetUserById(userId);

// Get user by ID (returns null if not found)
var user = EntityCache.GetUserByIdNullable(userId);

// Get page by ID
var page = EntityCache.GetPage(pageId);

// Get multiple users
var users = EntityCache.GetUsersByIds(userIds);

// Get all users
var allUsers = EntityCache.GetAllUsers();
```

## Writing Pattern (DB + Cache)

**Critical:** When modifying data, always update BOTH the database AND the cache.

### Pattern 1: Update via Repository + Cache Refresh

```csharp
// 1. Update in database via repository
_userWritingRepo.Update(user);

// 2. Update cache
var cacheItem = EntityCache.GetUserById(user.Id);
cacheItem.PropertyToUpdate = newValue;
// OR recreate from DB entity:
EntityCache.AddOrUpdate(UserCacheItem.ToCacheUser(user));
```

### Pattern 2: Direct SQL Update + Cache Update

```csharp
using var transaction = _session.BeginTransaction();
try
{
    // 1. Update database directly
    var query = _session.CreateSQLQuery(@"
        UPDATE user SET SomeColumn = :value WHERE Id = :userId");
    query.SetParameter("value", newValue);
    query.SetParameter("userId", userId);
    query.ExecuteUpdate();

    transaction.Commit();

    // 2. Update cache AFTER successful commit
    var userCacheItem = EntityCache.GetUserById(userId);
    if (userCacheItem != null)
    {
        userCacheItem.SomeProperty = newValue;
    }
}
catch (Exception exception)
{
    transaction.Rollback();
    throw;
}
```

### Pattern 3: Add New Entity

```csharp
// 1. Create in database
_userWritingRepo.Create(newUser);

// 2. Add to cache
EntityCache.AddOrUpdate(UserCacheItem.ToCacheUser(newUser));
```

### Pattern 4: Delete Entity

```csharp
// 1. Remove from cache first (to prevent reads during deletion)
EntityCache.RemoveUser(userId);

// 2. Delete from database
_userWritingRepo.Delete(user);
```

## CacheItem Classes

Each entity type has a corresponding `*CacheItem` class:

| Entity | CacheItem | Conversion Method |
|--------|-----------|-------------------|
| User | UserCacheItem | `UserCacheItem.ToCacheUser(user)` |
| Page | PageCacheItem | `PageCacheItem.ToCachePage(page)` |
| Question | QuestionCacheItem | `QuestionCacheItem.ToCacheQuestion(question)` |

### Populating CacheItems

CacheItems have `Populate()` methods to copy data from DB entities:

```csharp
var cacheItem = new UserCacheItem();
cacheItem.Populate(dbUser);  // Copy all properties from DB entity

// Or use static factory
var cacheItem = UserCacheItem.ToCacheUser(dbUser);
```

## Cache Initialization

On application startup, `EntityCacheInitializer` loads all entities from the database:

```csharp
// Called during app startup
EntityCacheInitializer.Init(userRepo, pageRepo, questionRepo, ...);
```

## Thread Safety

- `EntityCache` uses `ConcurrentDictionary<int, T>` internally
- Safe for concurrent reads
- Use `AddOrUpdate()` for safe concurrent writes
- For complex operations, use transactions + cache update pattern

## Common Pitfalls

### ❌ DON'T: Update only database

```csharp
// BAD: Cache becomes stale
_userWritingRepo.Update(user);
// Missing: EntityCache.AddOrUpdate(...)
```

### ❌ DON'T: Update only cache

```csharp
// BAD: Data lost on restart
var cacheItem = EntityCache.GetUserById(userId);
cacheItem.SomeProperty = newValue;
// Missing: Database update
```

### ✅ DO: Update both

```csharp
// GOOD: Consistent state
_userWritingRepo.Update(user);
EntityCache.AddOrUpdate(UserCacheItem.ToCacheUser(user));
```

### ❌ DON'T: Update cache before commit

```csharp
// BAD: Cache updated, but transaction may fail
var cacheItem = EntityCache.GetUserById(userId);
cacheItem.SomeProperty = newValue;  // Too early!
transaction.Commit();  // May fail
```

### ✅ DO: Update cache after successful commit

```csharp
// GOOD: Cache only updated if DB succeeds
transaction.Commit();
var cacheItem = EntityCache.GetUserById(userId);
cacheItem.SomeProperty = newValue;  // After commit
```

## Special Considerations

### Preserving Computed Properties

Some properties are computed and should be preserved when updating:

```csharp
public void AddOrUpdate(UserCacheItem user)
{
    user.PreserveContentLanguages();  // Preserve computed data
    AddOrUpdate(Users, user);
}
```

### Related Data

When entities have relationships, update related caches too:

```csharp
// When deleting a user, also clean up related shares
public static void RemoveUser(int id)
{
    RemoveAllSharesByUserId(id);  // Related data
    Remove(GetUserById(id));       // User itself
}
```

## Performance Notes

- Cache reads are O(1) dictionary lookups
- Avoid loading entire collections when only IDs are needed
- Use `GetQuestionIdsForPage()` instead of `GetQuestionsForPage()` when possible
- The cache is populated once at startup; individual updates are incremental
