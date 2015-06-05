using System;
using System.Collections.Generic;
using TrueOrFalse.Web;

public class EditDateModel : BaseModel
{
    public bool IsEditing;
    public UIMessage Message;

    public string Details { get; set; }

    public DateTime Date { get; set; }
    public string Time { get; set; }

    public IList<Set> Sets { get; set; }

    public EditDateModel()
    {
        Date = DateTime.Now.Date.AddDays(3);
        Time = "19:00";
        Sets = new List<Set>();
    }
}