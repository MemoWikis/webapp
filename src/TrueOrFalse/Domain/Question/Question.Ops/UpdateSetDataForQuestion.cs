using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHibernate;

public class UpdateSetDataForQuestion : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;
    private readonly QuestionRepo _questionRepo;

    public UpdateSetDataForQuestion(
        ISession session, 
        QuestionRepo questionRepo)
    {
        _session = session;
        _questionRepo = questionRepo;
    }

    public void Run()
    {
        Run(_questionRepo.GetAll());
    }

    public void Run(ISet<QuestionInSet> questionsInSet, bool skipUpdateQuestion = false)
    {
        Run(questionsInSet.Select(c => c.Question),skipUpdateQuestion);
    }

    public void Run(IEnumerable<Question> questions, bool skipUpdateQuestion = false)
    {
        foreach (var question in questions)
            Run(question, skipUpdateQuestion);
    }

    public void Run(Question question, bool skipUpdateQuestion = false)
    {
        var questionsInSet = _session.QueryOver<QuestionInSet>()
            .Where(s => s.Question.Id == question.Id)
            .List<QuestionInSet>();

        question.SetsAmount = questionsInSet.Count;
        question.SetsTop5Json = JsonConvert.SerializeObject(
            questionsInSet
                .Where(x => x.Set != null)
                .Take(5)
                .Select(x => new SetMini{Id = x.Set.Id, Name = x.Set.Name}));

        if (!skipUpdateQuestion)
            _questionRepo.Update(question);
    }
}