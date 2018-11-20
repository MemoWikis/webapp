using FluentNHibernate.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Frontend.Web.Code;


public class KnowledgeQuestions : BaseModel
{
    public List<QuestionValuation> TotalWishKnowledgeValuationsWithAuthor;
    public List<QuestionValuation> TotalWishKnowledgeValuationsPerPage;
    public List<Questions> TotalWishKnowledgeQuestions;
    public int CountWishQuestions;
    public int LastPage;

   

    public KnowledgeQuestions(bool isAuthor, int page, int per_page)
    {
        TotalWishKnowledgeValuationsWithAuthor = isAuthor
            ? UserValuationCache.GetQuestionValuations(UserId)
                .Where(v => v.Question.Creator.Id == UserId)
                .Distinct()
                .ToList()
            : UserValuationCache.GetQuestionValuations(UserId)
                .Distinct()
                .ToList();

        TotalWishKnowledgeValuationsPerPage = KnowledgeController.GetSiteForPagination(TotalWishKnowledgeValuationsWithAuthor, page, per_page).ToList();
        TotalWishKnowledgeQuestions = GetQuestionsWishFromDatabase();
        CountWishQuestions = TotalWishKnowledgeValuationsWithAuthor.Count;
        LastPage = KnowledgeController.getLastPage(CountWishQuestions, per_page);
    }

    private ImageFrontendData GetCategoryImage(int CategoryId)
    {
        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(CategoryId, ImageType.Category);
        return new ImageFrontendData(imageMetaData);
    }

  

    private List<Questions> ObjectFactory(
        IList<Question> questionsListForFactory,
        List<Questions> questionsList,
        string whichList
        )
    {
        var userImageSettings = new UserImageSettings();

        foreach (var question in questionsListForFactory)
        {
            var questions = new Questions();
            var categories = question.Categories;

            questions.Title = question.Text;
            
            questions.AuthorName = question.Creator.Name;
            questions.AuthorImageUrl = userImageSettings.GetUrl_30px_square(Sl.UserRepo.GetById(question.Creator.Id));
            questions.LinkToQuestion = Links.GetUrl(question);
            questions.AuthorId = question.Creator.Id;
            questions.LinkToCategory = categories.IsEmpty() ?  " " : Links.GetUrl(categories[0]);
            questions.Category = categories.IsEmpty() ? "keine Kategorie" : categories[0].Name;
            questions.ImageFrontendData = categories.IsEmpty() ? Sl.ImageMetaDataRepo.GetBy(682, ImageType.Category) : Sl.ImageMetaDataRepo.GetBy(categories[0].Id, ImageType.Category); //  GetCategoryImage(categories[0].Id);
            questions.TooltipLinkToCategory = "Kategorie " + questions.Category + " in neuem Tab öffnen";

            if (whichList.Equals("solid"))
            {
                questions.LearningStatus = "greenD";
                questions.LearningStatusNumber = 1;
                questions.LearningStatusTooltip = "Sicher gewusst";
            }

            if (whichList.Equals("questionsListShouldConsolidate"))
            {
                questions.LearningStatus = "yellow";
                questions.LearningStatusNumber = 2;
                questions.LearningStatusTooltip = "Zu festigen";
            }

            if (whichList.Equals("questionsListShouldLearning"))
            {
                questions.LearningStatus = "red";
                questions.LearningStatusNumber = 3;
                questions.LearningStatusTooltip = "Zu lernen";

            }

            if (whichList.Equals("questionsListNotLearned"))
            {
                questions.LearningStatus = "grey";
                questions.LearningStatusNumber = 4;
                questions.LearningStatusTooltip = "Nicht gelernt";
            }

            questionsList.Add(questions);
        }

        return questionsList;
    }

