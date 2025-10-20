using System.Collections.Concurrent;

public class QuestionFilterService(
    SessionUser _sessionUser,
    ExtendedUserCache _extendedUserCache) : IRegisterAsInstancePerLifetime
{
    /// <summary>
    /// Builds question properties based on various filter criteria
    /// </summary>
    /// <param name="question">The question to evaluate</param>
    /// <param name="config">Learning session configuration</param>
    /// <param name="allQuestionValuations">All available question valuations</param>
    /// <param name="userQuestionValuations">User-specific question valuations</param>
    /// <returns>Properties indicating filter results</returns>
    public QuestionProperties BuildQuestionProperties(
        QuestionCacheItem question,
        LearningSessionConfig config,
        IList<QuestionValuationCacheItem> allQuestionValuations,
        ConcurrentDictionary<int, QuestionValuationCacheItem>? userQuestionValuations)
    {
        var questionProperties = new QuestionProperties();

        questionProperties = FilterByCreator(config, question, questionProperties);
        questionProperties = FilterByVisibility(config, question, questionProperties);

        if (_sessionUser.IsLoggedIn)
        {
            if (userQuestionValuations != null && userQuestionValuations.TryGetValue(question.Id, out QuestionValuationCacheItem? userValuation))
            {
                questionProperties = FilterByWishKnowledge(userValuation, config, questionProperties);
                questionProperties = FilterByKnowledgeStatusFromUserValuation(config, question, questionProperties, userValuation);
            }
            else
            {
                var questionValuation = allQuestionValuations.FirstOrDefault(qv => qv.Question.Id == question.Id);
                questionProperties = FilterByWishKnowledge(questionValuation, config, questionProperties);
                questionProperties = FilterByKnowledgeStatus(config, question, questionProperties, questionValuation);
            }
        }
        else
        {
            questionProperties.NotLearned = true;
        }

        return questionProperties;
    }

    public IList<QuestionCacheItem> FilterQuestions(
        IList<QuestionCacheItem> allQuestions,
        LearningSessionConfig config,
        int userId)
    {
        var filteredQuestions = new List<QuestionCacheItem>();
        var allQuestionValuations = _extendedUserCache.GetQuestionValuations(userId);

        if (_sessionUser.IsLoggedIn)
        {
            foreach (var question in allQuestions)
            {
                var userQuestionValuations = _extendedUserCache.GetItem(userId)?.QuestionValuations;
                var questionProperties = BuildQuestionProperties(question, config, allQuestionValuations, userQuestionValuations);

                if (questionProperties.AddToLearningSession)
                {
                    filteredQuestions.Add(question);
                }
            }
        }
        else
        {
            filteredQuestions = allQuestions.ToList();
        }

        return filteredQuestions;
    }

    private static QuestionProperties FilterByCreator(
        LearningSessionConfig config,
        QuestionCacheItem questionCacheItem,
        QuestionProperties questionProperties)
    {
        if (questionCacheItem.CreatorId == config.CurrentUserId)
        {
            questionProperties.CreatedByCurrentUser = true;

            if (!config.CreatedByCurrentUser && config.NotCreatedByCurrentUser)
            {
                questionProperties.AddToLearningSession = false;
            }
        }
        else
        {
            questionProperties.NotCreatedByCurrentUser = true;

            if (!config.NotCreatedByCurrentUser && config.CreatedByCurrentUser)
            {
                questionProperties.AddToLearningSession = false;
            }
        }

        return questionProperties;
    }

    private static QuestionProperties FilterByVisibility(
        LearningSessionConfig config,
        QuestionCacheItem question,
        QuestionProperties questionProperties)
    {
        if (question.Visibility == QuestionVisibility.Public)
        {
            questionProperties.Public = true;

            if (!config.PublicQuestions && config.PrivateQuestions)
            {
                questionProperties.AddToLearningSession = false;
            }
        }
        else
        {
            questionProperties.Private = true;

            if (!config.PrivateQuestions && config.PublicQuestions)
            {
                questionProperties.AddToLearningSession = false;
            }
        }

        return questionProperties;
    }

    private static QuestionProperties FilterByKnowledgeStatus(
        LearningSessionConfig config,
        QuestionCacheItem question,
        QuestionProperties questionProperties,
        QuestionValuationCacheItem? questionValuation)
    {
        if (questionValuation is not { CorrectnessProbabilityAnswerCount: > 0 })
        {
            questionProperties.NotLearned = true;

            if (!config.NotLearned)
            {
                questionProperties.AddToLearningSession = false;
            }
        }
        else if (questionValuation.CorrectnessProbability <= 50)
        {
            questionProperties.NeedsLearning = true;

            if (!config.NeedsLearning)
            {
                questionProperties.AddToLearningSession = false;
            }
        }
        else if (questionValuation.CorrectnessProbability is > 50 and < 80)
        {
            questionProperties.NeedsConsolidation = true;

            if (!config.NeedsConsolidation)
            {
                questionProperties.AddToLearningSession = false;
            }
        }
        else if (questionValuation.CorrectnessProbability >= 80)
        {
            questionProperties.Solid = true;

            if (!config.Solid)
            {
                questionProperties.AddToLearningSession = false;
            }
        }

        questionProperties.PersonalCorrectnessProbability =
            questionValuation?.CorrectnessProbability ?? question.CorrectnessProbability;

        // If user deselected all input, default settings should take place (add all)
        if (config is { NotLearned: false, NeedsConsolidation: false, NeedsLearning: false, Solid: false })
        {
            questionProperties.AddToLearningSession = true;
        }

        return questionProperties;
    }

    private static QuestionProperties FilterByKnowledgeStatusFromUserValuation(
        LearningSessionConfig config,
        QuestionCacheItem question,
        QuestionProperties questionProperties,
        QuestionValuationCacheItem? questionValuation)
    {
        if (questionValuation.CorrectnessProbabilityAnswerCount <= 0)
        {
            questionProperties.NotLearned = true;

            if (!config.NotLearned)
            {
                questionProperties.AddToLearningSession = false;
            }
        }
        else if (questionValuation.KnowledgeStatus == KnowledgeStatus.NeedsLearning)
        {
            questionProperties.NeedsLearning = true;

            if (!config.NeedsLearning)
            {
                questionProperties.AddToLearningSession = false;
            }
        }
        else if (questionValuation.KnowledgeStatus == KnowledgeStatus.NeedsConsolidation)
        {
            questionProperties.NeedsConsolidation = true;

            if (!config.NeedsConsolidation)
            {
                questionProperties.AddToLearningSession = false;
            }
        }
        else if (questionValuation.KnowledgeStatus == KnowledgeStatus.Solid)
        {
            questionProperties.Solid = true;

            if (!config.Solid)
            {
                questionProperties.AddToLearningSession = false;
            }
        }

        questionProperties.PersonalCorrectnessProbability =
            questionValuation?.CorrectnessProbability ?? question.CorrectnessProbability;

        // If user deselected all input, default settings should take place (add all)
        if (!config.NotLearned &&
            !config.NeedsConsolidation &&
            !config.NeedsLearning &&
            !config.Solid
           )
        {
            questionProperties.AddToLearningSession = true;
        }

        return questionProperties;
    }

    private static QuestionProperties FilterByWishKnowledge(
        QuestionValuationCacheItem? questionValuation,
        LearningSessionConfig config,
        QuestionProperties questionProperties)
    {
        if (questionValuation is { IsInWishknowledge: true })
        {
            questionProperties.InWishknowledge = true;

            if (!config.InWishknowledge && config.NotWishKnowledge)
            {
                questionProperties.AddToLearningSession = false;
            }
        }
        else
        {
            questionProperties.NotInWishknowledge = true;

            if (!config.NotWishKnowledge && config.InWishknowledge)
            {
                questionProperties.AddToLearningSession = false;
            }
        }

        return questionProperties;
    }
}
