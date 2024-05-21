public class FollowingAnswer
{
    public static Answer Get(Answer answer, AnswerRepo answerRepo)
    {
        var allAnswers = answerRepo.GetByQuestion(answer.Question.Id, answer.UserId);
        var allAnswersOrdered = allAnswers.OrderBy(x => x.DateCreated).ToList();

        for (var i = 0; i < allAnswersOrdered.Count; i++)
        {
            if (answer.Id == allAnswersOrdered[i].Id)
            {
                if (allAnswersOrdered.Count - 1 > i)
                    return allAnswersOrdered[i + 1];

                return null;
            }
        }

        return null;
    }
}