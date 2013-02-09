using System.Collections.Generic;
using Seedworks.Web.State;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse.Web.Context
{
    public class SessionUiData : SessionBase, IRegisterAsInstancePerLifetime
    {
        public Menu Menu { get { return Data.Get("menu", new Menu()); } }

        public UserNavigationModelList LastVisitedProfiles{
            get { return Data.Get("lastVisitedProfiles", new UserNavigationModelList()); }
        } 

        public QuestionSearchSpec QuestionSearchSpec{
            get { return Data.Get("questionSearchSpec", new QuestionSearchSpec{PageSize = 5}); }
        }

        public QuestionSetSearchSpec QuestionSetSearchSpec{
            get { return Data.Get("questionSetSearchSpec", new QuestionSetSearchSpec { PageSize = 10 }); }
        }
    }
}