namespace TrueOrFalse.Search
{
    public class MeiliSearchToUserMap
    {
        public static MeiliSearchUserMap Run(User user)
        {
            var result = new MeiliSearchUserMap
            {
                Id = user.Id,
                Name = user.Name,
                DateCreated = user.DateCreated,
                Rank = user.ReputationPos,
                WishCountQuestions = user.WishCountQuestions
        };
            return result;
        }

        public static MeiliSearchUserMap Run(UserCacheItem user)
        {
            var result = new MeiliSearchUserMap
            {
                Id = user.Id,
                Name = user.Name,
                Rank = user.ReputationPos,
                WishCountQuestions = user.WishCountQuestions
            };
            return result;
        }
    }
}
