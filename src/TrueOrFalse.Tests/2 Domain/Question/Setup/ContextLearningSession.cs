using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Tests;

internal class ContextLearningSession : BaseTest
{
    public static List<LearningSessionStep> GetSteps(int amountQuestionInMemory, int amountQuestions = 20)
    {
        var learningSession = GetLearningSessionForAnonymusUser(amountQuestionInMemory, R<CategoryRepository>(),R<LearningSessionCreator>(), amountQuestions);

        return learningSession.Steps.ToList();
    }

    public static LearningSession GetLearningSessionForAnonymusUser(int amountQuestions,
        CategoryRepository categoryRepository,
        LearningSessionCreator learningSessionCreator,
        int amountQuestionInMemory = 20)
    {
        ContextQuestion.PutQuestionsIntoMemoryCache(categoryRepository, amountQuestionInMemory);
        var learningSession =learningSessionCreator.BuildLearningSession(
            new LearningSessionConfig
            {
                CategoryId = 1,
                MaxQuestionCount = amountQuestions,
            });
        return learningSession;
    }

    public static LearningSession GetLearningSessionWithUser(LearningSessionConfig config, CategoryRepository categoryRepository)
    {
        ContextQuestion.PutQuestionsIntoMemoryCache(categoryRepository, config.MaxQuestionCount);
        return new LearningSession(GetSteps(config.MaxQuestionCount), config);
    }

    public static LearningSession GetLearningSession(LearningSessionConfig config )
    {
        ContextQuestion.PutQuestionsIntoMemoryCache(R<CategoryRepository>());
        return Resolve<LearningSessionCreator>().BuildLearningSession(config);
    }
}