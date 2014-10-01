<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ParkAndRideOptionsTab.ascx.cs" Inherits="SJP.UserPortal.SJPWeb.Controls.ParkAndRideOptionsTab" %>
<div class="tabContent">
     <div class="tabHeader">
        <asp:Image ID="parkAndRideOptions" runat="server" />
    </div>
    <div class="updateContent">
        <asp:UpdateProgress ID="updateProgress" runat="server" DynamicLayout="false" Visible="false">
            <ProgressTemplate>
                 <div class="progressBackgroundFilter"></div>
                    <div class="processMessage"> 
                         <asp:Image ID="loading" runat="server" />
                         <br /><br />
                         <div>
                            <asp:Label ID="loadingMessage" runat="server" />
                         </div>
                    </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="updateContent" runat="server" RenderMode="Block" UpdateMode="Conditional"
            ChildrenAsTriggers="true">
             <Triggers>
                <asp:PostBackTrigger ControlID="planParkAndRide" />
            </Triggers>
            <ContentTemplate>
                <div class="content" id="venueContent" runat="server"></div>
                <div class="submittab">
                    <asp:Button ID="planParkAndRide" CssClass="submit btnSmallPink" runat="server" Text="Next &gt;" OnClick="PlanParkAndRideJourney"  />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
     
    
</div>