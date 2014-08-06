using System.Linq;
using System.Collections.Generic;
using FluentNHibernate.Conventions;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse;

public class QuestionsModel : BaseModel
{
    private readonly SearchTab _searchTab;
    public IEnumerable<QuestionRowModel> QuestionRows { get; set; }

    public PagerModel Pager { get; set; }

    public string SearchTerm { get; set; }
    public string SearchUrl { get; set; }
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
        SearchTab searchTab
    )
    {
        ActiveTabAll = searchTab == SearchTab.All;
        ActiveTabMine = searchTab == SearchTab.Mine;
        ActiveTabWish = searchTab == SearchTab.Wish;

        int currentUserId = _sessionUser.IsLoggedIn ? _sessionUser.User.Id : -1;
        NotAllowed = !_sessionUser.IsLoggedIn && (searchTab == SearchTab.Wish || searchTab == SearchTab.Mine);

        var totalsForCurrentUser = Resolve<TotalsPersUserLoader>().Run(currentUserId, questions);
        var questionValutionsForCurrentUser = Resolve<QuestionValuationRepository>().GetBy(questions.GetIds(), currentUserId);

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

        TotalQuestionsInSystem = Resolve<GetTotalQuestionCount>().Run();
        TotalQuestionsMine = Resolve<GetTotalQuestionCount>().Run(currentUserId);
        TotalWishKnowledge = Resolve<GetWishQuestionCountCached>().Run(currentUserId);

        TotalQuestionsInResult = questionSearchSpec.TotalItems;

        OrderBy = questionSearchSpec.OrderBy;
        OrderByLabel = questionSearchSpec.OrderBy.ToText();
        SearchTerm = questionSearchSpec.Filter.SearchTerm;

        if (searchTab == SearchTab.All){
            Pager.Action = Action = Links.Questions;
            SearchUrl = "/Fragen/Suche";
        }else if (searchTab == SearchTab.Wish){
            Pager.Action = Action = Links.QuestionsWishAction;
            SearchUrl = "/Fragen/Wunschwissen/Suche";
        }else if (searchTab == SearchTab.Mine){
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