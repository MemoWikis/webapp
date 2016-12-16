using System;

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
        public InfoBoxTemplate InfoBoxTemplate;

        public string AllRegisteredLicenses;

        public string Notifications;

        public bool HasDescription(){ return !String.IsNullOrEmpty(Description_Raw); }
        public bool HasAuthorname() { return !String.IsNullOrEmpty(AuthorName); }
    }
}
