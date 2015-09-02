using System.Collections.Generic;
using System.Web.Mvc;

public class WelcomeModel : BaseModel
{

    public string Date;
    
    public WelcomeModel()
    {
    }

    public WelcomeBoxQuestionVModel GetQuestionBoxVModel(int questionId, int contextCategoryId = 0)
    {
        var question = R<QuestionRepository>().GetById(questionId);
        if (question == null) //if question doesn't exist, take new empty question to avoid broken starting page
        {
            question = new Question();
        }
        return new WelcomeBoxQuestionVModel(question, contextCategoryId);
    }

    public WelcomeBoxSetTextQuestionsModel GetWelcomeSetBoxTextQuestionsModel(int setId, int[] questionIds = null)
    {
        return new WelcomeBoxSetTextQuestionsModel(setId, questionIds);
    }
    public WelcomeBoxSetImgQuestionsModel GetWelcomeSetBoxImgQuestionsModel(int setId, int[] questionIds = null)
    {
        return new WelcomeBoxSetImgQuestionsModel(setId, questionIds);
    }
}
