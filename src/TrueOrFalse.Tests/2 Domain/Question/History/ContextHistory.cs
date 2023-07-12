using System;
using System.Collections.Generic;
using NHibernate;
using TrueOrFalse.Tests;

public class ContextHistory : IRegisterAsInstancePerLifetime
{
    private readonly ISession _nhibernateSession;
    private readonly QuestionRepo _questionRepo;
    private readonly AnswerQuestion _answerQuestion;
    private readonly AnswerRepo _answerRepo;
    private readonly UserRepo _userRepo;
    public List<Answer> All = new();
    public User User;

    public ContextHistory(ISession nhibernateSession,
        QuestionRepo questionRepo,
        AnswerQuestion answerQuestion,
        AnswerRepo answerRepo,
        UserRepo userRepo)
    {
        _nhibernateSession = nhibernateSession;
        _questionRepo = questionRepo;
        _answerQuestion = answerQuestion;
        _answerRepo = answerRepo;
        _userRepo = userRepo;
        User = ContextUser.New(_userRepo).Add("Firstname Lastname").Persist().All[0];
    }

    public void WriteHistory(int daysOffset = -3)
	{
	    WriteHistory(User, daysOffset);
	}

    public void WriteHistory(User user, int daysOffset = -3)
    {
        var _session = _nhibernateSession;

	    var historyItem = new Answer
	    {
		    UserId = user.Id,
		    Question = ContextQuestion.GetQuestion(_questionRepo, _answerRepo, _answerQuestion, _userRepo),
		    AnswerredCorrectly = AnswerCorrectness.True,
		    DateCreated = DateTime.Now.AddDays(daysOffset)
	    };

        _session.Save(historyItem);
        _session.Flush();

        All.Add(historyItem);
    }
}