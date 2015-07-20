using System.Collections.Generic;
using System.Linq;

public class GetDatesInNetwork : IRegisterAsInstancePerLifetime
{
    public IList<Date> Run(int userId)
    {
        return Run(Sl.R<UserRepo>().GetById(userId));
    }

    public IList<Date> Run(User user)
    {
        return Sl.R<DateRepo>()
            .GetBy(
                user.NetworkIds().ToArray(), 
                onlyUpcoming : true
            );
    }
}