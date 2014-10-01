// *********************************************** 
// NAME             : WebExtenders.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: DiscriptionPlaceholder
// ************************************************
// 

using System.Web.UI;

namespace SJP.Common.Web
{
    /// <summary>
    /// Contains extension methods for the web UI controls
    /// </summary>
    public static class WebExtenders
    {
        /// <summary>
        /// Finds a control recursively throughout decendet containers
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parentControl"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T FindControlRecursive<T>(this Control parentControl, string id) where T : Control
        {

            T ctrl = default(T);



            if ((parentControl is T) && (parentControl.ID == id))

                return (T)parentControl;



            foreach (Control c in parentControl.Controls)
            {

                ctrl = c.FindControlRecursive<T>(id);



                if (ctrl != null)

                    break;

            }

            return ctrl;

        }

    }
}