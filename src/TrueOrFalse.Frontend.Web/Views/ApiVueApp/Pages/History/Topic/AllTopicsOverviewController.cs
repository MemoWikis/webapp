﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class HistoryTopicAllTopicsOverviewController : BaseController
{
    private readonly AllTopicsHistory _allTopicsHistory;
    private readonly PermissionCheck _permissionCheck;
    private readonly CategoryChangeRepo _categoryChangeRepo;
    private readonly SessionUserCache _sessionUserCache;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HistoryTopicAllTopicsOverviewController(AllTopicsHistory allTopicsHistory,
        PermissionCheck permissionCheck, CategoryChangeRepo categoryChangeRepo,
        SessionUserCache sessionUserCache,
        IHttpContextAccessor httpContextAccessor, SessionUser sessionUser) : base(sessionUser)
    {
        _allTopicsHistory = allTopicsHistory;
        _permissionCheck = permissionCheck;
        _categoryChangeRepo = categoryChangeRepo;
        _sessionUserCache = sessionUserCache;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public JsonResult Get(int page)
    {
        const int revisionsToShow = 100;
        var days = GetDays(page, revisionsToShow);

        return Json(days);
    }

    private Day[] GetDays(int page, int revisionsToShow)
    {
        return _allTopicsHistory.GetGroupedChanges(page, revisionsToShow).Select(group => GetDay(
                @group.Key,
                @group.OrderByDescending(g => g.DateCreated).ToList()))
            .ToArray();
    }

    private Day GetDay(DateTime date, IList<CategoryChange> topicChanges)
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

    public class GroupedChange
    {
        public bool collapsed { get; set; } = true;
        public Change[] changes { get; set; }
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

    public Author GetAuthor(CategoryChange change)
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

    public Change BuildChange(CategoryChange topicChange)
    {

        var change = new Change
        {
            topicId = topicChange.Category.Id,
            topicName = topicChange.Category.Name,
            topicImgUrl = new CategoryImageSettings(topicChange.Category.Id,
                _httpContextAccessor)
                .GetUrl(50)
                .Url,
            author = GetAuthor(topicChange),
            timeCreated = topicChange.DateCreated.ToString("HH:mm"),
            topicChangeType = topicChange.Type,
            revisionId = topicChange.Id,
        };

        if (topicChange.Type == CategoryChangeType.Relations)
        {
            var previousChange = _categoryChangeRepo.GetForTopic(topicChange.Category.Id).OrderBy(c => c.Id).LastOrDefault(c => c.Id < topicChange.Id);
            
            if (previousChange == null) 
                return change;

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
            }
            else if (previousRelations.Count < currentRelations.Count)
            {
                change.relationAdded = true;
                var lastRelationDifference = currentRelations.Except(previousRelations).LastOrDefault();

                if (_permissionCheck.CanViewCategory(lastRelationDifference.RelatedCategoryId) && lastRelationDifference.CategoryId == topicChange.Category.Id)
                    change = GetAffectedTopicData(change, lastRelationDifference.RelatedCategoryId);
                else if (_permissionCheck.CanViewCategory(lastRelationDifference.CategoryId))
                    change = GetAffectedTopicData(change, lastRelationDifference.CategoryId);
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