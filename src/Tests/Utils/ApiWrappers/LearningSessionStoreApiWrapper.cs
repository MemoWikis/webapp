using static LearningSessionStoreController;

public class LearningSessionStoreApiWrapper(TestHarness _testHarness)
{
    public async Task<LearningSessionResponse> NewSession(LearningSessionConfigRequest sessionConfig) =>
        await _testHarness.ApiPostJson<LearningSessionConfigRequest, LearningSessionResponse>(
            "/apiVue/LearningSessionStore/NewSession",
            sessionConfig);

    public async Task<LearningSessionResponse> NewSession(object sessionConfig) =>
        await _testHarness.ApiPostJson<object, LearningSessionResponse>(
            "/apiVue/LearningSessionStore/NewSession",
            sessionConfig);

    public async Task<LearningSessionResponse> GetCurrentSession() => 
        await _testHarness.ApiGet<LearningSessionResponse>("/apiVue/LearningSessionStore/GetCurrentSession");

    public async Task<LearningSessionResponse> LoadSpecificQuestion(int questionIndex) => 
        await _testHarness.ApiGet<LearningSessionResponse>($"/apiVue/LearningSessionStore/LoadSpecificQuestion/{questionIndex}");

    public async Task<StepResult[]> LoadSteps() => 
        await _testHarness.ApiGet<StepResult[]>("/apiVue/LearningSessionStore/LoadSteps");

    public async Task<LastStepInQuestionListResult> GetLastStepInQuestionList(int stepIndex) => 
        await _testHarness.ApiGet<LastStepInQuestionListResult>($"/apiVue/LearningSessionStore/GetLastStepInQuestionList/{stepIndex}");
}
