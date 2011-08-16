using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core
{
    public class UserSearchSpec : SearchSpecificationBase<UserFilter, UserOrderBy>{}

    public class UserFilter : ConditionContainer
    {
        public readonly ConditionString UserName;

        public UserFilter(){
            UserName = new ConditionString(this, "UserName");
        }
    }

    public class UserOrderBy : SpecOrderByBase{}
}