    private List<Questions> QuestionsFactory(
        IList<Question> questionsListSolid,
        IList<Question> questionsListShouldConsolidate,
        IList<Question> questionsListShouldLearning,
        IList<Question> questionsListNotLearned
        )
    {
        var questionsList = new List<Questions>();


        if (questionsListSolid.Count != 0)
            questionsList = ObjectFactory(questionsListSolid, questionsList, "solid");

        if (questionsListShouldConsolidate.Count != 0)
            questionsList = ObjectFactory(questionsListShouldConsolidate, questionsList, "questionsListShouldConsolidate");


        if (questionsListShouldConsolidate.Count != 0)
            questionsList = ObjectFactory(questionsListShouldLearning, questionsList, "questionsListShouldLearning");


        if (questionsListShouldConsolidate.Count != 0)
            questionsList = ObjectFactory(questionsListNotLearned, questionsList, "questionsListNotLearned");

        return questionsList;
    }

    public List<Questions> GetSortList(List<Questions> unSortList, string sortCondition)
    {
        switch (sortCondition)
        {
            case "knowWas|asc":
                unSortList.Sort((x, y) => y.LearningStatusNumber.CompareTo(x.LearningStatusNumber));
                break;
            case "knowWas|desc":
                unSortList.Sort((x, y) => -1 * y.LearningStatusNumber.CompareTo(x.LearningStatusNumber));
                break;
            case "author|asc":
                unSortList.Sort((x, y) => String.CompareOrdinal(x.AuthorName, y.AuthorName));
                break;
            case "author|desc":
                unSortList.Sort((x, y) => String.CompareOrdinal(y.AuthorName, x.AuthorName));
                break;
            case "category|asc":
                unSortList.Sort((x, y) => String.CompareOrdinal(x.Category, y.Category));
                break;
            case "category|desc":
                unSortList.Sort((x, y) => String.CompareOrdinal(y.Category, x.Category));
                break;
        }

        var sortList = unSortList;
        return sortList;
    }

    public List<Questions> GetQuestionsWishFromDatabase()
    {
        var unsortedListQuestions = EntityCache.GetQuestionsByIds(TotalWishKnowledgeValuationsPerPage.QuestionIds());

        var solidIdsPerPageIds = TotalWishKnowledgeValuationsPerPage.Where(v => v.KnowledgeStatus == KnowledgeStatus.Solid).Select(v => v.Question.Id).ToList();
        var shouldConsolidateIdsPerPageIds = TotalWishKnowledgeValuationsPerPage.Where(v => v.KnowledgeStatus == KnowledgeStatus.NeedsConsolidation).Select(v => v.Question.Id).ToList();
        var shouldLearningIdsPerPageIds = TotalWishKnowledgeValuationsPerPage.Where(v => v.KnowledgeStatus == KnowledgeStatus.NeedsLearning).Select(v => v.Question.Id).ToList();
        var notLearnedIdsPerPageIds = TotalWishKnowledgeValuationsPerPage.Where(v => v.KnowledgeStatus == KnowledgeStatus.NotLearned).Select(v => v.Question.Id).ToList();

        var solidsPerPage = unsortedListQuestions.Where(u => solidIdsPerPageIds.Contains(u.Id)).ToList();
        var shouldConsolidatePerPage = unsortedListQuestions.Where(u => shouldConsolidateIdsPerPageIds.Contains(u.Id)).ToList();
        var shouldLearningIdsPerPage = unsortedListQuestions.Where(u => shouldLearningIdsPerPageIds.Contains(u.Id)).ToList();
        var notLearnedIdsPerPage = unsortedListQuestions.Where(u => notLearnedIdsPerPageIds.Contains(u.Id)).ToList();

        var unsortList = QuestionsFactory(solidsPerPage, shouldConsolidatePerPage, shouldLearningIdsPerPage, notLearnedIdsPerPage);

        return unsortList;
    }

    public class Questions
    {
        public string Title { get; set; }
        public string Category = " "; 
        public ImageMetaData ImageFrontendData { get; set; }
        public string LearningStatus { get; set; }
        public string AuthorName { get; set; }
        public ImageUrl AuthorImageUrl { get; set; }
        public int LearningStatusNumber { get; set; }
        public string LearningStatusTooltip { get; set; }
        public string LinkToCategory { get; set; }
        public string LinkToQuestion { get; set; }
        public int AuthorId { get; set; }
        public string TooltipLinkToCategory { get; set; }
    }
}
