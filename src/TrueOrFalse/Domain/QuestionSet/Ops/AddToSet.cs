using System;
using System.Collections.Generic;
using System.Linq;

public static class AddToSet 
{
    public static AddToSetResult Run(IEnumerable<int> questionIds, int questionSet)
    {
        return Run(
            Sl.QuestionRepo.GetByIds(questionIds.ToArray()), 
            Sl.SetRepo.GetById(questionSet));
    }

    public static AddToSetResult Run(Question question, Set set) 
        => Run(new List<Question> {question}, set);

    public static AddToSetResult Run(IList<Question> questions, Set set)
    {
        var notAddedQuestions = new List<Question>();
        foreach (var question in questions)
        {
            if (set.QuestionsInSet.Any(q => q.Question.Id == question.Id))
                notAddedQuestions.Add(question);
            else
            {
                var questionInSet = new QuestionInSet();
                questionInSet.Question = question;
                questionInSet.Set = set;
                questionInSet.DateCreated = DateTime.Now;
                questionInSet.DateModified = DateTime.Now;
                set.QuestionsInSet.Add(questionInSet);
                Sl.SetRepo.Update(set);
                EntityCache.AddOrUpdate(questionInSet);
                Sl.UpdateSetDataForQuestion.Run(question);
            }
        }

        foreach (var category in set.Categories)
        {
            category.UpdateCountQuestionsAggregated();
            Sl.CategoryRepo.Update(category);
        }

        Sl.R<AddValuationEntries_ForQuestionsInSetsAndDates>().Run(set, Sl.R<SessionUser>().User);

        return new AddToSetResult
        {
            AmountAddedQuestions = questions.Count - notAddedQuestions.Count,
            AmountOfQuestionsAlreadyInSet = notAddedQuestions.Count,
            Set = set
        };
    }
}