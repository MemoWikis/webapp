using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

public class LearningSessionCreator
{
    struct QuestionDetail
    {
        public bool NotLearned;
        public bool NeedsLearning;
        public bool NeedsConsolidation;
        public bool Solid;
        public bool InWuwi;
        public bool NotInWuwi;
        public bool CreatedByCurrentUser;
        public bool NotCreatedByCurrentUser;
        public bool Private;
        public bool Public;
        public bool FilterByKnowledgeSummary;
        public bool AddByWuwi;
        public bool AddByCreator;
        public bool AddByVisibility;
        public int PersonalCorrectnessProbability;
    }

    struct KnowledgeSummaryDetail
    {
        public int QuestionId;
        public int PersonalCorrectnessProbability;
    }

    public static LearningSession BuildLearningSession(LearningSessionConfig config)
    {
        var allQuestions = EntityCache.GetCategoryCacheItem(config.CategoryId)
            .GetAggregatedQuestionsFromMemoryCache(onlyVisible: true);

        var questionCounter = new QuestionCounter();
        var allQuestionValuation = UserCache.GetQuestionValuations(Sl.SessionUser.UserId);

        IList<Question> filteredQuestions = new List<Question>();
        IList<KnowledgeSummaryDetail> knowledgeSummaryDetails = new List<KnowledgeSummaryDetail>();

        if (Sl.SessionUser.IsLoggedIn)
        {
            foreach (var q in allQuestions)
            {
                var questionDetail = new QuestionDetail();

                questionDetail = FilterByCreator(config, q, questionDetail);
                questionDetail = FilterByVisibility(config, q, questionDetail);
                questionDetail = FilterByKnowledgeSummary(config, q, questionDetail, allQuestionValuation);

                if (questionDetail.AddByWuwi &&
                    questionDetail.AddByCreator &&
                    questionDetail.AddByVisibility &&
                    questionDetail.FilterByKnowledgeSummary)
                {
                    AddQuestionToFilteredList(filteredQuestions, questionDetail, questionCounter, q, knowledgeSummaryDetails);
                }
            }
        }
        else
        {
            filteredQuestions = allQuestions;
            questionCounter.Max = filteredQuestions.Count;
        }

        filteredQuestions = filteredQuestions.Shuffle();
        filteredQuestions = GetQuestionsByCount(config, filteredQuestions);
        filteredQuestions = SetQuestionOrder(filteredQuestions, config, knowledgeSummaryDetails);

        var learningSessionSteps = filteredQuestions
            .Distinct()
            .Select(q => new LearningSessionStep(q))
            .ToList();

        return new LearningSession(learningSessionSteps, config)
        {
            QuestionCounter = questionCounter
        };
    }

    private static void AddQuestionToFilteredList(IList<Question> filteredQuestions, QuestionDetail questionDetail,
        QuestionCounter questionCounter, Question question, IList<KnowledgeSummaryDetail> knowledgeSummaryDetails)
    {
        questionCounter = CountQuestionsForSessionConfig(questionDetail, questionCounter);

        if (Sl.SessionUser.IsLoggedIn)
            knowledgeSummaryDetails.Add(new KnowledgeSummaryDetail
            {
                PersonalCorrectnessProbability = questionDetail.PersonalCorrectnessProbability,
                QuestionId = question.Id
            });
            
        filteredQuestions.Add(question);
    }

    private static IList<Question> SetQuestionOrder(IList<Question> questions, LearningSessionConfig config, IList<KnowledgeSummaryDetail> knowledgeSummaryDetails)
    {
        if (config.QuestionOrder == QuestionOrder.SortByEasiest)
            return questions.OrderByDescending(q => q.CorrectnessProbability).ToList();

        if (config.QuestionOrder == QuestionOrder.SortByHardest)
            return questions.OrderBy(q => q.CorrectnessProbability).ToList();

        if (Sl.SessionUser.IsLoggedIn && config.QuestionOrder == QuestionOrder.SortByPersonalHardest)
        {
            var orderedKnowledgeSummaryDetails = knowledgeSummaryDetails.OrderBy(k => k.PersonalCorrectnessProbability).ToList();
            return questions.OrderBy(q => orderedKnowledgeSummaryDetails.IndexOf(o => q.Id == o.QuestionId)).ToList();
        }

        return questions;
    }

