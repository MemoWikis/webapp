using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Linq;

namespace VueApp;

public class HistoryPageDetailController(
    PermissionCheck _permissionCheck,
    SessionUser _sessionUser,
    RestorePage restorePage,
    PageChangeRepo pageChangeRepo,
    PageRepository pageRepository,
    IHttpContextAccessor _httpContextAccessor,
    IActionContextAccessor _actionContextAccessor,
    QuestionReadingRepo _questionReadingRepo,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo) : Controller
{
    [HttpGet]
    public ChangeDetailResult Get(
        [FromQuery] int pageId,
        int currentRevisionId,
        int firstEditId = 0)
    {
        if (!_permissionCheck.CanViewPage(pageId))
            throw new Exception("not allowed");

        var listWithAllVersions = pageChangeRepo.GetForPage(pageId).OrderBy(c => c.Id);
        var isCategoryDeleted = listWithAllVersions.Any(cc => cc.Type == PageChangeType.Delete);

        var currentRevision = listWithAllVersions.FirstOrDefault(c => c.Id == currentRevisionId);

        var previousRevision = firstEditId <= 0
            ? listWithAllVersions.LastOrDefault(c => c.Id < currentRevisionId)
            : listWithAllVersions.LastOrDefault(c => c.Id < firstEditId);

        if (currentRevision.Page.Id != previousRevision.Page.Id)
            throw new Exception("different page ids");

        var nextRevision = listWithAllVersions.FirstOrDefault(c => c.Id > currentRevisionId);
        var topicHistoryDetailModel = new CategoryHistoryDetailModel(currentRevision,
            previousRevision,
            nextRevision,
            isCategoryDeleted,
            _permissionCheck,
            pageChangeRepo,
            pageRepository,
            _imageMetaDataReadingRepo,
            _httpContextAccessor,
            _actionContextAccessor,
            _questionReadingRepo);

        var currentAuthor = currentRevision.Author();
        var result = new ChangeDetailResult
        {
            PageName = topicHistoryDetailModel.CategoryName,
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
        string PageName,
        bool ImageWasUpdated,
        bool IsCurrent,
        PageChangeType ChangeType,
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
        int pageId,
        int firstEditId,
        int selectedRevId)
    {
        var listWithAllVersions = pageChangeRepo.GetForCategory(pageId).OrderBy(c => c.Id);
        var isCategoryDeleted = listWithAllVersions.Any(cc => cc.Type == PageChangeType.Delete);

        var currentRevision = listWithAllVersions.FirstOrDefault(c => c.Id == selectedRevId);
        var previousRevision = listWithAllVersions.LastOrDefault(c => c.Id < firstEditId);
        var nextRevision = listWithAllVersions.FirstOrDefault(c => c.Id > selectedRevId);
        return new CategoryHistoryDetailModel(currentRevision,
            previousRevision,
            nextRevision,
            isCategoryDeleted,
            _permissionCheck,
            pageChangeRepo,
            pageRepository,
            _imageMetaDataReadingRepo,
            _httpContextAccessor,
            _actionContextAccessor,
            _questionReadingRepo);
    }

    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public void RestorePage(int topicChangeId)
    {
        var topicChange = pageChangeRepo.GetByIdEager(topicChangeId);
        var isCorrectType =
            topicChange.Type is PageChangeType.Text or PageChangeType.Renamed;

        if (!_permissionCheck.CanViewPage(topicChange.Page.Id) ||
            !_permissionCheck.CanEditCategory(topicChange.Page.Id))
            throw new Exception("not allowed");

        restorePage.Run(topicChangeId, _sessionUser.User);
    }
}