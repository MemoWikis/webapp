using System;
using System.Collections.Generic;
using System.Linq;

public class GetLearningSessionSteps
{

    public static IList<LearningSessionStep> Run(Set set)
    {
        var _allQuestions = set.Questions();
        return GetSteps(_allQuestions, numberOfSteps: 10);
    }


    public static IList<LearningSessionStep> GetSteps(IList<Question> allQuestions, int numberOfSteps)
    {
        var user = Sl.R<SessionUser>().User;

        var allQuestionsIds = allQuestions.Select(x => x.Id).ToList();

        var ids = allQuestions.GetIds();

        var allTotals = Sl.Resolve<TotalsPersUserLoader>().Run(user.Id, ids);
        var allValuations = Sl.Resolve<QuestionValuationRepo>().GetBy(allQuestionsIds, user.Id);
        var allAnswerHistories = Sl.Resolve<AnswerHistoryRepository>().GetBy(allQuestionsIds, user.Id);

        var rnd = new Random();

        var _unansweredQuestions = allQuestions
            .Where(q => allTotals.ByQuestionId(q.Id).Total() == 0)
            .OrderBy(q => rnd.Next()) //Randomize order
            .ToList();

        var _answeredQuestions = allQuestions
            .Except(_unansweredQuestions)
            .OrderBy(q => allValuations.ByQuestionId(q.Id).CorrectnessProbability)//Questions with lower probability first
            .ThenBy(q => rnd.Next())
            .ToList();

        var _questionsAnsweredToday = _answeredQuestions
            .Where(q => allAnswerHistories.ByQuestionId(q.Id).Any(ah => ah.DateCreated.Date == DateTime.Today))
            .ToList();

        var hf = allAnswerHistories.ByQuestionId(1);

        var _allAnsweredToday = !allQuestions.Except(_questionsAnsweredToday).Any();

        return GetSteps(numberOfSteps, _unansweredQuestions, _answeredQuestions, _questionsAnsweredToday);
    }

    private static IList<LearningSessionStep> GetSteps(int numberOfSteps, 
        IList<Question> unansweredQuestions,
        IList<Question> answeredQuestions,
        IList<Question> questionsAnsweredToday)
    {
        //ToDo: Refine selection algorithm:

        var allQuestionsExceptTodayOrdered = unansweredQuestions
            .Concat(answeredQuestions.Except(questionsAnsweredToday));

        var allQuestionsOrdered = allQuestionsExceptTodayOrdered
            .Concat(questionsAnsweredToday);

        if (numberOfSteps >= unansweredQuestions.Count)
        {

        }
        //.Except(UnansweredQuestions)
        //.Except(questionsAnsweredToday)

        return allQuestionsOrdered
            .Take(numberOfSteps)
            .Select(q => new LearningSessionStep {Question = q})
            .ToList();
    }
}
