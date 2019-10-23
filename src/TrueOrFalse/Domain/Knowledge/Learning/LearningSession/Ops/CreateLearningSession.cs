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

        if (questions != null)
        {
            questions = FilterQuestions(questions, questionFilterJson, user);
        }


        var learningSession = new LearningSession
        {
            CategoryToLearn = category,
            Steps = GetLearningSessionSteps.Run(questions),
            User = user
        };

        Sl.LearningSessionRepo.Create(learningSession);

        return learningSession;
    }

    public static IList<Question> FilterQuestions(IList<Question> questions, QuestionFilterJson questionFilter, User user)
    {
        var questionValuation = UserValuationCache.GetItem(user.Id).QuestionValuations;
        Dictionary<int, int> probabilityDictionary = new Dictionary<int, int>();
        var newQuestionsList = new List<Question>();

        foreach (Question q in questions)
        {
            var elemToRemove = false;
            if (questionValuation.ContainsKey(q.Id))
            {
                q.CorrectnessProbability = questionValuation[q.Id].CorrectnessProbability;
                elemToRemove = true;
            }
            newQuestionsList.Add(q);
            if (elemToRemove)
            {
                questionValuation.TryRemove(q.Id, out _);
            }
        }

        if (questionFilter.GetQuestionOrderBy() == "DescendingProbability")
            newQuestionsList.OrderByDescending(q => q.CorrectnessProbability);
        else if (questionFilter.GetQuestionOrderBy() == "AscendingProbability")
            newQuestionsList.OrderBy(q => q.CorrectnessProbability);

        var filteredQuestions = newQuestionsList
            .Where(
            q => q.CorrectnessProbability > questionFilter.MinProbability &&
                 q.CorrectnessProbability < questionFilter.MaxProbability)
            .Take(questionFilter.MaxQuestionCount)
            .ToList();
        return filteredQuestions;
    }
}
