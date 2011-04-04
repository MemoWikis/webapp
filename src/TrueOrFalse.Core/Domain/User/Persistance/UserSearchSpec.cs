using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seedworks.Lib.Persistance;

namespace TrueOrFalse.Core
{
    public class UserSearchSpec : SearchSpecificationBase<UserFilter, UserOrderBy>{}

    public class UserFilter : ConditionContainer
    {
        public readonly ConditionString UserName;

        public UserFilter(){
            UserName = new ConditionString(this, "Name");
        }
    }

    public class UserOrderBy : SpecOrderByBase{}
}
