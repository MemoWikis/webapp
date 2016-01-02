using System.Collections.Generic;
using System.Linq;

public class AddToSet : IRegisterAsInstancePerLifetime
{
    private readonly SetRepo _setRepo;
    private readonly QuestionRepo _questionRepo;
    private readonly QuestionInSetRepo _questionInSetRepo;
    private readonly UpdateSetDataForQuestion _updateSetData;

    public AddToSet(
            SetRepo setRepo, 
            QuestionRepo questionRepo,
            QuestionInSetRepo questionInSetRepo,
            UpdateSetDataForQuestion updateSetData){
        _setRepo = setRepo;
        _questionRepo = questionRepo;
        _questionInSetRepo = questionInSetRepo;
        _updateSetData = updateSetData;
            }

    public AddToSetResult Run(int[] questionIds, int questionSet)
    {
        return Run(
            _questionRepo.GetByIds(questionIds), 
            _setRepo.GetById(questionSet));
    }

    public AddToSetResult Run(IList<Question> questions, Set set)
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
                _questionInSetRepo.Create(questionInSet);
                _updateSetData.Run(question);
            }
        }

        Sl.R<AddValuationEntries_ForQuestionsInSetsAndDates>().Run(set, Sl.R<SessionUser>().User);

        return new AddToSetResult
        {
            AmountAddedQuestions = questions.Count() - nonAddedQuestions.Count(),
            AmountOfQuestionsAlreadyInSet = nonAddedQuestions.Count(),
            Set = set
        };
    }
}