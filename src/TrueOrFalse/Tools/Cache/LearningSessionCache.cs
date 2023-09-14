﻿using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;

public class LearningSessionCache: IRegisterAsInstancePerLifetime
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private static readonly ConcurrentDictionary<string, LearningSession> _learningSessions = new();

    public LearningSessionCache(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public void AddOrUpdate(LearningSession learningSession)
    {
        _learningSessions.AddOrUpdate(
            _httpContextAccessor.HttpContext.Session.Id,
            learningSession,
            (a, b) => learningSession
        );
    }

    public  LearningSession TryRemove()
    {
        _learningSessions.TryRemove(_httpContextAccessor.HttpContext.Session.Id, out var learningSession);
        return GetLearningSession();
    }

    public  LearningSession GetLearningSession()
    {
        _learningSessions.TryGetValue(_httpContextAccessor.HttpContext.Session.Id, out var learningSession);

        if (learningSession != null)
        {
            AddOrUpdate(learningSession);
            return learningSession;
        }
        else
        {

        }

        throw new NullReferenceException("learningSession is null"); 
    }

    public void EditQuestionInLearningSession(QuestionCacheItem question)
    {
        var learningSession = GetLearningSession();

        foreach (var step in learningSession.Steps)
            if (step.Question.Id == question.Id)
                step.Question = question;
    }

    public int RemoveQuestionFromLearningSession(int sessionIndex, int questionId)
    {
        var learningSession = GetLearningSession();
        learningSession.Steps = learningSession.Steps.Where(s => s.Question.Id != questionId).ToList();

        if (learningSession.Steps.Count > sessionIndex + 1)
            return sessionIndex;

        return learningSession.Steps.Count - 1;
    }

    public class RemovalResult
    {
        public bool reloadAnswerBody;
        public int sessionIndex;
    }
    public RemovalResult RemoveQuestionFromLearningSession(int questionId)
    {
        var learningSession = GetLearningSession();
        var reloadAnswerBody = learningSession.CurrentStep.Question.Id == questionId;

        learningSession.Steps = learningSession.Steps.Where(s => s.Question.Id != questionId).ToList();

        return new RemovalResult
        {
            reloadAnswerBody = reloadAnswerBody,
            sessionIndex = learningSession.Steps.Count > learningSession.CurrentIndex + 1 ? learningSession.CurrentIndex : learningSession.Steps.Count - 1
        };
    }
}