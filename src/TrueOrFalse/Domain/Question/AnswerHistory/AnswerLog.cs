using System;
using System.Linq;

public class AnswerLog : IRegisterAsInstancePerLifetime
{
    private readonly AnswerRepo _answerRepo;

    public AnswerLog(AnswerRepo answerRepo)
    {
        _answerRepo = answerRepo;
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
        var answerHistory = new Answer
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

        _answerRepo.Create(answerHistory);
    }

    public void CountLastAnswerAsCorrect(Question question, int userId)
    {
        var correctedAnswerHistory = _answerRepo.GetByQuestion(question.Id, userId).OrderByDescending(x => x.DateCreated).FirstOrDefault();
        if (correctedAnswerHistory != null && correctedAnswerHistory.AnswerredCorrectly == AnswerCorrectness.False)
        {
            correctedAnswerHistory.AnswerredCorrectly = AnswerCorrectness.MarkedAsTrue;
            _answerRepo.Update(correctedAnswerHistory);
        }
    }

    public void CountUnansweredAsCorrect(Question question, int userId)
    {
        var answerHistory = new Answer
        {
            QuestionId = question.Id,
            UserId = userId,
            AnswerText = "",
            AnswerredCorrectly = AnswerCorrectness.MarkedAsTrue,
            DateCreated = DateTime.Now
        };

        _answerRepo.Create(answerHistory);
    }
}
