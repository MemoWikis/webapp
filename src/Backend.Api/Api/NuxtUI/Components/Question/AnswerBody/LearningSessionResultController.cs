public class VueLearningSessionResultController(
    LearningSessionCache _learningSessionCache,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    IHttpContextAccessor _httpContextAccessor,
    QuestionReadingRepo _questionReadingRepo) : ApiBaseController
{
    public record struct LearningSessionResult(
        int UniqueQuestionCount,
        CorrectWrongOrNotAnswered Correct,
        CorrectWrongOrNotAnswered CorrectAfterRepetition,
        CorrectWrongOrNotAnswered Wrong,
        CorrectWrongOrNotAnswered NotAnswered,
        string PageName,
        int PageId,
        bool InWishknowledge,
        TinyQuestion[] Questions);

    public record struct CorrectWrongOrNotAnswered(int Percentage, int Count);

    public record struct TinyQuestion(
        string CorrectAnswerHtml,
        int Id,
        string ImgUrl,
        string Title,
        SessionAnswer[] SessionAnswers,
        SolutionType SolutionType = SolutionType.FlashCard);

    public record struct SessionAnswer(AnswerState AnswerState, string AnswerAsHtml);

    [HttpGet]
    public LearningSessionResult Get() => GetLearningSessionResult();

    private LearningSessionResult GetLearningSessionResult()
    {
        var learningSession = _learningSessionCache.GetLearningSession();
        if (learningSession == null)
        {
            throw new Exception(FrontendMessageKeys.Error.Default);
        }
        
        var model = new global::LearningSessionResult(learningSession);
        var tinyQuestions = model.AnsweredStepsGrouped
            .Where(g => g.First().Question.Id != 0)
            .Select(g =>
            {
                var question = g.First().Question;
                return new TinyQuestion(
                    CorrectAnswerHtml: GetQuestionSolution.Run(question).GetCorrectAnswerAsHtml(),
                    Id: question.Id,
                    ImgUrl: GetQuestionImageFrontendData.Run(
                            question,
                            _imageMetaDataReadingRepo,
                            _httpContextAccessor,
                            _questionReadingRepo)
                        .GetImageUrl(128, true).Url,
                    Title: question.GetShortTitle(),
                    SessionAnswers: g.Select(s => new SessionAnswer(
                        AnswerState: s.AnswerState,
                        AnswerAsHtml: Question.AnswersAsHtml(s.Answer, question.SolutionType)
                    )).ToArray(),
                    SolutionType: question.SolutionType
                );
            }).ToArray();

        return new LearningSessionResult(
            UniqueQuestionCount: model.NumberUniqueQuestions,
            Correct: new CorrectWrongOrNotAnswered(
                Percentage: model.NumberCorrectPercentage,
                Count: model.NumberCorrectAnswers
            ),
            CorrectAfterRepetition: new CorrectWrongOrNotAnswered(
                Percentage: model.NumberCorrectAfterRepetitionPercentage,
                Count: model.NumberCorrectAfterRepetitionAnswers
            ),
            Wrong: new CorrectWrongOrNotAnswered(
                Percentage: model.NumberWrongAnswersPercentage,
                Count: model.NumberWrongAnswers
            ),
            NotAnswered: new CorrectWrongOrNotAnswered(
                Percentage: model.NumberNotAnsweredPercentage,
                Count: model.NumberNotAnswered
            ),
            PageName: learningSession.Config.GetPage().Name,
            PageId: learningSession.Config.GetPage().Id,
            InWishknowledge: learningSession.Config.InWishKnowledge,
            Questions: tinyQuestions
        );
    }
}