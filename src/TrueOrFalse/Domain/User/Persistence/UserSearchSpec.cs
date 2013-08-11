using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    public class UserSearchSpec : SearchSpecificationBase<UserFilter, UserOrderBy>
    {
        public string SearchTearm;   
    }

    public class UserFilter : ConditionContainer
    {
        public readonly ConditionString EmailAddress;

        public UserFilter(){
            EmailAddress = new ConditionString(this, "EmailAddress");
        }
    }

    public class UserOrderBy : SpecOrderByBase{}
}
