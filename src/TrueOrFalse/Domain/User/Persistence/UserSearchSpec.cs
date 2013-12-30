using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    public class UserSearchSpec : SearchSpecificationBase<UserFilter, UserOrderBy>
    {
        public string SearchTerm;   
    }

    public class UserFilter : ConditionContainer
    {
        public readonly ConditionString EmailAddress;

        public UserFilter(){
            EmailAddress = new ConditionString(this, "EmailAddress");
        }
    }

    public class UserOrderBy : SpecOrderByBase
    {
        public OrderBy Reputation;
        public OrderBy WishCount;

        public UserOrderBy()
        {
            Reputation = new OrderBy("Reputation", this);
            WishCount = new OrderBy("WishCountQuestions", this);
        }

        public string ToText()
        {
            if (Reputation.IsCurrent())
                return "Reputation";

            if (WishCount.IsCurrent())
                return "Wunschwissen";

            return "";
        }
    }
}
