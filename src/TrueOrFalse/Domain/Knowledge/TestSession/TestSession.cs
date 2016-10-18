﻿using System;
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


    public TestSession(int setId, List<int> excludeQuestionIds = null)
    {
        CurrentStep = 1;
        TestSessionType = TestSessionType.Set;
        var set = Sl.R<SetRepo>().GetById(setId);
        TestSessionTypeTypeId = set.Id;
        QuestionIds = new List<int>();
        AnsweredQuestionsQuestionViewGuid = new List<Guid>();
        QuestionIds = Sl.R<SetRepo>().GetRandomQuestions(set, 5, excludeQuestionIds, true).GetIds();
    }

    //private void Populate(TestSession testSession)
    //{
    //}
}
