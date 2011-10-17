using System.Collections.Generic;
using Seedworks.Web.State;
using TrueOrFalse.Core.Infrastructure;

namespace TrueOrFalse.Core.Web.Context
{
    public class SessionUiData : SessionBase, IRegisterAsInstancePerLifetime
    {
        public UserNavigationModelList LastVisitedProfiles
        {
            get { return Data.Get("lastVisitedProfiles", new UserNavigationModelList()); }
        } 
    }
}