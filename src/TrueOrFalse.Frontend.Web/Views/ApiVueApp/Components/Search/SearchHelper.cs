using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Seedworks.Lib;

public class SearchHelper
{
    private readonly ImageMetaDataReadingRepo _imageMetaDataReadingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly QuestionReadingRepo _questionReadingRepo;

    public SearchHelper(ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        IHttpContextAccessor httpContextAccessor,
        QuestionReadingRepo questionReadingRepo)
    {
        _imageMetaDataReadingRepo = imageMetaDataReadingRepo;
        _httpContextAccessor = httpContextAccessor;
        _questionReadingRepo = questionReadingRepo;
    }
    public void AddTopicItems(List<SearchTopicItem> items, TrueOrFalse.Search.GlobalSearchResult elements, PermissionCheck permissionCheck, int userId)
    {
        items.AddRange(
            elements.Categories.Where(permissionCheck.CanView).Select(c => FillSearchTopicItem(c, userId)));
    }

    public SearchTopicItem FillSearchTopicItem(CategoryCacheItem topic, int userId)
    {
        return new SearchTopicItem
        {
            Id = topic.Id,
            Name = topic.Name,
            QuestionCount = EntityCache.GetCategory(topic.Id).GetCountQuestionsAggregated(userId),
            ImageUrl = new CategoryImageSettings(topic.Id,
                    _httpContextAccessor).GetUrl_128px(asSquare: true)
                .Url,
            MiniImageUrl = new ImageFrontendData(_imageMetaDataReadingRepo
                    .GetBy(topic.Id, ImageType.Category), _httpContextAccessor, _questionReadingRepo)
                .GetImageUrl(30, true, false, ImageType.Category).Url,
            Visibility = (int)topic.Visibility
        };
    }

    public void AddQuestionItems(List<SearchQuestionItem> items,
        TrueOrFalse.Search.GlobalSearchResult elements, 
        PermissionCheck permissionCheck, 
        QuestionReadingRepo questionReadingRepo)
    {
        items.AddRange(
            elements.Questions.Where(q => permissionCheck.CanView(q) && q.CategoriesVisibleToCurrentUser(permissionCheck).Any()).Select((q, index) => new SearchQuestionItem
            {
                Id = q.Id,
                Name = q.Text.Wrap(200),
                ImageUrl = new QuestionImageSettings(q.Id, _httpContextAccessor,questionReadingRepo)
                    .GetUrl_50px_square()
                    .Url,
                PrimaryTopicId = q.CategoriesVisibleToCurrentUser(permissionCheck).FirstOrDefault()!.Id,
                PrimaryTopicName = q.CategoriesVisibleToCurrentUser(permissionCheck).FirstOrDefault()!.Name
            }));
    }

    public void AddUserItems(List<SearchUserItem> items, TrueOrFalse.Search.GlobalSearchResult elements)
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