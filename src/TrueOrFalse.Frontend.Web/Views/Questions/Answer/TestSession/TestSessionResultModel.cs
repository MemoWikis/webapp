using System;
using System.Collections.Generic;
using System.Linq;
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
    public IList<TestSessionStep> Steps;

    public Set TestedSet;
    public IList<Set> TestedSets;
    public Category TestedCategory;
    public string LinkForRepeatTest;

    public ContentRecommendationResult ContentRecommendationResult;

    public TestSessionResultModel(TestSession testSession)
    {
        TestSession = testSession;

        if (TestSession.IsSetSession)
        {
            TestedSet = Sl.R<SetRepo>().GetById(TestSession.SetToTestId);
            LinkForRepeatTest = Links.TestSessionStartForSet(TestedSet.Name, TestedSet.Id);
            ContentRecommendationResult = ContentRecommendation.GetForSet(TestedSet, 6);
        }
        else if (TestSession.IsSetsSession)
        {
            TestedSets = R<SetRepo>().GetByIds(TestSession.SetsToTestIds.ToList());
            LinkForRepeatTest = Links.TestSessionStartForSets(testSession.SetsToTestIds.ToList(), testSession.SetListTitle);
            ContentRecommendationResult = ContentRecommendation.GetForSet(TestedSets.FirstOrDefault(), 6);
        }
        else if (TestSession.IsCategorySession)
        {
            TestedCategory = TestSession.CategoryToTest;
            LinkForRepeatTest = Links.TestSessionStartForCategory(TestedCategory.Name, TestedCategory.Id);
            ContentRecommendationResult = ContentRecommendation.GetForCategory(TestedCategory, 6);
        }
        else
        {
            throw new Exception("Type of TestSession is not defined.");
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
    }
}
