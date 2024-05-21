using Microsoft.AspNetCore.Mvc;

namespace TrueOrFalse.View.Web.Views.ApiVueApp.Stores;

public class LearningSessionConfigurationStoreController : BaseController
{
    private readonly LearningSessionCreator _learningSessionCreator;

    public LearningSessionConfigurationStoreController(
        SessionUser sessionUser,
        LearningSessionCreator learningSessionCreator) : base(sessionUser)
    {
        _learningSessionCreator = learningSessionCreator;
    }

    [HttpPost]
    public QuestionCounter GetCount([FromBody] LearningSessionConfig config)
    {
        return _learningSessionCreator.GetQuestionCounterForLearningSession(config);
    }
}