using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse.Search
{
    public class ToUserSolrMap
    {
        public static UserSolrMap Run(User user)
        {
            var result = new UserSolrMap();
            result.Id = user.Id;
            result.Name = user.Name;
            result.DateCreated = user.DateCreated;

            return result;
        }
    }
}
