// *********************************************** 
// NAME             : RemotePost.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 07 Apr 2011
// DESCRIPTION  	: RemotePost class to allow posting form data to an external site
// ************************************************
// 

using System.Collections.Specialized;

namespace SJP.UserPortal.SJPWeb
{
    /// <summary>
    /// RemotePost class to allow posting form data to an external site
    /// </summary>
    public class RemotePost
    {
        #region Private members

        private NameValueCollection inputs = new NameValueCollection();
        
        private string url = "";
        private string method = "post";
        private string formName = "form1";
        private string targetType = "_self";

        private string jsDisabledText = string.Empty;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public RemotePost()
        {
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read/Write. URL to send data to 
        /// </summary>
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        /// <summary>
        /// Read/Write. Method of sending data. 
        /// Default is set to "post"
        /// </summary>
        public string Method
        {
            get { return method; }
            set { method = value; }
        }
        
        /// <summary>
        /// Read/Write. Name of form submitting post
        /// </summary>
        public string FormName
        {
            get { return formName; }
            set { formName = value; }
        }

        /// <summary>
        /// Read/Write. Setting the target for the remote post
        /// Set to "_blank" to open the remote site in new window
        /// Default is "_self" which will open the remote posting site in same window
        /// </summary>
        public string TargetType
        {
            get { return targetType; }
            set { targetType = value; }
        }

        /// <summary>
        /// Read/Write. This is the text which will be displayed when javascript is disabled.
        /// </summary>
        public string JSDisabledText
        {
            get { return jsDisabledText; }
            set { jsDisabledText = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds input values to be included in the form post
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Add(string name, string value)
        {
            inputs.Add(name, value);
        }
                
        /// <summary>
        /// Initiates the Post to the url set by generating an html page sent to client 
        /// containg form data and automatically submiting on its load
        /// </summary>
        public void Post()
        {
            System.Web.HttpContext.Current.Response.Clear();

            System.Web.HttpContext.Current.Response.Write("<html><head><style>.button{background:#fff; border: none;cursor: pointer;cursor: hand;color: #0000EE;}.button:hover{color: #8800CC; background-color:  #FFFFFF;text-decoration: underline;}</style>");

            System.Web.HttpContext.Current.Response.Write(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));
            System.Web.HttpContext.Current.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" target=\"{3}\" >", FormName, Method, Url, targetType));
            for (int i = 0; i < inputs.Keys.Count; i++)
            {
                System.Web.HttpContext.Current.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", inputs.Keys[i], inputs[inputs.Keys[i]]));
            }
            System.Web.HttpContext.Current.Response.Write("<noscript>");
            System.Web.HttpContext.Current.Response.Write(string.Format("<input type=\"submit\" class=\"button\" value=\"{0}\" />", jsDisabledText));

            System.Web.HttpContext.Current.Response.Write("</noscript>");
            System.Web.HttpContext.Current.Response.Write("</form>");
            System.Web.HttpContext.Current.Response.Write("</body></html>");

            System.Web.HttpContext.Current.Response.End();
        }

        #endregion
    }
}