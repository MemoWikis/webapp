using static LearningSessionStoreController;
using static AnswerBodyController;

/// <summary>
/// Integration tests for learning session functionality including session creation,
/// question answering, navigation, and error handling scenarios.
/// Uses the tiny scenario for faster test execution with minimal test data.
/// </summary>
[TestFixture]
internal class LearningSessionApiTests : BaseTestHarness
{
    public LearningSessionApiTests() => _useTinyScenario = true;

    /// <summary>
    /// Tests the complete learning session workflow: creating a session, answering questions correctly and incorrectly,
    /// marking questions as correct, and verifying the final state of the session and database.
    /// </summary>
    [Test]
    public async Task Should_Start_Learning_Session_And_Answer_Questions()
    {
        // Arrange - Get the first page from the scenario to use for learning session
        var pages = await _testHarness.DbData.AllPagesSummaryAsync();
        var firstPage = pages.First!;

        // Create a comprehensive session configuration with all filter options enabled
        var sessionConfig = new LearningSessionConfigRequest
        {
            PageId = Convert.ToInt32(firstPage["Id"]),
            MaxQuestionCount = 5,
            CurrentUserId = 2, // LearningUser from test scenario
            IsInTestMode = false,
            QuestionOrder = QuestionOrder.SortByEasiest,
            AnswerHelp = true,
            Repetition = RepetitionType.None,
            // Enable all question visibility filters
            InWuwi = true,
            NotInWuwi = true,
            CreatedByCurrentUser = true,
            NotCreatedByCurrentUser = true,
            PrivateQuestions = true,
            PublicQuestions = true,
            // Enable all knowledge status filters
            NotLearned = true,
            NeedsLearning = true,
            NeedsConsolidation = true,
            Solid = true
        };

        // Act - Start new learning session
        var newSessionResponse = await _testHarness.ApiLearningSessionStore.NewSession(sessionConfig);

        // Assert - Verify session was created successfully and test question answering workflow
        await Verify(new { newSessionResponse });
        await AnswerQuestionsAndVerifyState(newSessionResponse);
    }

    /// <summary>
    /// Handles the question answering workflow within a learning session.
    /// Tests both correct and incorrect answers, question marking, and session navigation.
    /// </summary>
    /// <param name="sessionResponse">The initial session response containing questions to answer</param>
    private async Task AnswerQuestionsAndVerifyState(LearningSessionResponse sessionResponse)
    {
        var currentStep = sessionResponse.CurrentStep!.Value;
        var questionId = currentStep.id;

        // Answer the first question with a correct response
        var answerRequest = new SendAnswerToLearningSessionRequest(
            Id: questionId,
            QuestionViewGuid: Guid.NewGuid(),
            Answer: "Correct answer", // Generic correct answer for testing
            InTestMode: false
        );

        // Verify the answer response contains expected feedback
        var answerResponse = await _testHarness.ApiAnswerBody.SendAnswerToLearningSession(answerRequest);
        await Verify(new
        {
            correct = answerResponse.Correct,
            hasCorrectAnswer = !string.IsNullOrEmpty(answerResponse.CorrectAnswer),
            newStepAdded = answerResponse.NewStepAdded,
            isLastStep = answerResponse.IsLastStep
        }).UseMethodName("FirstQuestionAnswer");

        // Test the MarkAsCorrect endpoint functionality
        var markCorrectRequest = new MarkAsCorrectParam(
            Id: questionId,
            QuestionViewGuid: answerRequest.QuestionViewGuid,
            AmountOfTries: 1
        );

        // Mark the question as correct and get updated session state
        var markCorrectResponse = await _testHarness.ApiAnswerBody.MarkAsCorrect(markCorrectRequest);
        var currentSessionResponse = await _testHarness.ApiLearningSessionStore.GetCurrentSession();

        // Test answering a second question if available in the session
        if (currentSessionResponse.Success && currentSessionResponse.Steps.Length > 1)
        {
            // Navigate to the second question in the session
            var nextQuestionIndex = 1;
            var loadSpecificSession = await _testHarness.ApiLearningSessionStore.LoadSpecificQuestion(nextQuestionIndex);

            if (loadSpecificSession.Success && loadSpecificSession.CurrentStep != null)
            {
                // Answer second question incorrectly to test wrong answer handling
                var secondQuestionId = loadSpecificSession.CurrentStep.Value.id;
                var secondAnswerRequest = new SendAnswerToLearningSessionRequest(
                    Id: secondQuestionId,
                    QuestionViewGuid: Guid.NewGuid(),
                    Answer: "Wrong answer",
                    InTestMode: false
                );

                var secondAnswerResponse = await _testHarness.ApiAnswerBody.SendAnswerToLearningSession(secondAnswerRequest);

                // Verify incorrect answer response
                await Verify(new
                {
                    correct = secondAnswerResponse.Correct,
                    hasCorrectAnswer = !string.IsNullOrEmpty(secondAnswerResponse.CorrectAnswer),
                    newStepAdded = secondAnswerResponse.NewStepAdded,
                    isLastStep = secondAnswerResponse.IsLastStep
                }).UseMethodName("SecondQuestionAnswer");
            }
        }

        // Perform comprehensive verification of final system state
        await VerifyFinalState();
    }

