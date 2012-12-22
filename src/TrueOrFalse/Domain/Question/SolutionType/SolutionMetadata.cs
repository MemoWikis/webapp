using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace TrueOrFalse
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SolutionMetadata
    {
        public string Json
        {
            get { return JsonConvert.SerializeObject(this); }
            set { InitFromJson(value); }
        }

        protected virtual void InitFromJson(string json){
            throw new Exception("invalid use"); //NHibernate does not allow abstract classes
        }
    }
}
