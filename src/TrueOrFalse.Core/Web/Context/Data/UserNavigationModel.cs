using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse.Core.Web.Uris;

namespace TrueOrFalse.Core
{
    public class UserNavigationModel
    {
        public UserNavigationModel(User user)
        {
            Id = user.Id;
            Name = user.Name;
            UrlName = UriSegmentFriendlyUser.Run(user.Name);
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string UrlName { get; private set; }
    }
}