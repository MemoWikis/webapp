using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class LearningSessionCache: IRegisterAsInstancePerLifetime
{
    private readonly LearningSessionCreator _learningSessionCreator;
    private static readonly ConcurrentDictionary<string, LearningSession> _learningSessions = new();

    public LearningSessionCache(LearningSessionCreator learningSessionCreator)
    {
        _learningSessionCreator = learningSessionCreator;
    }

    public void AddOrUpdate(LearningSession learningSession)
    {
        _learningSessions.AddOrUpdate(
            HttpContext.Current.Session.SessionID,
            learningSession,
            (a, b) => learningSession
        );
    }

    public  LearningSession TryRemove()
    {
        _learningSessions.TryRemove(
            HttpContext.Current.Session.SessionID, out var learningSession
        );
        return GetLearningSession();
    }

    public  LearningSession GetLearningSession()
    {
        _learningSessions.TryGetValue(HttpContext.Current.Session.SessionID, out var learningSession);
        AddOrUpdate(learningSession);
        return learningSession;
    }

    public  void InsertNewQuestionToLearningSession(QuestionCacheItem question, int sessionIndex, LearningSessionConfig config)
    {
        var learningSession = GetLearningSession();
        var step = new LearningSessionStep(question);

        if (learningSession != null)
        {
            var allQuestionValuation = SessionUserCache.GetQuestionValuations(config.CurrentUserId);
            var questionDetail = _learningSessionCreator.BuildQuestionDetail(config, question, allQuestionValuation);

            learningSession.QuestionCounter = LearningSessionCreator.CountQuestionsForSessionConfig(questionDetail, learningSession.QuestionCounter);

            if (questionDetail.AddByWuwi &&
                questionDetail.AddByCreator &&  
                questionDetail.AddByVisibility &&
                questionDetail.FilterByKnowledgeSummary)
            {
                if (learningSession.Steps.Count > sessionIndex + 1)
                    learningSession.Steps.Insert(sessionIndex + 1, step);
                else learningSession.Steps.Add(step);
            }

            learningSession.QuestionCounter.Max += 1;
            AddOrUpdate(learningSession);
        }
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
}