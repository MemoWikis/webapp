using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

public class QuestionController(
    SessionUser _sessionUser,
    PermissionCheck _permissionCheck,
    RestoreQuestion _restoreQuestion,
    LearningSessionCache _learningSessionCache,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    UserReadingRepo _userReadingRepo,
    QuestionReadingRepo _questionReadingRepo,
    ExtendedUserCache _extendedUserCache,
    IHttpContextAccessor _httpContextAccessor,
    IActionContextAccessor _actionContextAccessor,
    TotalsPerUserLoader totalsPerUserLoader,
    SaveQuestionView _saveQuestionView) : ApiBaseController
{
    public readonly record struct QuestionPageResult(
        AnswerBodyModel? AnswerBodyModel,
        SolutionData? SolutionData,
        AnswerQuestionDetailsResult? AnswerQuestionDetailsModel,
        string MessageKey,
        NuxtErrorPageType? ErrorCode);

    public readonly record struct AnswerBodyModel(
        int Id,
        string Text,
        string Title,
        SolutionType SolutionType,
        string RenderedQuestionTextExtended,
        string Description,
        bool HasPages,
        int? PrimaryPageId,
        string PrimaryPageName,
        string Solution,
        bool IsCreator,
        bool IsInWishKnowledge,
        Guid QuestionViewGuid,
        bool IsLastStep);

    public readonly record struct SolutionData(
        string AnswerAsHTML,
        string Answer,
        string AnswerDescription,
        AnswerReferences[] AnswerReferences);

    public readonly record struct AnswerReferences(
        int ReferenceId,
        int? PageId,
        string ReferenceType,
        string AdditionalInfo,
        string ReferenceText);

    [HttpGet]
    public QuestionPageResult GetQuestionPage([FromRoute] int id)
    {
        var question = EntityCache.GetQuestion(id);

        if (question == null)
        {
            return new QuestionPageResult
            {
                ErrorCode = NuxtErrorPageType.NotFound,
                MessageKey = FrontendMessageKeys.Error.Question.NotFound
            };
        }

        if (_permissionCheck.CanViewQuestion(id))
        {
            var primaryPage = question.Pages.LastOrDefault();
            var solution = GetQuestionSolution.Run(question);

            EscapeReferencesText(question.References);
            _saveQuestionView.Run(question, new UserTinyModel(EntityCache.GetUserById(_sessionUser.UserId)));
            return new QuestionPageResult
            {
                AnswerBodyModel = new AnswerBodyModel
                {
                    Id = question.Id,
                    Text = question.Text,
                    Title = SafeQuestionTitle.Get(question.Text),
                    SolutionType = question.SolutionType,
                    RenderedQuestionTextExtended = question.GetRenderedQuestionTextExtended(),
                    Description = question.Description,
                    HasPages = question.Pages.Any(),
                    PrimaryPageId = primaryPage?.Id,
                    PrimaryPageName = primaryPage?.Name,
                    Solution = question.Solution,
                    IsCreator = question.Creator.Id == _sessionUser.UserId,
                    IsInWishKnowledge = _sessionUser.IsLoggedIn &&
                                        question.IsInWishKnowledge(_sessionUser.UserId, _extendedUserCache),
                    QuestionViewGuid = Guid.NewGuid(),
                    IsLastStep = true
                },
                SolutionData = new SolutionData
                {
                    AnswerAsHTML = solution.GetCorrectAnswerAsHtml(),
                    Answer = solution.CorrectAnswer(),
                    AnswerDescription =
                        question.Description != null ? MarkdownMarkdig.ToHtml(question.Description) : "",
                    AnswerReferences = question.References.Select(r => new AnswerReferences
                    {
                        ReferenceId = r.Id,
                        PageId = r.Page?.Id ?? null,
                        ReferenceType = r.ReferenceType.GetName(),
                        AdditionalInfo = r.AdditionalInfo ?? "",
                        ReferenceText = r.ReferenceText ?? ""
                    }).ToArray()
                },
                AnswerQuestionDetailsModel =
                    GetData(id)
            };
        }

        if (_sessionUser.IsLoggedIn)
        {
            return new QuestionPageResult
            {
                ErrorCode = NuxtErrorPageType.Unauthorized,
                MessageKey = FrontendMessageKeys.Error.Question.NoRights
            };
        }

        return new QuestionPageResult
        {
            ErrorCode = NuxtErrorPageType.Unauthorized,
            MessageKey = FrontendMessageKeys.Error.Question.Unauthorized
        };
    }


    [HttpGet]
    public AnswerQuestionDetailsResult? GetData(int id)
    {
        var question = EntityCache.GetQuestionById(id);

        if (question.Id == 0 || !_permissionCheck.CanView(question))
            return null;

        var dateNow = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var answerQuestionModel = new AnswerQuestionModel(question,
            _sessionUser,
            totalsPerUserLoader,
            _extendedUserCache);

        var correctnessProbability =
            answerQuestionModel.HistoryAndProbability.CorrectnessProbability;
        var history = answerQuestionModel.HistoryAndProbability.AnswerHistory;

        var userQuestionValuation = _sessionUser.IsLoggedIn
            ? _extendedUserCache.GetItem(_sessionUser.UserId).QuestionValuations
            : new ConcurrentDictionary<int, QuestionValuationCacheItem>();
        var hasUserValuation =
            userQuestionValuation.ContainsKey(question.Id) && _sessionUser.IsLoggedIn;
        var result = new AnswerQuestionDetailsResult(
            QuestionId: question.Id,
            KnowledgeStatus: hasUserValuation
                ? userQuestionValuation[question.Id].KnowledgeStatus
                : KnowledgeStatus.NotLearned,
            PersonalProbability: correctnessProbability.CPPersonal,
            PersonalColor: correctnessProbability.CPPColor,
            AvgProbability: correctnessProbability.CPAll,
            PersonalAnswerCount: history.TimesAnsweredUser,
            PersonalAnsweredCorrectly: history.TimesAnsweredUserTrue,
            PersonalAnsweredWrongly: history.TimesAnsweredUserWrong,
            OverallAnswerCount: history.TimesAnsweredTotal,
            OverallAnsweredCorrectly: history.TimesAnsweredCorrect,
            OverallAnsweredWrongly: history.TimesAnsweredWrongTotal,
            IsInWishKnowledge: answerQuestionModel.HistoryAndProbability.QuestionValuation
                .IsInWishKnowledge,
            Pages: question.PagesVisibleToCurrentUser(_permissionCheck).Select(t =>
                new AnswerQuestionDetailsPageItem(
                    Id: t.Id,
                    Name: t.Name,
                    QuestionCount: t.GetCountQuestionsAggregated(_sessionUser.UserId),
                    ImageUrl: new PageImageSettings(t.Id, _httpContextAccessor)
                        .GetUrl_128px(asSquare: true).Url,
                    MiniImageUrl: new ImageFrontendData(
                            _imageMetaDataReadingRepo.GetBy(t.Id, ImageType.Page),
                            _httpContextAccessor,
                            _questionReadingRepo)
                        .GetImageUrl(30, true, false, ImageType.Page).Url,
                    Visibility: (int)t.Visibility,
                    IsSpoiler: IsSpoilerPage.Yes(t.Name, question)
                )).Distinct().ToArray(),
            Visibility: question.Visibility,
            DateNow: dateNow,
            EndTimer: DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            Creator: new MacroCreator(
                Id: question.CreatorId,
                Name: question.Creator.Name
            ),
            CreationDate: question.DateCreated,
            TotalViewCount: question.TotalViews,
            WishKnowledgeCount: question.TotalRelevancePersonalEntries,
            LicenseId: question.License.Id
        );
        return result;
    }

    public record struct Question(
        int Id,
        string Title,
        int CorrectnessProbability,
        string LinkToQuestion,
        string ImageData,
        bool IsInWishKnowledge,
        bool HasPersonalAnswer,
        int LearningSessionStepCount,
        string LinkToComment,
        string LinkToQuestionVersions,
        int SessionIndex,
        QuestionVisibility Visibility,
        int LicenseId,
        int CreatorId = 0,
        KnowledgeStatus KnowledgeStatus = KnowledgeStatus.NotLearned
    );

    [HttpGet]
    public Question LoadQuestion(int questionId)
    {
        var userQuestionValuation = _sessionUser.IsLoggedIn
            ? _extendedUserCache.GetItem(_sessionUser.UserId)
                .QuestionValuations
            : null;

        var q = EntityCache.GetQuestionById(questionId);
        var question = new Question();
        question.Id = q.Id;
        question.Title = q.Text;
        var links = new Links(_actionContextAccessor, _httpContextAccessor);
        question.LinkToQuestion = links.GetUrl(q);
        question.ImageData = new ImageFrontendData(
                _imageMetaDataReadingRepo.GetBy(q.Id, ImageType.Question),
                _httpContextAccessor,
                _questionReadingRepo)
            .GetImageUrl(40, true).Url;

        question.LinkToQuestion = links.GetUrl(q);
        question.LinkToQuestionVersions = links.QuestionHistory(q.Id);
        question.LinkToComment = links.GetUrl(q) + "#JumpLabel";
        question.CorrectnessProbability = q.CorrectnessProbability;
        question.Visibility = q.Visibility;
        question.CreatorId = q.CreatorId;
        question.LicenseId = q.License.Id;

        var learningSession = _learningSessionCache.GetLearningSession(log: false);
        if (learningSession != null)
        {
            var steps = learningSession.Steps;
            var index = steps.IndexOf(s => s.Question.Id == q.Id);
            question.SessionIndex = index;
        }

        if (userQuestionValuation != null && userQuestionValuation.ContainsKey(q.Id))
        {
            question.CorrectnessProbability = userQuestionValuation[q.Id].CorrectnessProbability;
            question.IsInWishKnowledge = userQuestionValuation[q.Id].IsInWishKnowledge;
            question.HasPersonalAnswer =
                userQuestionValuation[q.Id].CorrectnessProbabilityAnswerCount > 0;
        }

        return question;
    }

    [RedirectToErrorPage_IfNotLoggedIn]
    [HttpGet]
    public ActionResult Restore(int questionId, int questionChangeId)
    {
        _restoreQuestion.Run(questionChangeId, _userReadingRepo.GetById(_sessionUser.UserId));

        var question = _questionReadingRepo.GetById(questionId);
        return Redirect(new Links(_actionContextAccessor, _httpContextAccessor)
            .AnswerQuestion(question));
    }

    private static void EscapeReferencesText(IList<ReferenceCacheItem> references)
    {
        foreach (var reference in references)
        {
            if (reference.ReferenceText != null)
            {
                reference.ReferenceText = reference.ReferenceText.Replace("\n", "<br/>")
                    .Replace("\\n", "<br/>");
            }

            if (reference.AdditionalInfo != null)
            {
                reference.AdditionalInfo = reference.AdditionalInfo.Replace("\n", "<br/>")
                    .Replace("\\n", "<br/>");
            }
        }
    }
}