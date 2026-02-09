# backend-test Skill

## Aliases

This skill can be invoked with any of these names:
- `backend-test`
- `run-backend-tests`
- `test-backend`

## Description

Analyzes which Backend files have changed and runs the corresponding unit tests. This skill is typically called at the end of a backend development session to verify changes.

## When This Skill is Triggered

- After completing Backend code changes
- When user asks to run tests for their changes
- Automatically invoked by `backend-workflow` skill after completing changes

## Copilot Execution Steps

**IMPORTANT: Follow these steps exactly when this skill is invoked:**

### Step 1: Stop Running Backend Process

Before running tests, stop any running Backend process to avoid DLL file locks:

```powershell
Get-Process -Name "MemoWikis.Backend.Api" -ErrorAction SilentlyContinue | Stop-Process -Force
```

### Step 2: Identify Changed Files

Use `get_changed_files` tool to see what Backend files have been modified:
- Look for files in `src/Backend.Core/` and `src/Backend.Api/`
- Note the domain/feature area (e.g., `Domain/AI/`, `Domain/User/`, `Infrastructure/Cache/`)

### Step 3: Map Changes to Test Files

Use this mapping to find corresponding test files:

| Changed Path Pattern | Test Location |
|---------------------|---------------|
| `Backend.Core/Domain/AI/` | `Tests/Backend.Core/AIContent/` |
| `Backend.Core/Domain/User/` | `Tests/Backend.Core/User/` |
| `Backend.Core/Domain/Page/` | `Tests/Backend.Core/Page/` |
| `Backend.Core/Domain/Question/` | `Tests/Backend.Core/Question/` |
| `Backend.Core/Infrastructure/Cache/` | `Tests/Cache/` |
| `Backend.Api/Api/NuxtUI/` | Look for matching controller tests |

Use `file_search` or `grep_search` to find test files that reference the changed classes/methods.

### Step 4: Run Tests

Use the `runTests` tool with the identified test files:

```typescript
runTests({
  files: ["c:\\Projects\\memoWikis\\src\\Tests\\Backend.Core\\<matched-test-file>.cs"]
})
```

If no specific test file is found, run tests in the general area:

```powershell
cd c:\Projects\memoWikis\src\Tests
dotnet test --filter "FullyQualifiedName~<FeatureArea>" --no-build
```

### Step 5: Report Results

Tell the user:
- ✅ Which tests passed
- ❌ Which tests failed (with error details)
- 🔍 Suggestions for new tests if coverage seems incomplete

## Test Discovery Patterns

### Finding Tests by Class Name

If you changed `AiUsageLogRepo.cs`, search for tests:

```
grep_search with pattern: "AiUsageLogRepo" in Tests/**/*.cs
```

### Finding Tests by Method Name

If you changed a specific method like `GetCurrentWeekTokenUsage`:

```
grep_search with pattern: "GetCurrentWeekTokenUsage" in Tests/**/*.cs
```

## Common Test Locations

| Feature | Test File(s) |
|---------|--------------|
| AI Usage | `Tests/Backend.Core/AIContent/AiUsageLogRepo_tests.cs` |
| AI Page Generation | `Tests/Backend.Core/AIContent/GenerateAiPage_tests.cs` |
| AI Flashcards | `Tests/Backend.Core/AIContent/GenerateFlashCards_tests.cs` |
| Token Deduction | `Tests/Backend.Core/AIContent/` (look for TokenDeduction) |

## When to Suggest New Tests

Suggest writing new tests when:
- A new public method was added
- An existing method's signature changed
- A bug was fixed (regression test)
- No existing tests cover the changed code

## Test Writing Guidelines

When creating new tests:
- Use `BaseTestHarness` as base class
- Use `R<T>()` to resolve dependencies
- Follow naming: `MethodName_Scenario_ExpectedResult`
- Use Verify() for snapshot testing where appropriate

## Example Test Structure

```csharp
class MyFeature_tests : BaseTestHarness
{
    [Test]
    public void MethodName_returns_expected_result()
    {
        // Arrange
        var service = R<MyService>();
        
        // Act
        var result = service.MyMethod();
        
        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }
}
```
