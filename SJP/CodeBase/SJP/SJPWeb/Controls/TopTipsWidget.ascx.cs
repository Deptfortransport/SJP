// *********************************************** 
// NAME             : TopTipsWidget.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 29 May 2011
// DESCRIPTION  	: Represents the TopTips widget on right hand side bar
// ************************************************


using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using SJP.Common.Web;
using SJP.UserPortal.JourneyControl;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// Represents the TopTips widget on right hand side bar
    /// </summary>
    public partial class TopTipsWidget : System.Web.UI.UserControl
    {
        #region Private Fields
        private SJPPage page = null;
        private int currentVisibleItemIndex = 0;
        #endregion


        #region Page_Init, Page_Load, Page_PreRender
        /// <summary>
        /// Page_PreRender Event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            page = (SJPPage)Page;
            SetupResources();
            LoadTips();
            
        }

        #endregion

        #region Event Control Hanlders
        /// <summary>
        /// TopTips repeater item databound event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TopTips_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem
                || e.Item.ItemType == ListItemType.SelectedItem)
            {
                if (e.Item.ItemIndex == currentVisibleItemIndex)
                {
                    HtmlGenericControl tipContainer = e.Item.FindControlRecursive<HtmlGenericControl>("tipContainer");
                    tipContainer.Style[HtmlTextWriterStyle.Display] = "block";
                }
            }
        }

        /// <summary>
        /// Previous button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PrevButton_Click(object sender, EventArgs e)
        {
            currentVisibleItemIndex = currentTopTip.Value.Parse(0);

            if (currentVisibleItemIndex == 0)
            {
                currentVisibleItemIndex = topTips.Items.Count - 1;
            }
            else
            {
                currentVisibleItemIndex--;
            }

        }

        /// <summary>
        /// Next button event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void NextButton_Click(object sender, EventArgs e)
        {
            currentVisibleItemIndex = currentTopTip.Value.Parse(0);

            if (currentVisibleItemIndex == topTips.Items.Count - 1)
            {
                currentVisibleItemIndex = 0;
            }
            else
            {
                currentVisibleItemIndex++ ;
            }
           
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets up Resource content
        /// </summary>
        private void SetupResources()
        {
            topTipsHeading.Text = page.GetResource("TopTipsWidget.TopTipsHeading.Text");
            navigationPrev.Text = page.GetResource("TopTipsWidget.navigationPrev.Text");
            navigationNext.Text = page.GetResource("TopTipsWidget.navigationNext.Text");
            prevButton.ImageUrl = page.ImagePath + page.GetResource("TopTipsWidget.PrevButton.ImageUrl");
            nextButton.ImageUrl = page.ImagePath + page.GetResource("TopTipsWidget.NextButton.ImageUrl");
            prevButton.AlternateText = prevButton.ToolTip = page.GetResource("TopTipsWidget.PrevButton.AlternateText");
            nextButton.AlternateText = nextButton.ToolTip = page.GetResource("TopTipsWidget.NextButton.AlternateText");
            
        }

        /// <summary>
        /// Binds top tips from content database to the topTips repeater
        /// </summary>
        private void LoadTips()
        {
            List<string> tips = GetToolTips();

            if (this.Visible)
            {
                this.Visible = tips.Count != 0
                    && Properties.Current["Promos.TopTipsWidget.Visible"].Parse(false)
                    && Properties.Current[string.Format("Promos.TopTipsWidget.{0}.Visible", page.PageId)].Parse(true);
            }

            currentTipNo.Text = (currentVisibleItemIndex+1).ToString();
            currentTopTip.Value = currentVisibleItemIndex.ToString();

            if (this.Visible)
            {
                totalTipNo.Text = tips.Count.ToString();

                topTips.DataSource = tips;
                topTips.DataBind();

               
            }

        }

        /// <summary>
        /// Gets the top tips from the content database
        /// </summary>
        /// <returns></returns>
        private List<string> GetToolTips()
        {
            List<string> tips = new List<string>();

            string toptipIds = string.Empty;
            // Special case for the Journey locations
            if (page.PageId == Common.PageId.JourneyLocations)
            {
                SessionHelper sessionHelper = new SessionHelper();
                ISJPJourneyRequest journeyRequest = sessionHelper.GetSJPJourneyRequest();
                if (journeyRequest != null)
                {
                    // top tips defined by TopTipsWidget.<pageId>.<SJPPlannerMode>.Toptips
                    toptipIds = page.GetResource(string.Format("TopTipsWidget.{0}.{1}.Toptips", page.PageId, journeyRequest.PlannerMode));
                }
            }
            else
            {
                // top tips defined by TopTipsWidget.<pageId>.Toptips
                toptipIds = page.GetResource(string.Format("TopTipsWidget.{0}.Toptips", page.PageId));
            }

            if (string.IsNullOrEmpty(toptipIds))
            {
                // default top tips
                toptipIds = page.GetResource("TopTipsWidget.Toptips");
            }

            string[] tipIdArr = toptipIds.Split(new char[] {','});

            foreach (string id in tipIdArr)
            {
                tips.Add(page.GetResource(string.Format("TopTipsWidget.Toptips.{0}",id)));
            }

            return tips;
                
        }
        #endregion
    }
}
