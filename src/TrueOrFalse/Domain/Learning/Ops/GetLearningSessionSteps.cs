using System;
using System.Collections.Generic;
using System.Linq;

public class GetLearningSessionSteps
{
    static bool _allAnsweredToday;

    static IList<Question> _unansweredQuestions;
    static IList<Question> _answeredQuestions;
    static IList<Question> _questionsAnsweredToday;

    public static IList<LearningSessionStep> Run(Set set)
    {
        var _allQuestions = set.Questions();
        Prepare_QuestionsSubsets(_allQuestions);
        return GetSteps(numberOfQuestions: 10);
    }

    public static void Prepare_QuestionsSubsets(IList<Question> _allQuestions)
    {
        var user = Sl.R<SessionUser>().User;

        var allQuestionsIds = _allQuestions.Select(x => x.Id).ToList();

        var allTotals = Sl.Resolve<TotalsPersUserLoader>().Run(user.Id, _allQuestions.GetIds());
        var allValuations = Sl.Resolve<QuestionValuationRepo>().GetBy(allQuestionsIds, user.Id);
        var allAnswerHistories = Sl.Resolve<AnswerHistoryRepository>().GetBy(allQuestionsIds, user.Id);

        var rnd = new Random();

        _unansweredQuestions = _allQuestions
            .Where(q => allTotals.ByQuestionId(q.Id).Total() == 0)
            .OrderBy(q => rnd.Next()) //Randomize order
            .ToList();

        _answeredQuestions = _allQuestions
            .Except(_unansweredQuestions)
            .OrderBy(q => allValuations.ByQuestionId(q.Id).CorrectnessProbability)//Questions with lower probability
            .ThenBy(q => rnd.Next())
            .ToList();//Randomize order of questions with equal probability

        _questionsAnsweredToday = _answeredQuestions
            .Where(q => allAnswerHistories.ByQuestionId(q.Id).DateCreated.Date == DateTime.Today)
            .ToList();

        _allAnsweredToday = !_allQuestions.Except(_questionsAnsweredToday).Any();
    }

    private static IList<LearningSessionStep> GetSteps(int numberOfQuestions)
    {
        //ToDo: Refine selection algorithm:

        var allQuestionsExceptTodayOrdered = _unansweredQuestions
            .Concat(_answeredQuestions.Except(_questionsAnsweredToday));

        var allQuestionsOrdered = allQuestionsExceptTodayOrdered
            .Concat(_questionsAnsweredToday);

        if (numberOfQuestions >= _unansweredQuestions.Count)
        {

        }
        //.Except(UnansweredQuestions)
        //.Except(questionsAnsweredToday)

        return allQuestionsOrdered
            .Take(numberOfQuestions)
            .Select(q => new LearningSessionStep {Question = q})
            .ToList();
    }
}
