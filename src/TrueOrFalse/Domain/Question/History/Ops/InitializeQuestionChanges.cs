using System.Linq;

public class InitializeQuestionChanges
{
    public static void Go()
    {
        var allQuestions = Sl.QuestionRepo.GetAll();
        var allQuestionChanges = Sl.QuestionChangeRepo.GetAllEager();

        var memuchoUser = Sl.UserRepo.GetMemuchoUser();

        foreach (var question in allQuestions)
        {
            if (allQuestionChanges.All(x => x.Question.Id != question.Id))
                Sl.QuestionChangeRepo.AddUpdateEntry(question, memuchoUser, imageWasChanged: true);
        }
    }
}