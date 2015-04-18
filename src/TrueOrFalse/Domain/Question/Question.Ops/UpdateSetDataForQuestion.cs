using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHibernate;

public class UpdateSetDataForQuestion : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;
    private readonly QuestionRepository _questionRepo;

    public UpdateSetDataForQuestion(
        ISession session, 
        QuestionRepository questionRepo)
    {
        _session = session;
        _questionRepo = questionRepo;
    }

    public void Run()
    {
        Run(_questionRepo.GetAll());
    }

    public void Run(IList<QuestionInSet> questionsInSet)
    {
        Run(questionsInSet.Select(c => c.Question));
    }

    public void Run(IEnumerable<Question> questions)
    {
        foreach (var question in questions)
            Run(question);
    }

    public void Run(Question question)
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

        _questionRepo.Update(question);
    }
}