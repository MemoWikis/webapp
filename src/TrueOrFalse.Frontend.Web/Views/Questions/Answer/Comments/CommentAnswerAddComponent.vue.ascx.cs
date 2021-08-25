public class CommentAnswerAddModel : BaseModel
{
    public string AuthorImageUrl;
    public int ParentCommentId;

    public CommentAnswerAddModel()
    {
        var user = _sessionUser.User;
        AuthorImageUrl = new UserImageSettings(user.Id).GetUrl_128px_square(user).Url;
    }
}
