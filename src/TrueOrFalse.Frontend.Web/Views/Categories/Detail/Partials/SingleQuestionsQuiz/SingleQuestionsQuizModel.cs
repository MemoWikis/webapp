using System;
using System.Collections.Generic;
using System.Linq;

public class SingleQuestionsQuizModel : BaseContentModule
{
    public string Title;
    public string Text;
    public IList<Question> Questions;

    public SingleQuestionsQuizModel(Category category, int n) : this(category, new SingleQuestionsQuizJson { MaxQuestions = n })
    {
    }

    public SingleQuestionsQuizModel(Category category, SingleQuestionsQuizJson singleQuestionsQuizJson)
    {
        Title = String.IsNullOrEmpty(singleQuestionsQuizJson.Title) ? "Wie viel weißt du über das Thema " + category.Name + "?" : singleQuestionsQuizJson.Title;
        Text = singleQuestionsQuizJson.Text;

        if (string.IsNullOrEmpty(singleQuestionsQuizJson.QuestionIds))
        {
            Questions = category.GetAggregatedQuestionsFromMemoryCache().Where(q => q.IsVisibleToCurrentUser()).ToList();
        }
        else
        {
            var questionIdsList = singleQuestionsQuizJson.QuestionIds
                .Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Convert.ToInt32(x));

            Questions = questionIdsList
                .Select(questionId => Sl.QuestionRepo.GetById(questionId))
                .Where(q => q != null)
                .Where(q => q.IsVisibleToCurrentUser())
                .ToList();
        }

        switch (singleQuestionsQuizJson.Order)
        {
            case "ViewsDescending":
                Questions = Questions.OrderByDescending(q => q.TotalViews).ToList();
                break;

            case "CorrectnessProbabilityAscending":
                Questions = Questions.OrderBy(q => q.CorrectnessProbability).ToList();
                break;

            case "CorrectnessProbabilityDescending":
                Questions = Questions.OrderByDescending(q => q.CorrectnessProbability).ToList();
                break;

            case null:
            case "":
            case "Random":
            default:
                Random rnd = new Random();
                Questions = Questions.OrderByDescending(q => rnd.Next()).ToList();
                break;
        }

        Questions = Questions.Take(singleQuestionsQuizJson.MaxQuestions).ToList();
    }

}
