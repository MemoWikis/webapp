using Microsoft.AspNetCore.Mvc.Infrastructure;

public class QuestionLoader(
    SessionUser _sessionUser,
    ExtendedUserCache _extendedUserCache,
    IHttpContextAccessor _httpContextAccessor,
    IActionContextAccessor _actionContextAccessor,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    QuestionReadingRepo _questionReadingRepo,
    LearningSessionCache _learningSessionCache)
{
    public QuestionListJson.Question LoadQuestion(int questionId)
    {
        var user = _sessionUser.User;
        var userQuestionValuation = _extendedUserCache.GetItem(user.Id).QuestionValuations;
        var q = EntityCache.GetQuestionById(questionId);
        var question = new QuestionListJson.Question();
        question.Id = q.Id;
        question.Title = q.Text;

        var links = new Links(_actionContextAccessor, _httpContextAccessor);
        question.LinkToQuestion = links.GetUrl(q);
        question.ImageData = new ImageFrontendData(_imageMetaDataReadingRepo.GetBy(q.Id,
                    ImageType.Question),
                _httpContextAccessor,
                _questionReadingRepo)
            .GetImageUrl(40, true)
            .Url;
        question.LinkToQuestion = links.GetUrl(q);
        question.LinkToQuestionVersions = links.QuestionHistory(q.Id);
        question.LinkToComment = links.GetUrl(q) + "#JumpLabel";
        question.CorrectnessProbability = q.CorrectnessProbability;
        question.Visibility = q.Visibility;

        var learningSession = _learningSessionCache.GetLearningSession(log: false);
        if (learningSession != null)
        {
            var steps = learningSession.Steps;
            var index = steps.IndexOf(s => s.Question.Id == q.Id);
            question.SessionIndex = index;
        }

        if (userQuestionValuation.ContainsKey(q.Id) && user != null)
        {
            question.CorrectnessProbability =
                userQuestionValuation[q.Id].CorrectnessProbability;
            question.IsInWishknowledge = userQuestionValuation[q.Id].IsInWishknowledge;
            question.HasPersonalAnswer =
                userQuestionValuation[q.Id].CorrectnessProbabilityAnswerCount > 0;
        }

        return question;
    }
}