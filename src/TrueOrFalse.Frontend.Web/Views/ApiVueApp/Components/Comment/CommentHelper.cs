﻿using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Web;
using static System.Net.Mime.MediaTypeNames;

namespace VueApp;

public class CommentHelper : IRegisterAsInstancePerLifetime
{
    private readonly CommentRepository _commentRepository;
    private readonly UserRepo _userRepo;

    public CommentHelper(CommentRepository commentRepository, UserRepo userRepo)
    {
        _commentRepository = commentRepository;
        _userRepo = userRepo;
    }

    public static CommentJson GetComment(Comment c, bool showSettled = false)
    {
        var comment = new CommentJson
        {
            id = c.Id,
            title = c.Title,
            text = c.Text,

            creatorName = c.Creator.Name,
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

    public void SaveComment(CommentType type,int typeId, string text, string title, int userId )
    {
        var comment = new Comment();
        comment.Type = type;
        comment.TypeId = typeId;
        comment.Text = text;
        comment.Title = title;
        comment.Creator = _userRepo.GetById(userId);

        _commentRepository.Create(comment);
    }


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