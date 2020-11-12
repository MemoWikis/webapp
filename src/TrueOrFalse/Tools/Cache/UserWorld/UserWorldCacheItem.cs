using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse.Tools.Cache.UserWorld
{
   public class UserWorldCacheItem
    {
        public User User;
        public ConcurrentDictionary<int, Category> Categories; 

    }
}
