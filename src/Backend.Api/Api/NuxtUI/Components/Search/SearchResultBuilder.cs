public class SearchResultBuilder(
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    IHttpContextAccessor _httpContextAccessor,
    QuestionReadingRepo _questionReadingRepo,
    SessionUser _sessionUser) : IRegisterAsInstancePerLifetime
{
    public void AddPageItems(
        List<SearchPageItem> items,
        GlobalSearchResult elements,
        PermissionCheck permissionCheck,
        int userId,
        int[] pageIdsToFilter = null) => items.AddRange(
            elements.Pages
                .Where(c => permissionCheck.CanView(c) &&
                    (pageIdsToFilter == null || !pageIdsToFilter.Contains(c.Id)))
                .Select(c => FillSearchPageItem(c, userId))
            );

    public void AddPageItems(
        List<SearchPageItem> items,
        GlobalSearchResult elements,
        PermissionCheck permissionCheck,
        int userId,
        List<Language> languages,
        int[] pageIdsToFilter = null) => items.AddRange(
        elements.Pages
            .Where(c => permissionCheck.CanView(c) &&
                        (pageIdsToFilter == null || !pageIdsToFilter.Contains(c.Id)))
            .Select(c => FillSearchPageItem(c, userId))
    );

    public void AddPublicPageItems(
        List<SearchPageItem> items,
        GlobalSearchResult elements,
        int userId,
        int[] pageIdsToFilter = null) => items.AddRange(
        elements.Pages
            .Where(c => c.Visibility == PageVisibility.Public &&
                        (pageIdsToFilter == null || !pageIdsToFilter.Contains(c.Id)))
            .Select(c => FillSearchPageItem(c, userId))
    );



    public SearchPageItem FillSearchPageItem(PageCacheItem page, int userId)
    {
        return new SearchPageItem
        {
            Id = page.Id,
            Name = page.Name,
            QuestionCount = EntityCache.GetPage(page.Id).GetCountQuestionsAggregated(userId),
            ImageUrl = new PageImageSettings(page.Id, _httpContextAccessor).GetUrl_128px(true).Url,
            MiniImageUrl = new ImageFrontendData(
                    _imageMetaDataReadingRepo.GetBy(page.Id, ImageType.Page), 
                    _httpContextAccessor, 
                    _questionReadingRepo
                )
                .GetImageUrl(30, true, false, ImageType.Page).Url,
            Visibility = (int)page.Visibility,
            LanguageCode = page.Language
        };
    }

    public void AddQuestionItems(List<SearchQuestionItem> items,
        GlobalSearchResult elements,
        PermissionCheck permissionCheck,
        QuestionReadingRepo questionReadingRepo)
    {
        items.AddRange(
            elements.Questions
                .Where(q => permissionCheck.CanView(q) && q.PagesVisibleToCurrentUser(permissionCheck).Any())
                .Select((q, _) => new SearchQuestionItem
                {
                    Id = q.Id,
                    Name = q.Text.Wrap(200),
                    ImageUrl = new QuestionImageSettings(q.Id, _httpContextAccessor, questionReadingRepo)
                        .GetUrl_50px_square()
                        .Url,
                    PrimaryPageId = q.PagesVisibleToCurrentUser(permissionCheck).FirstOrDefault()!.Id,
                    PrimaryPageName = q.PagesVisibleToCurrentUser(permissionCheck).FirstOrDefault()!.Name,
                    LanguageCode = q.Language
                }));
    }

    public void AddUserItems(List<SearchUserItem> items, GlobalSearchResult elements)
    {
        items.AddRange(
            elements.Users.Select(u => new SearchUserItem
            {
                Id = u.Id,
                Name = u.Name,
                ImageUrl = new UserImageSettings(u.Id, _httpContextAccessor)
                    .GetUrl_50px_square(u)
                    .Url,
                LanguageCodes = u.ContentLanguages.Select(l => l.GetCode()).ToList()
            }));
    }
}