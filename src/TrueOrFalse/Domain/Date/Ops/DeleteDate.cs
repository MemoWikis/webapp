public class DeleteDate : IRegisterAsInstancePerLifetime
{
    public void Run(int dateId)
    {
        var dateRepo = Sl.R<DateRepo>();
        var date = dateRepo.GetById(dateId);

        ThrowIfNot_IsLoggedInUserOrAdmin.Run(date.User.Id);

        //Delete connected DB-entries
        Sl.R<UserActivityRepo>().DeleteForDate(dateId); //remove this if soft delete for dates is implemented

        dateRepo.Delete(date);
    }
}