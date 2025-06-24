internal class LearningSessionApi_testUtils
{
    /// <summary>
    /// Creates a learning session configuration object for testing purposes.
    /// This helper method simplifies the creation of session configurations for tests.
    /// </summary>
    /// <param name="pageId">The ID of the page to start the session from.</param>
    /// <param name="maxQuestionCount">The maximum number of questions in the session.</param>
    /// <param name="repetitionType">The repetition type for the session.</param>
    /// <returns>A configured LearningSessionConfigRequest object.</returns>
    public static LearningSessionStoreController.LearningSessionConfigRequest GetSessionConfig(
        int pageId,
        int maxQuestionCount = 3,
        RepetitionType repetitionType = RepetitionType.None
    )
    {
        // Returns a new learning session configuration with the specified parameters.
        // All filter options are enabled by default in the request object.
        return new LearningSessionStoreController.LearningSessionConfigRequest
        {
            PageId = pageId,
            MaxQuestionCount = maxQuestionCount,
            Repetition = repetitionType,
            AnswerHelp = true,
            IsInTestMode = false
        };
    }

    /// <summary>
    /// Verifies the final state of the learning session, database, and session results
    /// after all question answering activities have been completed.
    /// </summary>
    public static async Task VerifyFinalState(TestHarness testHarness, string scenarioPrefix)
    {
        // Retrieve the final session state and learning results.
        var finalSessionResponse = await testHarness.ApiLearningSessionStore.GetCurrentSession();
        var sessionResult = await testHarness.ApiLearningSessionResult.Get();

        // Get database state summaries for verification.
        var usersData = await testHarness.DbData.AllUsersSummaryAsync();
        var pagesData = await testHarness.DbData.AllPagesSummaryAsync();
        var questionsData = await testHarness.DbData.AllQuestionsSummaryAsync();

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
        }).UseMethodName($"{scenarioPrefix}-FinalState");
    }
}