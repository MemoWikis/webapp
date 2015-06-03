using System;
using TrueOrFalse.Web;

public class EditDateModel : BaseModel
{
    public bool IsEditing;
    public UIMessage Message;

    public string Details { get; set; }

    public DateTime Date { get; set; }
    public string Time { get; set; }

    public EditDateModel()
    {
        Date = DateTime.Now.Date.AddDays(3);
        Time = "19:00";
    }
}