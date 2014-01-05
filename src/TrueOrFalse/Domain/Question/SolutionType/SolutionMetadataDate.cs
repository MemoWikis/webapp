using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace TrueOrFalse
{
    public enum DatePrecision
    {
        Day = 1,
        Month = 5,
        Year = 10,
        Decade = 15,
        Century = 20,
        Millenium = 25
    }

    public class SolutionMetadataDate : SolutionMetadata
    {
        public SolutionMetadataDate(){
            IsDate = true;
        }

        [JsonProperty("Precision")]
        public DatePrecision Precision;

        protected override void InitFromJson(string json)
        {
            var tmp = JsonConvert.DeserializeObjectAsync<SolutionMetadataDate>(json);
            Precision = tmp.Result.Precision;
        }
    }
}