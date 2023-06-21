using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NHibernate;
using TrueOrFalse.Web;

namespace VueApp;

public class HistoryTopicAllTopicsOverviewController : BaseController
{
    private IOrderedEnumerable<CategoryChange> _orderedTopicChangesOnPage;

    [HttpGet]
    public JsonResult Get(int page)
    {

        const int revisionsToShow = 100;
        var revisionsToSkip = (page - 1) * revisionsToShow;
        var query = $@"SELECT * FROM CategoryChange cc ORDER BY cc.DateCreated DESC LIMIT {revisionsToSkip},{revisionsToShow}";
        _orderedTopicChangesOnPage = Sl.R<ISession>().CreateSQLQuery(query).AddEntity(typeof(CategoryChange)).List<CategoryChange>().OrderBy(c => c.Id);
            
        var days = _orderedTopicChangesOnPage
            .Where(ChangeVisibilityCheck)
            .GroupBy(change => change.DateCreated.Date)
            .OrderByDescending(group => group.Key)
            .Select(group => GetDay(
                group.Key,
                group.OrderByDescending(g => g.DateCreated).ToList()))
            .ToArray();


        return Json(days, JsonRequestBehavior.AllowGet);
    }

    private bool ChangeVisibilityCheck(CategoryChange change)
    {
        return change.Category.Id > 0 && 
               PermissionCheck.CanView(change.Category) &&
               PermissionCheck.CanView(change.Category.Creator.Id, change.GetCategoryChangeData().Visibility);
    }

    public Day GetDay(DateTime date, IList<CategoryChange> topicChanges)
    {

        var day = new Day
        {
            date = date.ToString("dd.MM.yyyy"),
        };

        var authors = new List<Author>();
        var changes = new List<Change>();

        foreach (var change in topicChanges)
        {
            authors.Add(SetAuthor(change));
            changes.Add(SetChange(change));
        }

        day.changes = changes.ToArray();

        return day;
    }

    public Author SetAuthor(CategoryChange change)
    {
        if (change.AuthorId < 1)
            return null;
        var author = SessionUserCache.GetItem(change.AuthorId);
        return new Author
        {
            id = author.Id,
            name = author.Name
        };
    }

    public Change SetChange(CategoryChange topicChange)
    {
        
        var change = new Change
        {
            topicId = topicChange.Category.Id,
            topicName = topicChange.Category.Name,
            topicImgUrl = new CategoryImageSettings(topicChange.Category.Id).GetUrl_50px().Url,
            author = SetAuthor(topicChange),
            timeCreated = topicChange.DateCreated.ToString("HH:mm"),
            topicChangeType = topicChange.Type,
            revisionId = topicChange.Id,
        };

        if (topicChange.Type == CategoryChangeType.Relations)
        {
            var previousChange = _orderedTopicChangesOnPage.LastOrDefault(c => c.Id < topicChange.Id && topicChange.Category.Id == c.Category.Id);
            if (previousChange != null)
            {
                var previousRelations = CategoryEditData_V2.CreateFromJson(previousChange.Data).CategoryRelations;
                var currentRelations = CategoryEditData_V2.CreateFromJson(topicChange.Data).CategoryRelations;

                if (previousRelations.Count > currentRelations.Count)
                {
                    change.relationAdded = false;
                    var lastRelationDifference = previousRelations.Except(currentRelations).LastOrDefault();

                    if (PermissionCheck.CanViewCategory(lastRelationDifference.RelatedCategoryId) && lastRelationDifference.CategoryId == topicChange.Category.Id)
                        change = GetAffectedTopicData(change, lastRelationDifference.RelatedCategoryId);
                    else if (PermissionCheck.CanViewCategory(lastRelationDifference.CategoryId))
                        change = GetAffectedTopicData(change, lastRelationDifference.CategoryId);
                }
                else if (previousRelations.Count < currentRelations.Count)
                {
                    change.relationAdded = true;
                    var lastRelationDifference = currentRelations.Except(previousRelations).LastOrDefault();

                    if (PermissionCheck.CanViewCategory(lastRelationDifference.RelatedCategoryId) && lastRelationDifference.CategoryId == topicChange.Category.Id)
                        change = GetAffectedTopicData(change, lastRelationDifference.RelatedCategoryId);
                    else if (PermissionCheck.CanViewCategory(lastRelationDifference.CategoryId))
                        change = GetAffectedTopicData(change, lastRelationDifference.CategoryId);
                }
            }
        }

        return change;
    }

    private Change GetAffectedTopicData(Change change, int id)
    {
        var affectedTopic = EntityCache.GetCategory(id);
        change.affectedTopicId = affectedTopic.Id;
        change.affectedTopicNameEncoded = UriSanitizer.Run(affectedTopic.Name);
        change.affectedTopicName = affectedTopic.Name;

        return change;
    }

    public class Day
    {
        public string date { get; set; }
        public Change[] changes { get; set; }
    }

    public class Author
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Change
    {
        public int topicId { get; set; }
        public string topicName { get; set; }
        public string topicImgUrl { get; set; }
        public Author author { get; set; }
        public string timeCreated { get; set; }
        public CategoryChangeType topicChangeType { get; set; } = CategoryChangeType.Update;
        public int revisionId { get; set; }
        public bool relationAdded { get; set; }
        public int affectedTopicId { get; set; }
        public string affectedTopicName { get; set; }
        public string affectedTopicNameEncoded { get; set; }
    }
}