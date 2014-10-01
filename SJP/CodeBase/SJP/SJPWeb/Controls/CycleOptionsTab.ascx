<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CycleOptionsTab.ascx.cs"
    Inherits="SJP.UserPortal.SJPWeb.Controls.CycleOptionsTab" %>
<div class="tabContent">
     <div class="tabHeader">
        <asp:Image ID="cycleOptions" runat="server" />
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
                <asp:PostBackTrigger ControlID="planCycle" />
            </Triggers>
            <ContentTemplate>
                <div class="content" runat="server" id="venueContent">
                    Some text with interesting cycle info related to the venue...
                </div>
                <div class="submittab">
                    <asp:Button ID="planCycle" CssClass="submit btnSmallPink" runat="server" Text="Next &gt;" OnClick="PlanCycleJourney"  />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
