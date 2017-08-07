using System;

namespace Seedworks.Lib.Persistence
{
    public interface WithDateCreated
    {
        DateTime DateCreated { get; set; }
    }
}