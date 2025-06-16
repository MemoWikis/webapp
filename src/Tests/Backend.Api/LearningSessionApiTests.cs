using System.Text.Json;

[TestFixture]
internal class LearningSessionApiTests : BaseTestHarness
{
    public LearningSessionApiTests() => _useTinyScenario = true;

    [Test]
    public async Task Should_Start_Learning_Session_And_Answer_Questions()
    {
        // Arrange - Get the first page from the scenario to use for learning session
        var pages = await _testHarness.DbData.AllPagesSummaryAsync();
        var firstPage = pages.First;
        if (firstPage == null)
            throw new InvalidOperationException("No pages found in test data");
        
        var sessionConfig = new
        {
            pageId = Convert.ToInt32(firstPage["Id"]),
            maxQuestionCount = 5,
            currentUserId = 2, // LearningUser from scenario
            isInTestMode = false,
            questionOrder = 0, // SortByEasiest
            answerHelp = true,
            repetition = 0, // None
            inWuwi = true,
            notInWuwi = true,
            createdByCurrentUser = true,
            notCreatedByCurrentUser = true,
            privateQuestions = true,
            publicQuestions = true,
            notLearned = true,
            needsLearning = true,
            needsConsolidation = true,
            solid = true
        };

        // Act - Start new learning session
        var newSessionResponse = await _testHarness.ApiPostJson("/apiVue/LearningSessionStore/NewSession", sessionConfig);

        // Assert - Verify session was created successfully
        await Verify(new
        {
            success = GetBoolProperty(newSessionResponse, "success"),
            stepsCount = GetArrayLength(newSessionResponse, "steps"),
            activeQuestionCount = GetIntProperty(newSessionResponse, "activeQuestionCount"),
            hasCurrentStep = HasProperty(newSessionResponse, "currentStep"),
            firstQuestionId = GetNestedIntProperty(newSessionResponse, "currentStep", "id")
        }).UseMethodName("StartLearningSession");

        // Continue with answering questions if session was created successfully
        if (GetBoolProperty(newSessionResponse, "success") && GetArrayLength(newSessionResponse, "steps") > 0)
        {
            await AnswerQuestionsAndVerifyState(newSessionResponse);
        }
    }

    private async Task AnswerQuestionsAndVerifyState(JsonElement sessionResponse)
    {
        var currentStep = sessionResponse.GetProperty("currentStep");
        var questionId = currentStep.GetProperty("id").GetInt32();

        // Answer the first question correctly
        var answerRequest = new
        {
            id = questionId,
            questionViewGuid = Guid.NewGuid(),
            answer = "Correct answer", // We'll use a generic correct answer
            inTestMode = false
        };

        var answerResponse = await _testHarness.ApiPostJson("/apiVue/AnswerBody/SendAnswerToLearningSession", answerRequest);

        // Verify the answer response
        await Verify(new
        {
            correct = GetBoolProperty(answerResponse, "correct"),
            hasCorrectAnswer = HasProperty(answerResponse, "correctAnswer"),
            newStepAdded = GetBoolProperty(answerResponse, "newStepAdded"),
            isLastStep = GetBoolProperty(answerResponse, "isLastStep")
        }).UseMethodName("FirstQuestionAnswer");

        // Mark the first question as correct to test the MarkAsCorrect endpoint
        var markCorrectRequest = new
        {
            id = questionId,
            questionViewGuid = answerRequest.questionViewGuid,
            amountOfTries = 1
        };

        var markCorrectResponse = await _testHarness.ApiPostJson("/apiVue/AnswerBody/MarkAsCorrect", markCorrectRequest);

        // Get current session state after marking correct
        var currentSessionResponse = await _testHarness.ApiGet("/apiVue/LearningSessionStore/GetCurrentSession");
        var currentSessionJson = JsonSerializer.Deserialize<JsonElement>(currentSessionResponse);

        // Answer a second question if available
        if (GetBoolProperty(currentSessionJson, "success") && GetArrayLength(currentSessionJson, "steps") > 1)
        {
            // Move to next question
            var nextQuestionIndex = 1; // Move to second question
            var loadSpecificUri = $"/apiVue/LearningSessionStore/LoadSpecificQuestion/{nextQuestionIndex}";
            var loadSpecificResponse = await _testHarness.Client.PostAsync(loadSpecificUri, null);
            loadSpecificResponse.EnsureSuccessStatusCode();
            var loadSpecificContent = await loadSpecificResponse.Content.ReadAsStringAsync();
            var loadSpecificJson = JsonSerializer.Deserialize<JsonElement>(loadSpecificContent);

            if (GetBoolProperty(loadSpecificJson, "success") && HasProperty(loadSpecificJson, "currentStep"))
            {
                var secondQuestionId = GetNestedIntProperty(loadSpecificJson, "currentStep", "id");

                // Answer second question incorrectly
                var secondAnswerRequest = new
                {
                    id = secondQuestionId,
                    questionViewGuid = Guid.NewGuid(),
                    answer = "Wrong answer",
                    inTestMode = false
                };

                var secondAnswerResponse = await _testHarness.ApiPostJson("/apiVue/AnswerBody/SendAnswerToLearningSession", secondAnswerRequest);

                // Verify second answer response
                await Verify(new
                {
                    correct = GetBoolProperty(secondAnswerResponse, "correct"),
                    hasCorrectAnswer = HasProperty(secondAnswerResponse, "correctAnswer"),
                    newStepAdded = GetBoolProperty(secondAnswerResponse, "newStepAdded"),
                    isLastStep = GetBoolProperty(secondAnswerResponse, "isLastStep")
                }).UseMethodName("SecondQuestionAnswer");
            }
        }

        // Final verification of database and cache state
        await VerifyFinalState();
    }

