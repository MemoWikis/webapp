using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seedworks.Lib.Persistence;

[Serializable]
public class TestSession
{
    public virtual User User { get; set; }
    public virtual IList<int> QuestionIds { get; set; }
    public virtual TestSessionType TestSessionType { get; set; }
    public virtual int TestSessionTypeTypeId { get; set; }
    public virtual int CurrentStep { get; set; }
    public virtual int NumberOfSteps => QuestionIds.Count;
    public virtual List<Guid> AnsweredQuestionsQuestionViewGuid { get; set; }


    public TestSession(Set set, List<int> excludeQuestionIds = null)
    {
        CurrentStep = 1;
        TestSessionType = TestSessionType.Set;
        TestSessionTypeTypeId = set.Id;
        QuestionIds = new List<int>();
        AnsweredQuestionsQuestionViewGuid = new List<Guid>();
        QuestionIds = Sl.R<SetRepo>().GetRandomQuestions(set, 10, excludeQuestionIds, true).GetIds();
    }

    public TestSession(Category category, List<int> excludeQuestionIds = null)
    {
        CurrentStep = 1;
        TestSessionType = TestSessionType.Category;
        TestSessionTypeTypeId = category.Id;
        QuestionIds = new List<int>();
        AnsweredQuestionsQuestionViewGuid = new List<Guid>();
        QuestionIds = Sl.R<CategoryRepository>().GetRandomQuestions(category, 10, excludeQuestionIds, true).GetIds();
    }

    //private void Populate(TestSession testSession)
    //{
    //}
}
