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
        public string Description_Raw;
        public string Description;

        public string AuthorName_Raw;
        public string AuthorName;

        public Template InfoTemplate;

        public bool LicenseIsPublicDomain;
        public bool LicenseIsCreativeCommons;

        /// <summary>http://en.wikipedia.org/wiki/GNU_Free_Documentation_License</summary>
        public bool LicenseIsGFDL;

        /// <summary>
        /// e.g.: {{PD-USGov-Military-Air Force}}
        /// </summary>
        public string LicenseTemplateString;

        public bool IsUnknown(){ 
            return !LicenseIsPublicDomain && !LicenseIsCreativeCommons && !LicenseIsGFDL;
        }

        public bool HasDescription(){ return !String.IsNullOrEmpty(Description_Raw); }
        public bool HasAuthorname() { return !String.IsNullOrEmpty(AuthorName); }
    }
}