    private async Task VerifyFinalState()
    {
        // Get final session state
        var finalSessionResponse = await _testHarness.ApiGet("/apiVue/LearningSessionStore/GetCurrentSession");
        var finalSessionJson = JsonSerializer.Deserialize<JsonElement>(finalSessionResponse);

        // Get learning session result/summary
        var sessionResultResponse = await _testHarness.ApiGet("/apiVue/LearningSessionResult/Get");
        var sessionResultJson = JsonSerializer.Deserialize<JsonElement>(sessionResultResponse);

        // Get database state summaries
        var usersData = await _testHarness.DbData.AllUsersSummaryAsync();
        var pagesData = await _testHarness.DbData.AllPagesSummaryAsync();
        var questionsData = await _testHarness.DbData.AllQuestionsSummaryAsync();

        // Verify final state
        await Verify(new
        {
            finalSession = new
            {
                success = GetBoolProperty(finalSessionJson, "success"),
                stepsCount = GetArrayLength(finalSessionJson, "steps"),
                activeQuestionCount = GetIntProperty(finalSessionJson, "activeQuestionCount"),
                currentStepId = GetNestedIntProperty(finalSessionJson, "currentStep", "id"),
                messageKey = GetStringProperty(finalSessionJson, "messageKey")
            },
            sessionResult = new
            {
                uniqueQuestionCount = GetIntProperty(sessionResultJson, "uniqueQuestionCount"),
                correctCount = GetNestedIntProperty(sessionResultJson, "correct", "count"),
                wrongCount = GetNestedIntProperty(sessionResultJson, "wrong", "count"),
                notAnsweredCount = GetNestedIntProperty(sessionResultJson, "notAnswered", "count"),
                pageId = GetIntProperty(sessionResultJson, "pageId"),
                pageName = GetStringProperty(sessionResultJson, "pageName")
            },
            databaseState = new
            {
                userCount = usersData.Count,
                pageCount = pagesData.Count,
                questionCount = questionsData.Count,
                firstUserId = usersData.First != null ? Convert.ToInt32(usersData.First["Id"]) : 0,
                firstPageId = pagesData.First != null ? Convert.ToInt32(pagesData.First["Id"]) : 0
            }
        }).UseMethodName("FinalState");
    }

