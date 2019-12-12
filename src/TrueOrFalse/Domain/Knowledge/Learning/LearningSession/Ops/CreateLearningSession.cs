using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        Stopwatch timer = new Stopwatch();
        timer.Start();
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

        timer.Stop();
        Logg.r().Information("Create LearningSession CategoryId {categoryId} with question count of {questionsCount}, Time elapsed: {stopwatchElapsed}",categoryId, questions.Count, timer.Elapsed);
        return learningSession;
    }

    public static IList<Question> FilterQuestions(IList<Question> questions, QuestionFilterJson questionFilter, User user)
    {
        Stopwatch timer = new Stopwatch();
        timer.Start();

        var questionValuation = UserCache.GetItem(user.Id).QuestionValuations;
        var newQuestionsList = new List<Question>();

        foreach (Question q in questions)
        {
            var isInWishknowledge = false;
            if (questionValuation.ContainsKey(q.Id))
            {
                q.CorrectnessProbability = questionValuation[q.Id].CorrectnessProbability;
                isInWishknowledge = questionValuation[q.Id].IsInWishKnowledge();
            }

            if ((questionFilter.QuestionsInWishknowledge && isInWishknowledge) || !questionFilter.QuestionsInWishknowledge)
                newQuestionsList.Add(q);
        }

        var filteredQuestions = newQuestionsList
            .Where(
            q => q.CorrectnessProbability >= questionFilter.MinProbability &&
                 q.CorrectnessProbability <= questionFilter.MaxProbability)
            .ToList();

        var questionCount = newQuestionsList.Count;
        if (questionFilter.HasMaxQuestionCount())
            questionCount = questionFilter.MaxQuestionCount + (newQuestionsList.Count / 4);

        if (questionFilter.GetQuestionOrderBy() == "HighProbability")
            filteredQuestions = filteredQuestions.OrderByDescending(f => f.CorrectnessProbability).ToList();
        else if (questionFilter.GetQuestionOrderBy() == "LowProbability")
            filteredQuestions = filteredQuestions.OrderBy(f => f.CorrectnessProbability).ToList();

        if (questionFilter.IsTestMode())
            filteredQuestions = filteredQuestions.Shuffle().ToList();
        
        filteredQuestions = filteredQuestions.Take(questionCount).ToList();
        
        timer.Stop();
        Logg.r().Information("Filtering {questionCount} questions, Time elapsed {stopwatchElapsed}", questions.Count, timer.Elapsed);
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
