using TrueOrFalse.Web;

public class ToolsModel : BaseModel
{
    public UIMessage Message;

    public int CategoryToAddId { get; set; }

    public int CategoryToRemoveId { get; set; }
    public new int UserId { get; set; }
}
