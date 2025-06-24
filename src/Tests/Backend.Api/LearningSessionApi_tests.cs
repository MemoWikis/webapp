using static LearningSessionStoreController;
using static AnswerBodyController;

/// <summary>
/// Integration tests for learning session functionality including session creation,
/// question answering, navigation, and error handling scenarios.
/// Uses the tiny scenario for faster test execution with minimal test data.
/// </summary>
[TestFixture]
internal class LearningSessionApi_tests : BaseTestHarness
{
    public LearningSessionApi_tests() => _useTinyScenario = true;

    /// <summary>
    /// Tests the complete learning session workflow: creating a session, answering questions correctly and incorrectly,
    /// marking questions as correct, and verifying the final state of the session and database.
    /// </summary>
    [Test]
    public async Task Should_Start_Learning_Session_And_Answer_Questions()
    {
        // Arrange: Get the first page from the scenario to use for the learning session.
        var pages = await _testHarness.DbData.AllPagesSummaryAsync();
        var firstPage = pages.First!;

        // Create a comprehensive session configuration with all filter options enabled.
        var sessionConfig = GetSessionConfig(
            pageId: Convert.ToInt32(firstPage["Id"]),
            maxQuestionCount: 5,
            repetitionType: RepetitionType.Normal
        );

        // Act: Start a new learning session.
        var newSessionResponse = await _testHarness.ApiLearningSessionStore.NewSession(sessionConfig);


        // Assert: Verify the session was created successfully and test the question answering workflow.
        var currentStep = newSessionResponse.Steps.First(step => step.id == newSessionResponse.CurrentStep!.Value.id);
        await Verify(new
        {
            indexIsZero = currentStep.index == 0,
            newSessionResponse.ActiveQuestionCount
        }).UseMethodName("LearningSession-Start");

        await AnswerQuestionsAndVerifyState(newSessionResponse);
    }

    /// <summary>
    /// Handles the question answering workflow within a learning session.
    /// This method tests both correct and incorrect answers, marking questions as correct, and session navigation.
    /// </summary>
    /// <param name="sessionResponse">The initial session response containing questions to answer.</param>
    private async Task AnswerQuestionsAndVerifyState(LearningSessionResponse sessionResponse)
    {
        var currentStep = sessionResponse.CurrentStep!.Value;
        var questionId = currentStep.id;

        // Answer the first question with a correct response.
        var answerRequest = new SendAnswerToLearningSessionRequest(
            Id: questionId,
            QuestionViewGuid: Guid.NewGuid(),
            Answer: "Correct answer",
            InTestMode: false
        );

        // Verify the answer response contains the expected feedback.
        var answerResponse = await _testHarness.ApiAnswerBody.SendAnswerToLearningSession(answerRequest);
        await Verify(new
        {
            firstAnswerCorrect = answerResponse.Correct
        }).UseMethodName("LearningSession-Answer1");

        // Test the MarkAsCorrect endpoint functionality.
        var markCorrectRequest = new MarkAsCorrectParam(
            Id: questionId,
            QuestionViewGuid: answerRequest.QuestionViewGuid,
            AmountOfTries: 1
        );

        // Mark the question as correct and get the updated session state.
        var markCorrectResponse = await _testHarness.ApiAnswerBody.MarkAsCorrect(markCorrectRequest);
        var currentSessionResponse = await _testHarness.ApiLearningSessionStore.GetCurrentSession();

        // Test answering a second question if available in the session.
        // Navigate to the second question in the session.
        var nextQuestionIndex = 1;
        var loadSpecificSession = await _testHarness.ApiLearningSessionStore.LoadSpecificQuestion(nextQuestionIndex);

        // Answer the second question incorrectly to test wrong answer handling.
        var secondQuestionId = loadSpecificSession.CurrentStep.Value.id;
        var secondAnswerRequest = new SendAnswerToLearningSessionRequest(
            Id: secondQuestionId,
            QuestionViewGuid: Guid.NewGuid(),
            Answer: "Wrong answer",
            InTestMode: false
        );

        var secondAnswerResponse = await _testHarness.ApiAnswerBody.SendAnswerToLearningSession(secondAnswerRequest);

        // Verify the incorrect answer response.
        await Verify(new
        {
            markCorrectResponse,
            secondAnswerIsCorrect = secondAnswerResponse.Correct,
        }).UseMethodName("LearningSession-Answer2");

        // Perform a comprehensive verification of the final system state.
        await VerifyFinalState();
    }

