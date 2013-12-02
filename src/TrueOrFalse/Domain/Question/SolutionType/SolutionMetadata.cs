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

        public bool IsDate;
        public bool IsNumber;
        public bool IsText;

        protected virtual void InitFromJson(string json){
            var tmp = JsonConvert.DeserializeObjectAsync<SolutionMetadata>(json);
            IsDate = tmp.Result.IsDate;
            IsNumber = tmp.Result.IsNumber;
            IsText = tmp.Result.IsNumber;
        }
    }
}
