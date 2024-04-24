using Newtonsoft.Json;
using TrueOrFalse;

public class QuestionUpdater(
    PermissionCheck _permissionCheck,
    CategoryRepository _categoryRepository,
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

        var preEditedCategoryIds = question.Categories.Select(c => c.Id);
        var newCategoryIds = questionDataParam.CategoryIds.ToList();

        var categoriesToRemove = preEditedCategoryIds.Except(newCategoryIds);

        foreach (var categoryId in categoriesToRemove)
            if (!_permissionCheck.CanViewCategory(categoryId))
                newCategoryIds.Add(categoryId);

        question.Categories =
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
                _categoryRepository);
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
        int[] CategoryIds,
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

    public List<Category> GetAllParentsForQuestion(List<int> newCategoryIds, Question question)
    {
        var categories = new List<Category>();
        var privateCategories =
            question.Categories.Where(c => !_permissionCheck.CanEdit(c)).ToList();
        categories.AddRange(privateCategories);

        foreach (var categoryId in newCategoryIds)
            categories.Add(_categoryRepository.GetById(categoryId));

        return categories;
    }

    public List<Category> GetAllParentsForQuestion(int newCategoryId, Question question) =>
        GetAllParentsForQuestion(new List<int> { newCategoryId }, question);
}