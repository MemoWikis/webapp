using static AnswerBodyController;

public class AnswerBodyApiWrapper
{
    private readonly TestHarness testHarness;

    public AnswerBodyApiWrapper(TestHarness testHarness)
    {
        this.testHarness = testHarness;
    }

    public async Task<LearningResult> SendAnswerToLearningSession(SendAnswerToLearningSessionRequest request)
    {
        return await testHarness.ApiPostJson<SendAnswerToLearningSessionRequest, LearningResult>(
            "/apiVue/AnswerBody/SendAnswerToLearningSession",
            request);
    }

    public async Task<bool> MarkAsCorrect(MarkAsCorrectParam markCorrectRequest)
    {
        return await testHarness.ApiPostJson<MarkAsCorrectParam, bool>(
            "/apiVue/AnswerBody/MarkAsCorrect",
            markCorrectRequest);
    }
}
