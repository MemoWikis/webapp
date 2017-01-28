using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;
using TrueOrFalse.Web;

[Serializable]
public class TestSession
{
    public virtual int Id { get; set; }
    public virtual string UriName { get; set; }
    public virtual IList<TestSessionStep> Steps { get; set; }
    public virtual TestSessionType TestSessionType { get; set; }
    public virtual int TestSessionTypeTypeId { get; set; }
    public virtual int CurrentStep { get; set; }
    public virtual int NumberOfSteps => Steps.Count;

    public TestSession(Set set)
    {
        UriName = "Fragesatz-" + UriSanitizer.Run(set.Name);
        TestSessionType = TestSessionType.Set;
        TestSessionTypeTypeId = set.Id;
        var excludeQuestionIds = Sl.R<SessionUser>().AnsweredQuestionIds.ToList();
        var questions = GetRandomQuestions.Run(set, 5, excludeQuestionIds, true).ToList();
        Populate(questions);
    }

    public TestSession(IList<Set> setsFromCategory, string setListTitle, int categoryId)
    {
        UriName = UriSanitizer.Run(setListTitle);
        TestSessionType = TestSessionType.Category;
        TestSessionTypeTypeId = categoryId;
        var excludeQuestionIds = Sl.R<SessionUser>().AnsweredQuestionIds.ToList();
        var questions = GetRandomQuestions.Run(setsFromCategory, 5, excludeQuestionIds, true).ToList();
        Populate(questions);
    }

    public TestSession(Category category)
    {
        UriName = "Kategorie-" + UriSanitizer.Run(category.Name);
        TestSessionType = TestSessionType.Category;
        TestSessionTypeTypeId = category.Id;
        var excludeQuestionIds = Sl.R<SessionUser>().AnsweredQuestionIds.ToList();
        var questions = GetRandomQuestions.Run(category, 5, excludeQuestionIds, true).ToList();
        Populate(questions);
    }

    private void Populate(List<Question> questions)
    {
        Id = Sl.R<SessionUser>().GetNextTestSessionId();
        CurrentStep = 1;
        Steps = new List<TestSessionStep>();
        questions.ForEach(q => Steps.Add(new TestSessionStep { QuestionId = q.Id}));   
    }

    public void FillUpStepProperties() => 
        Steps.ForEach(s => s.Question = Sl.R<QuestionRepo>().GetById(s.QuestionId));
}
