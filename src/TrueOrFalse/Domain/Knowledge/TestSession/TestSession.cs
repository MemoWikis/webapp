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

    public virtual Set SetToTest { get; set; }
    public virtual IList<Set> SetsToTest { get; set; }
    public virtual string SetListTitle { get; set; }
    public virtual Category CategoryToTest { get; set; }

    public virtual bool IsSetSession => SetToTest != null;
    public virtual bool IsSetsSession => SetsToTest != null;
    public virtual bool IsCategorySession => CategoryToTest != null;

    public virtual int CurrentStep { get; set; }
    public virtual int NumberOfSteps => Steps.Count;

    public virtual int TotalPossibleQuestions
    {
        get
        {
            if (IsSetSession) { 
                return SetToTest.Questions().Count;
            }

            if (IsSetsSession)
            {
                return SetsToTest.SelectMany(s => s.QuestionsInSet).Select(x => x.Question).Distinct().Count();
            }

            if (IsCategorySession)
                return GetQuestionsForCategory.AllIncludingQuestionsInSet(CategoryToTest.Id).Count;

            throw new Exception("unknown session type");
        }
    }

    public TestSession(Set set)
    {
        UriName = "Fragesatz-" + UriSanitizer.Run(set.Name);
        SetToTest = set;
        var excludeQuestionIds = Sl.R<SessionUser>().AnsweredQuestionIds.ToList();
        var questions = GetRandomQuestions.Run(set, Settings.TestSessionQuestionCount, excludeQuestionIds, true).ToList();
        Populate(questions);
    }

    public TestSession(IList<Set> sets, string setListTitle)
    {
        UriName = "Fragesaetze-" + UriSanitizer.Run(setListTitle);
        SetsToTest = sets;
        SetListTitle = setListTitle;
        var excludeQuestionIds = Sl.R<SessionUser>().AnsweredQuestionIds.ToList();
        var questions = GetRandomQuestions.Run(sets, Settings.TestSessionQuestionCount, excludeQuestionIds, true).ToList();
        Populate(questions);
    }

    public TestSession(IList<Set> setsFromCategory, string setListTitle, Category category)
    {
        UriName = UriSanitizer.Run(setListTitle);
        CategoryToTest = category;
        var excludeQuestionIds = Sl.R<SessionUser>().AnsweredQuestionIds.ToList();
        var questions = GetRandomQuestions.Run(setsFromCategory, Settings.TestSessionQuestionCount, excludeQuestionIds, true).ToList();
        Populate(questions);
    }

    public TestSession(Category category)
    {
        UriName = "Thema-" + UriSanitizer.Run(category.Name);
        CategoryToTest = category;
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
