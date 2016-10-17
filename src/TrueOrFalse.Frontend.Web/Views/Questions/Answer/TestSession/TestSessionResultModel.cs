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
    public IList<Answer> Answers;

    public TestSessionResultModel()
    {
        TestSession = _sessionUser.TestSession;
        if (TestSession == null)
            throw new Exception("TestSession is not defined");

        TestSessionTypeIsSet = TestSession.TestSessionType == TestSessionType.Set;
        TestSessionTypeIsCategory = TestSession.TestSessionType == TestSessionType.Category;
        Answers = Sl.R<AnswerRepo>().GetByQuestionViewGuids(TestSession.AnsweredQuestionsQuestionViewGuid, true);
        if (Answers.Count != TestSession.AnsweredQuestionsQuestionViewGuid.Count)
            throw new Exception("There should be an equal number of answers and questions asked in a test session (no duplicate/second answers for same question asked).");

        NumberCorrectAnswers = Answers.Count(a => a.AnsweredCorrectly());
        NumberWrongAnswers = Answers.Count(a => !a.AnsweredCorrectly());

        NumberCorrectPercentage = NumberCorrectAnswers / (NumberCorrectAnswers + NumberWrongAnswers);
        NumberWrongAnswersPercentage = NumberWrongAnswers / (NumberCorrectAnswers + NumberWrongAnswers);
    }
}
