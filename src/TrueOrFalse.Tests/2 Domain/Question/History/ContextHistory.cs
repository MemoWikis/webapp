﻿using System;
using System.Collections.Generic;
using NHibernate;
using TrueOrFalse.Tests;

public class ContextHistory : IRegisterAsInstancePerLifetime
{
    private readonly ISession _nhibernateSession;
    private readonly AnswerQuestion _answerQuestion;
    private readonly AnswerRepo _answerRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly CategoryRepository _categoryRepository;
    private readonly QuestionWritingRepo _questionWritingRepo;
    public List<Answer> All = new();
    public User User;

    public ContextHistory(ISession nhibernateSession,
        AnswerQuestion answerQuestion,
        AnswerRepo answerRepo,
        UserReadingRepo userReadingRepo,
        CategoryRepository categoryRepository,
        QuestionWritingRepo questionWritingRepo)
    {
        _nhibernateSession = nhibernateSession;
        _answerQuestion = answerQuestion;
        _answerRepo = answerRepo;
        _userReadingRepo = userReadingRepo;
        _categoryRepository = categoryRepository;
        _questionWritingRepo = questionWritingRepo;
        User = ContextUser.New(_userReadingRepo).Add("Firstname Lastname").Persist().All[0];
    }

    public void WriteHistory(User user, int daysOffset = -3)
    {
        var _session = _nhibernateSession;

	    var historyItem = new Answer
	    {
		    UserId = user.Id,
		    Question = ContextQuestion.GetQuestion(_answerRepo, 
                _answerQuestion, 
                _userReadingRepo,
                _categoryRepository, 
                _questionWritingRepo),
		    AnswerredCorrectly = AnswerCorrectness.True,
		    DateCreated = DateTime.Now.AddDays(daysOffset)
	    };

        _session.Save(historyItem);
        _session.Flush();

        All.Add(historyItem);
    }
}