using TrueOrFalse.Frontend.Web.Code;

public class WidgetSetResultModel : WidgetBaseModel
{
    public TestSessionResultModel TestSessionResultModel;

    public string StartSessionUrl;

    public WidgetSetResultModel(TestSessionResultModel testSessionResultModel, string host, int questionCount) : base(host)
    {
        TestSessionResultModel = testSessionResultModel;
        TestSessionResultModel.IsInWidget = true;

        TestSessionResultModel.LinkForRepeatTest =
            Links.GetUrlHelper().Action("Set", "Widget", new {setId = testSessionResultModel.TestSession.SetToTestId});

        ShowUserReportWidget = false;

        StartSessionUrl = WidgetSetStartModel.GetStartTestSessionUrl(
            TestSessionResultModel.TestedSet.Id, 
            testSessionResultModel.TestSession.HideAddKnowledge, 
            host,
            questionCount
        );
    }
}
