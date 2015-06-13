using System.Collections.Generic;

public class SignalRUser
{
    public User User;
    public string Name { get; set; }
    public HashSet<string> ConnectionIds { get; set; }
}