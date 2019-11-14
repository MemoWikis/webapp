using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Mapping;
using SolrNet.Impl.FacetQuerySerializers;

public class CreateLearningSession
{
    public static LearningSession ForCategory(int categoryId, QuestionFilterJson questionFilterJson = null)
    {
        var category = Sl.CategoryRepo.GetByIdEager(categoryId);

        var questions = category.GetAggregatedQuestionsFromMemoryCache();

        if (questions.Count == 0)
            throw new Exception("Cannot start LearningSession with 0 questions.");

        var user = Sl.R<SessionUser>().User;

        if (questionFilterJson != null)
            questions = FilterQuestions(questions, questionFilterJson, user);

        var stepsCount = 10;
        if (questionFilterJson != null && questionFilterJson.MaxQuestionCount > 0)
            stepsCount = questionFilterJson.MaxQuestionCount;

        var learningSession = new LearningSession
        {
            CategoryToLearn = category,
            Steps = GetLearningSessionSteps.Run(questions, stepsCount),
            User = user
        };

        Sl.LearningSessionRepo.Create(learningSession);

        return learningSession;
    }

    public static IList<Question> FilterQuestions(IList<Question> questions, QuestionFilterJson questionFilter, User user)
    {
        var questionValuation = UserValuationCache.GetItem(user.Id).QuestionValuations;
        var newQuestionsList = new List<Question>();

        foreach (Question q in questions)
        {
            var elemToRemove = false;
            var isInWishknowledge = false;
            if (questionValuation.ContainsKey(q.Id))
            {
                q.CorrectnessProbability = questionValuation[q.Id].CorrectnessProbability;
                isInWishknowledge = questionValuation[q.Id].IsInWishKnowledge();
                elemToRemove = true;
            }

            if ((questionFilter.QuestionsInWishknowledge && isInWishknowledge) || !questionFilter.QuestionsInWishknowledge)
                newQuestionsList.Add(q);

            if (elemToRemove)
            {
                questionValuation.TryRemove(q.Id, out _);
            }
        }

        var questionCount = questionFilter.HasMaxQuestionCount() ? questionFilter.MaxQuestionCount : newQuestionsList.Count;

        var filteredQuestions = newQuestionsList
            .Where(
            q => q.CorrectnessProbability > questionFilter.MinProbability &&
                 q.CorrectnessProbability < questionFilter.MaxProbability)
            .ToList();

        if (questionFilter.GetQuestionOrderBy() == "DescendingProbability")
            filteredQuestions.OrderByDescending(q => q.CorrectnessProbability);
        else if (questionFilter.GetQuestionOrderBy() == "AscendingProbability")
            filteredQuestions.OrderBy(q => q.CorrectnessProbability);

        filteredQuestions.Take(questionCount).ToList();

        return filteredQuestions;
    }

    public static int QuestionsInLearningSessionCount(int categoryId, QuestionFilterJson questionFilter, bool isLoggedIn)
    {
        var category = Sl.CategoryRepo.GetByIdEager(categoryId);
        var questions = category.GetAggregatedQuestionsFromMemoryCache();

        if (isLoggedIn)
        {
            var user = Sl.R<SessionUser>().User;
            questions = FilterQuestions(questions, questionFilter, user);
            return questions.Count;
        }

        return questions.Where(q => q.CorrectnessProbability > questionFilter.MinProbability && q.CorrectnessProbability < questionFilter.MaxProbability).ToList().Count;
    }
}
