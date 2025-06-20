using static VueLearningSessionResultController;

public class LearningSessionResultApiWrapper
{
    private readonly TestHarness testHarness;

    public LearningSessionResultApiWrapper(TestHarness testHarness)
    {
        this.testHarness = testHarness;
    }

    public async Task<VueLearningSessionResultController.LearningSessionResult> Get()
    {
        return await testHarness.ApiGet<VueLearningSessionResultController.LearningSessionResult>("/apiVue/LearningSessionResult/Get");
    }
}
