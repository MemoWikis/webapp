using System.Collections.Generic;

public class SponsorModel : BaseResolve
{
    public bool IsAdFree;
    public Sponsor Sponsor = SponsorRepo.GetSponsor();

    public SponsorModel()
    {
        IsAdFree = !Settings.AdvertisementTurnedOn || (SessionUser.User != null && SessionUser.User.IsMember());
    }

    public SponsorModel(Sponsor sponsor) : this()
    {
        Sponsor = sponsor;
    }
}