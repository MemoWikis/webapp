using System;

public class TestSetWidgetModel : BaseModel
{
    public Set Set;
    public string Title;
    public string Text;

    public TestSetWidgetModel(int setId, string title = null, string text = null)
    {
        Set = Sl.SetRepo.GetById(setId);
        if (Set == null)
            throw new Exception("Die angegebene Fragesatz-ID verweist nicht auf einen existierenden Fragesatz");
        Title = title;
        Text = text;
    }

}
