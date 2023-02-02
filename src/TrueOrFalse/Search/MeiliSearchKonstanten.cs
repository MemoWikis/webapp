using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse.Search
{
    public static class MeiliSearchKonstanten
    {
        public const string Categories = "Categories";
        public const string Questions = "Questions";
        public const string Users = "Users";
        public static readonly string Url = Settings.MeiliSearchUrl;
        public static readonly string MasterKey = Settings.MeiliSearcMasterKey;
    }
}
