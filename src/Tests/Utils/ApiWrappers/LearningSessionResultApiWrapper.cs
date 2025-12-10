public class LearningSessionResultApiWrapper(TestHarness _testHarness)
{
    public async Task<VueLearningSessionResultController.LearningSessionResult> Get() => 
        await _testHarness.ApiGet<VueLearningSessionResultController.LearningSessionResult>(
            "/apiVue/VueLearningSessionResult/Get"
        );

    public async Task<VueLearningSessionResultController.LearningSessionResult> GetForWishknowledge() => 
        await _testHarness.ApiGet<VueLearningSessionResultController.LearningSessionResult>(
            "/apiVue/VueLearningSessionResult/GetForWishknowledge"
        );
}
