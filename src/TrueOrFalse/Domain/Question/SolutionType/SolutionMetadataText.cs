using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace TrueOrFalse
{
    public class SolutionMetadataText : SolutionMetadata
    {
        [JsonProperty("IsCaseSensitive")]
        public bool IsCaseSensitive;

        [JsonProperty("IsExtracInput")]
        public bool IsExactInput;

        protected override void InitFromJson(string json)
        {
            var tmp = JsonConvert.DeserializeObjectAsync<SolutionMetadataText>(json);
            IsCaseSensitive = tmp.Result.IsCaseSensitive;
            IsExactInput = tmp.Result.IsExactInput;
        }
    }
}