public class CopyDate : IRegisterAsInstancePerLifetime
{
    public int Run(int sourceDateId)
    {
        var sourceDateRepo = Sl.R<DateRepo>();
        var sourceDate = sourceDateRepo.GetById(sourceDateId);

        ThrowIfNot_IsLoggedInUserOrAdmin.Run(sourceDate.User.Id);

        return sourceDateRepo.Copy(sourceDate);
    }
}
