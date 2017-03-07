using System.Collections.Generic;

public class SponsorModel : BaseResolve
{
    public bool IsAdFree;

    public string SponsorUrl = "http://www.tutory.de";
    public string ImageUrl = "/Images/Sponsors/tutory.png";
    public string LinkText = "Tutory";
    public string PresentationText = "Wir empfehlen ";

    public SponsorModel()
    {
        var userSession = Resolve<SessionUser>();

        if (userSession.User != null)
        {
            IsAdFree = userSession.User.IsMember();
        }
    }
}