using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class HistoryPageAllPagesOverviewController(
    AllPagesHistory _allPagesHistory,
    PermissionCheck _permissionCheck,
    PageChangeRepo pageChangeRepo,
    IHttpContextAccessor _httpContextAccessor) : Controller
{
    [HttpGet]
    public Day[] Get(int page)
    {
        const int revisionsToShow = 100;
        var days = GetDays(page, revisionsToShow);

        return days;
    }

    private Day[] GetDays(int page, int revisionsToShow)
    {
        return _allPagesHistory.GetGroupedChanges(page, revisionsToShow).Select(group => GetDay(
                @group.Key,
                @group.OrderByDescending(g => g.DateCreated).ToList()))
            .ToArray();
    }

    private Day GetDay(DateTime date, IList<PageChange> topicChanges)
    {
        var day = new Day
        {
            date = date.ToString("dd.MM.yyyy"),
        };

        var authors = new List<Author>();
        var changes = new List<Change>();

        foreach (var change in topicChanges)
        {
            authors.Add(GetAuthor(change));
            changes.Add(BuildChange(change));
        }

        day.groupedChanges = BuildGroupedChanges(changes);
        return day;
    }

    public readonly record struct GroupedChange(
        Change[] Changes,
        bool Collapsed = true);

    public class TempGroup
    {
        public IList<Change> Changes { get; set; }
    };

    private GroupedChange[] BuildGroupedChanges(List<Change> changes)
    {
        var tempGroupChanges = new List<TempGroup>();
        foreach (var change in changes)
        {
            if (tempGroupChanges.IsEmpty() ||
                !ChangeCanBeGrouped(tempGroupChanges.LastOrDefault(), change))
            {
                var newGroup = new TempGroup
                {
                    Changes = new List<Change> { change }
                };
                tempGroupChanges.Add(newGroup);
                continue;
            }

            tempGroupChanges.LastOrDefault()?.Changes.Add(change);
        }

        return tempGroupChanges
            .Select(@group => new GroupedChange { Changes = @group.Changes.ToArray() }).ToArray();
    }

    private bool ChangeCanBeGrouped(TempGroup tempGroup, Change change)
    {
        var currentGroup = tempGroup.Changes.LastOrDefault();
        return currentGroup != null && currentGroup.pageId == change.pageId &&
               change.topicChangeType == PageChangeType.Text &&
               currentGroup.topicChangeType == change.topicChangeType &&
               currentGroup.author.id == change.author.id;
    }

    public Author GetAuthor(PageChange change)
    {
        if (change.AuthorId < 1)
            return null;

        var author = EntityCache.GetUserById(change.AuthorId);

        return new Author
        {
            id = author.Id,
            name = author.Name
        };
    }

    public Change BuildChange(PageChange topicChange)
    {
        var change = CreateChangeObject(topicChange);

        if (topicChange.Type == PageChangeType.Relations)
        {
            var previousChange = pageChangeRepo.GetForPage(topicChange.Page.Id)
                .OrderBy(c => c.Id).LastOrDefault(c => c.Id < topicChange.Id);

            if (previousChange == null)
                return change;

            var previousRelations = PageEditData_V2.CreateFromJson(previousChange.Data)
                .CategoryRelations;
            var currentRelations =
                PageEditData_V2.CreateFromJson(topicChange.Data).CategoryRelations;
            if (previousRelations != null && currentRelations != null)
            {
                if (previousRelations.Count > currentRelations.Count)
                {
                    change.relationAdded = false;
                    var lastRelationDifference =
                        previousRelations.Except(currentRelations).LastOrDefault();

                    if (_permissionCheck.CanViewPage(lastRelationDifference.RelatedPageId) &&
                        lastRelationDifference.PageId == topicChange.Page.Id)
                    {
                        change = GetAffectedPageData(change, lastRelationDifference.RelatedPageId);
                    }
                    else if (_permissionCheck.CanViewPage(lastRelationDifference.PageId))
                    {
                        change = GetAffectedPageData(change, lastRelationDifference.PageId);
                    }
                }
                else if (previousRelations.Count < currentRelations.Count)
                {
                    change.relationAdded = true;
                    var lastRelationDifference =
                        currentRelations.Except(previousRelations).LastOrDefault();

                    if (_permissionCheck.CanViewPage(lastRelationDifference.RelatedPageId) &&
                        lastRelationDifference.PageId == topicChange.Page.Id)
                        change = GetAffectedPageData(change, lastRelationDifference.RelatedPageId);
                    else if (_permissionCheck.CanViewPage(lastRelationDifference.PageId))
                        change = GetAffectedPageData(change, lastRelationDifference.PageId);
                }
            }
        }

        return change;
    }

    private Change CreateChangeObject(PageChange topicChange)
    {
        return new Change
        {
            pageId = topicChange.Page.Id,
            topicName = topicChange.Page.Name,
            topicImgUrl = new PageImageSettings(topicChange.Page.Id,
                    _httpContextAccessor)
                .GetUrl(50)
                .Url,
            author = GetAuthor(topicChange),
            timeCreated = topicChange.DateCreated.ToString("HH:mm"),
            topicChangeType = topicChange.Type,
            revisionId = topicChange.Id,
        };
    }

    private Change GetAffectedPageData(Change change, int id)
    {
        var affectedPage = EntityCache.GetPage(id);
        change.affectedPageId = affectedPage.Id;
        change.affectedPageName = affectedPage.Name;

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
    }

    public class Change
    {
        public int pageId { get; set; }
        public string topicName { get; set; }
        public string topicImgUrl { get; set; }
        public Author author { get; set; }
        public string timeCreated { get; set; }
        public PageChangeType topicChangeType { get; set; } = PageChangeType.Update;
        public int revisionId { get; set; }
        public bool relationAdded { get; set; }
        public int affectedPageId { get; set; }
        public string affectedPageName { get; set; }
        public string affectedPageNameEncoded { get; set; }
    }
}
