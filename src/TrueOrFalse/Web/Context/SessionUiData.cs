using System.Collections.Generic;
using Seedworks.Web.State;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse.Web.Context
{
    public class SessionUiData : SessionBase, IRegisterAsInstancePerLifetime
    {
        public Menu Menu { get { return Data.Get("menu", new Menu()); } }

        /* History **************/
        public QuestionHistory VisitedQuestions{
            get { return Data.Get("lastVisitedQuestions", new QuestionHistory()); }
            set { Data["lastVisitedQuestions"] = value; }
        }

        public QuestionSetHistory VisitedSets{
            get { return Data.Get("lastVisitedQuestionSets", new QuestionSetHistory()); }
        }

        public UserHistory VisitedUserDetails{
            get { return Data.Get("lastVisitedUsers", new UserHistory()); }
        }

        public CategoryHistory VisitedCategories{
            get { return Data.Get("lastVisitedCategories", new CategoryHistory()); }
        }

        public HelpHistory VisitedHelpPages{
            get { return Data.Get("lastVisitedHelpPages", new HelpHistory()); }
        }

        /* SearchSpecs *************/
        public QuestionSearchSpec SearchSpecQuestionAll{ get { return Data.Get("searchSpecQuestionAll", new QuestionSearchSpec { PageSize = 10 }); } }

        public QuestionSearchSpec SearchSpecQuestionMine
        {
            get { return Data.Get("searchSpecQuestionMine", new QuestionSearchSpec(ignorePrivates:false) { PageSize = 10 }); }
        }

        public QuestionSearchSpec SearchSpecQuestionWish
        {
            get { return Data.Get("searchSpecQuestionWish", new QuestionSearchSpec(ignorePrivates: false) { PageSize = 10 }); }
        }

        public List<QuestionSearchSpec> SearchSpecQuestions { get { return Data.Get("searchSpecQuestions", new List<QuestionSearchSpec>()); } } 

        public SetSearchSpec SearchSpecSetsAll{ get { return Data.Get("searchSpecSetAll", new SetSearchSpec { PageSize = 10 }); } }
        public SetSearchSpec SearchSpecSetMine { get { return Data.Get("searchSpecSetMine", new SetSearchSpec { PageSize = 10 }); } }
        public SetSearchSpec SearchSpecSetWish { get { return Data.Get("searchSpecSetWish", new SetSearchSpec { PageSize = 10 }); } }

        public CategorySearchSpec SearchSpecCategory{
            get { return Data.Get("searchSpecCategory", new CategorySearchSpec { PageSize = 10 }); }
        }

        public UserSearchSpec SearchSpecUser{
            get { return Data.Get("searchSpecUser", new UserSearchSpec { PageSize = 10 }); }
        }

        public TmpImageStore TmpImagesStore { get { return Data.Get("tmpImageStore", new TmpImageStore()); } }
    }
}