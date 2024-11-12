using Newtonsoft.Json;
using TrueOrFalse;

public class QuestionUpdater(
    PermissionCheck _permissionCheck,
    PageRepository pageRepository,
    SessionUser _sessionUser) : IRegisterAsInstancePerLifetime
{
    public Question UpdateQuestion(
        Question question,
        QuestionDataParam questionDataParam,
        string safeText)
    {
        question.TextHtml = questionDataParam.TextHtml;
        question.Text = safeText;
        question.DescriptionHtml = questionDataParam.DescriptionHtml;
        question.SolutionType =
            (SolutionType)Enum.Parse(typeof(TrueOrFalse.SolutionType),
                questionDataParam.SolutionType);

        var preEditedCategoryIds = question.Pages.Select(c => c.Id);
        var newCategoryIds = questionDataParam.PageIds.ToList();

        var categoriesToRemove = preEditedCategoryIds.Except(newCategoryIds);

        foreach (var pageId in categoriesToRemove)
            if (!_permissionCheck.CanViewPage(pageId))
                newCategoryIds.Add(pageId);

        question.Pages =
            GetAllParentsForQuestion(newCategoryIds, question);
        question.Visibility = (QuestionVisibility)questionDataParam.Visibility;

        if (question.SolutionType == SolutionType.FlashCard)
        {
            var solutionModelFlashCard = new QuestionSolutionFlashCard();
            solutionModelFlashCard.Text = questionDataParam.Solution;
            question.Solution = JsonConvert.SerializeObject(solutionModelFlashCard);
        }
        else
            question.Solution = questionDataParam.Solution;

        question.SolutionMetadataJson = questionDataParam.SolutionMetadataJson;

        if (!String.IsNullOrEmpty(questionDataParam.ReferencesJson))
        {
            var references = ReferenceJson.LoadFromJson(questionDataParam.ReferencesJson, question,
                pageRepository);
            foreach (var reference in references)
            {
                reference.DateCreated = DateTime.Now;
                reference.DateModified = DateTime.Now;
                question.References.Add(reference);
            }
        }

        question.License = _sessionUser.IsInstallationAdmin
            ? LicenseQuestionRepo.GetById(questionDataParam.LicenseId)
            : LicenseQuestionRepo.GetDefaultLicense();
        var questionCacheItem = QuestionCacheItem.ToCacheQuestion(question);
        EntityCache.AddOrUpdate(questionCacheItem);

        return question;
    }

    public readonly record struct QuestionDataParam(
        int[] PageIds,
        int QuestionId,
        string TextHtml,
        string DescriptionHtml,
        dynamic Solution,
        string SolutionMetadataJson,
        int Visibility,
        string SolutionType,
        bool AddToWishknowledge,
        int SessionIndex,
        int LicenseId,
        string ReferencesJson,
        bool IsLearningTab,
        LearningSessionConfig SessionConfig
    );

    public List<Page> GetAllParentsForQuestion(List<int> newCategoryIds, Question question)
    {
        var pages = new List<Page>();
        var privatePages = question.Pages.Where(c => !_permissionCheck.CanEdit(c)).ToList();
        pages.AddRange(privatePages);

        foreach (var pageId in newCategoryIds)
            pages.Add(pageRepository.GetById(pageId));

        return pages;
    }
}