# backend-workflow Skill

## Aliases

This skill can be invoked with any of these names:
- `backend-workflow`
- `backend`
- `backend-änderung`
- `backend-development`

## Description

Master skill for Backend development workflow. Ensures consistent patterns, proper cache handling, and automatic test execution after changes.

**This skill automatically chains to `backend-test` after completing changes.**

## When This Skill is Triggered

- When modifying any file in `src/Backend.Core/` or `src/Backend.Api/`
- When user asks to implement a backend feature
- When fixing backend bugs

## Workflow Overview

```
┌─────────────────────┐
│  1. Plan Changes    │
├─────────────────────┤
│  2. Apply Changes   │
│     - Follow patterns│
│     - Update cache  │
├─────────────────────┤
│  3. Build & Verify  │
├─────────────────────┤
│  4. Run Tests       │  ◀── Chains to backend-test skill
└─────────────────────┘
```

## Copilot Execution Steps

### Phase 1: Before Making Changes

1. **Understand the context**: Read related files to understand existing patterns
2. **Check for existing patterns**: Use `entity-cache-pattern` skill if working with cached entities
3. **Identify affected areas**: Note which tests might need to run later

### Phase 2: Making Changes

Follow these guidelines based on what you're changing:

#### Database Queries with NHibernate

**CRITICAL: MySQL aggregate function types**

When using native SQL queries with `AliasToBeanResultTransformer`:

| SQL Function | MySQL Returns | C# Property Type |
|-------------|---------------|------------------|
| `COUNT(*)` | BIGINT | `long` |
| `SUM()` | DECIMAL | `decimal` |
| `AVG()` | DECIMAL | `decimal` |

Example:
```csharp
public class MySummary
{
    public long RequestCount { get; set; }      // COUNT(*)
    public decimal TotalTokens { get; set; }    // SUM()
}
```

#### EntityCache Updates

When modifying cached entities, **always update both DB and cache**:

```csharp
// 1. Update database
_repository.Update(entity);

// 2. Update cache AFTER successful DB update
EntityCache.AddOrUpdate(CacheItem.ToCacheItem(entity));
```

See `entity-cache-pattern` skill for detailed patterns.

#### API Controllers

- Use `[HttpGet]` or `[HttpPost]` attributes
- Return record structs for responses
- Use `AccessOnlyAsLoggedIn` attribute when needed

#### Code Style

- No namespaces in C# files
- Spell out variable names (no abbreviations)
- Always use braces after if/loops (no single-line statements)
- Write comments in English

### Phase 3: Build & Verify

After completing changes, verify the build:

```powershell
# Stop running backend first (avoid DLL locks)
Get-Process -Name "MemoWikis.Backend.Api" -ErrorAction SilentlyContinue | Stop-Process -Force

# Build
cd c:\Projects\memoWikis
dotnet build src/Backend.Api/Backend.Api.csproj
```

Check for:
- ✅ Build succeeds
- ⚠️ No new warnings related to your changes
- ❌ Fix any errors before proceeding

### Phase 4: Run Tests (Chain to backend-test)

**IMPORTANT: After completing all changes, invoke the `backend-test` skill.**

Read the skill file at: `.github/skills/backend-test/SKILL.md`

This will:
1. Stop the running Backend process
2. Identify which tests correspond to your changes
3. Run those tests
4. Report results

## Quick Reference: Common Patterns

### Adding a New API Endpoint

```csharp
[HttpGet]
[AccessOnlyAsLoggedIn]
public MyResponse GetSomething()
{
    // Implementation
    return new MyResponse(data);
}

public readonly record struct MyResponse(int Value);
```

### Adding Usage Logging

```csharp
_aiUsageLogRepo.AddUsage(
    userId,
    pageId,
    tokenIn,
    tokenOut,
    modelId
);

// Update cache if tracking usage
var extendedUser = EntityCache.GetExtendedUserByIdNullable(userId);
if (extendedUser != null)
{
    extendedUser.CurrentWeekTokenUsage += tokenIn + tokenOut;
}
```

### Creating a New Test

```csharp
class MyFeature_tests : BaseTestHarness
{
    [Test]
    public void Method_scenario_expected_result()
    {
        // Arrange
        var service = R<MyService>();
        
        // Act
        var result = service.DoSomething();
        
        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }
}
```

## Related Skills

- **entity-cache-pattern**: Detailed EntityCache read/write patterns
- **backend-test**: Automatic test execution after changes
- **app-start**: Start Backend and Frontend services
- **app-stop**: Stop running services

## Files Commonly Modified Together

| When changing... | Also check/update... |
|-----------------|---------------------|
| `*Repo.cs` | Cache update code, related tests |
| `*Controller.cs` | Frontend store that calls the API |
| `*CacheItem.cs` | EntityCache methods, related tests |
| DTO/Response records | Frontend types.ts |

## Checklist Before Completing

- [ ] Build succeeds without new errors
- [ ] EntityCache updated if entity data changed
- [ ] API changes reflected in frontend (if applicable)
- [ ] Tests written/updated for new functionality
- [ ] `backend-test` skill invoked to verify changes
