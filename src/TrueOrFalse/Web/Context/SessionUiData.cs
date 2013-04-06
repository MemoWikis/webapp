using System.Collections.Generic;
using Seedworks.Web.State;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse.Web.Context
{
    public class SessionUiData : SessionBase, IRegisterAsInstancePerLifetime
    {
        public Menu Menu { get { return Data.Get("menu", new Menu()); } }

        public QuestionHistory VisitedQuestions{
            get { return Data.Get("lastVisitedQuestions", new QuestionHistory()); }
        }

        public QuestionSetHistory VisitedQuestionSets{
            get { return Data.Get("lastVisitedQuestionSets", new QuestionSetHistory()); }
        }

        public UserHistory VisitedProfiles{
            get { return Data.Get("lastVisitedProfiles", new UserHistory()); }
        }

        public CategoryHistory VisitedCategories{
            get { return Data.Get("lastVisitedCategories", new CategoryHistory()); }
        }

        public QuestionSearchSpec QuestionSearchSpec{
            get { return Data.Get("questionSearchSpec", new QuestionSearchSpec{PageSize = 5}); }
        }

        public QuestionSetSearchSpec SetSearchSpec{
            get { return Data.Get("questionSetSearchSpec", new QuestionSetSearchSpec { PageSize = 10 }); }
        }

        public TmpImageStore TmpImagesStore { get { return Data.Get("tmpImageStore", new TmpImageStore()); } }
    }
}