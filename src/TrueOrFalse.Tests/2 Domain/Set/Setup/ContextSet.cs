using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Tests;

public class ContextSet
{
    private readonly ContextQuestion _contextQuestion = ContextQuestion.New();
    public readonly User Creator;
    private readonly ContextCategory _contextCategory = ContextCategory.New();
    private readonly SetRepo _setRepo;
    private readonly QuestionInSetRepo _questionInSetRepo;

    public List<Set> All = new List<Set>();
        
    private ContextSet()
    {
        Creator = ContextUser.GetUser();
        _setRepo = Sl.R<SetRepo>();
        _questionInSetRepo = Sl.R<QuestionInSetRepo>();
    }

    public static ContextSet New()
    {
        return new ContextSet();
    }

    public ContextSet AddSet(
        string name, 
        string text = "", 
        User creator = null, 
        int numberOfQuestions = 0,
        int amountCategoriesPerQuestion = 0,
        IList<Question> questions = null,
        List<Category> categories = null)
    {
        var set = new Set{
            Name = name, 
            Text = text, 
            Creator =  creator ?? Creator,
            Categories = categories ?? new List<Category>()
        };
            
        All.Add(set);

        var categoriesForQuestions = ContextCategory.New().Add(amountCategoriesPerQuestion).Persist().All;

        if (questions != null)
            AddQuestions(questions, categoriesForQuestions);

        for (var i = 0; i < numberOfQuestions; i++)
            AddQuestion("question_" + i, "answer_" + i, categoriesForQuestions);

        return this;
    }

    public ContextSet AddCategory(string name)
    {
        var category = _contextCategory.Add(name).Persist().All.Last();
        _contextCategory.Persist();
            
        All.Last().Categories.Add(category);
        return this;
    }

    public ContextSet AddQuestions(IList<Question> questions, IList<Category> categories)
    {
        foreach (var question in questions)
            AddQuestion(question);

        return this;
    }

    public ContextSet AddQuestion(string question, string solution, List<Category> categories =  null)
    {
        _contextQuestion.AddQuestion(questionText: question, solutionText: solution, categories: categories).Persist();
        var addedQuestion = _contextQuestion.All.Last();

        return AddQuestion(addedQuestion);
    }

    public ContextSet AddQuestion(Question question)
    {
        var set = All.Last();
        this.Persist();

        var newQuestionInSet = new QuestionInSet
        {
            Question = question,
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
            _setRepo.Create(set);

        _setRepo.Flush();

        return this;
    }

    public static Set GetPersistedSampleSet()
    {
        var questionContext = ContextQuestion.New()
            .AddQuestion(questionText: "Q1", solutionText: "A1").AddCategory("A")
            .AddQuestion(questionText: "Q2", solutionText: "A2").AddCategory("A")
            .AddQuestion(questionText: "Q3", solutionText: "A3")
            .AddQuestion(questionText: "Q4", solutionText: "A4").AddCategory("B")
            .Persist();

        var set = new Set { Creator = ContextUser.GetUser() };
        set.Add(questionContext.All);

        Sl.SetRepo.Create(set);

        return set;
    }
}