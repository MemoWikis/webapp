using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace TrueOrFalse.Domain.Question.Answer;

public record struct AnswerQuestionDetailsResult(
    KnowledgeStatus KnowledgeStatus,
    int PersonalProbability,
    string PersonalColor,
    int AvgProbability,
    int PersonalAnswerCount,
    int PersonalAnsweredCorrectly,
    int PersonalAnsweredWrongly,
    int OverallAnswerCount,
    int OverallAnsweredCorrectly,
    int OverallAnsweredWrongly,
    bool IsInWishknowledge,
    AnswerQuestionDetailsTopic[] Topics,
    QuestionVisibility Visibility,
    long DateNow,
    long EndTimer,
    MacroCreator Creator,
    string CreationDate,
    int TotalViewCount,
    int WishknowledgeCount,
    License License);

public record struct MacroCreator(int Id, string Name);

public record struct AnswerQuestionDetailsTopic(
    int Id,
    string Name,
    string Url,
    int QuestionCount,
    string ImageUrl,
    string IconHtml,
    string MiniImageUrl,
    int Visibility,
    bool IsSpoiler);

public record struct License(
    bool IsDefault,
    string ShortText,
    string FullText);

public class AnswerQuestionDetailsService(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    TotalsPersUserLoader _totalsPersUserLoader,
    IHttpContextAccessor _httpContextAccessor,
    SessionUserCache _sessionUserCache,
    IActionContextAccessor _actionContextAccessor,
    QuestionReadingRepo _questionReadingRepo) : IRegisterAsInstancePerLifetime

{
}