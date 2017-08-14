using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Seedworks.Lib.Web
{
    public class UriUtils
    {
        public static List<string> AllTopLevelDomains = new List<string> { "AC", "AD", "AE", "AERO", "AF", "AG", "AI", "AL", "AM", "AN", "AO", "AQ", "AR", "ARPA", "AS", "ASIA", "AT", "AU", "AW", "AX", "AZ", "BA", "BB", "BD", "BE", "BF", "BG", "BH", "BI", "BIZ", "BJ", "BM", "BN", "BO", "BR", "BS", "BT", "BV", "BW", "BY", "BZ", "CA", "CAT", "CC", "CD", "CF", "CG", "CH", "CI", "CK", "CL", "CM", "CN", "CO", "COM", "COOP", "CR", "CU", "CV", "CX", "CY", "CZ", "DE", "DJ", "DK", "DM", "DO", "DZ", "EC", "EDU", "EE", "EG", "ER", "ES", "ET", "EU", "FI", "FJ", "FK", "FM", "FO", "FR", "GA", "GB", "GD", "GE", "GF", "GG", "GH", "GI", "GL", "GM", "GN", "GOV", "GP", "GQ", "GR", "GS", "GT", "GU", "GW", "GY", "HK", "HM", "HN", "HR", "HT", "HU", "ID", "IE", "IL", "IM", "IN", "INFO", "INT", "IO", "IQ", "IR", "IS", "IT", "JE", "JM", "JO", "JOBS", "JP", "KE", "KG", "KH", "KI", "KM", "KN", "KP", "KR", "KW", "KY", "KZ", "LA", "LB", "LC", "LI", "LK", "LR", "LS", "LT", "LU", "LV", "LY", "MA", "MC", "MD", "ME", "MG", "MH", "MIL", "MK", "ML", "MM", "MN", "MO", "MOBI", "MP", "MQ", "MR", "MS", "MT", "MU", "MUSEUM", "MV", "MW", "MX", "MY", "MZ", "NA", "NAME", "NC", "NE", "NET", "NF", "NG", "NI", "NL", "NO", "NP", "NR", "NU", "NZ", "OM", "ORG", "PA", "PE", "PF", "PG", "PH", "PK", "PL", "PM", "PN", "PR", "PRO", "PS", "PT", "PW", "PY", "QA", "RE", "RO", "RS", "RU", "RW", "SA", "SB", "SC", "SD", "SE", "SG", "SH", "SI", "SJ", "SK", "SL", "SM", "SN", "SO", "SR", "ST", "SU", "SV", "SY", "SZ", "TC", "TD", "TEL", "TF", "TG", "TH", "TJ", "TK", "TL", "TM", "TN", "TO", "TP", "TR", "TRAVEL", "TT", "TV", "TW", "TZ", "UA", "UG", "UK", "US", "UY", "UZ", "VA", "VC", "VE", "VG", "VI", "VN", "VU", "WF", "WS", "YE", "YT", "YU", "ZA", "ZM", "ZW" };

        /// <summary>
        /// Gets the filename + extension of a given path, 
        /// </summary>
        /// <remarks>
        /// The difference to: System.IO.Path.GetFileName(filePath) is the removal of the query string.
        /// </remarks>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileName(string filePath)
        {
            string[] fileParts = filePath.Split('/');
            string fileName = fileParts[fileParts.Length - 1];

            fileName = RemoveQueryString(fileName);

            return fileName;
        }

        /// <summary>
        /// Removes a web query string, from a given path.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string RemoveQueryString(string fileName)
        {
            int indexOfQuestion = fileName.IndexOf("?");

            if (indexOfQuestion != -1)
                fileName = fileName.Remove(fileName.IndexOf("?"));
            return fileName;
        }
		
        public static string GetQueryString(string requestPath)
        {
            if (!requestPath.Contains('?'))
                return "";

            return requestPath.Split('?')[1];
        }

        public static bool IsRelativePath(string url)
        {
            return !string.IsNullOrEmpty(url) && url.StartsWith("/");
        }
		
        public static NameValueCollection GetNameValueCollectionFromQuery(string query)
        {
            if(query.Trim().Length == 0)
                return new NameValueCollection();
            
            var result = new NameValueCollection();

            query = query.Substring(1);

            foreach(var nameValue in query.Split('&'))
            {
                var pairNameValue = nameValue.Split('=');
                result.Add(pairNameValue[0], pairNameValue[1]);
            }

            return result;
		}

		/// <summary>
		/// Returns &lt;www.domain.tld> without any path or query component.
		/// </summary>
		/// <param name="uri"></param>
		/// <returns></returns>
		public static string GetBaseUrl(Uri uri)
		{
			return string.Format("{0}://{1}", uri.Scheme, uri.Host);
		}

        public static string FirstSubdomainNotWww(Uri uri)
        {
            return FirstSubdomainNotWww(uri.Host);
        }

        /// <summary>
        /// Returns the first subdomain under the domain that is not www.<br/>
        /// sub.pl.teamaton.com -> pl<br/>
        /// www.teamaton.com -> string.Empty.
        /// </summary>
        /// <param name="uri">Uri.Host</param>
        /// <returns></returns>
        public static string FirstSubdomainNotWww(string uriHost)
        {
            if (string.IsNullOrEmpty(uriHost))
                return string.Empty;

            uriHost = RemoveTopLevelDomain(uriHost);

            if (!uriHost.Contains("."))
                return string.Empty;

            // remove leading www.
            if (uriHost.StartsWith("www."))
                uriHost = uriHost.Substring("www.".Length);

            var parts = uriHost.Split('.');

            if (parts.Count() >= 2)
                return parts[parts.Count() - 2];

            return string.Empty;
        }

        public static string RemoveTopLevelDomain(string host)
        {
            foreach (var topLevelDomain in AllTopLevelDomains)
            {
                var suffix = "." + topLevelDomain;
                if (host.EndsWith(suffix, StringComparison.InvariantCultureIgnoreCase))
                    return host.Substring(0, host.Length - suffix.Length);
            }

            return host;
        }

        /// <summary>
        /// Returns the domain part of the string.<br/>
        /// sub.pl.camping.info -> camping.info
        /// camping.info        -> camping.info
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GetDomainPart(Uri uri)
        {
            var subdomains = GetSubdomain(uri);

            var tmp = uri.Host.Remove(0, subdomains.Length);
            
            return tmp.StartsWith(".") ? tmp.Remove(0, 1) : tmp;
        }

        /// <summary>
        /// Returns all the subdomains in front of rootDomain as a string.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GetSubdomain(Uri uri)
        {
            var host = uri.Host;

            if (string.IsNullOrEmpty(host))
                return string.Empty;

            host = RemoveTopLevelDomain(host);

            if (!host.Contains("."))
                return string.Empty;
            
            var subdomains = host.Remove(host.LastIndexOf("."));

            return subdomains;
        }

        /// <summary>
        /// http://www.foobar.com, de -> http://de.foobar.com
        /// http://de.www.foobar.com, en -> http://en.foobar.com
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="newSubdomain"></param>
        /// <returns></returns>
        public static Uri ReplaceLeftMostSubdomain(Uri uri, string newSubdomain)
        {
            var domain = GetDomainPart(uri);
            var subdomains = GetSubdomain(uri);

            if (string.IsNullOrEmpty(subdomains))
            {
                domain = newSubdomain + "." + domain;
            }
            else
            {
                var parts = subdomains.Split('.').ToList();

                parts[0] = newSubdomain;

                subdomains = string.Empty;

                parts.ForEach(part => subdomains += part + ".");

                domain = subdomains + domain;
            }

            // use the port if it's a non-standard one (other than 80)
            var port = uri.Authority == uri.Host ? string.Empty : (":" + uri.Port);

            return new Uri(uri.Scheme + "://" + domain + port + uri.PathAndQuery);
        }
    }
}