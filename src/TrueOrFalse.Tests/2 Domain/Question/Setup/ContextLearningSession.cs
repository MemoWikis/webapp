using System.Collections.Generic;
using System.Linq;
using BDDish.Model;
using TrueOrFalse.Tests;

internal static class ContextLearningSession
{
    public static List<LearningSessionStepNew> GetSteps(int amountQuestions)
    {
        var learningSession = GetLearningSessionForAnonymusUser(amountQuestions);

        return learningSession.Steps.ToList();
    }

    public static LearningSessionNew GetLearningSessionForAnonymusUser(int amountQuestions)
    {
        ContextQuestion.PutQuestionsIntoMemoryCache();
        var learningSession = LearningSessionNewCreator.ForAnonymous(
            new LearningSessionConfig
            {
                CategoryId = 0,
                MaxQuestions = amountQuestions,
                UserId = -1
            });
        return learningSession;
    }

    public static LearningSessionNew GetLearningSessionWithUser(int userId, int amountQuestions)
    {
        ContextQuestion.PutQuestionsIntoMemoryCache(amountQuestions);
        return new LearningSessionNew(GetSteps(amountQuestions), new LearningSessionConfig { UserId = userId });
    }

    public static LearningSessionNew GetLearningSession(LearningSessionConfig config )
    {

        ContextQuestion.PutQuestionsIntoMemoryCache();
        if (config.UserId == -1)
        {
            return LearningSessionNewCreator.ForAnonymous(config);
        }

        return LearningSessionNewCreator.ForLoggedInUser(config); 
        

    }
}