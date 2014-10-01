// *********************************************** 
// NAME             : GroupDropDownList.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 13 Jun 2011
// DESCRIPTION  	: Extended drop down list control to enable grouping of the list items
// ************************************************

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SJP.Common.Web
{
    /// <summary>
    /// Extended drop down list control to enable grouping of the list items
    /// </summary>
    public class GroupDropDownList : DropDownList
    {
        #region Private Constants
        private const string OptionGroupTag = "optgroup";
        private const string OptionTag = "option";
        private const string OptionGroupAttr = "OptionGroup";
        #endregion

        #region Protected Overridden Methods
        /// <summary>
        /// Overridden RenderContents method to render custom HTML
        /// </summary>
        /// <param name="writer">HTML text writer object</param>
        protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
        {
            string currentOptionGroup;
            List<string> renderedOptionGroups = new List<string>();
            foreach (ListItem item in this.Items)
            {
                // if no option gropu specified render as normal
                if (item.Attributes[OptionGroupAttr] == null)
                {
                    RenderListItem(item, writer);
                }
                else // render the optGroup tag with label and render the list items in the option group
                {
                    currentOptionGroup = item.Attributes[OptionGroupAttr];
                    if (renderedOptionGroups.Contains(currentOptionGroup))
                    {
                        RenderListItem(item, writer);
                    }
                    else
                    {
                        if (renderedOptionGroups.Count > 0)
                        {
                            RenderOptionGroupEndTag(writer);
                        }
                        RenderOptionGroupBeginTag(currentOptionGroup,
                                                  writer);
                        renderedOptionGroups.Add(currentOptionGroup);
                        RenderListItem(item, writer);
                    }
                }
            }
            // Render end tag
            if (renderedOptionGroups.Count > 0)
            {
                RenderOptionGroupEndTag(writer);
            }
        }

        

        /// <summary>
        /// Overridden method to save view state
        /// </summary>
        /// <returns></returns>
        protected override object SaveViewState()
        {
            object[] state = new object[this.Items.Count + 1];
            object baseState = base.SaveViewState();
            state[0] = baseState;
            bool itemHasAttributes = false;

            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i].Attributes.Count > 0)
                {
                    itemHasAttributes = true;
                    object[] attributes = new object[this.Items[i].Attributes.Count * 2];
                    int k = 0;

                    foreach (string key in this.Items[i].Attributes.Keys)
                    {
                        attributes[k] = key;
                        k++;
                        attributes[k] = this.Items[i].Attributes[key];
                        k++;
                    }
                    state[i + 1] = attributes;
                }
            }

            if (itemHasAttributes)
                return state;
            return baseState;
        }

        /// <summary>
        /// Overridden method to load view state
        /// </summary>
        /// <returns></returns>
        protected override void LoadViewState(object savedState)
        {
            if (savedState == null)
                return;

            if (!(savedState.GetType().GetElementType() == null) &&
                (savedState.GetType().GetElementType().Equals(typeof(object))))
            {
                object[] state = (object[])savedState;
                base.LoadViewState(state[0]);

                for (int i = 1; i < state.Length; i++)
                {
                    if (state[i] != null)
                    {
                        object[] attributes = (object[])state[i];
                        for (int k = 0; k < attributes.Length; k += 2)
                        {
                            this.Items[i - 1].Attributes.Add
                                (attributes[k].ToString(), attributes[k + 1].ToString());
                        }
                    }
                }
            }
            else
            {
                base.LoadViewState(savedState);
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Renders option group bigining tag
        /// </summary>
        /// <param name="name"></param>
        /// <param name="writer"></param>
        private void RenderOptionGroupBeginTag(string name,
                 HtmlTextWriter writer)
        {
            writer.WriteBeginTag("optgroup");
            writer.WriteAttribute("label", name);
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.WriteLine();
        }


        /// <summary>
        /// Renders option group end tag
        /// </summary>
        /// <param name="writer"></param>
        private void RenderOptionGroupEndTag(HtmlTextWriter writer)
        {
            writer.WriteEndTag("optgroup");
            writer.WriteLine();
        }

        /// <summary>
        /// Render List item
        /// </summary>
        /// <param name="item">List item to render</param>
        /// <param name="writer"></param>
        private void RenderListItem(ListItem item,
                     HtmlTextWriter writer)
        {
            writer.WriteBeginTag("option");
            writer.WriteAttribute("value", item.Value, true);
            if (item.Selected)
            {
                writer.WriteAttribute("selected", "selected", false);
            }
            foreach (string key in item.Attributes.Keys)
            {
                if (!key.Equals(OptionGroupAttr, StringComparison.InvariantCultureIgnoreCase))
                {
                    writer.WriteAttribute(key, item.Attributes[key]);
                }
            }
            writer.Write(HtmlTextWriter.TagRightChar);
            HttpUtility.HtmlEncode(item.Text, writer);
            writer.WriteEndTag("option");
            writer.WriteLine();
        }
        #endregion
    }
}