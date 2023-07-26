using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FluentNHibernate.Conventions;
using TrueOrFalse;

namespace VueApp;

public class HistoryTopicOverviewController : Controller
{
    private readonly PermissionCheck _permissionCheck;
    private readonly CategoryChangeRepo _categoryChangeRepo;
    private readonly CategoryValuationReadingRepo _categoryValuationReadingRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly QuestionValuationRepo _questionValuationRepo;
    private IOrderedEnumerable<CategoryChange> _allOrderedTopicChanges;

    public HistoryTopicOverviewController(PermissionCheck permissionCheck,
        CategoryChangeRepo categoryChangeRepo,
        CategoryValuationReadingRepo categoryValuationReadingRepo,
        UserReadingRepo userReadingRepo,
        QuestionValuationRepo questionValuationRepo)
    {
        _permissionCheck = permissionCheck;
        _categoryChangeRepo = categoryChangeRepo;
        _categoryValuationReadingRepo = categoryValuationReadingRepo;
        _userReadingRepo = userReadingRepo;
        _questionValuationRepo = questionValuationRepo;
    }

    [HttpGet]
    public JsonResult Get(int id)
    {
        var topic = EntityCache.GetCategory(id);

        if (_permissionCheck.CanView(topic))
        {
            _allOrderedTopicChanges = _categoryChangeRepo.GetForTopic(id).OrderBy(c => c.Id);

            var days = _allOrderedTopicChanges
                .GroupBy(change => change.DateCreated.Date)
                .OrderByDescending(group => group.Key)
                .Select(group => GetDay(
                    group.Key,
                    group.OrderByDescending(g => g.DateCreated).ToList())).ToArray();

            return Json(new
            {
                topicName = topic.Name,
                days = days
            }, JsonRequestBehavior.AllowGet);
        }

        return Json(null, JsonRequestBehavior.AllowGet);
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

        day.groupedChanges = BuildGroupedChanges(changes);

        return day;
    }

    public class GroupedChange
    {
        public bool collapsed = true;
        public Change[] changes;
    }

    public class TempGroup
    {
        public IList<Change> changes;
    }

    private GroupedChange[] BuildGroupedChanges(List<Change> changes)
    {
        var tempGroupChanges = new List<TempGroup>();
        foreach (var change in changes)
        {
            if (tempGroupChanges.IsEmpty() || !ChangeCanBeGrouped(tempGroupChanges.LastOrDefault(), change))
            {
                var newGroup = new TempGroup
                {
                    changes = new List<Change> { change }
                };
                tempGroupChanges.Add(newGroup);
                continue;
            }
            tempGroupChanges.LastOrDefault()?.changes.Add(change);
        }

        return tempGroupChanges.Select(@group => new GroupedChange { changes = @group.changes.ToArray() }).ToArray();
    }

    private bool ChangeCanBeGrouped(TempGroup tempGroup, Change change)
    {
        var currentGroup = tempGroup.changes.LastOrDefault();
        return currentGroup != null && currentGroup.topicId == change.topicId && change.topicChangeType == CategoryChangeType.Text &&
               currentGroup.topicChangeType == change.topicChangeType && currentGroup.author.id == change.author.id;
    }

    public Author SetAuthor(CategoryChange change)
    {
        if (change.AuthorId < 1)
            return null;
        var author = SessionUserCache.GetItem(change.AuthorId, _categoryValuationReadingRepo, _userReadingRepo, _questionValuationRepo);
        return new Author
        {
            id = author.Id,
            name = author.Name,
            imgUrl = new UserImageSettings(author.Id).GetUrl_50px_square(author).Url,
        };
    }

    public Change SetChange(CategoryChange topicChange)
    {
        var change = new Change
        {
            topicId = topicChange.Category.Id,
            author = SetAuthor(topicChange),
            elapsedTime = TimeElapsedAsText.Run(topicChange.DateCreated),
            topicChangeType = topicChange.Type,
            revisionId = topicChange.Id
        };

        if (topicChange.Type == CategoryChangeType.Relations)
        {
            var previousChange = _allOrderedTopicChanges.LastOrDefault(c => c.Id < topicChange.Id);
            if (previousChange != null)
            {
                var previousRelations = CategoryEditData_V2.CreateFromJson(previousChange.Data).CategoryRelations;
                var currentRelations = CategoryEditData_V2.CreateFromJson(topicChange.Data).CategoryRelations;

                if (previousRelations.Count > currentRelations.Count)
                {
                    change.relationAdded = false;
                    var lastRelationDifference = previousRelations.Except(currentRelations).LastOrDefault();

                    if (_permissionCheck.CanViewCategory(lastRelationDifference.RelatedCategoryId) && lastRelationDifference.CategoryId == topicChange.Category.Id)
                        change = GetAffectedTopicData(change, lastRelationDifference.RelatedCategoryId);
                    else if (_permissionCheck.CanViewCategory(lastRelationDifference.CategoryId))
                        change = GetAffectedTopicData(change, lastRelationDifference.CategoryId);
                    else return null;
                }
                else if (previousRelations.Count < currentRelations.Count)
                {
                    change.relationAdded = true;
                    var lastRelationDifference = currentRelations.Except(previousRelations).LastOrDefault();

                    if (_permissionCheck.CanViewCategory(lastRelationDifference.RelatedCategoryId) && lastRelationDifference.CategoryId == topicChange.Category.Id)
                        change = GetAffectedTopicData(change, lastRelationDifference.RelatedCategoryId);
                    else if (_permissionCheck.CanViewCategory(lastRelationDifference.CategoryId))
                        change = GetAffectedTopicData(change, lastRelationDifference.CategoryId);
                    else return null;
                }
            }
        }

        return change;
    }

    private Change GetAffectedTopicData(Change change, int id)
    {
        var affectedTopic = EntityCache.GetCategory(id);
        change.affectedTopicId = affectedTopic.Id;
        change.affectedTopicName = affectedTopic.Name;

        return change;
    }

    public class Day
    {
        public string date { get; set; }
        public GroupedChange[] groupedChanges { get; set; }
    }

    public class Author
    {
        public int id { get; set; }
        public string name { get; set; }
        public string imgUrl { get; set; }
    }

    public class Change
    {
        public int topicId { get; set; }
        public Author author { get; set; }
        public string elapsedTime { get; set; }
        public CategoryChangeType topicChangeType { get; set; } = CategoryChangeType.Update;
        public int revisionId { get; set; }
        public bool relationAdded { get; set; }
        public int affectedTopicId { get; set; }
        public string affectedTopicName { get; set; }
        public string affectedTopicNameEncoded { get; set; }
    }
}