using TrueOrFalse.Web;

public class EditDateModel : BaseModel
{
    public bool IsEditing;
    public UIMessage Message;

    public string Details { get; set; }

    public EditDateModel()
    {
    }
}