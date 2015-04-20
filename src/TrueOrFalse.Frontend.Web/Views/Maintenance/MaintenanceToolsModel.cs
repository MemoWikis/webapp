using System.ComponentModel;
using System.Web.Mvc;
using TrueOrFalse.Web;

public class MaintenanceToolsModel : BaseModel
{
    public UIMessage Message;

    [DisplayName("Concentration level:")]
    public int TxtConcentrationLevel { get; set; }
}
