<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="List.aspx.cs" Inherits="CCDashboard.List" %>

<%@ Register src="~/DynamicData/Content/GridViewPager.ascx" tagname="GridViewPager" tagprefix="asp" %>
<%@ Register src="~/DynamicData/Content/FilterUserControl.ascx" tagname="DynamicFilter" tagprefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />

    <br />
    <div class="back">
        <a id="A1" runat="server" href="~/"><img id="Img1" alt="Back to home page" runat="server" src="DynamicData/Content/Images/back.gif" />Back to home page</a>
    </div>
    <br />

    <h2><%= table.DisplayName%>
<%--        <asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/SiteConfidenceStatus.xml" XPath="EventStatus/Status">
       </asp:XmlDataSource>--%>
    </h2>

    <asp:ScriptManagerProxy runat="server" ID="ScriptManagerProxy1" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                HeaderText="List of validation errors" />
            <asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="GridView1" Display="None" />
            
<%--            <asp:Label ID="Label1" runat="server" Text="Site Confidence Status"></asp:Label>

                        <br />
            <br />
--%>
<%--            <asp:Repeater ID="Repeater" runat="server" DataSourceID="XmlDataSource1">
             <ItemTemplate>
                <Strong><%# XPath("@EventName")%></Strong>
                <%#XPath("@Duration")%>
                <%#XPath(".")%><br />
             </ItemTemplate>
            </asp:Repeater>--%>

            <br />
            <br />

            <asp:FilterRepeater ID="FilterRepeater" runat="server">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("DisplayName") %>' AssociatedControlID="DynamicFilter$DropDownList1" />
                    <asp:DynamicFilter runat="server" ID="DynamicFilter" OnSelectedIndexChanged="OnFilterSelectedIndexChanged" />
                </ItemTemplate>
                <FooterTemplate><br /><br /></FooterTemplate>
            </asp:FilterRepeater>

            <asp:GridView ID="GridView1" runat="server" DataSourceID="GridDataSource"
                AllowPaging="True" AllowSorting="True" CssClass="gridview" PageSize="50">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HyperLink ID="EditHyperLink" runat="server"
                                NavigateUrl='<%# table.GetActionPath(PageAction.Edit, GetDataItem()) %>'
                            Text="Edit" />&nbsp;<asp:LinkButton ID="DeleteLinkButton" runat="server" CommandName="Delete"
                                CausesValidation="false" Text="Delete"
                                OnClientClick='return confirm("Are you sure you want to delete this item?");'
                            />&nbsp;<asp:HyperLink ID="DetailsHyperLink" runat="server"
                                NavigateUrl='<%# table.GetActionPath(PageAction.Details, GetDataItem()) %>'
                                Text="Details" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <PagerStyle CssClass="footer"/>        
                <PagerTemplate>
                    <asp:GridViewPager runat="server" />
                </PagerTemplate>
                <EmptyDataTemplate>
                    There are currently no items in this table.
                </EmptyDataTemplate>
            </asp:GridView>

            <asp:LinqDataSource ID="GridDataSource" runat="server" EnableDelete="true">
                <WhereParameters>
                    <asp:DynamicControlParameter ControlID="FilterRepeater" />
                </WhereParameters>
            </asp:LinqDataSource>

            <br />

            <div class="bottomhyperlink">
                <asp:HyperLink ID="InsertHyperLink" runat="server"><img runat="server" src="~/DynamicData/Content/Images/plus.gif" alt="Insert new item" />Insert new item</asp:HyperLink>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
