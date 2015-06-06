using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Tests;

public class ContextSet : IRegisterAsInstancePerLifetime
{
    private readonly ContextQuestion _contextQuestion = ContextQuestion.New();
    private readonly ContextUser _contextUser = ContextUser.New();
    private readonly ContextCategory _contextCategory = ContextCategory.New();
    private readonly SetRepository _setRepository;
    private readonly QuestionInSetRepo _questionInSetRepo;

    public List<Set> All = new List<Set>();
        
    public ContextSet(
        SetRepository setRepository,
        QuestionInSetRepo questionInSetRepo)
    {
        _contextUser.Add("Context Set").Persist();
        _setRepository = setRepository;
        _questionInSetRepo = questionInSetRepo;
    }

    public static ContextSet New()
    {
        return BaseTest.Resolve<ContextSet>();
    }

    public ContextSet AddSet(string name, string text = "", User creator = null)
    {
        var set = new Set{
            Name = name, 
            Text = text, 
            Creator =  creator ?? _contextUser.All.First()
        };

        All.Add(set);

        _contextUser.Persist();
        return this;
    }

    public ContextSet AddCategory(string name)
    {
        var category = _contextCategory.Add(name).Persist().All.Last();
        _contextCategory.Persist();
            
        All.Last().Categories.Add(category);
        return this;
    }

    public ContextSet AddQuestion(string question, string solution)
    {
        _contextQuestion.AddQuestion(question, solution).Persist();
        var addedQuestion = _contextQuestion.All.Last();
            
        var set = All.Last();
        this.Persist();

        var newQuestionInSet = new QuestionInSet
        {
            Question = addedQuestion,
            Set = set
        };
        _questionInSetRepo.Create(newQuestionInSet);
        _questionInSetRepo.Flush();

        set.QuestionsInSet.Add(newQuestionInSet);
            

        return this;
    }

    public ContextSet Persist()
    {
        foreach (var set in All)
            _setRepository.Create(set);

        _setRepository.Flush();

        return this;
    }
}