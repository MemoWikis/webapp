using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrueOrFalse;

namespace VueApp;

public class HistoryPageOverviewController(
    PermissionCheck permissionCheck,
    PageChangeRepo pageChangeRepo,
    IHttpContextAccessor httpContextAccessor,
    SessionUser sessionUser)
    : BaseController(sessionUser)
{
    private IOrderedEnumerable<PageChange> _allOrderedPageChanges;

    public readonly record struct TinyPage(string PageName, Day[] Days);

    [HttpGet]
    public TinyPage? Get(int id)
    {
        var topic = EntityCache.GetPage(id);

        if (permissionCheck.CanView(topic))
        {
            _allOrderedPageChanges = pageChangeRepo.GetForPage(id).OrderBy(c => c.Id);

            var days = _allOrderedPageChanges
                .GroupBy(change => change.DateCreated.Date)
                .OrderByDescending(group => group.Key)
                .Select(group => GetDay(
                    group.Key,
                    group.OrderByDescending(g => g.DateCreated).ToArray())
                ).ToArray();

            return new TinyPage
            {
                PageName = topic.Name,
                Days = days
            };
        }

        return null;
    }

    public Day GetDay(DateTime date, IList<PageChange> topicChanges)
    {
        var day = new Day
        {
            Date = date.ToString("dd.MM.yyyy"),
        };

        var authors = new List<Author?>();
        var changes = new List<Change?>();

        foreach (var change in topicChanges)
        {
            authors.Add(GetAuthor(change));
            changes.Add(BuildChange(change));
        }

        day.GroupedChanges = BuildGroupedChanges(changes);

        return day;
    }

    public record struct GroupedChange(
        Change?[] Changes,
        bool Collapsed = true
    );

    public record struct TempGroup(IList<Change?> Changes);

    private GroupedChange[] BuildGroupedChanges(List<Change?> changes)
    {
        var tempGroupChanges = new List<TempGroup>();
        foreach (var change in changes)
        {
            if (tempGroupChanges.IsEmpty() ||
                !ChangeCanBeGrouped(tempGroupChanges.LastOrDefault(), change))
            {
                var newGroup = new TempGroup
                {
                    Changes = new List<Change?> { change }
                };
                tempGroupChanges.Add(newGroup);
                continue;
            }

            tempGroupChanges.LastOrDefault().Changes.Add(change);
        }

        return tempGroupChanges
            .Select(@group => new GroupedChange { Changes = @group.Changes.ToArray() }).ToArray();
    }

    private bool ChangeCanBeGrouped(TempGroup tempGroup, Change? change)
    {
        var currentGroup = tempGroup.Changes.LastOrDefault();
        return currentGroup != null && currentGroup?.PageId == change?.PageId &&
               change?.PageChangeType == PageChangeType.Text &&
               currentGroup?.PageChangeType == change?.PageChangeType &&
               currentGroup?.Author?.Id == change?.Author?.Id;
    }

    public Author? GetAuthor(PageChange change)
    {
        if (change.AuthorId < 1)
            return null;

        var author = EntityCache.GetUserById(change.AuthorId);

        return new Author
        {
            Id = author.Id,
            Name = author.Name,
            ImgUrl = new UserImageSettings(author.Id,
                    httpContextAccessor).GetUrl_50px_square(author)
                .Url,
        };
    }

    public Change? BuildChange(PageChange topicChange)
    {
        var change = new Change
        {
            PageId = topicChange.Page.Id,
            Author = GetAuthor(topicChange),
            ElapsedTime = TimeElapsedAsText.Run(topicChange.DateCreated),
            PageChangeType = topicChange.Type,
            RevisionId = topicChange.Id
        };

        if (topicChange.Type == PageChangeType.Relations)
        {
            var previousChange = _allOrderedPageChanges.LastOrDefault(c => c.Id < topicChange.Id);
            if (previousChange != null)
            {
                var previousRelations = PageEditData_V2.CreateFromJson(previousChange.Data)
                    .CategoryRelations;
                var currentRelations = PageEditData_V2.CreateFromJson(topicChange.Data)
                    .CategoryRelations;

                if (previousRelations != null && currentRelations != null)
                {
                    if (previousRelations.Count > currentRelations.Count)
                    {
                        change.RelationAdded = false;
                        var lastRelationDifference =
                            previousRelations.Except(currentRelations).LastOrDefault();

                        if (permissionCheck.CanViewPage(lastRelationDifference
                                .RelatedPageId) && lastRelationDifference.PageId ==
                            topicChange.Page.Id)
                            change = GetAffectedPageData(change,
                                lastRelationDifference.RelatedPageId);
                        else if (permissionCheck.CanViewPage(lastRelationDifference.PageId))
                            change = GetAffectedPageData(change, lastRelationDifference.PageId);
                        else return null;
                    }
                    else if (previousRelations.Count < currentRelations.Count)
                    {
                        change.RelationAdded = true;
                        var lastRelationDifference =
                            currentRelations.Except(previousRelations).LastOrDefault();

                        if (permissionCheck.CanViewPage(lastRelationDifference
                                .RelatedPageId) && lastRelationDifference.PageId ==
                            topicChange.Page.Id)
                            change = GetAffectedPageData(change,
                                lastRelationDifference.RelatedPageId);
                        else if (permissionCheck.CanViewPage(lastRelationDifference.PageId))
                            change = GetAffectedPageData(change, lastRelationDifference.PageId);
                        else return null;
                    }
                }
            }
        }

        return change;
    }

    private Change GetAffectedPageData(Change change, int id)
    {
        var affectedPage = EntityCache.GetPage(id);
        change.AffectedPageId = affectedPage.Id;
        change.AffectedPageName = affectedPage.Name;

        return change;
    }

    public record struct Day(string Date, GroupedChange[] GroupedChanges);

    public record struct Author(int Id, string Name, string ImgUrl);

    public record struct Change(
        int PageId,
        Author? Author,
        string ElapsedTime,
        int RevisionId,
        bool RelationAdded,
        int AffectedPageId,
        string AffectedPageName,
        string AffectedPageNameEncoded,
        PageChangeType PageChangeType = PageChangeType.Update
    );
}