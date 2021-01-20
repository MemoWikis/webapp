using System.Collections.Generic;
using System.Linq;
using BDDish.Model;
using TrueOrFalse.Tests;

internal static class ContextLearningSession
{
    public static List<LearningSessionStep> GetSteps(int amountQuestions)
    {
        var learningSession = GetLearningSessionForAnonymusUser(amountQuestions);

        return learningSession.Steps.ToList();
    }

    public static LearningSession GetLearningSessionForAnonymusUser(int amountQuestions)
    {
        ContextQuestion.PutQuestionsIntoMemoryCache();
        var learningSession = LearningSessionCreator.ForAnonymous(
            new LearningSessionConfig
            {
                CategoryId = 1,
                MaxQuestionCount = amountQuestions,
                CurrentUserId = -1,
                MaxProbability = 100
                
            });
        return learningSession;
    }

    public static LearningSession GetLearningSessionWithUser(int userId, int amountQuestions)
    {
        ContextQuestion.PutQuestionsIntoMemoryCache(amountQuestions);
        return new LearningSession(GetSteps(amountQuestions), new LearningSessionConfig { CurrentUserId = userId, CategoryId = 1});
    }

    public static LearningSession GetLearningSession(LearningSessionConfig config )
    {

        ContextQuestion.PutQuestionsIntoMemoryCache();
        if (config.CurrentUserId == -1)
        {
            return LearningSessionCreator.ForAnonymous(config);
        }

        return LearningSessionCreator.ForLoggedInUser(config); 
        

    }
}