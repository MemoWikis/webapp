﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace VueApp;

public class CommentsStoreController(
    CommentRepository _commentRepository,
    IHttpContextAccessor _httpContextAccessor) : Controller
{
    [HttpGet]
    public Comments GetAllComments([FromRoute] int id)
    {
        var _comments = _commentRepository.GetForDisplay(id);
        var settledComments = _comments.Where(c => c.IsSettled).Select(c => GetComment(c)).ToArray();
        var unsettledComments = _comments.Where(c => !c.IsSettled).Select(c => GetComment(c)).ToArray();

        return new Comments(

            SettledComments: settledComments,
            UnsettledComments: unsettledComments
        );
    }

    private CommentJson GetComment(Comment c, bool showSettled = false)
    {
        var comment = new CommentJson
        {
            id = c.Id,
            title = c.Title,
            text = c.Text,

            creatorName = c.Creator.Name,
            creatorId = c.Creator.Id,
            creatorImgUrl = new UserImageSettings(c.Creator.Id, _httpContextAccessor)
                .GetUrl_128px_square(c.Creator)
                .Url,

            creationDate = c.DateCreated.ToString("U"),
            creationDateNiceText = DateTimeUtils.TimeElapsedAsText(c.DateCreated),

            shouldBeImproved = c.ShouldImprove,
            shouldBeDeleted = c.ShouldRemove,
            shouldReasons = TrueOrFalse.ShouldReasons.ByKeys(c.ShouldKeys),

            isSettled = c.IsSettled,
            showSettledAnswers = showSettled
        };

        if (c.Answers != null)
        {
            if (showSettled)
            {
                comment.answers = c.Answers
                    .OrderBy(x => x.DateCreated)
                    .Select(x => GetComment(x, true)).ToArray();
            }
            else
            {
                comment.answers = c.Answers
                    .Where(x => !x.IsSettled)
                    .OrderBy(x => x.DateCreated)
                    .Select(x => GetComment(x)).ToArray();
            }
            comment.answersSettledCount = c.Answers.Count(x => x.IsSettled);
        }
        return comment;
    }

    public record struct Comments(CommentJson[] SettledComments, CommentJson[] UnsettledComments);
    public class CommentJson
    {
        public int id { get; set; }
        public string title { get; set; }
        public string text { get; set; }

        public string creatorName { get; set; }
        public int creatorId { get; set; }
        public string creatorImgUrl { get; set; }

        public string creationDate { get; set; }
        public string creationDateNiceText { get; set; }


        public bool shouldBeImproved { get; set; }
        public bool shouldBeDeleted { get; set; }
        public bool isSettled { get; set; }

        public List<string> shouldReasons { get; set; }

        public CommentJson[] answers { get; set; }
        public int answersSettledCount { get; set; } = 0;
        public bool showSettledAnswers { get; set; }
    }
}

