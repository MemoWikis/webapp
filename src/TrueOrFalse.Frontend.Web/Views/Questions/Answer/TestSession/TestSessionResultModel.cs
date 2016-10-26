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
    public int NumberOnlySolutionView;
    public int NumberCorrectPercentage;
    public int NumberWrongAnswersPercentage;
    public int NumberOnlySolutionViewPercentage;
    public int PercentageAverageRightAnswers;
    public bool MeBetterThanAverage;
    public bool TestSessionTypeIsSet;
    public bool TestSessionTypeIsCategory;
    //public IList<Answer> Answers;
    public IList<TestSessionStep> Steps;

    public Set TestedSet;
    public Category TestedCategory;
    public string LinkForRepeatTest;

    public TestSessionResultModel(TestSession testSession)
    {
        TestSession = testSession;

        TestSessionTypeIsSet = TestSession.TestSessionType == TestSessionType.Set;
        TestSessionTypeIsCategory = TestSession.TestSessionType == TestSessionType.Category;
        if (TestSessionTypeIsSet)
        {
            TestedSet = Sl.R<SetRepo>().GetById(TestSession.TestSessionTypeTypeId);
            LinkForRepeatTest = Links.TestSessionStartForSet(TestedSet.Name, TestedSet.Id);
        } else if (TestSessionTypeIsCategory)
        {
            TestedCategory = Sl.R<CategoryRepository>().GetById(TestSession.TestSessionTypeTypeId);
            LinkForRepeatTest = Links.TestSessionStartForCategory(TestedCategory.Name, TestedCategory.Id);
        }
        else
        {
            throw new Exception("TestSessionType is not defined.");
        }

        TestSession.FillUpStepProperties();
        Steps = TestSession.Steps.Where(s => s.AnswerState != TestSessionStepAnswerState.Uncompleted).ToList();

        NumberQuestions = Steps.Count(s => s.AnswerState != TestSessionStepAnswerState.Uncompleted);
        NumberCorrectAnswers = Steps.Count(s => s.AnswerState == TestSessionStepAnswerState.AnsweredCorrect);
        NumberWrongAnswers = Steps.Count(s => s.AnswerState == TestSessionStepAnswerState.AnsweredWrong);
        NumberOnlySolutionView = Steps.Count(s => s.AnswerState == TestSessionStepAnswerState.OnlyViewedSolution);
        NumberCorrectPercentage = (int)Math.Round(NumberCorrectAnswers / (float)NumberQuestions * 100);
        NumberWrongAnswersPercentage = (int)Math.Round(NumberWrongAnswers / (float)NumberQuestions * 100);
        NumberOnlySolutionViewPercentage = (int)Math.Round(NumberOnlySolutionView / (float)NumberQuestions * 100);

        PercentageAverageRightAnswers = (int)Math.Round(Steps.Sum(s => s.Question.CorrectnessProbability) / (float)NumberQuestions);
        MeBetterThanAverage = NumberCorrectPercentage > PercentageAverageRightAnswers;

    }
}
