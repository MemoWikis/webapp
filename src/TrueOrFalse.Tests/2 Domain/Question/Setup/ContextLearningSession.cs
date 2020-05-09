using System.Collections.Generic;
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
}