    [Test]
    public async Task Should_Handle_Learning_Session_With_Page_Navigation()
    {
        // Arrange - Get a page with multiple questions
        var pages = await _testHarness.DbData.AllPagesSummaryAsync();
        var targetPage = pages.First; // Use first page which should have questions
        if (targetPage == null)
            throw new InvalidOperationException("No pages found in test data");
        var sessionConfig = new
        {
            pageId = Convert.ToInt32(targetPage["Id"]),
            maxQuestionCount = 3,
            currentUserId = 2, // LearningUser
            isInTestMode = false,
            questionOrder = 0,
            answerHelp = true,
            repetition = 1, // Normal repetition
            inWuwi = true,
            notInWuwi = true,
            createdByCurrentUser = true,
            notCreatedByCurrentUser = true,
            privateQuestions = true,
            publicQuestions = true,
            notLearned = true,
            needsLearning = true,
            needsConsolidation = true,
            solid = true
        };

        // Act - Create session and navigate through questions
        var sessionResponse = await _testHarness.ApiPostJson("/apiVue/LearningSessionStore/NewSession", sessionConfig);

        if (GetBoolProperty(sessionResponse, "success") && GetArrayLength(sessionResponse, "steps") > 0)
        {
            // Test loading steps
            var stepsResponse = await _testHarness.ApiGet("/apiVue/LearningSessionStore/LoadSteps");
            var stepsJson = JsonSerializer.Deserialize<JsonElement>(stepsResponse);

            // Test getting last step in question list
            var stepCount = GetArrayLength(sessionResponse, "steps");
            var lastStepUri = $"/apiVue/LearningSessionStore/GetLastStepInQuestionList/{stepCount - 1}";
            var lastStepResponse = await _testHarness.ApiGet(lastStepUri);
            var lastStepJson = JsonSerializer.Deserialize<JsonElement>(lastStepResponse);

            // Verify navigation functionality
            await Verify(new
            {
                sessionCreated = GetBoolProperty(sessionResponse, "success"),
                totalSteps = stepCount,
                stepsLoaded = GetArrayLength(stepsJson, "data"),
                lastStepSuccess = GetBoolProperty(lastStepJson, "success"),
                activeQuestionCount = GetIntProperty(lastStepJson, "activeQuestionCount")
            }).UseMethodName("NavigationTest");
        }
    }

    [Test]
    public async Task Should_Handle_Invalid_Learning_Session_Requests()
    {
        // Test with invalid page ID
        var invalidSessionConfig = new
        {
            pageId = 99999, // Non-existent page
            maxQuestionCount = 5,
            currentUserId = 2,
            isInTestMode = false,
            questionOrder = 0,
            answerHelp = true,
            repetition = 0,
            inWuwi = true,
            notInWuwi = true,
            createdByCurrentUser = true,
            notCreatedByCurrentUser = true,
            privateQuestions = true,
            publicQuestions = true,
            notLearned = true,
            needsLearning = true,
            needsConsolidation = true,
            solid = true
        };

        var invalidResponse = await _testHarness.ApiPostJson("/apiVue/LearningSessionStore/NewSession", invalidSessionConfig);

        // Test answering question without session
        var invalidAnswerRequest = new
        {
            id = 1,
            questionViewGuid = Guid.NewGuid(),
            answer = "Some answer",
            inTestMode = false
        };

        try
        {
            var invalidAnswerResponse = await _testHarness.ApiPostJson("/apiVue/AnswerBody/SendAnswerToLearningSession", invalidAnswerRequest);

            await Verify(new
            {
                invalidSessionSuccess = GetBoolProperty(invalidResponse, "success"),
                invalidSessionMessage = GetStringProperty(invalidResponse, "messageKey"),
                answeredWithoutSession = true // If we get here, it didn't throw
            }).UseMethodName("InvalidRequests");
        }
        catch (Exception ex)
        {
            await Verify(new
            {
                invalidSessionSuccess = GetBoolProperty(invalidResponse, "success"),
                invalidSessionMessage = GetStringProperty(invalidResponse, "messageKey"),
                answerWithoutSessionThrew = true,
                exceptionType = ex.GetType().Name
            }).UseMethodName("InvalidRequests");
        }
    }

    // Helper methods for JSON property access
    private static bool GetBoolProperty(JsonElement element, string propertyName)
    {
        return element.TryGetProperty(propertyName, out var property) && property.GetBoolean();
    }

    private static int GetIntProperty(JsonElement element, string propertyName)
    {
        return element.TryGetProperty(propertyName, out var property) ? property.GetInt32() : 0;
    }

    private static string GetStringProperty(JsonElement element, string propertyName)
    {
        return element.TryGetProperty(propertyName, out var property) ? property.GetString() ?? "" : "";
    }

    private static int GetArrayLength(JsonElement element, string propertyName)
    {
        return element.TryGetProperty(propertyName, out var property) && property.ValueKind == JsonValueKind.Array
            ? property.GetArrayLength() : 0;
    }

    private static bool HasProperty(JsonElement element, string propertyName)
    {
        return element.TryGetProperty(propertyName, out _);
    }

    private static int GetNestedIntProperty(JsonElement element, string parentProperty, string childProperty)
    {
        if (element.TryGetProperty(parentProperty, out var parent) &&
            parent.TryGetProperty(childProperty, out var child))
        {
            return child.GetInt32();
        }
        return 0;
    }
}
