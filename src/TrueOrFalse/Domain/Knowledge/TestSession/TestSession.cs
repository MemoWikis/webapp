using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Util;
using Seedworks.Lib.Persistence;

[Serializable]
public class TestSession
{
    public virtual User User { get; set; }
    public virtual IList<TestSessionStep> Steps { get; set; }
    public virtual TestSessionType TestSessionType { get; set; }
    public virtual int TestSessionTypeTypeId { get; set; }
    public virtual int CurrentStep { get; set; }
    public virtual int NumberOfSteps => Steps.Count;


    public TestSession(Set set, List<int> excludeQuestionIds = null)
    {
        CurrentStep = 1;
        TestSessionType = TestSessionType.Set;
        TestSessionTypeTypeId = set.Id;
        var questions = Sl.R<SetRepo>().GetRandomQuestions(set, 10, excludeQuestionIds, true).ToList();
        Populate(questions);
    }

    public TestSession(Category category, List<int> excludeQuestionIds = null)
    {
        CurrentStep = 1;
        TestSessionType = TestSessionType.Category;
        TestSessionTypeTypeId = category.Id;
        var questions = Sl.R<CategoryRepository>().GetRandomQuestions(category, 10, excludeQuestionIds, true).ToList();
        Populate(questions);
    }

    private void Populate(List<Question> questions)
    {
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
