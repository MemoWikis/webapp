using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class TestSessionResultModel : BaseModel
{
    public TestSession TestSession;
    public int NumberQuestions;
    public int NumberCorrectAnswers; //answered correctly at first try
    public int NumberWrongAnswers;
    public int NumberNotAnswered;
    public int NumberCorrectPercentage;
    public int NumberWrongAnswersPercentage;
    public int NumberNotAnsweredPercentage;
    public bool TestSessionTypeIsSet;
    public bool TestSessionTypeIsCategory;
    public int TestSessionTypeTypeId;

    public TestSessionResultModel()
    {
        TestSession = _sessionUser.TestSession;
        TestSessionTypeIsSet = TestSession.TestSessionType == TestSessionType.Set;
        TestSessionTypeIsCategory = TestSession.TestSessionType == TestSessionType.Category;
    }
}
