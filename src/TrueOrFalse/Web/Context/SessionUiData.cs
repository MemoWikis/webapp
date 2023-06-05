using System.Collections.Generic;
using Seedworks.Web.State;

public class SessionUiData : SessionBase, IRegisterAsInstancePerLifetime
{
    public MainMenu MainMenu => SessionDataLegacy.Get("menu", new MainMenu());
    public UserMenu UserMenu => SessionDataLegacy.Get("UserMenu", new UserMenu());

    /* History **************/
    public QuestionHistory VisitedQuestions{
        get { return SessionDataLegacy.Get("lastVisitedQuestions", new QuestionHistory()); }
        set { SessionDataLegacy.Set("lastVisitedQuestions", value); }
    }

    public QuestionSetHistory VisitedSets => SessionDataLegacy.Get("lastVisitedQuestionSets", new QuestionSetHistory());
    public UserHistory VisitedUserDetails => SessionDataLegacy.Get("lastVisitedUsers", new UserHistory());
    public CategoryHistory VisitedCategories => SessionDataLegacy.Get("lastVisitedCategories", new CategoryHistory());
    public HelpHistory VisitedHelpPages => SessionDataLegacy.Get("lastVisitedHelpPages", new HelpHistory());

    /* SearchSpecs *************/
    public QuestionSearchSpec SearchSpecQuestionAll => SessionDataLegacy.Get("searchSpecQuestionAll", new QuestionSearchSpec { PageSize = 10 });
    public QuestionSearchSpec SearchSpecQuestionMine => SessionDataLegacy.Get("searchSpecQuestionMine", new QuestionSearchSpec(ignorePrivates:false) { PageSize = 10 });
    public QuestionSearchSpec SearchSpecQuestionWish => SessionDataLegacy.Get("searchSpecQuestionWish", new QuestionSearchSpec(ignorePrivates: false) { PageSize = 10 });
    public QuestionSearchSpec SearchSpecQuestionSearchBox => SessionDataLegacy.Get("searchSpecQuestionSearchBox", new QuestionSearchSpec{Key = "searchbox"});

    public List<QuestionSearchSpec> SearchSpecQuestions => SessionDataLegacy.Get("searchSpecQuestions", new List<QuestionSearchSpec>());

    public SetSearchSpec SearchSpecSetsAll => SessionDataLegacy.Get("searchSpecSetAll", new SetSearchSpec { PageSize = 10 });
    public SetSearchSpec SearchSpecSetMine => SessionDataLegacy.Get("searchSpecSetMine", new SetSearchSpec { PageSize = 10 });
    public SetSearchSpec SearchSpecSetWish => SessionDataLegacy.Get("searchSpecSetWish", new SetSearchSpec { PageSize = 10 });

    public CategorySearchSpec SearchSpecCategory => SessionDataLegacy.Get("searchSpecCategory", new CategorySearchSpec { PageSize = 10 });
    public CategorySearchSpec SearchSpecCategoryWish => SessionDataLegacy.Get("searchSpecCategoryWish", new CategorySearchSpec { PageSize = 10 });

    public UserSearchSpec SearchSpecUser => SessionDataLegacy.Get("searchSpecUser", new UserSearchSpec { PageSize = 10 });
    public ImageMetaDataSearchSpec ImageMetaDataSearchSpec => SessionDataLegacy.Get("searchSpecImageMetaData", new ImageMetaDataSearchSpec { PageSize = 50 });

    public TmpImageStore TmpImagesStore => SessionDataLegacy.Get("tmpImageStore", new TmpImageStore());
}