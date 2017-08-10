public class TestSetWidgetModel : BaseModel
{
    public int SetId;
    public string Title;
    public string Text;

    public TestSetWidgetModel(int setId, string title = null, string text = null)
    {
        SetId = setId;
        Title = title;
        Text = text;
    }

}
