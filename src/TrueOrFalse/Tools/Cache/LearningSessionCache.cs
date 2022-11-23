using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class LearningSessionCache
{
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
            var allQuestionValuation = SessionUserCache.GetQuestionValuations(SessionUser.UserId);
            var questionDetail = LearningSessionCreator.BuildQuestionDetail(config, question, allQuestionValuation);

            learningSession.QuestionCounter = LearningSessionCreator.CountQuestionsForSessionConfig(questionDetail, learningSession.QuestionCounter);

            if (questionDetail.AddByWuwi &&
                questionDetail.AddByCreator &&  
                questionDetail.AddByVisibility &&
                questionDetail.FilterByKnowledgeSummary)
            {
                learningSession.Steps.Insert(sessionIndex, step);
            }

            learningSession.QuestionCounter.Max += 1;
            AddOrUpdate(learningSession);
        }
    }

    public static void EditQuestionInLearningSession(QuestionCacheItem question, int sessionIndex)
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