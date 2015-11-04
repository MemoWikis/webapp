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
        var answer = new Answer
        {
            Question = question,
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

        _answerRepo.Create(answer);
    }

    public void CountLastAnswerAsCorrect(Question question, int userId)
    {
        var correctedAnswer = _answerRepo.GetByQuestion(question.Id, userId).OrderByDescending(x => x.DateCreated).FirstOrDefault();
        if (correctedAnswer != null && correctedAnswer.AnswerredCorrectly == AnswerCorrectness.False)
        {
            correctedAnswer.AnswerredCorrectly = AnswerCorrectness.MarkedAsTrue;
            _answerRepo.Update(correctedAnswer);
        }
    }

    public void CountUnansweredAsCorrect(Question question, int userId)
    {
        var answer = new Answer
        {
            Question = question,
            UserId = userId,
            AnswerText = "",
            AnswerredCorrectly = AnswerCorrectness.MarkedAsTrue,
            DateCreated = DateTime.Now
        };

        _answerRepo.Create(answer);
    }
}
