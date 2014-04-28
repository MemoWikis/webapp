using System.ComponentModel;
using System.Web.Mvc;
using TrueOrFalse.Web;
using TrueOrFalse.Frontend.Web.Models;

public class MaintenanceMessagesModel : BaseModel
{
    public UIMessage Message;

    [DisplayName("Receiver Id")]
    public int TestMsgReceiverId { get; set; }
    
    [DisplayName("Subject")]
    public string TestMsgSubject { get; set; }

    [AllowHtml]
    [DisplayName("Body")]
    public string TestMsgBody { get; set; }
}
