[Serializable]
public class CommentCacheItem
{
    public virtual CommentType Type { get; set; }
    public virtual int TypeId { get; set; }

    public virtual int AnswerToId { get; set; }

    public virtual IList<Comment> Answers { get; set; }
    public virtual UserCacheItem Creator { get; set; }
    public virtual string Title { get; set; }

    public virtual string Text { get; set; }

    public virtual bool ShouldRemove { get; set; }

    public virtual bool ShouldImprove { get; set; }

    public virtual string ShouldKeys { get; set; }

    public virtual bool IsSettled { get; set; }

    public CommentCacheItem()
    {
        Type = CommentType.AnswerQuestion;
    }

    public static CommentCacheItem ToCommentCacheItem(Comment comment)
    {
        return new CommentCacheItem
        {
            Type = comment.Type,
            TypeId = comment.TypeId,
            AnswerToId = comment.AnswerTo.Id,
            Answers = comment.Answers,
            Creator = EntityCache.GetUserById(comment.Creator.Id),
            Title = comment.Title,
            Text = comment.Text,
            ShouldRemove = comment.ShouldRemove,
            ShouldImprove = comment.ShouldImprove,
            ShouldKeys = comment.ShouldKeys,
            IsSettled = comment.IsSettled
        };
    }
}