using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Tests;

internal static class ContextLearningSession
{
    public static IList<LearningSessionStepNew> GetSteps(int amountQuestions)
    {
        var learningSession = Get(amountQuestions);

        return learningSession.Steps;
    }

    public static LearningSessionNew Get(int amountQuestions)
    {
        ContextQuestion.PutQuestionsIntoMemoryCache();
        var learningSession = LearningSessionNewCreator.ForAnonymous(
            new LearningSessionConfig
            {
                CategoryId = 0,
                MaxQuestions = amountQuestions
            });
        return learningSession;
    }

    public static LearningSessionNew GetLearningSessionWithoutAnswerState(int userId, int amountQuestions)
    {
        ContextQuestion.PutQuestionsIntoMemoryCache(amountQuestions);
        var steps = GetSteps(amountQuestions);
        return new LearningSessionNew(steps.ToList(), new LearningSessionConfig { UserId = userId });
    }
}