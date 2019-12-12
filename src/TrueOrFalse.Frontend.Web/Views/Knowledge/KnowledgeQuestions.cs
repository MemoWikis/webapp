using FluentNHibernate.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls.Expressions;
using Antlr.Runtime.Misc;
using Seedworks.Lib.Persistence;
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
            ? UserCache.CreateItemFromDatabase(UserId).QuestionValuations.Values
                .Where(v => v.Question.Creator != null && v.Question.Creator.Id == UserId && v.IsInWishKnowledge())
                .Distinct()
                .ToList()
            : UserCache.CreateItemFromDatabase(UserId).QuestionValuations.Values
                .Where(v => v.IsInWishKnowledge())
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
        
        var i = 0;
        foreach (var question in unsortedListQuestions)
        {
            var userTinyModel = new UserTinyModel(question.Creator);
            var userImageSettings = new UserImageSettings(userTinyModel.Id);
         
            var questions = new Questions();
            var categories = question.Categories;

            questions.Title = question.Text;

            questions.AuthorName = userTinyModel.Name;
            questions.AuthorImageUrl =  userImageSettings.GetUrl_128px_square(userTinyModel);
            questions.LinkToQuestion = Links.GetUrl(question);
            questions.AuthorId = userTinyModel.Id;
            questions.LinkToCategory = categories.IsEmpty() ? " " : Links.GetUrl(categories[0]);
            questions.Category = categories.IsEmpty() ? "keine Kategorie" : categories[0].Name;
            questions.CategoryImageData = categories.IsEmpty() ? null : new ImageFrontendData(question.Categories[0].Id, ImageType.Category).GetImageUrl(128); ;
            questions.QuestionMetaData = new ImageFrontendData(Sl.ImageMetaDataRepo.GetBy(question.Id, ImageType.Question)).GetImageUrl(30);
            questions.TooltipLinkToCategory = "Kategorie " + questions.Category + " in neuem Tab öffnen";
            questions.CountQuestions = CountWishQuestions;

            if ((int)TotalWishKnowledgeValuationsPerPage[i].KnowledgeStatus == 4)
            {
                questions.LearningStatus = "greenD";
                questions.LearningStatusNumber = 1;
                questions.LearningStatusTooltip = TotalWishKnowledgeValuationsWithAuthor[i].KnowledgeStatus.GetText();
            }

            if ((int)TotalWishKnowledgeValuationsPerPage[i].KnowledgeStatus == 3)
            {
                questions.LearningStatus = "yellow";
                questions.LearningStatusNumber = 2;
                questions.LearningStatusTooltip = TotalWishKnowledgeValuationsWithAuthor[i].KnowledgeStatus.GetText();
            }

            if ((int)TotalWishKnowledgeValuationsPerPage[i].KnowledgeStatus == 2)
            {
                questions.LearningStatus = "red";
                questions.LearningStatusNumber = 3;
                questions.LearningStatusTooltip = TotalWishKnowledgeValuationsWithAuthor[i].KnowledgeStatus.GetText();

            }

            if ((int)TotalWishKnowledgeValuationsPerPage[i].KnowledgeStatus == 1)
            {
                questions.LearningStatus = "grey";
                questions.LearningStatusNumber = 4;
                questions.LearningStatusTooltip = TotalWishKnowledgeValuationsWithAuthor[i].KnowledgeStatus.GetText();
            }

            questionsList.Add(questions);
            i++;
        }

        return questionsList;
    }

    public List<QuestionValuation> GetSortList(List<QuestionValuation> unSortList, string sortCondition)
    {
        var sortList = new List<QuestionValuation>();
        if (sortCondition.Equals("knowWas|asc,author|asc,category|asc"))
        {
            sortList = unSortList.OrderBy(v => v.Question.Text ?? "").ToList();
        }
        else
        {
            switch (sortCondition)
            {

                case "knowWas|asc":
                    sortList = unSortList.OrderBy(v => (int)v.KnowledgeStatus).ToList();
                    break;
                case "knowWas|desc":
                    sortList = unSortList.OrderByDescending(v => (int)v.KnowledgeStatus).ToList();
                    break;
                case "author|asc":
                    sortList = unSortList.OrderBy(v => v.Question.Creator != null ? v.Question.Creator.Id : -1  ).ToList();
                    break;
                case "author|desc":
                    sortList = unSortList.OrderByDescending(v => v.Question.Creator != null ? v.Question.Creator.Id : -1).ToList();
                    break;
                case "category|asc":
                    sortList = unSortList
                        .OrderBy(qv => qv.Question.Categories.IsEmpty() ? "" : qv.Question.Categories[0].Name).ToList();
                    break;
                case "category|desc":
                    sortList = unSortList
                        .OrderByDescending(qv => qv.Question.Categories.IsEmpty() ? "" : qv.Question.Categories[0].Name).ToList();
                    break;
            }
        }
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
