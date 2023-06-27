﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Web;

namespace VueApp;

public class HistoryTopicAllTopicsOverviewController : BaseController
{
    private readonly AllTopicsHistory _allTopicsHistory;

    public HistoryTopicAllTopicsOverviewController(AllTopicsHistory allTopicsHistory)
    {
        _allTopicsHistory = allTopicsHistory;
    }

    [HttpGet]
    public JsonResult Get(int page)
    {
        const int revisionsToShow = 100;
        var days = GetDays(page, revisionsToShow);

        return Json(days, JsonRequestBehavior.AllowGet);
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
            var previousChange = Sl.CategoryChangeRepo.GetForTopic(topicChange.Category.Id).OrderBy(c => c.Id).LastOrDefault(c => c.Id < topicChange.Id);
            
            if (previousChange == null) 
                return change;

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