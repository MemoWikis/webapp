using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse.Frontend.Web.Code;

public class TestSessionResultModel : BaseModel
{
    public TestSession TestSession;
    public int NumberQuestions;
    public int NumberCorrectAnswers;
    public int NumberWrongAnswers;
    //public int NumberNotAnswered;
    public int NumberCorrectPercentage;
    public int NumberWrongAnswersPercentage;
    //public int NumberNotAnsweredPercentage;
    public int PercentageAverageRightAnswers;
    public bool MeBetterThanAverage;
    public bool TestSessionTypeIsSet;
    public bool TestSessionTypeIsCategory;
    public IList<Answer> Answers;

    public Set TestedSet;
    public Category TestedCategory;
    public string LinkForRepeatTest;

    public TestSessionResultModel()
    {
        TestSession = _sessionUser.TestSession;
        if (TestSession == null)
            throw new Exception("TestSession is not defined");

        TestSessionTypeIsSet = TestSession.TestSessionType == TestSessionType.Set;
        TestSessionTypeIsCategory = TestSession.TestSessionType == TestSessionType.Category;
        if (TestSessionTypeIsSet)
        {
            TestedSet = Sl.R<SetRepo>().GetById(TestSession.TestSessionTypeTypeId);
            LinkForRepeatTest = Links.TestSessionStartForSet(TestSession.TestSessionTypeTypeId);
        } else if (TestSessionTypeIsCategory)
        {
            TestedCategory = Sl.R<CategoryRepository>().GetById(TestSession.TestSessionTypeTypeId);
            LinkForRepeatTest = Links.TestSessionStartForCategory(TestSession.TestSessionTypeTypeId);
        }
        else
        {
            throw new Exception("TestSessionType is not defined.");
        }

        Answers = Sl.R<AnswerRepo>().GetByQuestionViewGuids(TestSession.AnsweredQuestionsQuestionViewGuid, true);
        if (Answers.Count != TestSession.AnsweredQuestionsQuestionViewGuid.Count)
            throw new Exception("There should be an equal number of answers and questions asked in a test session (no duplicate/second answers for same question asked).");

        NumberQuestions = Answers.Count();
        NumberCorrectAnswers = Answers.Count(a => a.AnsweredCorrectly());
        NumberWrongAnswers = Answers.Count(a => !a.AnsweredCorrectly());
        NumberCorrectPercentage = (int)Math.Round(NumberCorrectAnswers / (float)NumberQuestions * 100);
        NumberWrongAnswersPercentage = (int)Math.Round(NumberWrongAnswers / (float)NumberQuestions * 100);

        PercentageAverageRightAnswers = (int)Math.Round(Answers.Sum(a => a.Question.CorrectnessProbability) / (float)NumberQuestions);
        MeBetterThanAverage = NumberCorrectPercentage > PercentageAverageRightAnswers;

    }
}
