<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DetailsSummaryControl.ascx.cs" Inherits="SJP.UserPortal.SJPMobile.Controls.DetailsSummaryControl" %>
<%@ Register Namespace="SJP.UserPortal.SJPMobile.Controls" TagPrefix="cc1" assembly="sjp.userportal.sjpmobile" %> 

<div class="journeySummary" aria-atomic="true" aria-live="assertive">
    <div class="summary jsshow">
        
        <div id="results">
        
            <div class="journeySummaryHead ui-grid-b" id="navresults">
                <div id="earlierJourneyDiv" runat="server" class="ui-block-a">
                    <asp:LinkButton ID="btnEarlierJourney" CssClass="earlier jshide" runat="server" OnClick="BtnEarlierJourney_Click" data-icon="arrow-l" data-transition='none' data-role='button'/>
                    <noscript>
                        <asp:Button ID="btnEarlierJourneyNonJS" runat="server" CssClass="earlierNonJS" OnClick="BtnEarlierJourney_Click" />
                    </noscript>
                </div>       
                <div class="ui-block-b">
                    <asp:Label ID="lblJourneyCount" runat="server" EnableViewState="true" />
                </div>  
                <div id="laterJouneyDiv" runat="server" class="ui-block-c">
                    <asp:LinkButton ID="btnLaterJourney" CssClass="later jshide"  runat="server" OnClick="BtnLaterJourney_Click" data-icon="arrow-r" data-transition='none' data-iconpos="right" data-role='button'/>
                    <noscript>
                        <asp:Button ID="btnLaterJourneyNonJS" runat="server" CssClass="laterNonJS" OnClick="BtnLaterJourney_Click" />
                    </noscript>
                </div>  
            </div>
        
            <div class="clear"></div>

            <asp:Repeater ID="journeySummary" runat="server" 
                OnItemDataBound="JourneySummary_DataBound"
                OnItemCommand="JourneySummary_Command" EnableViewState="true">

                <HeaderTemplate>
                    <ul data-role="listview">
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <asp:LinkButton ID="showDetailsBtn" runat="server" OnClick="showDetailsBtn_Click" data-role="dialog" data-transition="none">
                            <div id="transportContainer">
                                <strong>
                                    <asp:Label ID="transport" runat="server" EnableViewState="true" /> 
                                </strong>
                            </div>
                            <div class="ui-grid-b">
                                <div class="ui-block-a">
                                    <asp:Label ID="leave" runat="server" EnableViewState="true" />
                                    &nbsp;&nbsp;
                                    <asp:Label ID="arrive" runat="server" EnableViewState="true" />
                                </div>       
                                <div class="ui-block-b">
                                    <asp:Label ID="changes" runat="server" EnableViewState="true" />
                                </div>  
                                <div class="ui-block-c">
                                    <asp:Label ID="journeyTime" runat="server" EnableViewState="true" />
                                </div>  
                            </div>
                        </asp:LinkButton>
                        <noscript>
                            <asp:Button ID="showDetailsBtnNonJS" runat="server" CssClass="showDetailsNonJS" OnClick="showDetailsBtnNonJS_Click" />
                        </noscript>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>

            </asp:Repeater>
                
            <div class="clear"></div>
                
        </div>
    </div>
</div>