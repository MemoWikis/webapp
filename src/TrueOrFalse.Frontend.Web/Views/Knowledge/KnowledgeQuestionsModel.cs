using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EasyNetQ.Events;
using Microsoft.AspNet.SignalR;


public class KnowledgeQuestionsModel
{
    public List<Questions> GetQuestionsWishFromDatabase(int userId)
    {
       var questionsListSolid = GetListWithKnowWas("solid", userId);
        var shouldConsolidate = GetListWithKnowWas("shouldConsolidate", userId);
        var shouldLearning = GetListWithKnowWas("shouldLearning", userId);
        var NotLearned = GetListWithKnowWas("", userId);

        return QuestionsFactory(questionsListSolid, shouldConsolidate, shouldLearning, NotLearned);
    }

    public List<Questions> QuestionsFactory(
        IList<Question> questionsListSolid, 
        IList<Question> questionsListShouldConsolidate,
        IList<Question> questionsListShouldLearning,
        IList<Question> questionsListNotLearned
        )
    {
        var questionsList = new List<Questions>();
        var categoryAndSetDataWishKnowledge = new CategoryAndSetDataWishKnowledge();
        
        foreach (var question in questionsListSolid)
        {
            var questions = new Questions();
            var categories = question.Categories;
            
            questions.Titel = question.GetShortTitle();
            questions.Categories = categories[0].Name;
            questions.ImageFrontendData = categoryAndSetDataWishKnowledge.GetCategoryImage(categories[0].Id);
            questions.LearningStatus = "greenD";
            questionsList.Add(questions);
        }

        foreach (var question in questionsListShouldConsolidate)
        {
            var questions = new Questions();
            var categories = question.Categories;

            questions.Titel = question.GetShortTitle();
            questions.Categories = categories[0].Name;
            questions.ImageFrontendData = categoryAndSetDataWishKnowledge.GetCategoryImage(categories[0].Id);
            questions.LearningStatus = "yellow";
            questionsList.Add(questions);
        }


        foreach (var question in questionsListShouldLearning)
        {
            var questions = new Questions();
            var categories = question.Categories;

            questions.Titel = question.GetShortTitle();
            questions.Categories = categories[0].Name;
            questions.ImageFrontendData = categoryAndSetDataWishKnowledge.GetCategoryImage(categories[0].Id);
            questions.LearningStatus = "red";
            questionsList.Add(questions);
        }


        foreach (var question in questionsListNotLearned)
        {
            var questions = new Questions();
            var categories = question.Categories;

            questions.Titel = question.GetShortTitle();
            questions.Categories = categories[0].Name;
            questions.ImageFrontendData = categoryAndSetDataWishKnowledge.GetCategoryImage(categories[0].Id);
            questions.LearningStatus = "grey";
            questionsList.Add(questions);
        }

        return questionsList;
    }

    public string GetKnowledgeKnowWasRound(string knowWas)
    {
        var solid = "<div id = 'box1' style='background-color: green; position: absolute; width: 200px; height: 100px;'><p> Box als Karteikartenreiter</p></div>";
        return solid;
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

    public class Questions
    {
        public string Titel { get; set; }
        public string Categories { get; set; }
        public ImageFrontendData ImageFrontendData { get; set; }
        public string LearningStatus { get; set; }
    }
}
