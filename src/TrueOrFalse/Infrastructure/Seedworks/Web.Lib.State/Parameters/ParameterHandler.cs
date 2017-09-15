using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seedworks.Web.State
{
    public class ParameterHandler
    {
        /// <summary>
        /// The name of the URL parameter to catch
        /// For eg: "domain.com?name=value" 
        /// </summary>
        public string Name;

        /// <summary>
        /// The description of what the Handler is supposed to, 
        /// do with the caught parameter.
        /// </summary>
        public string Description = string.Empty;

        public Action<string> Action;

        public bool AppliesOnlyLocal;
    }
}
