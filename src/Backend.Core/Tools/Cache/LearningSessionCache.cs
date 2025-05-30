﻿using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;

public class LearningSessionCache(IHttpContextAccessor httpContextAccessor) : IRegisterAsInstancePerLifetime
{
    private readonly HttpContext _httpContext = httpContextAccessor.HttpContext!;
    private static readonly ConcurrentDictionary<string, LearningSession> _learningSessions = new();

    public void AddOrUpdate(LearningSession learningSession)
    {
        _httpContext.Session.ForceInit();

        _learningSessions.AddOrUpdate(
            _httpContext.Session.Id,
            learningSession,
            (a, b) => learningSession
        );
    }

    public void TryRemove()
    {
        _learningSessions.TryRemove(_httpContext.Session.Id, out _);
    }

    public LearningSession? GetLearningSession(bool log = true)
    {
        _learningSessions.TryGetValue(_httpContext.Session.Id, out var learningSession);

        if (learningSession != null)
        {
            AddOrUpdate(learningSession);
            return learningSession;
        }

        if (learningSession == null && log)
        {
            var stackException = new Exception($"learningSession is null. Call stack: {Environment.StackTrace}");
            ErrorLogging.Log(stackException, _httpContext);
        }

        return null;
    }

    public void EditQuestionInLearningSession(QuestionCacheItem question)
    {
        var learningSession = GetLearningSession(log: false);

        if (learningSession != null)
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
        if (learningSession == null || learningSession.CurrentStep == null)
            throw new Exception("learningSession is null or currentStep is null");

        var reloadAnswerBody = learningSession.CurrentStep.Question.Id == questionId;

        learningSession.Steps = learningSession.Steps.Where(s => s.Question.Id != questionId).ToList();

        return new RemovalResult
        {
            reloadAnswerBody = reloadAnswerBody,
            sessionIndex = learningSession.Steps.Count > learningSession.CurrentIndex + 1 ?
                learningSession.CurrentIndex :
                learningSession.Steps.Count - 1
        };
    }
}