using static AnswerBodyController;

public class AnswerBodyApiWrapper(TestHarness _testHarness)
{
    public async Task<LearningResult> SendAnswerToLearningSession(SendAnswerToLearningSessionRequest request) =>
        await _testHarness.ApiPostJson<SendAnswerToLearningSessionRequest, LearningResult>(
            "/apiVue/AnswerBody/SendAnswerToLearningSession",
            request);

    public async Task<HttpResponseMessage> SendAnswerToLearningSessionCall(SendAnswerToLearningSessionRequest request) =>
        await _testHarness.ApiCall(
            "/apiVue/AnswerBody/SendAnswerToLearningSession",
            request);

    public async Task<bool> MarkAsCorrect(MarkAsCorrectParam markCorrectRequest) =>
        await _testHarness.ApiPostJson<MarkAsCorrectParam, bool>(
            "/apiVue/AnswerBody/MarkAsCorrect",
            markCorrectRequest);
}
