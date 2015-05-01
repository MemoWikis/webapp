using System;
using System.Collections.Generic;
using Seedworks.Lib.Persistence;

public class Game : DomainEntity
{
    public virtual DateTime WillStartAt { get; set; }

    public virtual User Creator { get; set; }
    public virtual IList<User> Players { get; set; }

    public virtual IList<Set> Sets { get; set; }
    public virtual GameStatus Status { get; set; }

    public virtual string Comment { get; set; }
}
