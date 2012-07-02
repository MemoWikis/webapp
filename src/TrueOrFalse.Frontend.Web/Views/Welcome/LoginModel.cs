using System.ComponentModel;
using TrueOrFalse.Core.Web;
using TrueOrFalse.Frontend.Web.Models;

public class LoginModel : ModelBase
{
    public Message Message;

    public string Password;

    [DisplayName("Email")]
    public string EmailAddress { get; set; }

    public bool PersistentLogin { get; set; }

    public void SetToWrongCredentials()
    {
        Message = new Message(MessageType.IsError, 
            "Du konntest nicht angemeldet werden. Bitte überprüfe Deinen Benutzernamen und Passwort"); ;
    }
}
