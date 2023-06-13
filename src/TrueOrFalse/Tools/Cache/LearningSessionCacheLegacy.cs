using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class LearningSessionCacheLegacy : IRegisterAsInstancePerLifetime
{
    private readonly HttpContext _context;

    private static readonly ConcurrentDictionary<string, LearningSession> _learningSessions =
        new ConcurrentDictionary<string, LearningSession>();

    public static void AddOrUpdate(LearningSession learningSession)
    {
        _learningSessions.AddOrUpdate(
            HttpContext.Current.Session.SessionID,
            learningSession,
            (a, b) => learningSession
        );
    }

    public static LearningSession TryRemove()
    {
        _learningSessions.TryRemove(
            HttpContext.Current.Session.SessionID, out var learningSession
        );
        return GetLearningSession();
    }

    public static LearningSession GetLearningSession()
    {
        _learningSessions.TryGetValue(HttpContext.Current.Session.SessionID, out var learningSession);
        AddOrUpdate(learningSession);
        return learningSession;
    }

    public static void InsertNewQuestionToLearningSession(QuestionCacheItem question, int sessionIndex, LearningSessionConfig config)
    {
        var learningSession = GetLearningSession();
        var step = new LearningSessionStep(question);

        if (learningSession != null)
        {
            var allQuestionValuation = SessionUserCache.GetQuestionValuations(config.CurrentUserId);
            var questionDetail = LearningSessionCreator.BuildQuestionDetail(config, question, allQuestionValuation);

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

    public static void EditQuestionInLearningSession(QuestionCacheItem question)
    {
        var learningSession = GetLearningSession();

        foreach (var step in learningSession.Steps)
            if (step.Question.Id == question.Id)
                step.Question = question;
    }

    public static int RemoveQuestionFromLearningSession(int sessionIndex, int questionId)
    {
        var learningSession = GetLearningSession();
        learningSession.Steps = learningSession.Steps.Where(s => s.Question.Id != questionId).ToList();

        if (learningSession.Steps.Count > sessionIndex + 1)
            return sessionIndex;

        return learningSession.Steps.Count - 1;
    }
}