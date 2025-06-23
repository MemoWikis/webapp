public class LearningSessionResultApiWrapper(TestHarness _testHarness)
{
    public async Task<VueLearningSessionResultController.LearningSessionResult> Get() => 
        await _testHarness.ApiGet<VueLearningSessionResultController.LearningSessionResult>(
            "/apiVue/VueLearningSessionResult/Get"
        );
}
