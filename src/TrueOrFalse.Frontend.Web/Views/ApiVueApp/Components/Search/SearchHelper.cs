using Microsoft.AspNetCore.Http;
using Seedworks.Lib;
using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Search;

public class SearchHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ImageMetaDataReadingRepo _imageMetaDataReadingRepo;
    private readonly QuestionReadingRepo _questionReadingRepo;

    public SearchHelper(ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        IHttpContextAccessor httpContextAccessor,
        QuestionReadingRepo questionReadingRepo)
    {
        _imageMetaDataReadingRepo = imageMetaDataReadingRepo;
        _httpContextAccessor = httpContextAccessor;
        _questionReadingRepo = questionReadingRepo;
    }

    public void AddPageItems(
        List<SearchPageItem> items,
        GlobalSearchResult elements,
        PermissionCheck permissionCheck,
        int userId,
        int[] topicIdsToFilter = null) => items.AddRange(
            elements.Categories
                .Where(c => permissionCheck.CanView(c) &&
                    (topicIdsToFilter == null || !topicIdsToFilter.Contains(c.Id)))
                .Select(c => FillSearchPageItem(c, userId))
            );

    public void AddPublicPageItems(
        List<SearchPageItem> items,
        GlobalSearchResult elements,
        int userId,
        int[] topicIdsToFilter = null) => items.AddRange(
        elements.Categories
            .Where(c => c.Visibility == PageVisibility.All &&
                        (topicIdsToFilter == null || !topicIdsToFilter.Contains(c.Id)))
            .Select(c => FillSearchPageItem(c, userId))
    );

    public int? SuggestNewParent(Crumbtrail breadcrumb, bool hasPublicQuestion)
    {
        CrumbtrailItem breadcrumbItem;

        if (hasPublicQuestion)
        {
            if (breadcrumb.Items.Any(i => i.Page.Visibility == PageVisibility.All))
            {
                breadcrumbItem = breadcrumb.Items.Last(i => i.Page.Visibility == PageVisibility.All);
                return breadcrumbItem.Page.Id;
            }

            return null;
        }

        breadcrumbItem = breadcrumb.Items.Last();
        return breadcrumbItem.Page.Id;
    }

    public SearchPageItem FillSearchPageItem(PageCacheItem topic, int userId)
    {
        return new SearchPageItem
        {
            Id = topic.Id,
            Name = topic.Name,
            QuestionCount = EntityCache.GetPage(topic.Id).GetCountQuestionsAggregated(userId),
            ImageUrl = new PageImageSettings(topic.Id,
                    _httpContextAccessor).GetUrl_128px(true)
                .Url,
            MiniImageUrl = new ImageFrontendData(_imageMetaDataReadingRepo
                    .GetBy(topic.Id, ImageType.Page), _httpContextAccessor, _questionReadingRepo)
                .GetImageUrl(30, true, false, ImageType.Page).Url,
            Visibility = (int)topic.Visibility
        };
    }

    public void AddQuestionItems(List<SearchQuestionItem> items,
        GlobalSearchResult elements,
        PermissionCheck permissionCheck,
        QuestionReadingRepo questionReadingRepo)
    {
        items.AddRange(
            elements.Questions
                .Where(q => permissionCheck.CanView(q) && q.CategoriesVisibleToCurrentUser(permissionCheck).Any())
                .Select((q, index) => new SearchQuestionItem
                {
                    Id = q.Id,
                    Name = q.Text.Wrap(200),
                    ImageUrl = new QuestionImageSettings(q.Id, _httpContextAccessor, questionReadingRepo)
                        .GetUrl_50px_square()
                        .Url,
                    PrimaryPageId = q.CategoriesVisibleToCurrentUser(permissionCheck).FirstOrDefault()!.Id,
                    PrimaryPageName = q.CategoriesVisibleToCurrentUser(permissionCheck).FirstOrDefault()!.Name
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
                    .Url
            }));
    }
}