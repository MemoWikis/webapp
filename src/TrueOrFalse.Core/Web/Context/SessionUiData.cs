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

        public QuestionSearchSpec QuestionSearchSpec
        {
            get { return Data.Get("questionSearchSpec", new QuestionSearchSpec{PageSize = 5}); }
        }

        public bool QuestionsFilterByMe { 
            get { return Data.Get("questionsFilterByMe", false); }
            set { Data["questionsFilterByMe"] = value; }
        }

        public bool QuestionsFilterByAll
        {
            get { return Data.Get("questionsFilterByAll", false); }
            set { Data["questionsFilterByAll"] = value; }
        }
    }
}