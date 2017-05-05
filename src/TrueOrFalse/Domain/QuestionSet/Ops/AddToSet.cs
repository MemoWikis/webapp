using System.Collections.Generic;
using System.Linq;

public class AddToSet : IRegisterAsInstancePerLifetime
{
    public static AddToSetResult Run(int[] questionIds, int questionSet)
    {
        return Run(
            Sl.QuestionRepo.GetByIds(questionIds), 
            Sl.SetRepo.GetById(questionSet));
    }

    public static AddToSetResult Run(Question question, Set set) 
        => Run(new List<Question> {question}, set);

    public static AddToSetResult Run(IList<Question> questions, Set set)
    {
        var nonAddedQuestions = new List<Question>();
        foreach (var question in questions)
        {
            if (set.QuestionsInSet.Any(q => q.Question.Id == question.Id))
                nonAddedQuestions.Add(question);
            else
            {
                var questionInSet = new QuestionInSet();
                questionInSet.Question = question;
                questionInSet.Set = set;
                Sl.QuestionInSetRepo.Create(questionInSet);
                Sl.UpdateSetDataForQuestion.Run(question);
            }
        }

        Sl.R<AddValuationEntries_ForQuestionsInSetsAndDates>().Run(set, Sl.R<SessionUser>().User);

        return new AddToSetResult
        {
            AmountAddedQuestions = questions.Count - nonAddedQuestions.Count,
            AmountOfQuestionsAlreadyInSet = nonAddedQuestions.Count,
            Set = set
        };
    }
}