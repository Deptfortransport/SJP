// *********************************************** 
// NAME             : URLHelper.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: URLHelper class containing convenience methods for URLs
// ************************************************
// 

using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace SJP.Common.Web
{
    /// <summary>
    /// URLHelper class
    /// </summary>
    public class URLHelper
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public URLHelper()
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds the specified query string name and value to the supplied URL
        /// </summary>
        /// <param name="url">String url to append to</param>
         /// <returns></returns>
        public string AddQueryStringPart(string url, string name, string value)
        {
            NameValueCollection nvc = new NameValueCollection(1);
            nvc.Add(name, value);

            return AddQueryStringParts(url, nvc);
        }

        /// <summary>
        /// Adds the specified query string name/values to the supplied URL
        /// </summary>
        /// <param name="url">String url to append to</param>
        /// <param name="queryValues">NameValueCollection containing the key names and values as strings</param>
        /// <returns></returns>
        public string AddQueryStringParts(string url, NameValueCollection queryValues)
        {
            string newUrl = url;

            if ((queryValues != null) && (queryValues.Count > 0))
            {
                StringBuilder newUrlSB = new StringBuilder();

                newUrlSB.Append(url);

                if (url.Contains("?"))
                    newUrlSB.Append("&");
                else
                    newUrlSB.Append('?');

                foreach (string key in queryValues.AllKeys)
                {
                    newUrlSB.Append(key);
                    newUrlSB.Append('=');
                    newUrlSB.Append(HttpContext.Current.Server.UrlEncode(queryValues[key]));
                    newUrlSB.Append('&');
                }

                newUrl = newUrlSB.ToString().TrimEnd('&');
            }

            return newUrl;
        }

        /// <summary>
        /// Removes the specified query string name and value from the supplied URL, 
        /// cleaning the ? and & values where required
        /// </summary>
        /// <returns></returns>
        public string RemoveQueryStringPart(string url, string queryKey)
        {
            string newUrl = url;

            // Only remove if query string key supplied
            if (!string.IsNullOrEmpty(queryKey))
            {
                StringBuilder newUrlSB = new StringBuilder();

                // Get the url path and query sections
                string[] urlParts = url.Split(new char[] { '?' });
                               
                // Keep path as it is
                newUrlSB.Append(urlParts[0]);

                // Where the query part exist
                if (urlParts.Length > 1)
                {
                    newUrlSB.Append('?');

                    // Get the individual query parts
                    string[] urlQueryParts = urlParts[1].Split(new char[] {'&'});

                    foreach (string urlQueryPart in urlQueryParts)
                    {
                        string[] queryPart = urlQueryPart.Split(new char[] {'='});

                        if (queryPart.Length > 0)
                        {
                            // Check if the query part is ok to be retained and add to the new url
                            if (!queryPart[0].Equals(queryKey, StringComparison.InvariantCultureIgnoreCase))
                            {
                                newUrlSB.Append(urlQueryPart);
                                newUrlSB.Append('&');
                            }
                        }
                    }
                }

                newUrl = newUrlSB.ToString().TrimEnd('&').TrimEnd('?');
            }

            return newUrl;
        }

        #endregion
    }
}