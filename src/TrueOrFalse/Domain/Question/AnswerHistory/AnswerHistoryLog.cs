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
        Round round = null)
    {
        var answerHistory = new AnswerHistory();
        answerHistory.QuestionId = question.Id;
        answerHistory.UserId = userId;
        answerHistory.AnswerText = answerQuestionResult.AnswerGiven;
        answerHistory.AnswerredCorrectly = answerQuestionResult.IsCorrect ? AnswerCorrectness.True : AnswerCorrectness.False;

        answerHistory.Round = round;
        answerHistory.Player = player;

        _answerHistoryRepository.Create(answerHistory);
    }

    public void CountLastAnswerAsCorrect(Question question, int userId)
    {
        var correctedAnswerHistory = _answerHistoryRepository.GetBy(question.Id, userId).OrderByDescending(x => x.DateCreated).FirstOrDefault();
        if (correctedAnswerHistory != null && correctedAnswerHistory.AnswerredCorrectly == AnswerCorrectness.False)
        {
            correctedAnswerHistory.AnswerredCorrectly = AnswerCorrectness.MarkedAsTrue;
            _answerHistoryRepository.Update(correctedAnswerHistory);
        }
    }
}
