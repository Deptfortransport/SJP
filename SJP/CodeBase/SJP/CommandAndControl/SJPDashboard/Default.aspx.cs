using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Xml;
using System.Web.DynamicData;

namespace CCDashboard
{
    public partial class _Default : System.Web.UI.Page
    {
        private bool filtered = false;

        protected void Page_Load(object sender, EventArgs e)
        {

            XmlDataSource1.DataFile = ConfigurationManager.AppSettings["SiteConfidenceStatusXmlFilePath"];
            string whereClause;

            whereClause = "";

            try
            {
                XmlDataSource1.DataBind();
                SiteStatusRepeater.DataBind();
                lblSiteConfidenceMessage.Visible = false;
                lblSiteConfidenceMessage.Text = "";
            }
            catch (System.IO.FileNotFoundException )
                {
                    SiteStatusRepeater.Visible = false;
                    lblSiteConfidenceMessage.Visible = true;
                    lblSiteConfidenceMessage.Text = "Site Confidence Status file not found - check SiteConfidenceStatusXmlFilePath is correct in web.config!";
                }

            using (MonitoringResultsDataContext dbContext = new MonitoringResultsDataContext())
            {
                if (txtMachineFilter.Text == "" && txtDescriptionFilter.Text == "" && txtCustomWhereClause.Text == "")
                {
                    filtered = false;
                    gvwLatestResults.DataSource = dbContext.GetLatestMonitoringResults().ToList();
                }
                else
                {
                    filtered = true;
                    if (txtCustomWhereClause.Text.Length > 0)
                        //fil;tering with a custom WHERE
                    {
                        if (txtCustomWhereClause.Text.StartsWith("WHERE") )
                        {
                            whereClause = txtCustomWhereClause.Text.Remove(0,5);
                        }
                        else
                        {
                            whereClause = txtCustomWhereClause.Text;
                        }
                    }
                    else
                    {
                        //Filtering with machine name or Description
                        if ((txtMachineFilter.Text.Length > 0) && (txtDescriptionFilter.Text.Length > 0))
                        {
                            whereClause = "SJP_Server Like '%" + txtMachineFilter.Text + "%' AND Description Like '%" + txtDescriptionFilter.Text + "%'";
                        }
                        else
                        {
                            if (txtMachineFilter.Text.Length > 0)
                            {
                                whereClause = "SJP_Server Like '%" + txtMachineFilter.Text + "%'";
                            }
                            if (txtDescriptionFilter.Text.Length > 0)
                            {
                                whereClause = "Description Like '%" + txtDescriptionFilter.Text + "%'";
                            }
                        }
                    }
                    gvwLatestResults.DataSource = dbContext.GetLatestMonitoringResultsFiltered(whereClause).ToList();
                }
            }
            gvwLatestResults.DataBind(); 


            System.Collections.IList visibleTables = MetaModel.Default.VisibleTables;
            if (visibleTables.Count == 0)
            {
                throw new InvalidOperationException("There are no accessible tables. Make sure that at least one data model is registered in Global.asax and scaffolding is enabled or implement custom pages.");
            }
            Menu1.DataSource = visibleTables;
            Menu1.DataBind();
        }

        private static bool OnlyLargerThanFive(string s)
        {
            return s.Length > 5;
        }


        protected void gvwLatestResults_DataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (filtered)
                {
                    if (((CCDashboard.GetLatestMonitoringResultsFilteredResult)(e.Row.DataItem)).IsInRed)
                    {
                        e.Row.BackColor = System.Drawing.Color.PaleVioletRed;
                        e.Row.Font.Bold = true;
                    }
                    else
                    {
                        e.Row.BackColor = System.Drawing.Color.PaleGreen;
                    }
                }
                else
                {
                    if (((CCDashboard.GetLatestMonitoringResultsResult)(e.Row.DataItem)).IsInRed)
                    {
                        e.Row.BackColor = System.Drawing.Color.PaleVioletRed;
                        e.Row.Font.Bold = true;
                    }
                    else
                    {
                        e.Row.BackColor = System.Drawing.Color.PaleGreen;
                    }
                }
            }
        }


        protected void gvwSiteConfStatus_DataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((CCDashboard.GetLatestMonitoringResultsResult)(e.Row.DataItem)).IsInRed)
                {
                    e.Row.BackColor = System.Drawing.Color.PaleVioletRed;
                    e.Row.Font.Bold = true;
                }
                else
                {
                    e.Row.BackColor = System.Drawing.Color.PaleGreen;
                }
            }
        }

        public void SiteStatusRepeater_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {

           if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (XPathBinder.Eval(e.Item.DataItem, ".", "") == "Green")
                {
                    foreach (Control c in e.Item.Controls)
                    {
                        if (c is System.Web.UI.HtmlControls.HtmlTable)
                        {
                            HtmlTable tbl = (HtmlTable)c;
                        }
                    }
                }
            }
        }

        protected void btnApplyFilter_Click(object sender, EventArgs e)
        {

        }

    }
}
