using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seedworks.Web.State
{
    public class ParameterHandlerList : List<ParameterHandler>
    {
        public bool Contains(string key)
        {
            if (key == null) return false;
            return GetByName(key) != null;
        }

        public ParameterHandler GetByName(string key)
        {
            return Find(handler => handler.Name.ToLower() == key.ToLower());
        }
    }
}
