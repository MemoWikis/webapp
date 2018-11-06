using System;
using System.Collections.Generic;
using FluentNHibernate.Conventions;
using TrueOrFalse.Frontend.Web.Code;


public class KnowledgeQuestionsModel 
{
    private UserImageSettings userImageSettings = new UserImageSettings();

    private IList<Question> getIdsPerPage(IList<int> unsortedListOneSite, IList<int> allLearningStatusIds)
    {
        IList<int> IdsPerPage = new List<int>();
        foreach (var learningStatusId in allLearningStatusIds)
        {
            if( unsortedListOneSite.Contains(learningStatusId))
                IdsPerPage.Add(learningStatusId); 
        }

        int[] idsPerPageArray = new int[IdsPerPage.Count];
        IdsPerPage.CopyTo(idsPerPageArray, 0);
        return Sl.QuestionRepo.GetByIds(idsPerPageArray);
    }

    private List<Questions> IsAuthor(List<Questions> unsortList, bool isAuthor, int userId)
    {
        var sortList = new List<Questions>();

        if (isAuthor)
        {
            foreach (var unsort in unsortList)
            {
                if (unsort.AuthorId == userId)
                {
                    sortList.Add(unsort);
                }
            }

            return sortList;
        }
        return unsortList;
    }

    private List<Questions> ObjectFactory(
        IList<Question> questionsListForFactory,
        List<Questions> questionsList,
        string whichList
        )
    {
        var categoryAndSetDataWishKnowledge = new CategoryAndSetDataWishKnowledge();
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
            questions.ImageFrontendData = categories.IsEmpty() ? categoryAndSetDataWishKnowledge.GetCategoryImage(682) : categoryAndSetDataWishKnowledge.GetCategoryImage(categories[0].Id);
       

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

    public List<Questions> GetQuestionsWishFromDatabase(int userId, bool isAuthor, IList<int> unsortedListOneSite)
    {
        var solidIdsAll = Sl.QuestionRepo.GetByKnowledge(userId, true, false, false, false);
        var shouldConsolidateIdsAll = Sl.QuestionRepo.GetByKnowledge(userId, false, true, false, false);
        var shouldLearningIdsAll = Sl.QuestionRepo.GetByKnowledge(userId, false, false, true, false);
        var notLearnedIdsAll = Sl.QuestionRepo.GetByKnowledge(userId, false, false, false, true);

        var solidIdsPerPage = getIdsPerPage(unsortedListOneSite, solidIdsAll);
        var shouldConsolidateIdsPerPage = getIdsPerPage(unsortedListOneSite, shouldConsolidateIdsAll);
        var shouldLearningeIdsPerPage = getIdsPerPage(unsortedListOneSite, shouldLearningIdsAll);
        var notLearnedIdsPerPage = getIdsPerPage(unsortedListOneSite, notLearnedIdsAll);


        var unsortList = QuestionsFactory(solidIdsPerPage, shouldConsolidateIdsPerPage, shouldLearningeIdsPerPage, notLearnedIdsPerPage);

        return IsAuthor(unsortList, isAuthor, userId);
    }

    public class Questions
    {
        public string Title { get; set; }
        public string Category = " "; 
        public ImageFrontendData ImageFrontendData { get; set; }
        public string LearningStatus { get; set; }
        public string AuthorName { get; set; }
        public ImageUrl AuthorImageUrl { get; set; }
        public int LearningStatusNumber { get; set; }
        public string LearningStatusTooltip { get; set; }
        public string LinkToCategory { get; set; }
        public string LinkToQuestion { get; set; }
        public int AuthorId { get; set; }
    }
}
