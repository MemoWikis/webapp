public class CopySet : IRegisterAsInstancePerLifetime
{
    public int Run(int sourceSetId, int userId)
    {
        var sourceSetRepo = Sl.R<SetRepo>();
        var sourceSet = sourceSetRepo.GetById(sourceSetId);

        ThrowIfNot_IsLoggedInUserOrAdmin.Run(userId);

        return sourceSetRepo.Copy(sourceSet);
    }
}
