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

        public bool LicenceIsPublicDomain;
        public bool LicenceIsCreativeCommons;

        /// <summary>http://en.wikipedia.org/wiki/GNU_Free_Documentation_License</summary>
        public bool LicenceIsGFDL;

        /// <summary>
        /// e.g.: {{PD-USGov-Military-Air Force}}
        /// </summary>
        public string LicenceTemplateString;

        public bool IsUnknown(){ 
            return !LicenceIsPublicDomain && !LicenceIsCreativeCommons && !LicenceIsGFDL;
        }

        public bool HasDescription(){ return !String.IsNullOrEmpty(DescriptionDE_Raw); }
        public bool HasAuthorname() { return !String.IsNullOrEmpty(AuthorName); }
    }
}
