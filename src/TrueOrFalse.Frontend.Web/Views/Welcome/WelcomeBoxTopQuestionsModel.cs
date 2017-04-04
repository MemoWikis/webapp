using System.Collections.Generic;

public class WelcomeBoxTopQuestionsModel : BaseModel
{
    public IEnumerable<Question> Questions;

    public static WelcomeBoxTopQuestionsModel CreateMostRecent(int amount)
    {
        var result = new WelcomeBoxTopQuestionsModel();
        result.Questions = Sl.QuestionRepo.GetMostRecent(amount);

        return result;
    }
    
}
