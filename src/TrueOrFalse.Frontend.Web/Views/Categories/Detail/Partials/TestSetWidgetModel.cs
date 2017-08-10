public class TestSetWidgetModel : BaseModel
{
    public int SetId;
    public string Title;
    public string Text;

    public TestSetWidgetModel(int setId, string title, string text)
    {
        SetId = setId;
        Title = title;
        Text = text;
    }

}
