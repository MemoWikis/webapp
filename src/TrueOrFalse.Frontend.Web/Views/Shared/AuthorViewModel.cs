using System.Collections.Generic;
using System.Linq;

public class AuthorViewModel
{
    public string Name => User.Name;
    public string ImageUrl;
    public UserTinyModel User;
    public bool ShowWishKnowledge;
    public int Reputation;
    public int ReputationPos;

    public static List<AuthorViewModel> Convert(IList<UserTinyModel> authors)
    {
        return authors.Select(author => new AuthorViewModel
        {
            ImageUrl = new UserImageSettings(author.Id).GetUrl_250px(author).Url,
            User = author,
            Reputation = author.Reputation,
            ReputationPos = author.ReputationPos
        }).ToList();
    }

}