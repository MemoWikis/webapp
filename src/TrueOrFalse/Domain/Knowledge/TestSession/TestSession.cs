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


    public TestSession(int setId = 19)
    {
        CurrentStep = 1;
        TestSessionType = TestSessionType.Set;
        var set = Sl.R<SetRepo>().GetById(setId);
        TestSessionTypeTypeId = set.Id;
        QuestionIds = new List<int>();
        QuestionIds = (IList<int>) set.QuestionIds().Take(4).ToList(); //create SetRepo-Method to get most interesting/relevant/popular questions

    }

    private void Populate(TestSession testSession)
    {
    }
}