    /// <summary>
    /// Verifies the final state of the learning session, database, and session results
    /// after all question answering activities have been completed.
    /// </summary>
    private async Task VerifyFinalState()
    {
        // Retrieve the final session state and learning results.
        var finalSessionResponse = await _testHarness.ApiLearningSessionStore.GetCurrentSession();
        var sessionResult = await _testHarness.ApiLearningSessionResult.Get();

        // Get database state summaries for verification.
        var usersData = await _testHarness.DbData.AllUsersSummaryAsync();
        var pagesData = await _testHarness.DbData.AllPagesSummaryAsync();
        var questionsData = await _testHarness.DbData.AllQuestionsSummaryAsync();

        // Verify the final state, including session status, results, and database integrity.
        await Verify(new
        {
            finalSession = new
            {
                success = finalSessionResponse.Success,
                stepsCount = finalSessionResponse.Steps.Length,
                activeQuestionCount = finalSessionResponse.ActiveQuestionCount,
                currentStepIndex = finalSessionResponse.CurrentStep?.index,
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
    /// Tests learning session navigation features, including step loading,
    /// session creation with different configurations, and step traversal.
    /// </summary>
    [Test]
    public async Task Should_Handle_Learning_Session_With_Page_Navigation()
    {
        // Arrange: Get a page for navigation testing.
        var pages = await _testHarness.DbData.AllPagesSummaryAsync();
        var targetPage = pages.First!;

        // Create a session with normal repetition mode for navigation testing.
        var sessionConfig = GetSessionConfig(
            pageId: Convert.ToInt32(targetPage["Id"])
        );

        // Act: Create the session and test navigation functionality.
        var sessionResponse = await _testHarness.ApiLearningSessionStore.NewSession(sessionConfig);

        // Load all steps in the session to verify they are accessible.
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
        }).UseMethodName("LearningSession-FinalState");
    }

    /// <summary>
    /// Tests error handling scenarios, including using invalid page IDs and
    /// attempting to answer questions without an active session.
    /// </summary>
    [Test]
    public async Task Should_Handle_Invalid_Learning_Session_Requests()
    {
        // Arrange - Create an invalid session configuration with a non-existent page ID
        var invalidSessionConfig = GetSessionConfig(
            pageId: Convert.ToInt32(99999)
        );

        // Act - Attempt to start a new session with the invalid configuration
        _ = await _testHarness.ApiLearningSessionStore.NewSession(invalidSessionConfig);

        // Test answering question without an active session
        var invalidAnswerRequest = new SendAnswerToLearningSessionRequest(
            Id: 1,
            QuestionViewGuid: Guid.NewGuid(),
            Answer: "Some answer",
            InTestMode: false
        );

        // Attempt to answer question and handle potential exceptions
        var invalidAnswerResponse = await _testHarness.ApiAnswerBody.SendAnswerToLearningSessionCall(invalidAnswerRequest);
        var responseContent = await invalidAnswerResponse.Content.ReadAsStringAsync();

        await Verify(new
        {
            statusCode = invalidAnswerResponse.StatusCode, // 500
            isSuccessStatusCode = invalidAnswerResponse.IsSuccessStatusCode, //false
        }).UseMethodName("InvalidRequests");
    }

    /// <summary>
    /// Creates a learning session configuration object for testing purposes.
    /// This helper method simplifies the creation of session configurations for tests.
    /// </summary>
    /// <param name="pageId">The ID of the page to start the session from.</param>
    /// <param name="maxQuestionCount">The maximum number of questions in the session.</param>
    /// <param name="repetitionType">The repetition type for the session.</param>
    /// <returns>A configured LearningSessionConfigRequest object.</returns>
    private static LearningSessionConfigRequest GetSessionConfig(
        int pageId,
        int maxQuestionCount = 3,
        RepetitionType repetitionType = RepetitionType.None
    )
    {
        // Returns a new learning session configuration with the specified parameters.
        // All filter options are enabled by default in the request object.
        return new LearningSessionConfigRequest
        {
            PageId = pageId,
            MaxQuestionCount = maxQuestionCount,
            Repetition = repetitionType,
            AnswerHelp = true,
            IsInTestMode = false
        };
    }
}
