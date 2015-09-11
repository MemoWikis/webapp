using System;
using System.Linq;

public class AnswerHistoryLog : IRegisterAsInstancePerLifetime
{
    private readonly AnswerHistoryRepository _answerHistoryRepository;

    public AnswerHistoryLog(AnswerHistoryRepository answerHistoryRepository)
    {
        _answerHistoryRepository = answerHistoryRepository;
    }

    public void Run(
        Question question, 
        AnswerQuestionResult answerQuestionResult, 
        int userId,
        Player player = null,
        Round round = null,
        LearningSessionStep learningSessionStep = null,
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

        _answerHistoryRepository.Create(answerHistory);
    }

    public void CountLastAnswerAsCorrect(Question question, int userId)
    {
        var correctedAnswerHistory = _answerHistoryRepository.GetByQuestion(question.Id, userId).OrderByDescending(x => x.DateCreated).FirstOrDefault();
        if (correctedAnswerHistory != null && correctedAnswerHistory.AnswerredCorrectly == AnswerCorrectness.False)
        {
            correctedAnswerHistory.AnswerredCorrectly = AnswerCorrectness.MarkedAsTrue;
            _answerHistoryRepository.Update(correctedAnswerHistory);
        }
    }
}
