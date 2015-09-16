public class DeleteDate : IRegisterAsInstancePerLifetime
{
    public void Run(int dateId)
    {
        var dateRepo = Sl.R<DateRepo>();
        var date = dateRepo.GetById(dateId);

        ThrowIfNot_IsLoggedInUserOrAdmin.Run(date.User.Id);

        dateRepo.Delete(date);
    }
}