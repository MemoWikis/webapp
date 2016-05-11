public class CopyDate : IRegisterAsInstancePerLifetime
{
    public int Run(int sourceDateId, int userId)
    {
        var sourceDateRepo = Sl.R<DateRepo>();
        var sourceDate = sourceDateRepo.GetById(sourceDateId);

        ThrowIfNot_IsLoggedInUserOrAdmin.Run(userId);

        return sourceDateRepo.Copy(sourceDate);
    }
}
