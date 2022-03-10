using System.Collections.Generic;
using Seedworks.Web.State;

public class SessionUiData : SessionBase, IRegisterAsInstancePerLifetime
{
    public MainMenu MainMenu => SessionData.Get("menu", new MainMenu());
    public UserMenu UserMenu => SessionData.Get("UserMenu", new UserMenu());

    /* History **************/
    public QuestionHistory VisitedQuestions{
        get { return SessionData.Get("lastVisitedQuestions", new QuestionHistory()); }
        set { SessionData.Set("lastVisitedQuestions", value); }
    }

    public QuestionSetHistory VisitedSets => SessionData.Get("lastVisitedQuestionSets", new QuestionSetHistory());
    public UserHistory VisitedUserDetails => SessionData.Get("lastVisitedUsers", new UserHistory());
    public CategoryHistory VisitedCategories => SessionData.Get("lastVisitedCategories", new CategoryHistory());
    public HelpHistory VisitedHelpPages => SessionData.Get("lastVisitedHelpPages", new HelpHistory());

    /* SearchSpecs *************/
    public QuestionSearchSpec SearchSpecQuestionAll => SessionData.Get("searchSpecQuestionAll", new QuestionSearchSpec { PageSize = 10 });
    public QuestionSearchSpec SearchSpecQuestionMine => SessionData.Get("searchSpecQuestionMine", new QuestionSearchSpec(ignorePrivates:false) { PageSize = 10 });
    public QuestionSearchSpec SearchSpecQuestionWish => SessionData.Get("searchSpecQuestionWish", new QuestionSearchSpec(ignorePrivates: false) { PageSize = 10 });
    public QuestionSearchSpec SearchSpecQuestionSearchBox => SessionData.Get("searchSpecQuestionSearchBox", new QuestionSearchSpec{Key = "searchbox"});

    public List<QuestionSearchSpec> SearchSpecQuestions => SessionData.Get("searchSpecQuestions", new List<QuestionSearchSpec>());

    public SetSearchSpec SearchSpecSetsAll => SessionData.Get("searchSpecSetAll", new SetSearchSpec { PageSize = 10 });
    public SetSearchSpec SearchSpecSetMine => SessionData.Get("searchSpecSetMine", new SetSearchSpec { PageSize = 10 });
    public SetSearchSpec SearchSpecSetWish => SessionData.Get("searchSpecSetWish", new SetSearchSpec { PageSize = 10 });

    public CategorySearchSpec SearchSpecCategory => SessionData.Get("searchSpecCategory", new CategorySearchSpec { PageSize = 10 });
    public CategorySearchSpec SearchSpecCategoryWish => SessionData.Get("searchSpecCategoryWish", new CategorySearchSpec { PageSize = 10 });

    public UserSearchSpec SearchSpecUser => SessionData.Get("searchSpecUser", new UserSearchSpec { PageSize = 10 });
    public ImageMetaDataSearchSpec ImageMetaDataSearchSpec => SessionData.Get("searchSpecImageMetaData", new ImageMetaDataSearchSpec { PageSize = 50 });

    public TmpImageStore TmpImagesStore => SessionData.Get("tmpImageStore", new TmpImageStore());
}