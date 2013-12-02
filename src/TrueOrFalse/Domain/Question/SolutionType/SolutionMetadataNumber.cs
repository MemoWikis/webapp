using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace TrueOrFalse
{
    public class SolutionMetadataNumber : SolutionMetadata
    {
        [JsonProperty("Currency")]
        public string Currency;

        public SolutionMetadataNumber()
        {
            IsNumber = true;
        }

        protected override void InitFromJson(string json)
        {
            
        }
    }
}