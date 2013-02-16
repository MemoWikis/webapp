using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse
{
    public class SolutionMetadataDate : SolutionMetadata
    {

        public bool Day;
        public bool Month;
        public bool Year;
        public bool Century;

        protected override void InitFromJson(string json)
        {
            
        }
    }
}