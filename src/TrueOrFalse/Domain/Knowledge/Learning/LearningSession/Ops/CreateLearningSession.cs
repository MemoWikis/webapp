using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using NHibernate;
using SolrNet.Impl.FacetQuerySerializers;

public class CreateLearningSession
{
    public static LearningSession ForCategory(int categoryId, string questionFilterString = "")
    {
        var category = Sl.CategoryRepo.GetByIdEager(categoryId);

        var questions = category.GetAggregatedQuestionsFromMemoryCache();

        if (questions.Count == 0)
            throw new Exception("Cannot start LearningSession with 0 questions.");
        if (questionFilterString.Length > 0)
        {
            var newQuestions = FilterQuestions(questions, questionFilterString);
        }

        var user = Sl.R<SessionUser>().User;

        var learningSession = new LearningSession
        {
            CategoryToLearn = category,
            Steps = GetLearningSessionSteps.Run(questions),
            User = user
        };

        Sl.LearningSessionRepo.Create(learningSession);

        return learningSession;
    }

    public static IList<Question> FilterQuestions(IList<Question> questions, string questionFilterString)
    {
        var user = Sl.R<SessionUser>().User;

        var questionValuation = UserValuationCache.GetItem(user.Id).QuestionValuations;
        var questionFilter = JsonConvert.DeserializeObject<QuestionFilterJson>(questionFilterString);

        if (questionFilter.GetQuestionOrderBy() == "DescendingProbability")
            questions.OrderByDescending(q => q.CorrectnessProbability);
        else if (questionFilter.GetQuestionOrderBy() == "AscendingProbability")
            questions.OrderBy(q => q.CorrectnessProbability);

        var filteredQuestions = questions
            .Where(
            q => q.CorrectnessProbability > questionFilter.MinProbability &&
                 q.CorrectnessProbability < questionFilter.MaxProbability)
            .Take(questionFilter.MaxQuestionCount)
            .ToList();
        return filteredQuestions;
    }
}
