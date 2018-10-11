public class GetCategoriesCount
{
    public static int All() => 
        (int)Sl.Session
            .CreateQuery("SELECT Count(Id) FROM Category")
            .UniqueResult<long>();

    public static int Wish(int userId) => 
        (int)Sl.Session.CreateQuery(
            "SELECT count(distinct cv.Id) FROM CategoryValuation cv " +
            "WHERE UserId = " + userId +
            "AND RelevancePersonal > -1 ")
        .UniqueResult<long>();
}