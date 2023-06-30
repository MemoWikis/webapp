using System;
using System.Linq;
using System.Web.Mvc;
using NHibernate;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Domain;
using TrueOrFalse.Search;
using TrueOrFalse.Web;

namespace VueApp;

public class HistoryTopicDetailController : Controller
{
    private readonly PermissionCheck _permissionCheck;
    private readonly ISession _nhibernatesession;
    private readonly SessionUser _sessionUser;
    private readonly RestoreCategory _restoreCategory;

    public HistoryTopicDetailController(PermissionCheck permissionCheck,
        ISession nhibernatesession,
        SessionUser sessionUser,
        RestoreCategory restoreCategory)
    {
        _permissionCheck = permissionCheck;
        _nhibernatesession = nhibernatesession;
        _sessionUser = sessionUser;
        _restoreCategory = restoreCategory;
    }

    [HttpGet]
    public JsonResult Get(int topicId, int currentRevisionId, int firstEditId = 0)
    {
        if(!_permissionCheck.CanViewCategory(topicId))
            throw new Exception("not allowed");

        var listWithAllVersions = Sl.CategoryChangeRepo.GetForTopic(topicId).OrderBy(c => c.Id);
        var isCategoryDeleted = listWithAllVersions.Any(cc => cc.Type == CategoryChangeType.Delete);

        var currentRevision = listWithAllVersions.FirstOrDefault(c => c.Id == currentRevisionId);

        var previousRevision = firstEditId <= 0 ? listWithAllVersions.LastOrDefault(c => c.Id < currentRevisionId) : listWithAllVersions.LastOrDefault(c => c.Id < firstEditId);

        if (currentRevision.Category.Id != previousRevision.Category.Id)
            throw new Exception("different topic ids");

        var nextRevision = listWithAllVersions.FirstOrDefault(c => c.Id > currentRevisionId);
        var topicHistoryDetailModel = new CategoryHistoryDetailModel(currentRevision, previousRevision, nextRevision, isCategoryDeleted,_permissionCheck, _nhibernatesession);

        var result = new ChangeDetailResult
        {
            topicName = topicHistoryDetailModel.CategoryName,
            imageWasUpdated = topicHistoryDetailModel.ImageWasUpdated,
            isCurrent = !topicHistoryDetailModel.NextRevExists,
            changeType = topicHistoryDetailModel.ChangeType,
            currentChangeDate = currentRevision.DateCreated.ToString("dd.MM.yyyy HH:mm:ss"),
            previousChangeDate = previousRevision.DateCreated.ToString("dd.MM.yyyy HH:mm:ss"),
            authorName = currentRevision.Author.Name,
            authorId = currentRevision.Author.Id,
            authorImgUrl = new UserImageSettings(currentRevision.Author.Id).GetUrl_20px(currentRevision.Author).Url
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

        return Json(result, JsonRequestBehavior.AllowGet);
    }

    class ChangeDetailResult
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


    public CategoryHistoryDetailModel GetCategoryHistoryDetailModel(int categoryId, int firstEditId, int selectedRevId)
    {
        var listWithAllVersions = Sl.CategoryChangeRepo.GetForCategory(categoryId).OrderBy(c => c.Id);
        var isCategoryDeleted = listWithAllVersions.Any(cc => cc.Type == CategoryChangeType.Delete);

        var currentRevision = listWithAllVersions.FirstOrDefault(c => c.Id == selectedRevId);
        var previousRevision = listWithAllVersions.LastOrDefault(c => c.Id < firstEditId);
        var nextRevision = listWithAllVersions.FirstOrDefault(c => c.Id > selectedRevId);
        return new CategoryHistoryDetailModel(currentRevision, previousRevision, nextRevision, isCategoryDeleted, _permissionCheck, _nhibernatesession);
    }

    [AccessOnlyAsLoggedIn]
    [HttpGet]
    public void RestoreTopic(int topicChangeId)
    {
        var topicChange = Sl.CategoryChangeRepo.GetByIdEager(topicChangeId);
        var isCorrectType = topicChange.Type is CategoryChangeType.Text or CategoryChangeType.Renamed;

        if (!_permissionCheck.CanViewCategory(topicChange.Category.Id) || !_permissionCheck.CanEditCategory(topicChange.Category.Id))
            throw new Exception("not allowed");

        _restoreCategory.Run(topicChangeId, _sessionUser.User);
    }
}