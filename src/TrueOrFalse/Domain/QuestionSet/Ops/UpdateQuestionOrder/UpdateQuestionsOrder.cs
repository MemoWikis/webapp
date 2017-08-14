
using System.Collections.Generic;

public class UpdateQuestionsOrder : IRegisterAsInstancePerLifetime
{
    private readonly QuestionInSetRepo _questionInSetRepo;

    public UpdateQuestionsOrder(QuestionInSetRepo questionInSetRepo){
        _questionInSetRepo = questionInSetRepo;
    }

    public void Run(IEnumerable<NewQuestionIndex> newIndicies)
    {   
        foreach (var newQuestionIndex in newIndicies)
        {   
            
            var questionInSet = _questionInSetRepo.Query.Where(
                x => x.Id == newQuestionIndex.Id).SingleOrDefault();
            if (questionInSet == null)
            {
                continue;
            }
            questionInSet.Sort = newQuestionIndex.NewIndex;
            _questionInSetRepo.Update(questionInSet);
        }
    }
}