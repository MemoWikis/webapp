﻿using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core
{
    public class UserSearchSpec : SearchSpecificationBase<UserFilter, UserOrderBy>{}

    public class UserFilter : ConditionContainer
    {
        public readonly ConditionString EmailAddress;

        public UserFilter(){
            EmailAddress = new ConditionString(this, "EmailAddress");
        }
    }

    public class UserOrderBy : SpecOrderByBase{}
}
