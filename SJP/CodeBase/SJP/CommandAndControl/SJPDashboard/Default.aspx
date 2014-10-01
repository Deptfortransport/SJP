<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="Default.aspx.cs" Inherits="CCDashboard._Default" %>

<%@ Register src="~/DynamicData/Content/GridViewPager.ascx" tagname="GridViewPager" tagprefix="asp" %>
<%@ Register src="~/DynamicData/Content/FilterUserControl.ascx" tagname="DynamicFilter" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" />

    <asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/SiteConfidenceStatus.xml" XPath="EventStatus/Status">
    </asp:XmlDataSource>

    <h1>SJP Main Dashboard</h1>

    <br /><br />

    <h2>Site Confidence Status</h2>

    <asp:Repeater ID="SiteStatusRepeater" runat="server" DataSourceID="XmlDataSource1" OnItemDataBound="SiteStatusRepeater_ItemDataBound" >
        <ItemTemplate>
        <table bgcolor='<%#(bool)(XPathBinder.Eval(Container.DataItem, ".", "") == "Green") ? "PaleGreen" : "PaleVioletRed"%>' id="SiteStatusTable" runat="server">
            <tr runat="server" id="SiteStatusRow"  >
                <td bgcolor='<%#(bool)(XPathBinder.Eval(Container.DataItem, ".", "") == "Green") ? "PaleGreen" : "PaleVioletRed"%>'>
                    <strong><%# XPath("@EventName")%></strong>
                </td>
                <td bgcolor='<%#(bool)(XPathBinder.Eval(Container.DataItem, ".", "") == "Green") ? "PaleGreen" : "PaleVioletRed"%>'>
                        Duration: 
                </td>
                <td bgcolor='<%#(bool)(XPathBinder.Eval(Container.DataItem, ".", "") == "Green") ? "PaleGreen" : "PaleVioletRed"%>'>
                    <%#XPath("@Duration")%>
                </td>
                <td bgcolor='<%#(bool)(XPathBinder.Eval(Container.DataItem, ".", "") == "Green") ? "PaleGreen" : "PaleVioletRed"%>'>
                        TimeSubmitted: 
                </td>
                <td bgcolor='<%#(bool)(XPathBinder.Eval(Container.DataItem, ".", "") == "Green") ? "PaleGreen" : "PaleVioletRed"%>'>
                    <%#XPath("@TimeSubmitted")%>
                </td>
            </tr>
        </table>
        </ItemTemplate>
    </asp:Repeater>


    <asp:Label ID="lblSiteConfidenceMessage" runat="server" Text=""></asp:Label>

    <%# XPath("@EventName")%>

    <br /><br />

    <h2>Latest Monitoring Data - Summary</h2>
    <p>&nbsp;</p>
    <asp:Label ID="lblMachineFilter" runat="server" Text="Filter machinename contains:"></asp:Label>
    <asp:TextBox ID="txtMachineFilter" runat="server"></asp:TextBox>
    <asp:Label ID="lblDescriptionFilter" runat="server" Text="        Filter description contains:"></asp:Label>
    <asp:TextBox ID="txtDescriptionFilter" runat="server"></asp:TextBox>
    <br />
    <asp:Label ID="lblCustomWhereClause" runat="server" Text="Custom WHERE condition: WHERE..."></asp:Label>
    <asp:TextBox ID="txtCustomWhereClause" runat="server"></asp:TextBox>
    <br /><br />
    <asp:Button ID="btnApplyFilter" runat="server" Text="Apply Filter" 
        onclick="btnApplyFilter_Click" />
    <br /><br />

    <asp:GridView ID="gvwLatestResults" runat="server" OnRowDataBound="gvwLatestResults_DataBound"
        AllowPaging="False" AllowSorting="False" CssClass="gridview" >
        <PagerStyle CssClass="footer"/>        
        <PagerTemplate>
            <asp:GridViewPager runat="server" />
        </PagerTemplate>
        <EmptyDataTemplate>
            There are currently no latest results to show.
        </EmptyDataTemplate>
    </asp:GridView>

    <br /><br />

    <h2>Historical Monitoring Data - All Records</h2>

    <br /><br />

    <asp:GridView ID="Menu1" runat="server" AutoGenerateColumns="false"
        CssClass="gridview" AlternatingRowStyle-CssClass="even">
        <Columns>
            <asp:TemplateField HeaderText="Table Name" SortExpression="TableName">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%#Eval("ListActionPath") %>'><%#Eval("DisplayName") %></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>