    private static QuestionCounter CountQuestionsForSessionConfig(QuestionDetail questionDetail, QuestionCounter counter)
    {
        if (questionDetail.NotLearned)
            counter.NotLearned++;

        if (questionDetail.NeedsLearning)
            counter.NeedsLearning++;

        if (questionDetail.NeedsConsolidation)
            counter.NeedsConsolidation++;

        if (questionDetail.Solid)
            counter.Solid++;

        if (questionDetail.InWuwi)
            counter.InWuwi++;

        if (questionDetail.NotInWuwi)
            counter.NotInWuwi++;

        if (questionDetail.CreatedByCurrentUser)
            counter.CreatedByCurrentUser++;

        if (questionDetail.NotCreatedByCurrentUser)
            counter.NotCreatedByCurrentUser++;

        if (questionDetail.Public)
            counter.Public++;

        if (questionDetail.Private)
            counter.Private++;

        counter.Max++;
        
        return counter;
    }

    private static IList<Question> GetQuestionsByCount(LearningSessionConfig config, IList<Question> questions)
    {
        if (config.MaxQuestionCount > questions.Count || config.MaxQuestionCount == -1 || config.MaxQuestionCount == 0)
            return questions;

        return questions.Take(config.MaxQuestionCount).ToList();
    }

    private static QuestionDetail FilterByCreator(LearningSessionConfig config, Question q, QuestionDetail questionDetail)
    {
        if (q.Creator == Sl.SessionUser.User)
        {
            if (config.CreatedByCurrentUser || !config.CreatedByCurrentUser && !config.NotCreatedByCurrentUser)
                questionDetail.AddByCreator = true;

            questionDetail.CreatedByCurrentUser = true;
        }
        else
        {
            if (config.NotCreatedByCurrentUser || !config.CreatedByCurrentUser && !config.NotCreatedByCurrentUser)
                questionDetail.AddByCreator = true;

            questionDetail.NotCreatedByCurrentUser = true;
        }

        return questionDetail;
    }

    private static QuestionDetail FilterByVisibility(LearningSessionConfig config, Question q, QuestionDetail questionDetail)
    {
        if (q.Visibility == QuestionVisibility.All)
        {
            if (config.PublicQuestions)
                questionDetail.AddByVisibility = true;

            questionDetail.Public = true;
        }
        else
        {
            if (config.PrivateQuestions)
                questionDetail.AddByVisibility = true;

            questionDetail.Private = true;
        }

        return questionDetail;
    }

    private static QuestionDetail FilterByKnowledgeSummary(LearningSessionConfig config, Question q, QuestionDetail questionDetail, IList<QuestionValuationCacheItem> allQuestionValuation)
    {
        if (Sl.SessionUser.IsLoggedIn)
        {
            var questionValuation = allQuestionValuation.FirstOrDefault(qv => qv.Question.Id == q.Id);

            questionDetail = FilterByWuwi(questionValuation, config, questionDetail);

            if (questionValuation == null || questionValuation.CorrectnessProbabilityAnswerCount <= 0)
            {
                if (config.NotLearned)
                    questionDetail.FilterByKnowledgeSummary = true;

                questionDetail.NotLearned = true;

                if (questionValuation != null)
                    questionDetail.PersonalCorrectnessProbability = questionValuation.CorrectnessProbability;
                else questionDetail.PersonalCorrectnessProbability = q.CorrectnessProbability;
            }
            else if (questionValuation.CorrectnessProbability <= 50)
            {
                if (config.NeedsLearning)
                    questionDetail.FilterByKnowledgeSummary = true;

                questionDetail.NeedsLearning = true;
                questionDetail.PersonalCorrectnessProbability = questionValuation.CorrectnessProbability;
            }
            else if (questionValuation.CorrectnessProbability > 50 && questionValuation.CorrectnessProbability < 80)
            {
                if (config.NeedsConsolidation)
                    questionDetail.FilterByKnowledgeSummary = true;

                questionDetail.NeedsConsolidation = true;
                questionDetail.PersonalCorrectnessProbability = questionValuation.CorrectnessProbability;
            }
            else if (questionValuation.CorrectnessProbability >= 80)
            {
                if (config.Solid)
                    questionDetail.FilterByKnowledgeSummary = true;

                questionDetail.Solid = true;
                questionDetail.PersonalCorrectnessProbability = questionValuation.CorrectnessProbability;
            }

        }
        else
        {
            questionDetail.FilterByKnowledgeSummary = true;
            questionDetail.NotLearned = true;
        }

        return questionDetail;
    }

    private static QuestionDetail FilterByWuwi(QuestionValuationCacheItem questionValuation, LearningSessionConfig config, QuestionDetail questionDetail)
    {
        if (questionValuation != null && questionValuation.IsInWishKnowledge)
        {
            if (config.InWuwi || !config.InWuwi && !config.NotInWuwi)
                questionDetail.AddByWuwi = true;

            questionDetail.InWuwi = true;
        }
        else
        {
            if (config.NotInWuwi || !config.InWuwi && !config.NotInWuwi)
                questionDetail.AddByWuwi = true;

            questionDetail.NotInWuwi = true;
        }

        return questionDetail;
    }
}