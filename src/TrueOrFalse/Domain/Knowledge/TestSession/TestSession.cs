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
    public virtual int SetToTestId { get; set; }
    public virtual IList<int> SetsToTestIds { get; set; }
    public virtual string SetListTitle { get; set; }
    public virtual int CategoryToTestId { get; set; }

    public virtual bool IsSetSession => SetToTestId > 0;
    public virtual bool IsSetsSession => SetsToTestIds != null;
    public virtual bool IsCategorySession => CategoryToTestId > 0;

    public virtual int CurrentStep { get; set; }
    public virtual int NumberOfSteps => Steps.Count;

    public TestSession(Set set)
    {
        UriName = "Fragesatz-" + UriSanitizer.Run(set.Name);
        SetToTestId = set.Id;
        var excludeQuestionIds = Sl.R<SessionUser>().AnsweredQuestionIds.ToList();
        var questions = GetRandomQuestions.Run(set, Settings.TestSessionQuestionCount, excludeQuestionIds, true).ToList();
        Populate(questions);
    }

    public TestSession(IList<Set> sets, string setListTitle)
    {
        UriName = "Fragesaetze-" + UriSanitizer.Run(setListTitle);
        SetsToTestIds = sets.Select(s => s.Id).ToList();
        SetListTitle = setListTitle;
        var excludeQuestionIds = Sl.R<SessionUser>().AnsweredQuestionIds.ToList();
        var questions = GetRandomQuestions.Run(sets, Settings.TestSessionQuestionCount, excludeQuestionIds, true).ToList();
        Populate(questions);
    }

    public TestSession(IList<Set> setsFromCategory, string setListTitle, int categoryId)
    {
        UriName = UriSanitizer.Run(setListTitle);
        CategoryToTestId = categoryId;
        var excludeQuestionIds = Sl.R<SessionUser>().AnsweredQuestionIds.ToList();
        var questions = GetRandomQuestions.Run(setsFromCategory, Settings.TestSessionQuestionCount, excludeQuestionIds, true).ToList();
        Populate(questions);
    }

    public TestSession(Category category)
    {
        UriName = "Thema-" + UriSanitizer.Run(category.Name);
        CategoryToTestId = category.Id;
        var excludeQuestionIds = Sl.R<SessionUser>().AnsweredQuestionIds.ToList();
        var questions = GetRandomQuestions.Run(category, Settings.TestSessionQuestionCount, excludeQuestionIds, true).ToList();
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
