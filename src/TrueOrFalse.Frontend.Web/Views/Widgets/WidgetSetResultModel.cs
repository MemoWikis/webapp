public class WidgetSetResultModel : BaseModel
{
    public TestSessionResultModel TestSessionResultModel;

    public WidgetSetResultModel(TestSessionResultModel testSession)
    {
        TestSessionResultModel = testSession;

        ShowUserReportWidget = false;
    }
}
