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

    public KnowledgeQuestions(bool isAuthor, int page, int per_page, string sort)
    {
        TotalWishKnowledgeValuationsWithAuthor = isAuthor
            ? UserValuationCache.GetQuestionValuations(UserId)
                .Where(v => v.Question.Creator.Id == UserId)
                .Distinct()
                .ToList()
            : UserValuationCache.GetQuestionValuations(UserId)
                .Distinct()
                .ToList();

        TotalWishKnowledgeValuationsWithAuthor = GetSortList(TotalWishKnowledgeValuationsWithAuthor, sort);
        
        CountWishQuestions = TotalWishKnowledgeValuationsWithAuthor.Count;
        TotalWishKnowledgeValuationsPerPage = KnowledgeController.GetSiteForPagination(TotalWishKnowledgeValuationsWithAuthor, page, per_page).ToList();
        TotalWishKnowledgeQuestions = ObjectFactory();
        LastPage = KnowledgeController.getLastPage(CountWishQuestions, per_page);
    }

    private List<Questions> ObjectFactory()
    {
        var unsortedListQuestions = EntityCache.GetQuestionsByIds(TotalWishKnowledgeValuationsPerPage.QuestionIds());
        var questionsList = new List<Questions>();
        var userImageSettings = new UserImageSettings();
        var i = 0;
        foreach (var question in unsortedListQuestions)
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
            questions.CategoryImageData = categories.IsEmpty() ? null : new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(categories[0].Id, ImageType.Category)).GetImageUrl(30);
            questions.QuestionMetaData = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(question.Id, ImageType.Question)).GetImageUrl(30);
            questions.TooltipLinkToCategory = "Kategorie " + questions.Category + " in neuem Tab öffnen";
            questions.CountQuestions = CountWishQuestions;

            if (questions.QuestionMetaData.Url.Equals("/Images/no-question-128.png"))
                questions.QuestionMetaData = questions.CategoryImageData;

            if ((int)TotalWishKnowledgeValuationsWithAuthor[i].KnowledgeStatus == 4)
            {
                questions.LearningStatus = "greenD";
                questions.LearningStatusNumber = 1;
                questions.LearningStatusTooltip = TotalWishKnowledgeValuationsWithAuthor[i].KnowledgeStatus.GetText();
            }

            if ((int)TotalWishKnowledgeValuationsWithAuthor[i].KnowledgeStatus == 3)
            {
                questions.LearningStatus = "yellow";
                questions.LearningStatusNumber = 2;
                questions.LearningStatusTooltip = TotalWishKnowledgeValuationsWithAuthor[i].KnowledgeStatus.GetText();
            }

            if ((int)TotalWishKnowledgeValuationsWithAuthor[i].KnowledgeStatus == 2)
            {
                questions.LearningStatus = "red";
                questions.LearningStatusNumber = 3;
                questions.LearningStatusTooltip = TotalWishKnowledgeValuationsWithAuthor[i].KnowledgeStatus.GetText();

            }

            if ((int)TotalWishKnowledgeValuationsWithAuthor[i].KnowledgeStatus == 1)
            {
                questions.LearningStatus = "grey";
                questions.LearningStatusNumber = 4 ;
                questions.LearningStatusTooltip = TotalWishKnowledgeValuationsWithAuthor[i].KnowledgeStatus.GetText();
            }

            questionsList.Add(questions);

            i++;
        }

        return questionsList;
    }

    public List<QuestionValuation> GetSortList(List<QuestionValuation> unSortList, string sortCondition)
    {
        switch (sortCondition)
        {// Orderby muss returned werden da es keine Liste ändert sondern zurück gibt

            case "knowWas|asc":
               unSortList.OrderBy(v => (int)v.KnowledgeStatus);
                break;
            case "knowWas|desc":
                unSortList.OrderByDescending(v => (int)v.KnowledgeStatus);
                break;
            case "author|asc":
                unSortList.Sort((x, y) => String.CompareOrdinal(x.Question.Creator.Name, y.Question.Creator.Name));
                break;
            case "author|desc":
                unSortList.Sort((x, y) => String.CompareOrdinal(y.Question.Creator.Name, x.Question.Creator.Name));
                break;
            case "category|asc":
                unSortList.Sort((x, y) => String.CompareOrdinal(x.Question.Categories[0].Name, y.Question.Categories[0].Name));
                break;
            case "category|desc":
                unSortList.Sort((x, y) => String.CompareOrdinal(y.Question.Categories[0].Name, x.Question.Categories[0].Name));
                break;
        }

        var sortList = unSortList;
        return sortList;
    }

    public class Questions
    {
        public string Title { get; set; }
        public string Category = " "; 
        public ImageUrl CategoryImageData { get; set; }
        public string LearningStatus { get; set; }
        public string AuthorName { get; set; }
        public ImageUrl AuthorImageUrl { get; set; }
        public int LearningStatusNumber { get; set; }
        public string LearningStatusTooltip { get; set; }
        public string LinkToCategory { get; set; }
        public string LinkToQuestion { get; set; }
        public int AuthorId { get; set; }
        public string TooltipLinkToCategory { get; set; }
        public int CountQuestions { get; set; }
        public ImageUrl QuestionMetaData { get; set; }
    }
}
