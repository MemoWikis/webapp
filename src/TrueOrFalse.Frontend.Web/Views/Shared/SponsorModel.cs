using System.Collections.Generic;

public class SponsorModel : BaseResolve
{
    public bool IsAdFree;
    public Sponsor Sponsor = SponsorRepo.GetSponsor();

    public SponsorModel()
    {
        var userSession = Resolve<SessionUser>();

        if (userSession.User != null)
        {
            IsAdFree = userSession.User.IsMember();
        }
    }
}