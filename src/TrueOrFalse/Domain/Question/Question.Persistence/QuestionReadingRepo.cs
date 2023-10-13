using NHibernate;
using Seedworks.Lib.Persistence;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class QuestionReadingRepo : RepositoryDbBase<Question>
{
    private readonly ISession _session;
    private readonly RepositoryDb<Question> _repo;

    public QuestionReadingRepo(ISession session) : base(session)
    {
       _repo =  new RepositoryDbBase<Question>(session); 
       _session = session;
    }

    public int TotalPublicQuestionCount()
    {
        return _session.QueryOver<Question>()
            .Where(q => q.Visibility == QuestionVisibility.All)
            .RowCount();
    }

    public int HowManyNewPublicQuestionsCreatedSince(DateTime since)
    {
        return _session.QueryOver<Question>()
            .Where(q => q.DateCreated > since)
            .And(q => q.Visibility == QuestionVisibility.All)
            .RowCount();
    }

    public IList<Question> GetForCategory(int categoryId)
    {
        return _session.QueryOver<Question>()
            .Where(q => q.Visibility == QuestionVisibility.All)
            .JoinQueryOver<Category>(q => q.Categories)
            .Where(c => c.Id == categoryId)
            .List<Question>();
    }

    public IList<Question> GetAllEager()
    {
        var questions = _session.QueryOver<Question>().Future().ToList();

        _session.QueryOver<Question>()
            .Fetch(SelectMode.Fetch, x => x.Categories)
            .Future();

        _session.QueryOver<Question>()
            .Fetch(SelectMode.Fetch, x => x.References)
            .Future();
        var result = questions;

        foreach (var question in result)
        {
            NHibernateUtil.Initialize(question.Creator);
            NHibernateUtil.Initialize(question.References);
        }

        return result.ToList();
    }

    public Question GetById(int id)
    {
        return _repo.GetById(id);
    }

    public IList<Question> GetAll()
    {
        return _repo.GetAll();
    }
}

