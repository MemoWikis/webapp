using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Util;
using Seedworks.Lib.Persistence;
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


    public TestSession(Set set, int testSessionId, List<int> excludeQuestionIds = null)
    {
        UriName = "Fragesatz-" + UriSanitizer.Run(set.Name);
        TestSessionType = TestSessionType.Set;
        TestSessionTypeTypeId = set.Id;
        var questions = Sl.R<SetRepo>().GetRandomQuestions(set, 10, excludeQuestionIds, true).ToList();
        Populate(questions, testSessionId);
    }

    public TestSession(Category category, int testSessionId, List<int> excludeQuestionIds = null)
    {
        UriName = "Kategorie-" + UriSanitizer.Run(category.Name);
        TestSessionType = TestSessionType.Category;
        TestSessionTypeTypeId = category.Id;
        var questions = Sl.R<CategoryRepository>().GetRandomQuestions(category, 10, excludeQuestionIds, true).ToList();
        Populate(questions, testSessionId);
    }

    private void Populate(List<Question> questions, int testSessionId)
    {
        Id = testSessionId;
        CurrentStep = 1;
        Steps = new List<TestSessionStep>();
        questions.ForEach(q => Steps.Add(new TestSessionStep { QuestionId = q.Id}));   
    }

    public void FillUpStepProperties()
    {
        // gets Questions from the repository for each TestSessionStep.
        Steps.ForEach(s =>
        {
            s.Question = Sl.R<QuestionRepo>().GetById(s.QuestionId);
        });
    }
}
