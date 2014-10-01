<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DetailsSummaryControl.ascx.cs" Inherits="SJP.UserPortal.SJPWeb.Controls.DetailsSummaryControl" %>
<%@ Register src="~/Controls/DetailsLegsControl.ascx" tagname="LegsControl" tagprefix="uc1" %>
<%@ Register Namespace="SJP.UserPortal.SJPWeb.Controls" TagPrefix="uc1" assembly="sjp.userportal.sjpweb" %> 
<%@ Register Namespace="SJP.Common.Web" TagPrefix="uc2" assembly="sjp.common.web" %> 

<div class="journeySummary">
    
    <div class="header">
        <div class="journeyDirection">
             <asp:Label ID="journeyDirection" runat="server" />
        </div>
        <div class="journeyHeading">
            <asp:Label ID="journeyHeader" CssClass="journeyHeader" runat="server" />
            <br />
            <asp:Label ID="journeyDateTime" CssClass="journeyDateTime" runat="server" />
        </div>
        <div class="clearboth"></div>
    </div>
    <div class="clearboth"></div>
    <div class="summary">

        <div class="jssettings">
            <asp:HiddenField ID="returnJourney" runat="server" />
        </div>
        <div class="floatleft">
            <uc2:LinkButton ID="btnEarlierJourney" CssClass="linkButton linkButtonPink replanButton"  MouseOverClass="linkButtonMouseOver" runat="server" OnClick="BtnEarlierJourney_Click" />
        </div>
        <div class="floatright">
            <uc2:LinkButton ID="btnLaterJourney" CssClass="linkButton linkButtonPink replanButton"  MouseOverClass="linkButtonMouseOver" runat="server" OnClick="BtnLaterJourney_Click" />
        </div>
        <div class="clearboth"></div>

        <asp:Repeater ID="journeySummary" runat="server" 
            OnItemDataBound="JourneySummary_DataBound"
            OnItemCommand="JourneySummary_Command" EnableViewState="true">

            <HeaderTemplate>
                <table class="legSummary" cellpadding="0" cellspacing="0">
                    <thead>
                        <tr class="headerRow">
                            <th id="jsHeader_Transport" class="summaryCol transport" runat="server">
                                <asp:Label ID="hdrTransport" runat="server"  EnableViewState="true" />
                            </th>
                            <th id="jsHeader_Changes" class="summaryCol changes" runat="server">
                                <asp:Label ID="hdrChanges" runat="server"  EnableViewState="true" />
                            </th>
                            <th id="jsHeader_Leave" class="summaryCol leave" runat="server">
                                <asp:Label ID="hdrLeave" runat="server"  EnableViewState="true" />
                            </th>
                            <th id="jsHeader_Arrive" class="summaryCol arrive" runat="server">
                                <asp:Label ID="hdrArrive" runat="server"  EnableViewState="true" />
                            </th>
                            <th id="jsHeader_JourneyTime" class="summaryCol journeyTime" runat="server">
                                <asp:Label ID="hdrJourneyTime" runat="server"  EnableViewState="true" />
                            </th>
                            <th id="jsHeader_Select" class="summaryCol select" runat="server">
                                <asp:Label ID="hdrSelect" runat="server"  EnableViewState="true" />
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                        <tr class="summaryRow" id="summaryRow" runat="server">
                            <td class="summaryCol transport">
                                <div class="arrow_btn">
                                        <asp:ImageButton ID="selectJourney" CssClass="closed" runat="server" CommandName="Select" />
                                </div>
                                <div class="transportModes">
                                    <asp:Label ID="lblTransport" runat="server" EnableViewState="true" />   
                                </div>
                            </td>
                            <td class="summaryCol changes">
                                <asp:Label ID="changes" runat="server"  EnableViewState="true"  />
                            </td>
                            <td class="summaryCol leave">
                                <asp:Label ID="leave" runat="server"  EnableViewState="true"  />
                            </td>
                            <td class="summaryCol arrive">
                                <asp:Label ID="arrive" runat="server"  EnableViewState="true"  />
                            </td>
                            <td class="summaryCol journeyTime">
                                <asp:Label ID="journeyTime" runat="server"  EnableViewState="true"  />
                            </td>
                            <td class="summaryCol select">
                                <uc1:GroupRadioButton ID="select" runat="server" AutoPostBack="false" OnCheckedChanged="DetailSummary_Selected" />
                               
                            </td>
                        </tr>
                        <tr class="detailsRow collapsed" id="detailsRow" runat="server" >
                            <td colspan="6">
                                <div class="rowDetail">
                                    <div class="jssettings">
                                        <asp:HiddenField ID="journeyRetailers" runat="server" />
                                        <asp:HiddenField ID="journeyId" runat="server" />
                                    </div>
                                    <div class="close">
                                        <uc2:LinkButton ID="btnClose" CssClass="linkButton"  MouseOverClass="linkButtonMouseOver" runat="server" OnClick="BtnClose_Click" Text="Close" />
                                    </div>
                                    <uc1:LegsControl ID="legsDetails" runat="server" />
                                </div>
                            </td>
                        </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                        <tr class="summaryRow odd" id="summaryRow" runat="server">
                            <td class="summaryCol transport">
                                <div class="arrow_btn">
                                      <asp:ImageButton ID="selectJourney" runat="server" CommandName="Select" />
                                </div>
                                <div class="transportModes">
                                    <asp:Label ID="lblTransport" runat="server" Visible="false" EnableViewState="true" />
                                </div>
                            </td>
                            <td class="summaryCol changes">
                                <asp:Label ID="changes" runat="server"  EnableViewState="true"  />
                            </td>
                            <td class="summaryCol leave">
                                <asp:Label ID="leave" runat="server"  EnableViewState="true"  />
                            </td>
                            <td class="summaryCol arrive">
                                <asp:Label ID="arrive" runat="server" EnableViewState="true"  />
                            </td>
                            <td class="summaryCol journeyTime">
                                <asp:Label ID="journeyTime" runat="server" EnableViewState="true"  />
                            </td>
                            <td class="summaryCol select">
                                <uc1:GroupRadioButton ID="select" runat="server"  AutoPostBack="false" OnCheckedChanged="DetailSummary_Selected" />
                               
                            </td>
                        </tr>
                            <tr class="detailsRow collapsed" id="detailsRow" runat="server">
                            <td colspan="6">
                                <div class="rowDetail">
                                    <div class="jssettings">
                                        <asp:HiddenField ID="journeyRetailers" runat="server" />
                                        <asp:HiddenField ID="journeyId" runat="server" />
                                    </div>
                                    <div class="close">
                                        <uc2:LinkButton ID="btnClose" CssClass="linkButton" MouseOverClass="linkButtonMouseOver" runat="server" OnClick="BtnClose_Click" Text="Close" />
                                    </div>
                                    <uc1:LegsControl ID="legsDetails" runat="server" />
                                </div>
                            </td>
                        </tr>

            </AlternatingItemTemplate>
            <FooterTemplate>
                    </tbody>
                </table>
            </FooterTemplate>

        </asp:Repeater>
                
        <div class="clearboth"></div>

    </div>
    
</div>