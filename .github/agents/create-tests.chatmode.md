---
description: "Create comprehensive test guidelines for C# NUnit tests with Verify framework."
tools: ['changes', 'codebase', 'fetch', 'findTestFiles', 'githubRepo', 'search', 'usages']
---

# Test Creation Guidelines

Follow these established patterns when creating tests for the memoWikis project.

## Framework and Tools

- Use **Verify(..)** for complex state verification and snapshot testing
- Use **NUnit** for simple assertions and basic test structure
- Use **FluentAssertions** (.Should()) 
- Use **TestHarness** for API integration tests
- Inherit from **BaseTestHarness** for most tests
- Do not import namespaces like `System.Linq` or `System.Collections.Generic` in test files. ImplicitUsings are enabled, so these namespaces are already available.

## Test Class Structure

### Naming Conventions

- Use **PascalCase** for class names  
- Add `_tests` suffix to test class filenames
- Use descriptive method names that explain the scenario being tested

**Examples:**
- `LearningSessionApi_tests.cs`
- `WikiDeletion_tests.cs`
- `PermissionCheck_tests.cs`

### Test Method Naming Patterns

Choose the appropriate pattern based on your test type:

1. **Should_Verb_Context** - For behavior verification:
   - `Should_delete_wiki_when_user_has_multiple_wikis()`
   - `Should_fail_to_delete_last_wiki()`

2. **MethodName_Scenario_ExpectedOutcome** - For API/method testing:
   - `Login_WithValidCredentials_ReturnsSuccess()`
   - `CanMovePage_MovePageCreator_IsUser()`

3. **Descriptive_Scenario** - For integration tests:
   - `Should_Start_Learning_Session_And_Answer_Questions()`

## Test Organization

### Base Classes

- **BaseTestHarness**: For most integration and unit tests
- **TestHarness**: Direct usage for simple API wrapper tests

### Test Lifecycle

```csharp
public class ExampleApi_tests : BaseTestHarness
{
    // Use _useTinyScenario = true for faster tests with minimal data
    public ExampleApi_tests() => _useTinyScenario = true;

    [Test]
    public async Task Should_perform_expected_behavior()
    {
        await ClearData(); // Reset database state when needed
        
        // Arrange
        // Act  
        // Assert
    }
}
```

## Shared Code and Utilities

### Test Utilities

- Place utility functions in a file beside the test file, using the suffix `_testUtils`
- Keep API wrappers in `src/Tests/Utils/ApiWrappers/`

**Examples:**
- `LearningSessionApi_tests.cs` // test file
- `LearningSessionApi_testUtils.cs` // test utilities

### API Wrappers

API wrappers provide a clean interface to test HTTP endpoints. Use the established pattern:

```csharp
public class ExampleApiWrapper(TestHarness _testHarness)
{
    public async Task<ResponseDto> MethodName(RequestDto request) =>
        await _testHarness.ApiPostJson<RequestDto, ResponseDto>(
            "/apiVue/Controller/Method",
            request);

    public async Task<HttpResponseMessage> MethodNameCall(RequestDto request) =>
        await _testHarness.ApiCall("/apiVue/Controller/Method", request);
}
```

## Test Structure and Patterns

### Arrange-Act-Assert Pattern

Always use clear AAA structure with descriptive comments:

```csharp
[Test]
public async Task Should_perform_expected_behavior()
{
    // Arrange: Set up test data and configure dependencies
    var context = NewPageContext();
    var user = new User { Id = 1 };
    
    // Act: Execute the behavior being tested
    var result = await serviceUnderTest.PerformOperation(input);
    
    // Assert: Verify the expected outcomes
    result.Should().NotBeNull();
    await Verify(result);
}
```

### Documentation and Attributes

Use XML documentation and Description attributes for complex tests:

```csharp
/// <summary>
/// Verifies that a user can successfully delete a wiki if they own multiple wikis.
/// The operation should succeed and not return any error messages.
/// </summary>
[Test]
[Description("Should successfully delete a wiki when user has multiple wikis")]
public async Task Should_delete_wiki_when_user_has_multiple_wikis()
{
    // Test implementation
}
```

### Context Creation Patterns

Use the established context creation methods:

```csharp
// For page-related tests
var contextPage = NewPageContext();

// For question-related tests  
var contextQuestion = NewQuestionContext();

// Reload caches after data changes
await ReloadCaches();
```

## Verification Patterns

### Simple Assertions

Use FluentAssertions for readable assertions:

```csharp
result.Should().NotBeNull();
result.IsSuccess.Should().BeTrue();
result.Items.Should().HaveCount(3);
```

### Complex State Verification

Use Verify() for snapshot testing of complex objects:

```csharp
// Simple verification
await Verify(result);

// Verification with custom object
await Verify(new
{
    result.IsSuccess,
    result.Data,
    PageVerificationData = await _testHarness.GetDefaultPageVerificationDataAsync()
});

// Verification with custom method name
await Verify(result).UseMethodName("CustomScenario");
```

### Integration Test Verification

For comprehensive integration tests, verify multiple aspects:

```csharp
await Verify(new
{
    deleteResult,
    originalTree,
    newTree,
    PageVerificationData = await _testHarness.GetDefaultPageVerificationDataAsync()
});
```

## API Integration Tests

### TestHarness Usage

For API controllers, use TestHarness directly or through wrappers:

```csharp
public class ControllerApi_tests : TestHarness
{
    private readonly ControllerApiWrapper apiWrapper;

    public ControllerApi_tests()
    {
        var testHarness = new TestHarness();
        apiWrapper = new ControllerApiWrapper(testHarness);
    }

    [Test]
    public async Task Should_handle_valid_request()
    {
        // Arrange
        var request = new RequestDto { /* properties */ };

        // Act
        var result = await apiWrapper.MethodName(request);

        // Assert
        await Verify(result);
    }
}
```

### HTTP Response Testing

Test both successful responses and HTTP-level concerns:

```csharp
// Test the response object
var result = await apiWrapper.Method(request);
await Verify(result);

// Test the HTTP response directly
var httpResponse = await apiWrapper.MethodCall(request);
httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
```

## Legacy and Domain Tests

### Domain Logic Tests

For domain-specific tests, use appropriate base classes:

```csharp
[Category(TestCategories.Programmer)]
public class Domain_persistence_tests : BaseTest
{
    [Test]
    public void Should_persist_entity_correctly()
    {
        // Use R<> for dependency resolution
        var repository = R<EntityRepository>();
        
        // Test persistence logic
        var entity = new Entity("Name", sessionUser.UserId);
        repository.Create(entity);
        
        // Verify persistence
        var entityFromDb = repository.GetById(entity.Id);
        Assert.That(entityFromDb.Name, Is.EqualTo("Name"));
    }
}
```

## Best Practices

1. **Use descriptive test names** that explain the scenario and expected outcome
2. **Always include XML documentation** for complex test scenarios  
3. **Use _useTinyScenario = true** for faster test execution when possible
4. **Clear test data** with `await ClearData()` when tests need clean state
5. **Group related assertions** using Verify() for better test output
6. **Use established naming patterns** consistently across the codebase
7. **Include context in Arrange comments** to explain test setup
8. **Test both happy path and error scenarios** for comprehensive coverage

