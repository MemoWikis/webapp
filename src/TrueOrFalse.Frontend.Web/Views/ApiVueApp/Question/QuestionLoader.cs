using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;

public class QuestionLoader(
    SessionUser _sessionUser,
    SessionUserCache _sessionUserCache,
    IHttpContextAccessor _httpContextAccessor,
    IActionContextAccessor _actionContextAccessor,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    QuestionReadingRepo _questionReadingRepo,
    LearningSessionCache _learningSessionCache)
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CorrectnessProbability { get; set; }
        public string LinkToQuestion { get; set; }
        public string ImageData { get; set; }
        public bool IsInWishknowledge { get; set; }
        public bool HasPersonalAnswer { get; set; }
        public int LearningSessionStepCount { get; set; }
        public string LinkToComment { get; set; }
        public string LinkToQuestionVersions { get; set; }
        public int SessionIndex { get; set; }
        public QuestionVisibility Visibility { get; set; }
        public int CreatorId { get; set; } = 0;
        public KnowledgeStatus KnowledgeStatus { get; set; } = KnowledgeStatus.NotLearned;
    }

    public Question LoadQuestion(int questionId)
    {
        var user = _sessionUser.User;
        var userQuestionValuation = _sessionUserCache.GetItem(user.Id).QuestionValuations;
        var q = EntityCache.GetQuestionById(questionId);
        var question = new Question();
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

        var learningSession = _learningSessionCache.GetLearningSession();
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
            question.IsInWishknowledge = userQuestionValuation[q.Id].IsInWishKnowledge;
            question.HasPersonalAnswer =
                userQuestionValuation[q.Id].CorrectnessProbabilityAnswerCount > 0;
        }

        return question;
    }
}