using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TrueOrFalse.Web;

public class MaintenanceToolsModel : BaseModel
{
    public UIMessage Message;

    [Range(0, 999999, ErrorMessage = "Must be a valid number")]
    [DisplayName("Concentration:")]
    public int TxtConcentrationLevel { get; set; }

    [Range(0, 999999, ErrorMessage = "Must be a valid number")]
    [DisplayName("User Id:")]
    public int TxtUserId { get; set; }
}
