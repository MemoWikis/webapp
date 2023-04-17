﻿using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Web;

namespace VueApp;

public static class CommentHelper
{
    public static CommentJson GetComment(Comment c, bool showSettled = false)
    {
        var comment = new CommentJson
        {
            id = c.Id,
            title = c.Title,
            text = c.Text,

            creatorName = c.Creator.Name,
            creatorEncodedName = UriSanitizer.Run(c.Creator.Name),
            creatorId = c.Creator.Id,
            creatorImgUrl = new UserImageSettings(c.Creator.Id).GetUrl_128px_square(c.Creator).Url,

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

    public class CommentJson
    {
        public int id { get; set; }
        public string title { get; set; }
        public string text { get; set; }

        public string creatorName { get; set; }
        public string creatorEncodedName { get; set; }
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