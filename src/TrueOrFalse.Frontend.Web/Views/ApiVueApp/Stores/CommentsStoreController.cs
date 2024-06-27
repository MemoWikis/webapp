using System.Collections.Generic;
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
            Id = c.Id,
            Title = c.Title,
            Text = c.Text,

            CreatorName = c.Creator.Name,
            CreatorId = c.Creator.Id,
            CreatorImgUrl = new UserImageSettings(c.Creator.Id, _httpContextAccessor)
                .GetUrl_128px_square(c.Creator)
                .Url,

            CreationDate = c.DateCreated.ToString("U"),
            CreationDateNiceText = DateTimeUtils.TimeElapsedAsText(c.DateCreated),

            ShouldBeImproved = c.ShouldImprove,
            ShouldBeDeleted = c.ShouldRemove,
            ShouldReasons = TrueOrFalse.ShouldReasons.ByKeys(c.ShouldKeys),

            IsSettled = c.IsSettled,
            ShowSettledAnswers = showSettled
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
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        public string CreatorName { get; set; }
        public int CreatorId { get; set; }
        public string CreatorImgUrl { get; set; }

        public string CreationDate { get; set; }
        public string CreationDateNiceText { get; set; }


        public bool ShouldBeImproved { get; set; }
        public bool ShouldBeDeleted { get; set; }
        public bool IsSettled { get; set; }

        public List<string> ShouldReasons { get; set; }

        public CommentJson[] answers { get; set; }
        public int answersSettledCount { get; set; } = 0;
        public bool ShowSettledAnswers { get; set; }
    }
}

