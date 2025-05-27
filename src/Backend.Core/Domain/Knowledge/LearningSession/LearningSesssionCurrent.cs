public class LearningSessionCurrent(
    LearningSessionCache _learningSessionCache,
    LearningSessionResultService _resultService
)
{
    public LearningSessionResultStep GetCurrentSession()
    {
        var learningSessionCached = _learningSessionCache.GetLearningSession();
        var result = new LearningSessionResultStep();

        if (learningSessionCached.Steps.Any())
        {
            var index = learningSessionCached
                .Steps
                .IndexOf(step => step.AnswerState == AnswerState.Unanswered);

            learningSessionCached.LoadSpecificQuestion(index);
            result = _resultService.FillLearningSessionResult(learningSessionCached, result);
        }

        return result;
    }
}