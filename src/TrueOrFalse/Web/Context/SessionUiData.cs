using System.Collections.Generic;
using Seedworks.Web.State;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse.Web.Context
{
    public class SessionUiData : SessionBase, IRegisterAsInstancePerLifetime
    {
        public Menu Menu { get { return Data.Get("menu", new Menu()); } }

        public QuestionHistory LastQuestions{
            get { return Data.Get("lastVisitedQuestions", new QuestionHistory()); }
        }

        public QuestionSetHistory LastQuestionSets{
            get { return Data.Get("lastVisitedQuestionSets", new QuestionSetHistory()); }
        }

        public UserHistory LastVisitedProfiles{
            get { return Data.Get("lastVisitedProfiles", new UserHistory()); }
        }

        public QuestionSearchSpec QuestionSearchSpec{
            get { return Data.Get("questionSearchSpec", new QuestionSearchSpec{PageSize = 5}); }
        }

        public QuestionSetSearchSpec SetSearchSpec{
            get { return Data.Get("questionSetSearchSpec", new QuestionSetSearchSpec { PageSize = 10 }); }
        }
    }
}