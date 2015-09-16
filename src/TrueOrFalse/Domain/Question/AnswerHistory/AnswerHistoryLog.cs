using System;
using System.Linq;

public class AnswerHistoryLog : IRegisterAsInstancePerLifetime
{
    private readonly AnswerHistoryRepo _answerHistoryRepo;

    public AnswerHistoryLog(AnswerHistoryRepo answerHistoryRepo)
    {
        _answerHistoryRepo = answerHistoryRepo;
    }

    public void Run(
        Question question, 
        AnswerQuestionResult answerQuestionResult, 
        int userId,
        Player player = null,
        Round round = null,
        LearningSessionStep learningSessionStep = null,
      //  bool countUnansweredAsCorrect = false,
        /*for testing*/ DateTime dateCreated = default(DateTime))
    {
        var answerHistory = new AnswerHistory
        {
            QuestionId = question.Id,
            UserId = userId,
            AnswerText = answerQuestionResult.AnswerGiven,
            AnswerredCorrectly = answerQuestionResult.IsCorrect ? AnswerCorrectness.True : AnswerCorrectness.False,
            Round = round,
            Player = player,
            LearningSessionStep = learningSessionStep,
            DateCreated = dateCreated == default(DateTime)
                ? DateTime.Now
                : dateCreated
        };

        _answerHistoryRepo.Create(answerHistory);
    }

    public void CountLastAnswerAsCorrect(Question question, int userId)
    {
        var correctedAnswerHistory = _answerHistoryRepo.GetByQuestion(question.Id, userId).OrderByDescending(x => x.DateCreated).FirstOrDefault();
        if (correctedAnswerHistory != null && correctedAnswerHistory.AnswerredCorrectly == AnswerCorrectness.False)
        {
            correctedAnswerHistory.AnswerredCorrectly = AnswerCorrectness.MarkedAsTrue;
            _answerHistoryRepo.Update(correctedAnswerHistory);
        }
    }

    public void CountUnansweredAsCorrect(Question question, int userId)
    {
        var answerHistory = new AnswerHistory
        {
            QuestionId = question.Id,
            UserId = userId,
            AnswerText = "",
            AnswerredCorrectly = AnswerCorrectness.MarkedAsTrue,
            DateCreated = DateTime.Now
        };

        _answerHistoryRepo.Create(answerHistory);
    }
}
