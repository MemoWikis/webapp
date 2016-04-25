public class CopyDate : IRegisterAsInstancePerLifetime
{
    public void Run(int dateId)
    {
        var dateRepo = Sl.R<DateRepo>();
        var date = dateRepo.GetById(dateId);

        ThrowIfNot_IsLoggedInUserOrAdmin.Run(date.User.Id);

        //dateRepo.Copy(date);
    }
}
