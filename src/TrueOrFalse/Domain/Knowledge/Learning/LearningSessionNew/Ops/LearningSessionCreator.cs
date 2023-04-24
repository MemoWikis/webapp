using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

public class LearningSessionCreator
{
    public struct QuestionDetail
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
        IList<QuestionCacheItem> allQuestions = EntityCache.GetCategory(config.CategoryId).GetAggregatedQuestionsFromMemoryCache().Where(q => q.Id > 0).ToList();

        var questionCounter = new QuestionCounter();
        var allQuestionValuation = SessionUserCache.GetQuestionValuations(SessionUser.UserId);

        IList<QuestionCacheItem> filteredQuestions = new List<QuestionCacheItem>();
        IList<KnowledgeSummaryDetail> knowledgeSummaryDetails = new List<KnowledgeSummaryDetail>();

        if (SessionUser.IsLoggedIn)
        {
            foreach (var q in allQuestions)
            {
                var questionDetail = BuildQuestionDetail(config, q, allQuestionValuation);

                if (questionDetail.AddByWuwi &&
                    questionDetail.AddByCreator &&
                    questionDetail.AddByVisibility &&
                    questionDetail.FilterByKnowledgeSummary)
                {
                    AddQuestionToFilteredList(filteredQuestions, questionDetail, q, knowledgeSummaryDetails);
                    questionCounter.Max++;
                }
                questionCounter = CountQuestionsForSessionConfig(questionDetail, questionCounter);
            }
        }
        else
        {
            filteredQuestions = allQuestions;
            questionCounter.Max = filteredQuestions.Count;
            questionCounter.NotInWuwi = filteredQuestions.Count;
            questionCounter.NotCreatedByCurrentUser = filteredQuestions.Count;
            questionCounter.NotLearned = filteredQuestions.Count;
            questionCounter.Public = filteredQuestions.Count;
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

    public static LearningSession BuildLearningSessionWithSpecificQuestion(LearningSessionConfig config, int id, IList<QuestionCacheItem> allQuestions)
    {
        var ad = LearningSessionCache.TryRemove();


        var questionCounter = new QuestionCounter();
        var allQuestionValuation = SessionUserCache.GetQuestionValuations(SessionUser.UserId);

        IList<QuestionCacheItem> filteredQuestions = new List<QuestionCacheItem>();
        IList<KnowledgeSummaryDetail> knowledgeSummaryDetails = new List<KnowledgeSummaryDetail>();

        if (SessionUser.IsLoggedIn)
        {
            foreach (var q in allQuestions)
            {
                var questionDetail = BuildQuestionDetail(config, q, allQuestionValuation);

                if (questionDetail.AddByWuwi &&
                    questionDetail.AddByCreator &&
                    questionDetail.AddByVisibility &&
                    questionDetail.FilterByKnowledgeSummary)
                {
                    AddQuestionToFilteredList(filteredQuestions, questionDetail, q, knowledgeSummaryDetails);
                    questionCounter.Max++;
                }
                questionCounter = CountQuestionsForSessionConfig(questionDetail, questionCounter);
            }
        }
        else
        {
            filteredQuestions = allQuestions;
            questionCounter.Max = filteredQuestions.Count;
            questionCounter.NotInWuwi = filteredQuestions.Count;
            questionCounter.NotCreatedByCurrentUser = filteredQuestions.Count;
            questionCounter.NotLearned = filteredQuestions.Count;
            questionCounter.Public = filteredQuestions.Count;
        }

        if (filteredQuestions.IndexOf(q => q.Id == id) < 0)
            return null;

        filteredQuestions = filteredQuestions.Where(q => q.Id != id).ToList();
        filteredQuestions = filteredQuestions.Shuffle();
        config.MaxQuestionCount -= 1;
        filteredQuestions = GetQuestionsByCount(config, filteredQuestions);
        config.MaxQuestionCount += 1;
        filteredQuestions.Add(EntityCache.GetQuestion(id));
        filteredQuestions = filteredQuestions.Shuffle();
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


    public static QuestionDetail BuildQuestionDetail(LearningSessionConfig config, QuestionCacheItem q,
        IList<QuestionValuationCacheItem> allQuestionValuation)
    {
        var questionDetail = new QuestionDetail();

        questionDetail = FilterByCreator(config, q, questionDetail);
        questionDetail = FilterByVisibility(config, q, questionDetail);
        questionDetail = FilterByKnowledgeSummary(config, q, questionDetail, allQuestionValuation);
        return questionDetail;
    }

    private static void AddQuestionToFilteredList(IList<QuestionCacheItem> filteredQuestions, QuestionDetail questionDetail, QuestionCacheItem question, IList<KnowledgeSummaryDetail> knowledgeSummaryDetails)
    {

        if (SessionUser.IsLoggedIn)
            knowledgeSummaryDetails.Add(new KnowledgeSummaryDetail
            {
                PersonalCorrectnessProbability = questionDetail.PersonalCorrectnessProbability,
                QuestionId = question.Id
            });
            
        filteredQuestions.Add(question);
    }

    private static IList<QuestionCacheItem> SetQuestionOrder(IList<QuestionCacheItem> questions, LearningSessionConfig config, IList<KnowledgeSummaryDetail> knowledgeSummaryDetails)
    {
        if (config.QuestionOrder == QuestionOrder.SortByEasiest)
            return questions.OrderByDescending(q => q.CorrectnessProbability).ToList();

        if (config.QuestionOrder == QuestionOrder.SortByHardest)
            return questions.OrderBy(q => q.CorrectnessProbability).ToList();

        if (SessionUser.IsLoggedIn && config.QuestionOrder == QuestionOrder.SortByPersonalHardest)
        {
            var orderedKnowledgeSummaryDetails = knowledgeSummaryDetails.OrderBy(k => k.PersonalCorrectnessProbability).ToList();
            return questions.OrderBy(q => orderedKnowledgeSummaryDetails.IndexOf(o => q.Id == o.QuestionId)).ToList();
        }

        return questions;
    }

    public static QuestionCounter CountQuestionsForSessionConfig(QuestionDetail questionDetail, QuestionCounter counter)
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

        return counter;
    }

    private static IList<QuestionCacheItem> GetQuestionsByCount(LearningSessionConfig config, IList<QuestionCacheItem> questions)
    {
        if (config.MaxQuestionCount > questions.Count || config.MaxQuestionCount == -1 || config.MaxQuestionCount == 0)
            return questions;

        return questions.Take(config.MaxQuestionCount).ToList();
    }

    private static QuestionDetail FilterByCreator(LearningSessionConfig config, QuestionCacheItem q, QuestionDetail questionDetail)
    {
        if (q.Creator?.Id == config.CurrentUserId)
        {
            if (config.CreatedByCurrentUser || (!config.CreatedByCurrentUser && !config.NotCreatedByCurrentUser))
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

    private static QuestionDetail FilterByVisibility(LearningSessionConfig config, QuestionCacheItem q, QuestionDetail questionDetail)
    {
        if (q.Visibility == QuestionVisibility.All)
        {
            if (config.PublicQuestions || !config.PrivateQuestions && !config.PublicQuestions)
                questionDetail.AddByVisibility = true;

            questionDetail.Public = true;
        }
        else
        {
            if (config.PrivateQuestions || !config.PrivateQuestions && !config.PublicQuestions)
                questionDetail.AddByVisibility = true;

            questionDetail.Private = true;
        }

        return questionDetail;
    }

    private static QuestionDetail FilterByKnowledgeSummary(LearningSessionConfig config, QuestionCacheItem q, QuestionDetail questionDetail, IList<QuestionValuationCacheItem> allQuestionValuation)
    {
        if (SessionUser.IsLoggedIn)
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