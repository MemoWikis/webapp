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

        var questions = Sl.QuestionRepo.GetByKnowledge(userId, true, true, true, true);
        var questionsIListInList = new List<int>(questions);
        var questionsList2 = new List<Question>(Sl.QuestionRepo.GetByIds(questionsIListInList));

        return QuestionsFactory(questionsList2);
    }


    public List<Questions> QuestionsFactory(IList<Question> questionsListFromDataBase)
    {
        var questionsList = new List<Questions>();
        var categoryAndSetDataWishKnowledge = new CategoryAndSetDataWishKnowledge();

        foreach (var question in questionsListFromDataBase)
        {
            var questions = new Questions();
            var categories = question.Categories;
            
            questions.Titel = question.GetShortTitle();
            questions.Categories = categories[0].Name;
            questions.ImageFrontendData = categoryAndSetDataWishKnowledge.GetCategoryImage(categories[0].Id);

            questionsList.Add(questions);
        }

        return questionsList;
    }

    public class Questions
    {
        public string Titel { get; set; }
        public string Categories { get; set; }
        public ImageFrontendData ImageFrontendData;
    }
}
