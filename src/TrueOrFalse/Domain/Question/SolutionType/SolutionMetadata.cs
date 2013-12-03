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

        [JsonProperty("IsDate")]
        public bool IsDate;
        [JsonProperty("IsNumber")]
        public bool IsNumber;
        [JsonProperty("IsText")]
        public bool IsText;

        protected virtual void InitFromJson(string json){
            var tmp = JsonConvert.DeserializeObjectAsync<SolutionMetadata>(json);
            IsDate = tmp.Result.IsDate;
            IsNumber = tmp.Result.IsNumber;
            IsText = tmp.Result.IsNumber;
        }
    }
}
