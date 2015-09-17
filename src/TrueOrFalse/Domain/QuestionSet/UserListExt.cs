using System.Collections.Generic;
using System.Linq;

public static class UserListExt
{
    public static User ById(this IEnumerable<User> users, int id)
    {
        return users.FirstOrDefault(user => user.Id == id);
    }
}