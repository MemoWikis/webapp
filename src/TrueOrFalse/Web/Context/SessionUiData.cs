using System.Collections.Generic;
using Seedworks.Web.State;
using TrueOrFalse.Search;

public class SessionUiData : SessionBase, IRegisterAsInstancePerLifetime
{
    public MainMenu MainMenu => Data.Get("menu", new MainMenu());
    public TopicMenu TopicMenu => Data.Get("ThemeMenu", new TopicMenu());
    public UserMenu UserMenu => Data.Get("UserMenu", new UserMenu());

    /* History **************/
    public QuestionHistory VisitedQuestions{
        get { return Data.Get("lastVisitedQuestions", new QuestionHistory()); }
        set { Data["lastVisitedQuestions"] = value; }
    }

    public QuestionSetHistory VisitedSets => Data.Get("lastVisitedQuestionSets", new QuestionSetHistory());
    public UserHistory VisitedUserDetails => Data.Get("lastVisitedUsers", new UserHistory());
    public CategoryHistory VisitedCategories => Data.Get("lastVisitedCategories", new CategoryHistory());
    public HelpHistory VisitedHelpPages => Data.Get("lastVisitedHelpPages", new HelpHistory());

    /* SearchSpecs *************/
    public QuestionSearchSpec SearchSpecQuestionAll => Data.Get("searchSpecQuestionAll", new QuestionSearchSpec { PageSize = 10 });
    public QuestionSearchSpec SearchSpecQuestionMine => Data.Get("searchSpecQuestionMine", new QuestionSearchSpec(ignorePrivates:false) { PageSize = 10 });
    public QuestionSearchSpec SearchSpecQuestionWish => Data.Get("searchSpecQuestionWish", new QuestionSearchSpec(ignorePrivates: false) { PageSize = 10 });
    public QuestionSearchSpec SearchSpecQuestionSearchBox => Data.Get("searchSpecQuestionSearchBox", new QuestionSearchSpec{Key = "searchbox"});

    public List<QuestionSearchSpec> SearchSpecQuestions => Data.Get("searchSpecQuestions", new List<QuestionSearchSpec>());

    public SetSearchSpec SearchSpecSetsAll => Data.Get("searchSpecSetAll", new SetSearchSpec { PageSize = 10 });
    public SetSearchSpec SearchSpecSetMine => Data.Get("searchSpecSetMine", new SetSearchSpec { PageSize = 10 });
    public SetSearchSpec SearchSpecSetWish => Data.Get("searchSpecSetWish", new SetSearchSpec { PageSize = 10 });

    public CategorySearchSpec SearchSpecCategory => Data.Get("searchSpecCategory", new CategorySearchSpec { PageSize = 10 });
    public CategorySearchSpec SearchSpecCategoryWish => Data.Get("searchSpecCategoryWish", new CategorySearchSpec { PageSize = 10 });

    public UserSearchSpec SearchSpecUser => Data.Get("searchSpecUser", new UserSearchSpec { PageSize = 10 });
    public ImageMetaDataSearchSpec ImageMetaDataSearchSpec => Data.Get("searchSpecImageMetaData", new ImageMetaDataSearchSpec { PageSize = 50 });

    public TmpImageStore TmpImagesStore => Data.Get("tmpImageStore", new TmpImageStore());
}