    /// <summary>
    /// Verifies the final state of the learning session, database, and session results
    /// after all question answering activities have been completed.
    /// </summary>
    private async Task VerifyFinalState()
    {
        // Retrieve final session state and learning results
        var finalSessionResponse = await _testHarness.ApiLearningSessionStore.GetCurrentSession();
        var sessionResult = await _testHarness.ApiLearningSessionResult.Get();

        // Get database state summaries for verification
        var usersData = await _testHarness.DbData.AllUsersSummaryAsync();
        var pagesData = await _testHarness.DbData.AllPagesSummaryAsync();
        var questionsData = await _testHarness.DbData.AllQuestionsSummaryAsync();

        // Verify final state includes session status, results, and database integrity
        await Verify(new
        {
            finalSession = new
            {
                success = finalSessionResponse.Success,
                stepsCount = finalSessionResponse.Steps.Length,
                activeQuestionCount = finalSessionResponse.ActiveQuestionCount,
                currentStepId = finalSessionResponse.CurrentStep?.id ?? 0,
                messageKey = finalSessionResponse.MessageKey
            },
            sessionResult = new
            {
                uniqueQuestionCount = sessionResult.UniqueQuestionCount,
                correctCount = sessionResult.Correct.Count,
                wrongCount = sessionResult.Wrong.Count,
                notAnsweredCount = sessionResult.NotAnswered.Count,
                pageId = sessionResult.PageId,
                pageName = sessionResult.PageName
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

    /// <summary>
    /// Tests learning session navigation features including step loading,
    /// session creation with different configurations, and step traversal.
    /// </summary>
    [Test]
    public async Task Should_Handle_Learning_Session_With_Page_Navigation()
    {
        // Arrange - Get page for navigation testing
        var pages = await _testHarness.DbData.AllPagesSummaryAsync();
        var targetPage = pages.First!;

        // Create session with normal repetition mode for navigation testing
        var sessionConfig = new
        {
            pageId = Convert.ToInt32(targetPage["Id"]),
            maxQuestionCount = 3,
            currentUserId = 2, // LearningUser from test scenario
            isInTestMode = false,
            questionOrder = 0,
            answerHelp = true,
            repetition = 1, // Normal repetition mode
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

        // Act - Create session and test navigation functionality
        var sessionResponse = await _testHarness.ApiLearningSessionStore.NewSession(sessionConfig);

        // Test loading all steps in the session
        var stepsResponse = await _testHarness.ApiLearningSessionStore.LoadSteps();

        // Test navigation to the last step in the question list
        var stepCount = sessionResponse.Steps.Length;
        var lastStepResponse = await _testHarness.ApiLearningSessionStore.GetLastStepInQuestionList(stepCount - 1);

        // Verify navigation functionality works correctly
        await Verify(new
        {
            sessionCreated = sessionResponse.Success,
            totalSteps = stepCount,
            stepsLoaded = stepsResponse.Length,
            lastStepSuccess = lastStepResponse.Success,
            activeQuestionCount = lastStepResponse.ActiveQuestionCount
        }).UseMethodName("NavigationTest");
    }

    /// <summary>
    /// Tests error handling scenarios including invalid page IDs and
    /// attempting to answer questions without an active session.
    /// </summary>
    [Test]
    public async Task Should_Handle_Invalid_Learning_Session_Requests()
    {
        // Test session creation with non-existent page ID
        var invalidSessionConfig = new
        {
            pageId = 99999, // Non-existent page ID
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

        var invalidResponse = await _testHarness.ApiLearningSessionStore.NewSession(invalidSessionConfig);

        // Test answering question without an active session
        var invalidAnswerRequest = new SendAnswerToLearningSessionRequest(
            Id: 1,
            QuestionViewGuid: Guid.NewGuid(),
            Answer: "Some answer",
            InTestMode: false
        );

        // Attempt to answer question and handle potential exceptions
        try
        {
            var invalidAnswerResponse = await _testHarness.ApiAnswerBody.SendAnswerToLearningSession(invalidAnswerRequest);

            await Verify(new
            {
                invalidAnswerResponse
            }).UseMethodName("InvalidRequests");
        }
        catch (Exception ex)
        {
            // Verify error handling behavior when session is invalid
            await Verify(new
            {
                invalidSessionSuccess = invalidResponse.Success,
                invalidSessionMessage = invalidResponse.MessageKey,
                answerWithoutSessionThrew = true,
                exceptionType = ex.GetType().Name
            }).UseMethodName("InvalidRequests");
        }
    }
}
