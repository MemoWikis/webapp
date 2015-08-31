using System.Collections.Generic;
using System.Web.Mvc;

public class WelcomeModel : BaseModel
{
    /*public IList<Question> MostPopular = new List<Question>();
    public IList<Question> MostWantend = new List<Question>();
    public IList<Question> MostImportant = new List<Question>();*/

    public string Date;
    
    public WelcomeModel()
    {
    }

    public WelcomeQuestionBoxVModel GetQuestionBoxVModel(int questionId, int contextCategoryId = 0)
    {
        var question = R<QuestionRepository>().GetById(questionId);
        if (question == null) //if question doesn't exist, take new empty question to avoid broken starting page
        {
            question = new Question();
        }
        return new WelcomeQuestionBoxVModel(question, contextCategoryId);
    }

    public WelcomeSetBoxTextQuestionsModel GetWelcomeSetBoxTextQuestionsModel(int setId, int[] questionIds = null)
    {
        return new WelcomeSetBoxTextQuestionsModel(setId, questionIds);
    }
    public WelcomeSetBoxImgQuestionsModel GetWelcomeSetBoxImgQuestionsModel(int setId, int[] questionIds = null)
    {
        return new WelcomeSetBoxImgQuestionsModel(setId, questionIds);
    }
}
