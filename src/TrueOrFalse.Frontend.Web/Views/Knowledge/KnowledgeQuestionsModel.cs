using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using EasyNetQ.Events;
using Microsoft.AspNet.SignalR;
using NHibernate.Bytecode;
using TrueOrFalse.Frontend.Web.Code;


public class KnowledgeQuestionsModel
{
    private UserImageSettings userImageSettings = new UserImageSettings();

    public List<Questions> GetQuestionsWishFromDatabase(int userId, bool isAuthor)
    {
        var questionsListSolid = GetListWithKnowWas("solid", userId);
        var shouldConsolidate = GetListWithKnowWas("shouldConsolidate", userId);
        var shouldLearning = GetListWithKnowWas("shouldLearning", userId);
        var NotLearned = GetListWithKnowWas("", userId);
        var unsortList = QuestionsFactory(questionsListSolid, shouldConsolidate, shouldLearning, NotLearned);

        return IsAuthor(unsortList, isAuthor, userId);
    }

    public List<Questions> IsAuthor(List<Questions> unsortList, bool isAuthor, int userId)
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

    private string GetCategoryNameShort(string name)
    {
        if (name.Length > 15)
        {
            return name.Substring(0, 16);
        }

        return name;
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
            try
            {
                questions.LinkToCategory = Links.GetUrl(categories[0]);
                questions.Category = categories[0].Name;
                questions.ImageFrontendData = categoryAndSetDataWishKnowledge.GetCategoryImage(categories[0].Id);
            }
            catch (Exception e)
            {
                questions.LinkToCategory = " ";
                questions.Category = "keine Kategorie";
                questions.ImageFrontendData = categoryAndSetDataWishKnowledge.GetCategoryImage(682);
            }

           


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

    public List<Questions> QuestionsFactory(
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

    public List<Question> GetListWithKnowWas(string knowWas, int userId)
    {
        List<int> questionsIListInList;
        if (knowWas.Equals("solid"))
        {
            var questionsSolid = Sl.QuestionRepo.GetByKnowledge(userId, true, false, false, false);
            questionsIListInList = new List<int>(questionsSolid);

            return new List<Question>(Sl.QuestionRepo.GetByIds(questionsIListInList));
        }
        if (knowWas.Equals("shouldConsolidate"))
        {
            var questionsShouldConsolidate = Sl.QuestionRepo.GetByKnowledge(userId, false, true, false, false);
            questionsIListInList = new List<int>(questionsShouldConsolidate);

            return new List<Question>(Sl.QuestionRepo.GetByIds(questionsIListInList));
        }
        if (knowWas.Equals("shouldLearning"))
        {
            var questionsshouldLearning = Sl.QuestionRepo.GetByKnowledge(userId, false, false, true, false);
            questionsIListInList = new List<int>(questionsshouldLearning);

            return new List<Question>(Sl.QuestionRepo.GetByIds(questionsIListInList));
        }

        var questionsNotLearned = Sl.QuestionRepo.GetByKnowledge(userId, false, false, false, true);
        questionsIListInList = new List<int>(questionsNotLearned);

        return new List<Question>(Sl.QuestionRepo.GetByIds(questionsIListInList));
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
