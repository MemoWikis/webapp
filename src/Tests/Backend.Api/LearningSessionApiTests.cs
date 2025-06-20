using static LearningSessionStoreController;
using static AnswerBodyController;

[TestFixture]
internal class LearningSessionApiTests : BaseTestHarness
{
    public LearningSessionApiTests() => _useTinyScenario = true;

    [Test]
    public async Task Should_Start_Learning_Session_And_Answer_Questions()
    {
        // Arrange - Get the first page from the scenario to use for learning session
        var pages = await _testHarness.DbData.AllPagesSummaryAsync();
        var firstPage = pages.First!;

        var sessionConfig = new LearningSessionConfigRequest
        {
            PageId = Convert.ToInt32(firstPage["Id"]),
            MaxQuestionCount = 5,
            CurrentUserId = 2, // LearningUser from scenario
            IsInTestMode = false,
            QuestionOrder = QuestionOrder.SortByEasiest,
            AnswerHelp = true,
            Repetition = RepetitionType.None,
            InWuwi = true,
            NotInWuwi = true,
            CreatedByCurrentUser = true,
            NotCreatedByCurrentUser = true,
            PrivateQuestions = true,
            PublicQuestions = true,
            NotLearned = true,
            NeedsLearning = true,
            NeedsConsolidation = true,
            Solid = true
        }; 
        
        // Act - Start new learning session
        var newSessionResponse = await _testHarness.ApiLearningSessionStore.NewSession(sessionConfig);

        // Assert - Verify session was created successfully
        await Verify(new { newSessionResponse });
        await AnswerQuestionsAndVerifyState(newSessionResponse);
    }

    private async Task AnswerQuestionsAndVerifyState(LearningSessionResponse sessionResponse)
    {
        var currentStep = sessionResponse.CurrentStep!.Value;
        var questionId = currentStep.id;        // Answer the first question correctly
        var answerRequest = new SendAnswerToLearningSessionRequest(
            Id: questionId,
            QuestionViewGuid: Guid.NewGuid(),
            Answer: "Correct answer", // We'll use a generic correct answer
            InTestMode: false
        ); 
        
        // Verify the answer response
        var answerResponse = await _testHarness.ApiAnswerBody.SendAnswerToLearningSession(answerRequest);
        await Verify(new
        {
            correct = answerResponse.Correct,
            hasCorrectAnswer = !string.IsNullOrEmpty(answerResponse.CorrectAnswer),
            newStepAdded = answerResponse.NewStepAdded,
            isLastStep = answerResponse.IsLastStep
        }).UseMethodName("FirstQuestionAnswer");

        // Mark the first question as correct to test the MarkAsCorrect endpoint
        var markCorrectRequest = new MarkAsCorrectParam(
            Id: questionId,
            QuestionViewGuid: answerRequest.QuestionViewGuid,
            AmountOfTries: 1
        ); 
        
        // Get current session state after marking correct
        var markCorrectResponse = await _testHarness.ApiAnswerBody.MarkAsCorrect(markCorrectRequest);
        var currentSessionResponse = await _testHarness.ApiLearningSessionStore.GetCurrentSession();

        // Answer a second question if available
        if (currentSessionResponse.Success && currentSessionResponse.Steps.Length > 1)
        {
            // Move to next question
            var nextQuestionIndex = 1; // Move to second question
            var loadSpecificSession = await _testHarness.ApiLearningSessionStore.LoadSpecificQuestion(nextQuestionIndex);

            if (loadSpecificSession.Success && loadSpecificSession.CurrentStep != null)
            {
                // Answer second question incorrectly
                var secondQuestionId = loadSpecificSession.CurrentStep.Value.id;
                var secondAnswerRequest = new SendAnswerToLearningSessionRequest(
                    Id: secondQuestionId,
                    QuestionViewGuid: Guid.NewGuid(),
                    Answer: "Wrong answer",
                    InTestMode: false
                );

                var secondAnswerResponse = await _testHarness.ApiAnswerBody.SendAnswerToLearningSession(secondAnswerRequest);

                // Verify second answer response
                await Verify(new
                {
                    correct = secondAnswerResponse.Correct,
                    hasCorrectAnswer = !string.IsNullOrEmpty(secondAnswerResponse.CorrectAnswer),
                    newStepAdded = secondAnswerResponse.NewStepAdded,
                    isLastStep = secondAnswerResponse.IsLastStep
                }).UseMethodName("SecondQuestionAnswer");
            }
        }

        // Final verification of database and cache state
        await VerifyFinalState();
    }

    private async Task VerifyFinalState()
    {        // Get final session state
        // Get learning session result/summary
        var finalSessionResponse = await _testHarness.ApiLearningSessionStore.GetCurrentSession();
        var sessionResult = await _testHarness.ApiLearningSessionResult.Get();

        // Get database state summaries
        var usersData = await _testHarness.DbData.AllUsersSummaryAsync();
        var pagesData = await _testHarness.DbData.AllPagesSummaryAsync();
        var questionsData = await _testHarness.DbData.AllQuestionsSummaryAsync();

        // Verify final state
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

    [Test]
    public async Task Should_Handle_Learning_Session_With_Page_Navigation()
    {
        var pages = await _testHarness.DbData.AllPagesSummaryAsync();
        var targetPage = pages.First!;

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
        };        // Act - Create session and navigate through questions
        var sessionResponse = await _testHarness.ApiLearningSessionStore.NewSession(sessionConfig);        // Test loading steps
        var stepsResponse = await _testHarness.ApiLearningSessionStore.LoadSteps();        // Test getting last step in question list
        var stepCount = sessionResponse.Steps.Length;
        var lastStepResponse = await _testHarness.ApiLearningSessionStore.GetLastStepInQuestionList(stepCount - 1);

        // Verify navigation functionality
        await Verify(new
        {
            sessionCreated = sessionResponse.Success,
            totalSteps = stepCount,
            stepsLoaded = stepsResponse.Length,
            lastStepSuccess = lastStepResponse.Success,
            activeQuestionCount = lastStepResponse.ActiveQuestionCount
        }).UseMethodName("NavigationTest");
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

        var invalidResponse = await _testHarness.ApiLearningSessionStore.NewSession(invalidSessionConfig);

        // Test answering question without session
        var invalidAnswerRequest = new SendAnswerToLearningSessionRequest(
            Id: 1,
            QuestionViewGuid: Guid.NewGuid(),
            Answer: "Some answer",
            InTestMode: false
        ); try
        {
            var invalidAnswerResponse = await _testHarness.ApiAnswerBody.SendAnswerToLearningSession(invalidAnswerRequest);

            await Verify(new
            {
                invalidAnswerResponse
            }).UseMethodName("InvalidRequests");
        }
        catch (Exception ex)
        {
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
