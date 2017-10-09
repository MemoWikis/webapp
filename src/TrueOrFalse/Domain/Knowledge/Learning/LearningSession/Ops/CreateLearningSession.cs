using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CreateLearningSession
{
    public static LearningSession ForCategory(int categoryId)
    {
        var category = Sl.CategoryRepo.GetByIdEager(categoryId);

        var questions = category.GetAggregatedQuestionsFromMemoryCache();

        if (questions.Count == 0)
            throw new Exception("Cannot start LearningSession with 0 questions.");

        var learningSession = new LearningSession
        {
            CategoryToLearn = category,
            Steps = GetLearningSessionSteps.Run(questions),
            User = Sl.R<SessionUser>().User
        };

        Sl.LearningSessionRepo.Create(learningSession);

        return learningSession;
    }
}
