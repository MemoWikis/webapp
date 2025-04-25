namespace TrueOrFalse.View.Web.Views.ApiVueApp.Stores;

public class LearningSessionConfigurationStoreController(LearningSessionCreator _learningSessionCreator) : ApiBaseController
{
    [HttpPost]
    public QuestionCounter GetCount([FromBody] LearningSessionConfig config) => 
        _learningSessionCreator.GetQuestionCounterForLearningSession(config);
}