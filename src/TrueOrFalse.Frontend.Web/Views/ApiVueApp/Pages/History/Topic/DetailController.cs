using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace VueApp;

public class HistoryTopicDetailController(
    PermissionCheck _permissionCheck,
    SessionUser _sessionUser,
    RestoreCategory _restoreCategory,
    CategoryChangeRepo _categoryChangeRepo,
    CategoryRepository _categoryRepository,
    ExtendedUserCache extendedUserCache,
    IHttpContextAccessor _httpContextAccessor,
    IWebHostEnvironment _webHostEnvironment,
    IActionContextAccessor _actionContextAccessor,
    QuestionReadingRepo _questionReadingRepo,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo) : Controller
{
    [HttpGet]
    public ChangeDetailResult Get(
        [FromQuery] int topicId,
        int currentRevisionId,
        int firstEditId = 0)
    {
        if (!_permissionCheck.CanViewCategory(topicId))
            throw new Exception("not allowed");

        var listWithAllVersions = _categoryChangeRepo.GetForTopic(topicId).OrderBy(c => c.Id);
        var isCategoryDeleted = listWithAllVersions.Any(cc => cc.Type == CategoryChangeType.Delete);

        var currentRevision = listWithAllVersions.FirstOrDefault(c => c.Id == currentRevisionId);

        var previousRevision = firstEditId <= 0
            ? listWithAllVersions.LastOrDefault(c => c.Id < currentRevisionId)
            : listWithAllVersions.LastOrDefault(c => c.Id < firstEditId);

        if (currentRevision.Category.Id != previousRevision.Category.Id)
            throw new Exception("different topic ids");

        var nextRevision = listWithAllVersions.FirstOrDefault(c => c.Id > currentRevisionId);
        var topicHistoryDetailModel = new CategoryHistoryDetailModel(currentRevision,
            previousRevision,
            nextRevision,
            isCategoryDeleted,
            _permissionCheck,
            _categoryChangeRepo,
            _categoryRepository,
            _imageMetaDataReadingRepo,
            _httpContextAccessor,
            _actionContextAccessor,
            _questionReadingRepo);

        var currentAuthor = currentRevision.Author();
        var result = new ChangeDetailResult
        {
            TopicName = topicHistoryDetailModel.CategoryName,
            ImageWasUpdated = topicHistoryDetailModel.ImageWasUpdated,
            IsCurrent = !topicHistoryDetailModel.NextRevExists,
            ChangeType = topicHistoryDetailModel.ChangeType,
            CurrentChangeDate = currentRevision.DateCreated.ToString("dd.MM.yyyy HH:mm:ss"),
            PreviousChangeDate = previousRevision.DateCreated.ToString("dd.MM.yyyy HH:mm:ss"),
            AuthorName = currentAuthor.Name,
            AuthorId = currentAuthor.Id,
            AuthorImgUrl = new UserImageSettings(currentAuthor.Id,
                    _httpContextAccessor)
                .GetUrl_20px_square(currentAuthor)
                .Url
        };

        if (topicHistoryDetailModel.CurrentName != topicHistoryDetailModel.PrevName)
        {
            result.CurrentName = topicHistoryDetailModel.CurrentName;
            result.PreviousName = topicHistoryDetailModel.PrevName;
        }

        if (topicHistoryDetailModel.CurrentMarkdown != topicHistoryDetailModel.PrevMarkdown)
        {
            result.CurrentMarkdown = topicHistoryDetailModel.CurrentMarkdown;
            result.PreviousMarkdown = topicHistoryDetailModel.PrevMarkdown;
        }

        if (topicHistoryDetailModel.CurrentContent != topicHistoryDetailModel.PrevContent)
        {
            result.CurrentContent = topicHistoryDetailModel.CurrentContent;
            result.PreviousContent = topicHistoryDetailModel.PrevContent;
        }

        if (topicHistoryDetailModel.CurrentSegments != topicHistoryDetailModel.PrevSegments)
        {
            result.CurrentSegments = topicHistoryDetailModel.CurrentSegments;
            result.PreviousSegments = topicHistoryDetailModel.PrevSegments;
        }

        if (topicHistoryDetailModel.CurrentDescription != topicHistoryDetailModel.PrevDescription)
        {
            result.CurrentDescription = topicHistoryDetailModel.CurrentDescription;
            result.PreviousDescription = topicHistoryDetailModel.PrevDescription;
        }

        if (topicHistoryDetailModel.CurrentRelations != topicHistoryDetailModel.PrevRelations)
        {
            result.CurrentRelations = topicHistoryDetailModel.CurrentRelations;
            result.PreviousRelations = topicHistoryDetailModel.PrevRelations;
        }

        return result;
    }

    public record struct ChangeDetailResult(
        string TopicName,
        bool ImageWasUpdated,
        bool IsCurrent,
        CategoryChangeType ChangeType,
        string AuthorName,
        int AuthorId,
        string AuthorImgUrl,
        string CurrentChangeDate,
        string PreviousChangeDate,
        string CurrentName,
        string PreviousName,
        string CurrentMarkdown,
        string PreviousMarkdown,
        string CurrentContent,
        string PreviousContent,
        string CurrentSegments,
        string PreviousSegments,
        string CurrentRelations,
        string PreviousRelations,
        string CurrentDescription,
        string PreviousDescription
    );

    public CategoryHistoryDetailModel GetCategoryHistoryDetailModel(
        int categoryId,
        int firstEditId,
        int selectedRevId)
    {
        var listWithAllVersions = _categoryChangeRepo.GetForCategory(categoryId).OrderBy(c => c.Id);
        var isCategoryDeleted = listWithAllVersions.Any(cc => cc.Type == CategoryChangeType.Delete);

        var currentRevision = listWithAllVersions.FirstOrDefault(c => c.Id == selectedRevId);
        var previousRevision = listWithAllVersions.LastOrDefault(c => c.Id < firstEditId);
        var nextRevision = listWithAllVersions.FirstOrDefault(c => c.Id > selectedRevId);
        return new CategoryHistoryDetailModel(currentRevision,
            previousRevision,
            nextRevision,
            isCategoryDeleted,
            _permissionCheck,
            _categoryChangeRepo,
            _categoryRepository,
            _imageMetaDataReadingRepo,
            _httpContextAccessor,
            _actionContextAccessor,
            _questionReadingRepo);
    }

    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public void RestoreTopic(int topicChangeId)
    {
        var topicChange = _categoryChangeRepo.GetByIdEager(topicChangeId);
        var isCorrectType =
            topicChange.Type is CategoryChangeType.Text or CategoryChangeType.Renamed;

        if (!_permissionCheck.CanViewCategory(topicChange.Category.Id) ||
            !_permissionCheck.CanEditCategory(topicChange.Category.Id))
            throw new Exception("not allowed");

        _restoreCategory.Run(topicChangeId, _sessionUser.User);
    }
}