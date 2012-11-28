using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace TrueOrFalse
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class SolutionMetadata
    {
        public string Json
        {
            get { return JsonConvert.SerializeObject(this); }
            set { InitFromJson(value); }
        }

        protected abstract void InitFromJson(string json);
    }
}
