using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Tests;

internal class ContextLearningSession : BaseTest
{
    public static List<LearningSessionStep> GetSteps(int amountQuestionInMemory, int amountQuestions = 20)
    {
        var learningSession = GetLearningSessionForAnonymusUser(amountQuestionInMemory, amountQuestions);

        return learningSession.Steps.ToList();
    }

    public static LearningSession GetLearningSessionForAnonymusUser(int amountQuestions, int amountQuestionInMemory = 20)
    {
        ContextQuestion.PutQuestionsIntoMemoryCache(amountQuestionInMemory);
        var learningSession =Resolve<LearningSessionCreator>().BuildLearningSession(
            new LearningSessionConfig
            {
                CategoryId = 1,
                MaxQuestionCount = amountQuestions,
            });
        return learningSession;
    }

    public static LearningSession GetLearningSessionWithUser(LearningSessionConfig config)
    {
        ContextQuestion.PutQuestionsIntoMemoryCache(config.MaxQuestionCount);
        return new LearningSession(GetSteps(config.MaxQuestionCount), config);
    }

    public static LearningSession GetLearningSession(LearningSessionConfig config )
    {
        ContextQuestion.PutQuestionsIntoMemoryCache();
        return Resolve<LearningSessionCreator>().BuildLearningSession(config);
    }
}