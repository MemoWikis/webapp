namespace TrueOrFalse.Search
{
    //todo mark to Delete
    public class ToUserSolrMap
    {
        public static UserSolrMap Run(User user)
        {
            var result = new UserSolrMap();
            result.Id = user.Id;
            result.Name = user.Name;
            result.DateCreated = user.DateCreated;

            result.Rank = user.ReputationPos;
            result.WishCountQuestions = user.WishCountQuestions;

            return result;
        }
    }
}
