using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Frontend.Web.Code;

public class QuestionsModel : BaseModel
{
    public IEnumerable<QuestionRowModel> QuestionRows { get; set; }

    public PagerModel Pager { get; set; }

    public string SearchTerm { get; set; }
    public string SearchUrl { get; set; }
    public string SearchTab;
    public IList<Category> FilteredCategories = new List<Category>();

    public int TotalWishKnowledge;
    public int TotalQuestionsInResult;
    public int TotalQuestionsInSystem;
    public int TotalQuestionsMine;

    public string OrderByLabel;
    public QuestionOrderBy OrderBy;

    public bool ActiveTabAll;
    public bool ActiveTabMine;
    public bool ActiveTabWish;

    public string Action;

    public string Suggestion;
    public IEnumerable<string> Suggestions = new List<string>();

    public bool NotAllowed;

    public QuestionsSearchResultModel SearchResultModel;

    public QuestionsModel(){
        QuestionRows = Enumerable.Empty<QuestionRowModel>();
    }

    public QuestionsModel(
        IList<Question> questions, 
        QuestionSearchSpec questionSearchSpec,
        SearchTabType searchTab
    )
    {
        ActiveTabAll = searchTab == SearchTabType.All;
        ActiveTabMine = searchTab == SearchTabType.Mine;
        ActiveTabWish = searchTab == SearchTabType.Wish;

        if (questionSearchSpec.Filter.Categories != null)
            FilteredCategories = R<CategoryRepository>().GetByIds(questionSearchSpec.Filter.Categories.ToArray());

        int currentUserId = _sessionUser.IsLoggedIn ? _sessionUser.User.Id : -1;
        NotAllowed = !_sessionUser.IsLoggedIn && (searchTab == SearchTabType.Wish || searchTab == SearchTabType.Mine);

        var totalsForCurrentUser = Resolve<TotalsPersUserLoader>().Run(currentUserId, questions);
        var questionValutionsForCurrentUser = Resolve<QuestionValuationRepo>().GetBy(questions.GetIds(), currentUserId);

        Pager = new PagerModel(questionSearchSpec);
        Suggestion = questionSearchSpec.GetSuggestion();

        int counter = 0; 
        QuestionRows = from question in questions
                       select new QuestionRowModel(
                                    question,
                                    totalsForCurrentUser.ByQuestionId(question.Id),
                                    NotNull.Run(questionValutionsForCurrentUser.ByQuestionId(question.Id)),
                                    ((questionSearchSpec.CurrentPage - 1) * questionSearchSpec.PageSize) + ++counter, 
                                    currentUserId,
                                    searchTab
                                  );

        TotalQuestionsInSystem = Resolve<GetQuestionCount>().Run();
        TotalQuestionsMine = Resolve<GetQuestionCount>().Run(currentUserId);
        TotalWishKnowledge = Resolve<GetWishQuestionCountCached>().Run(currentUserId);

        TotalQuestionsInResult = questionSearchSpec.TotalItems;

        OrderBy = questionSearchSpec.OrderBy;
        OrderByLabel = questionSearchSpec.OrderBy.ToText();
        SearchTerm = questionSearchSpec.Filter.SearchTerm;

        if (searchTab == SearchTabType.All){
            Pager.Action = Action = Links.Questions;
            SearchUrl = "/Fragen/Suche";
        }else if (searchTab == SearchTabType.Wish){
            Pager.Action = Action = Links.QuestionsWishAction;
            SearchUrl = "/Fragen/Wunschwissen/Suche";
        }else if (searchTab == SearchTabType.Mine){
            Pager.Action = Action = Links.QuestionsMineAction;
            SearchUrl = "/Fragen/Meine/Suche";
        }

        MenuLeftModel.Categories = questions.GetAllCategories()
                                    .GroupBy(c => c.Name)
                                    .OrderByDescending(g => g.Count())
                                    .Select(g =>  new MenuModelCategoryItem{
                                        SearchUrl = SearchUrl,
                                        Category = g.First(), OnPageCount = g.Count()
                                    })
                                    .ToList();

        SearchResultModel = new QuestionsSearchResultModel(this);
    }
}