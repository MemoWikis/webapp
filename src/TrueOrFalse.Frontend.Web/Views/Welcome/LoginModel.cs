using System.ComponentModel;
using TrueOrFalse.Web;

public class LoginModel : BaseModel
{
    public UIMessage Message;

    public string Password;

    [DisplayName("Email")]
    public string EmailAddress { get; set; }

    public bool PersistentLogin { get; set; }

    public void SetToWrongCredentials()
    {
        Message = new UIMessage(MessageType.IsError, 
            "Du konntest nicht angemeldet werden. Bitte überprüfe deinen Benutzernamen und Passwort"); ;
    }
}
