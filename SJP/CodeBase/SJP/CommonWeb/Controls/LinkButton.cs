// *********************************************** 
// NAME             : LinkButton.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 14 Apr 2011
// DESCRIPTION  	: Custom button control to add mouseoverclass to button 
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace SJP.Common.Web
{
    /// <summary>
    /// Custom button control to add mouseoverclass to button 
    /// </summary>
    public class LinkButton : Button
    {
        #region Private Fields
        private string mouseoverClass = string.Empty;
        #endregion

        #region Public Properties
        public string MouseOverClass
        {
            set
            {
                mouseoverClass = value;
            }
        }
        #endregion

        #region Overridden Event Handlers
        /// <summary>
        /// Overridden OnPreRender event handler to add logic to change mouseover class on mouseover and mouse out.
        /// Also, adds same for onfocus and onblur html input element events
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
           this.Attributes["onfocus"] = string.Format("this.className= this.className + ' {0}'", mouseoverClass);
           this.Attributes["onmouseover"] = string.Format("this.className= this.className + ' {0}'", mouseoverClass);
           this.Attributes["onmouseout"] = string.Format("this.className= '{0}'", this.CssClass);
           this.Attributes["onblur"] = string.Format("this.className= '{0}'", this.CssClass);
           
            base.OnPreRender(e);
        }
        #endregion
    }
}