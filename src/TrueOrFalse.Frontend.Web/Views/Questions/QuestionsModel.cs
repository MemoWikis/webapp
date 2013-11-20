using System.Linq;
using System.Collections.Generic;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Frontend.Web.Models;
using TrueOrFalse;

public class QuestionsModel : BaseModel
{
    public IEnumerable<QuestionRowModel> QuestionRows { get; set; }

    public PagerModel Pager { get; set; }

    public string SearchTerm { get; set; }
    public string SearchUrl { get; set; }

    public int TotalWishKnowledge;
    public int TotalQuestionsInResult;
    public int TotalQuestionsInSystem;
    public string OrderByLabel;
    public QuestionOrderBy OrderBy;

    public bool ActiveTabAll;
    public bool ActiveTabMine;
    public bool ActiveTabWish;

    public QuestionsModel(){
        QuestionRows = Enumerable.Empty<QuestionRowModel>();
    }

    public QuestionsModel(
        IList<Question> questions, 
        QuestionSearchSpec questionSearchSpec, 
        int currentUserId,
        bool isTabAllActive = false,
        bool isTabWishActice = false,
        bool isTabMineActive = false
    )
    {
        ActiveTabAll = isTabAllActive;
        ActiveTabMine = isTabMineActive;
        ActiveTabWish = isTabWishActice;

        var totalsForCurrentUser = Resolve<TotalsPersUserLoader>().Run(currentUserId, questions);
        var questionValutionsForCurrentUser = Resolve<QuestionValuationRepository>().GetBy(questions.GetIds(), _sessionUser.User.Id);

        Pager = new PagerModel(questionSearchSpec);

        int counter = 0; 
        QuestionRows = from question in questions
                       select new QuestionRowModel(
                                    question,
                                    totalsForCurrentUser.ByQuestionId(question.Id),
                                    NotNull.Run(questionValutionsForCurrentUser.ByQuestionId(question.Id)),
                                    ((questionSearchSpec.CurrentPage - 1) * questionSearchSpec.PageSize) + ++counter, 
                                    currentUserId
                                  );

        TotalQuestionsInSystem = Sl.Resolve<GetTotalQuestionCount>().Run();
        TotalWishKnowledge = Sl.Resolve<GetWishQuestionCountCached>().Run(_sessionUser.User.Id);

        TotalQuestionsInResult = questionSearchSpec.TotalItems;

        OrderBy = questionSearchSpec.OrderBy;
        OrderByLabel = questionSearchSpec.OrderBy.ToText();
        SearchTerm = questionSearchSpec.Filter.SearchTerm;

        MenuLeftModel.Categories = questions.GetAllCategories()
                                    .GroupBy(c => c.Name)
                                    .OrderByDescending(g => g.Count())
                                    .Select(g =>  new MenuModelCategoryItem{Category = g.First(), OnPageCount = g.Count()})
                                    .ToList();

        if (ActiveTabAll){
            Pager.Action = Links.Questions;
            SearchUrl = "/Fragen/Suche/";
        }else if (ActiveTabWish){
            Pager.Action = Links.QuestionsWishAction;
            SearchUrl = "/Fragen/Wunschwissen/Suche/";
        }else if (ActiveTabMine){
            Pager.Action = Links.QuestionsMineAction;
            SearchUrl = "/Fragen/Meine/Suche/";
        }
    }
}