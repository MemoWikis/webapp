using static LearningSessionStoreController;

public class LearningSessionStoreApiWrapper
{
    private readonly TestHarness testHarness;

    public LearningSessionStoreApiWrapper(TestHarness testHarness)
    {
        this.testHarness = testHarness;
    }

    public async Task<LearningSessionResponse> NewSession(LearningSessionConfigRequest sessionConfig)
    {
        return await testHarness.ApiPostJson<LearningSessionConfigRequest, LearningSessionResponse>(
            "/apiVue/LearningSessionStore/NewSession",
            sessionConfig);
    }

    public async Task<LearningSessionResponse> NewSession(object sessionConfig)
    {
        return await testHarness.ApiPostJson<object, LearningSessionResponse>(
            "/apiVue/LearningSessionStore/NewSession",
            sessionConfig);
    }

    public async Task<LearningSessionResponse> GetCurrentSession()
    {
        return await testHarness.ApiGet<LearningSessionResponse>("/apiVue/LearningSessionStore/GetCurrentSession");
    }

    public async Task<LearningSessionResponse> LoadSpecificQuestion(int questionIndex)
    {
        return await testHarness.ApiGet<LearningSessionResponse>($"/apiVue/LearningSessionStore/LoadSpecificQuestion/{questionIndex}");
    }

    public async Task<StepResult[]> LoadSteps()
    {
        return await testHarness.ApiGet<StepResult[]>("/apiVue/LearningSessionStore/LoadSteps");
    }

    public async Task<LastStepInQuestionListResult> GetLastStepInQuestionList(int stepIndex)
    {
        return await testHarness.ApiGet<LastStepInQuestionListResult>($"/apiVue/LearningSessionStore/GetLastStepInQuestionList/{stepIndex}");
    }
}
