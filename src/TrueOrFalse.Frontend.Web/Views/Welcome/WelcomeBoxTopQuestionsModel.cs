using System.Collections.Generic;

public class WelcomeBoxTopQuestionsModel : BaseModel
{
    public IEnumerable<Question> Questions;

    public WelcomeBoxTopQuestionsModel()
    {
    }

    public static WelcomeBoxTopQuestionsModel CreateMostRecent(int amount)
    {
        var result = new WelcomeBoxTopQuestionsModel();
        var questionRepo = Sl.R<QuestionRepo>();
        result.Questions = questionRepo.GetMostRecent(amount);

        return result;
    }

    
}
