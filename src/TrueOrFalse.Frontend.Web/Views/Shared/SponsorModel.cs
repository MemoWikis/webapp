using System.Collections.Generic;

public class SponsorModel : BaseResolve
{
    public bool IsAdFree;
    public Sponsor Sponsor = SponsorRepo.GetSponsor();

    public SponsorModel()
    {
        IsAdFree = !Settings.AdvertisementTurnedOn || (SessionUserLegacy.User != null && SessionUserLegacy.User.IsMember);
    }

    public SponsorModel(Sponsor sponsor) : this()
    {
        Sponsor = sponsor;
    }
}