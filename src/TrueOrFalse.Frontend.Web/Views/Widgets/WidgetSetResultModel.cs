public class WidgetSetResultModel : BaseModel
{
    public TestSessionResultModel TestSessionResultModel;

    public WidgetSetResultModel(TestSessionResultModel testSession)
    {
        TestSessionResultModel = testSession;
        TestSessionResultModel.IsInWidget = true;

        ShowUserReportWidget = false;
    }
}
