using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;

namespace SJP.Common.Web
{
    public class GroupRadioButtonList : RadioButtonList
    {
        #region Private members
        private Boolean firstColumn = true;
        #endregion

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
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
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
            firstColumn = true;
            writer.WriteBeginTag("fieldset");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.WriteBeginTag("legend");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write(name);
            writer.WriteEndTag("legend");
            writer.WriteLine();
        }


        /// <summary>
        /// Renders option group end tag
        /// </summary>
        /// <param name="writer"></param>
        private void RenderOptionGroupEndTag(HtmlTextWriter writer)
        {
            // adds a closing div tag if the number of options is uneven
            if (!firstColumn)
            {
                writer.WriteEndTag("div");
            }

            writer.WriteEndTag("fieldset");
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
            if (firstColumn)
            {
                writer.WriteBeginTag("div");
                writer.WriteAttribute("class", "ui-grid-a");
                writer.Write(HtmlTextWriter.TagRightChar);

                writer.WriteBeginTag("div");
                writer.WriteAttribute("class", "ui-block-a");
                writer.Write(HtmlTextWriter.TagRightChar);
            }
            else
            {
                writer.WriteBeginTag("div");
                writer.WriteAttribute("class", "ui-block-b");
                writer.Write(HtmlTextWriter.TagRightChar);
            }

            writer.WriteBeginTag("input");
            writer.WriteAttribute("type", "radio");
            writer.WriteAttribute("value", item.Text, true);
            writer.WriteAttribute("id", "id" + item.Value);
            writer.WriteAttribute("data-id", item.Value);
            writer.WriteAttribute("name", "locationSelector");
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

            writer.AddAttribute(HtmlTextWriterAttribute.For, "id" + item.Value);
            writer.RenderBeginTag(HtmlTextWriterTag.Label);
            writer.Write(item.Text);
            writer.RenderEndTag();

            writer.WriteEndTag("div");

            if (!firstColumn)
            {
                writer.WriteEndTag("div");
            }

            writer.WriteLine();

            firstColumn = !firstColumn;
        }
        #endregion
    }
}
