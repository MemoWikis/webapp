﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrueOrFalse;

namespace VueApp;

public class HistoryTopicOverviewController(
    PermissionCheck permissionCheck,
    PageChangeRepo pageChangeRepo,
    IHttpContextAccessor httpContextAccessor,
    SessionUser sessionUser)
    : BaseController(sessionUser)
{
    private IOrderedEnumerable<PageChange> _allOrderedTopicChanges;

    public readonly record struct TinyTopic(string TopicName, Day[] Days);

    [HttpGet]
    public TinyTopic? Get(int id)
    {
        var topic = EntityCache.GetPage(id);

        if (permissionCheck.CanView(topic))
        {
            _allOrderedTopicChanges = pageChangeRepo.GetForTopic(id).OrderBy(c => c.Id);

            var days = _allOrderedTopicChanges
                .GroupBy(change => change.DateCreated.Date)
                .OrderByDescending(group => group.Key)
                .Select(group => GetDay(
                    group.Key,
                    group.OrderByDescending(g => g.DateCreated).ToArray())
                ).ToArray();

            return new TinyTopic
            {
                TopicName = topic.Name,
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
        return currentGroup != null && currentGroup?.TopicId == change?.TopicId &&
               change?.TopicChangeType == PageChangeType.Text &&
               currentGroup?.TopicChangeType == change?.TopicChangeType &&
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
            TopicId = topicChange.Page.Id,
            Author = GetAuthor(topicChange),
            ElapsedTime = TimeElapsedAsText.Run(topicChange.DateCreated),
            TopicChangeType = topicChange.Type,
            RevisionId = topicChange.Id
        };

        if (topicChange.Type == PageChangeType.Relations)
        {
            var previousChange = _allOrderedTopicChanges.LastOrDefault(c => c.Id < topicChange.Id);
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

                        if (permissionCheck.CanViewCategory(lastRelationDifference
                                .RelatedPageId) && lastRelationDifference.PageId ==
                            topicChange.Page.Id)
                            change = GetAffectedTopicData(change,
                                lastRelationDifference.RelatedPageId);
                        else if (permissionCheck.CanViewCategory(lastRelationDifference.PageId))
                            change = GetAffectedTopicData(change, lastRelationDifference.PageId);
                        else return null;
                    }
                    else if (previousRelations.Count < currentRelations.Count)
                    {
                        change.RelationAdded = true;
                        var lastRelationDifference =
                            currentRelations.Except(previousRelations).LastOrDefault();

                        if (permissionCheck.CanViewCategory(lastRelationDifference
                                .RelatedPageId) && lastRelationDifference.PageId ==
                            topicChange.Page.Id)
                            change = GetAffectedTopicData(change,
                                lastRelationDifference.RelatedPageId);
                        else if (permissionCheck.CanViewCategory(lastRelationDifference.PageId))
                            change = GetAffectedTopicData(change, lastRelationDifference.PageId);
                        else return null;
                    }
                }
            }
        }

        return change;
    }

    private Change GetAffectedTopicData(Change change, int id)
    {
        var affectedTopic = EntityCache.GetPage(id);
        change.AffectedTopicId = affectedTopic.Id;
        change.AffectedTopicName = affectedTopic.Name;

        return change;
    }

    public record struct Day(string Date, GroupedChange[] GroupedChanges);

    public record struct Author(int Id, string Name, string ImgUrl);

    public record struct Change(
        int TopicId,
        Author? Author,
        string ElapsedTime,
        int RevisionId,
        bool RelationAdded,
        int AffectedTopicId,
        string AffectedTopicName,
        string AffectedTopicNameEncoded,
        PageChangeType TopicChangeType = PageChangeType.Update
    );
}