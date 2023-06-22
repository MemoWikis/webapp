using System.Collections.Concurrent;
using System.Linq;
using System.Web;

public class LearningSessionCache: IRegisterAsInstancePerLifetime
{
    private static readonly ConcurrentDictionary<string, LearningSession> _learningSessions = new();

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