using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

public class CreateLearningSession
{
    public static LearningSession ForCategory(int categoryId, int userId = -1)
    {
        var category = Sl.CategoryRepo.GetByIdEager(categoryId);

        var questions = category.GetAggregatedQuestionsFromMemoryCache();

        if (questions.Count == 0)
            throw new Exception("Cannot start LearningSession with 0 questions.");

        var user = userId != -1 ? Sl.Resolve<UserRepo>().GetById(userId) : Sl.R<SessionUser>().User;

        var learningSession = new LearningSession
        {
            CategoryToLearn = category,
            Steps = GetLearningSessionSteps.Run(questions, userId),
            User = user
        };

        Sl.LearningSessionRepo.Create(learningSession);

        return learningSession;
    }
}
