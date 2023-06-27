using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;

[Serializable]
public class UserSearchSpec : SearchSpecificationBase<UserFilter, UserOrderBy>
{
    public string SearchTerm;

}

[Serializable]
public class UserFilter : ConditionContainer
{
    public readonly ConditionString EmailAddress;

    public UserFilter(){
        EmailAddress = new ConditionString(this, "EmailAddress");
    }
}

[Serializable]
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