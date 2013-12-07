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
        private string _orginalJson;
        public string Json
        {
            get { return JsonConvert.SerializeObject(this); }
            set
            {
                _orginalJson = value;
                InitFromJson(value);
            }
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

        public SolutionMetadataDate GetAsDate(){
            return new SolutionMetadataDate { Json = _orginalJson };
        }

        public SolutionMetadataNumber GetAsNumber(){
            return new SolutionMetadataNumber { Json = _orginalJson };
        }

        public SolutionMetadataText GetAsText(){
            return new SolutionMetadataText { Json = _orginalJson };
        }
    }
}