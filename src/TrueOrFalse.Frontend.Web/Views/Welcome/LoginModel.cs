using System.ComponentModel;
using TrueOrFalse.Web;

public class LoginModel : BaseModel
{
    public UIMessage Message;

    public string Password { get; set; }

    [DisplayName("E-Mail")]
    public string EmailAddress { get; set; }

    public bool PersistentLogin { get; set; }
}
