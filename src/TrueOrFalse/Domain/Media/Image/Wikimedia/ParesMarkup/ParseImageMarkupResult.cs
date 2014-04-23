using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse.WikiMarkup
{
    public class ParseImageMarkupResult
    {
        /// <summary>May contain wiki markup</summary>
        public string DescriptionDE_Raw;
        public string Description;

        public string AuthorName_Raw;
        public string AuthorName;

        public Template InfoTemplate;

        public bool IsPublicDomain;
        public bool IsCreativeCommons;


        public bool IsUnknown(){ 
            return !IsPublicDomain && !IsCreativeCommons;
        }

        public bool HasDescription(){ return !String.IsNullOrEmpty(DescriptionDE_Raw); }
        public bool HasAuthorname() { return !String.IsNullOrEmpty(AuthorName); }
    }
}
