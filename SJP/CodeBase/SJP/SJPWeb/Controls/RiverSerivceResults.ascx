<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RiverSerivceResults.ascx.cs" Inherits="SJP.UserPortal.SJPWeb.Controls.RiverSerivceResults" %>
<%@ Register Namespace="SJP.UserPortal.SJPWeb.Controls" TagPrefix="uc1" assembly="sjp.userportal.sjpweb" %> 
<%@ Register Namespace="SJP.Common.Web" TagPrefix="uc2" assembly="sjp.common.web" %> 

<div class="stopEventResult">
   
    <div class="header">
        <div class="journeyDirection">
             <asp:Label ID="stopEventDirection" runat="server" />
        </div>
        <div class="journeyHeading">
            <asp:Label ID="stopEventHeader" runat="server" EnableViewState="true" />
            <br />
            <asp:Label ID="stopEventDateTime" CssClass="journeyDateTime" runat="server" />
        </div>
        <div class="clearboth"></div>
    </div>
    <div class="clearboth"></div>
    <div class="summary">

        <div class="jssettings clearboth">
            <asp:HiddenField ID="returnJourney" runat="server" />
        </div>
        <div class="floatleft">
            <uc2:LinkButton ID="btnEarlierJourney" CssClass="linkButton linkButtonPink replanButton"  MouseOverClass="linkButtonMouseOver" runat="server" OnClick="BtnEarlierJourney_Click" Visible="false" />
        </div>
        <div class="floatright">
            <uc2:LinkButton ID="btnLaterJourney" CssClass="linkButton linkButtonPink replanButton"  MouseOverClass="linkButtonMouseOver" runat="server" OnClick="BtnLaterJourney_Click" Visible="false" />
        </div>
        <div class="clearboth"></div>

        <asp:Repeater ID="stopEventResult" runat="server" 
             OnItemDataBound="StopEventResult_DataBound"
             EnableViewState="true">

            <HeaderTemplate>
                <table class="stopEventResultTable">
                    <thead>
                        <tr class="headerRow">
                            <th id="srHeader_Select" class="summaryCol select" runat="server">
                                <asp:Label ID="hdrSelect" runat="server"  EnableViewState="true" />
                            </th>
                            <th id="srHeader_Departure" class="summaryCol departure" runat="server">
                                <asp:Label ID="hdrDeparture" runat="server"  EnableViewState="true" />
                            </th>
                            <th id="srHeader_Arrive" class="summaryCol arrive" runat="server">
                                <asp:Label ID="hdrArrive" runat="server"  EnableViewState="true" />
                            </th>
                            <th id="srHeader_info" class="summaryCol info" runat="server">
                                <asp:Label ID="hdrService" runat="server"  EnableViewState="true" />
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                        <tr class="summaryRow">
                            <td class="summaryCol select">
                                <asp:HiddenField ID="journeyId" runat="server" />                             
                                <uc1:GroupRadioButton ID="select" runat="server" AutoPostBack="true" OnCheckedChanged="StopEvent_SelectedIndexChanged" />
                            </td>
                            <td class="summaryCol departure">
                                <asp:Label ID="departure" runat="server"  EnableViewState="true"  />
                            </td>
                            <td class="summaryCol arrive">
                                <asp:Label ID="arrive" runat="server"  EnableViewState="true"  />
                            </td>
                            <td class="summaryCol info">
                                <asp:Label ID="service" runat="server"  EnableViewState="true"  />
                            </td>
                        </tr>
                        
            </ItemTemplate>
            <AlternatingItemTemplate>
                        <tr class="summaryRow odd">
                            <td class="summaryCol select">
                                <asp:HiddenField ID="journeyId" runat="server" /> 
                                 <uc1:GroupRadioButton ID="select" runat="server" AutoPostBack="true" OnCheckedChanged="StopEvent_SelectedIndexChanged" />
                            </td>
                            <td class="summaryCol departure">
                                <asp:Label ID="departure" runat="server"  EnableViewState="true"  />
                            </td>
                            <td class="summaryCol arrive">
                                <asp:Label ID="arrive" runat="server"  EnableViewState="true"  />
                            </td>
                            <td class="summaryCol info">
                                <asp:Label ID="service" runat="server"  EnableViewState="true"  />
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
