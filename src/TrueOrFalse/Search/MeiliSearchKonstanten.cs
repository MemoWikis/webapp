using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse.Search
{
    internal static class MeiliSearchKonstanten
    {
        public const string Categories = "Categories";
        public static readonly string Url = Settings.MeiliSearchUrl;
        public static readonly string MasterKey = Settings.MeiliSearcMasterKey; 
    }
}
