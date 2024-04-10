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
    SessionUserCache _sessionUserCache,
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
            _sessionUserCache,
            _httpContextAccessor,
            _webHostEnvironment,
            _actionContextAccessor,
            _questionReadingRepo);

        var currentAuthor = currentRevision.Author();
        var result = new ChangeDetailResult
        {
            topicName = topicHistoryDetailModel.CategoryName,
            imageWasUpdated = topicHistoryDetailModel.ImageWasUpdated,
            isCurrent = !topicHistoryDetailModel.NextRevExists,
            changeType = topicHistoryDetailModel.ChangeType,
            currentChangeDate = currentRevision.DateCreated.ToString("dd.MM.yyyy HH:mm:ss"),
            previousChangeDate = previousRevision.DateCreated.ToString("dd.MM.yyyy HH:mm:ss"),
            authorName = currentAuthor.Name,
            authorId = currentAuthor.Id,
            authorImgUrl = new UserImageSettings(currentAuthor.Id,
                    _httpContextAccessor)
                .GetUrl_20px_square(currentAuthor)
                .Url
        };

        if (topicHistoryDetailModel.CurrentName != topicHistoryDetailModel.PrevName)
        {
            result.currentName = topicHistoryDetailModel.CurrentName;
            result.previousName = topicHistoryDetailModel.PrevName;
        }

        if (topicHistoryDetailModel.CurrentMarkdown != topicHistoryDetailModel.PrevMarkdown)
        {
            result.currentMarkdown = topicHistoryDetailModel.CurrentMarkdown;
            result.previousMarkdown = topicHistoryDetailModel.PrevMarkdown;
        }

        if (topicHistoryDetailModel.CurrentContent != topicHistoryDetailModel.PrevContent)
        {
            result.currentContent = topicHistoryDetailModel.CurrentContent;
            result.previousContent = topicHistoryDetailModel.PrevContent;
        }

        if (topicHistoryDetailModel.CurrentSegments != topicHistoryDetailModel.PrevSegments)
        {
            result.currentSegments = topicHistoryDetailModel.CurrentSegments;
            result.previousSegments = topicHistoryDetailModel.PrevSegments;
        }

        if (topicHistoryDetailModel.CurrentDescription != topicHistoryDetailModel.PrevDescription)
        {
            result.currentDescription = topicHistoryDetailModel.CurrentDescription;
            result.previousDescription = topicHistoryDetailModel.PrevDescription;
        }

        if (topicHistoryDetailModel.CurrentRelations != topicHistoryDetailModel.PrevRelations)
        {
            result.currentRelations = topicHistoryDetailModel.CurrentRelations;
            result.previousRelations = topicHistoryDetailModel.PrevRelations;
        }

        return result;
    }

    public class ChangeDetailResult
    {
        public string topicName { get; set; }
        public bool imageWasUpdated { get; set; }
        public bool isCurrent { get; set; }
        public CategoryChangeType changeType { get; set; }
        public string authorName { get; set; }
        public int authorId { get; set; }
        public string authorImgUrl { get; set; }
        public string currentChangeDate { get; set; }
        public string previousChangeDate { get; set; }
        public string currentName { get; set; }
        public string previousName { get; set; }
        public string currentMarkdown { get; set; }
        public string previousMarkdown { get; set; }
        public string currentContent { get; set; }
        public string previousContent { get; set; }
        public string currentSegments { get; set; }
        public string previousSegments { get; set; }
        public string currentRelations { get; set; }
        public string previousRelations { get; set; }
        public string currentDescription { get; set; }
        public string previousDescription { get; set; }
    }

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
            _sessionUserCache,
            _httpContextAccessor,
            _webHostEnvironment,
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