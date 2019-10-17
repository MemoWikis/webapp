using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TrueOrFalse.Web;

public class ToolsModel : BaseModel
{
    public UIMessage Message;

    [Range(0, 999999, ErrorMessage = "Must be a valid number")]
    [DisplayName("Concentration:")]
    public int TxtConcentrationLevel { get; set; }

    [Range(0, 999999, ErrorMessage = "Must be a valid number")]
    [DisplayName("User Id:")]
    public int TxtUserId { get; set; }

    public string SetsToAddCategoryToIds { get; set; }

    public int CategoryToAddId { get; set; }

    public string SetsToRemoveCategoryFromIds { get; set; }

    public int CategoryToRemoveId { get; set; }
    public int UserId { get; set; }
}
