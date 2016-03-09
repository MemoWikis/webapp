using System.ComponentModel;
using System.Web.Mvc;
using TrueOrFalse.Web;

public class MessagesModel : BaseModel
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
