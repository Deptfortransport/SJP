<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RiverServicesOptionsTab.ascx.cs"
    Inherits="SJP.UserPortal.SJPWeb.Controls.RiverServicesOptionsTab" %>
<div class="tabContent">
     <div class="tabHeader">
        <asp:Image ID="riverServicesOptions" runat="server" />
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
        <asp:UpdatePanel ID="updateContent" runat="server" RenderMode="Block" UpdateMode="Always"
            ChildrenAsTriggers="true">
             <Triggers>
                <asp:PostBackTrigger ControlID="planRiverServices" />
            </Triggers>
            <ContentTemplate>
                <div class="content" id="venueContent" runat="server">
                        <p>&lt;Venue&gt; is located near to the river so perhaps you'd</p>
                        <p>like to see a route which includes a trip on the river?</p>
                    
                </div>
                <div class="submittab">
                    <asp:Button ID="planRiverServices" CssClass="submit btnSmallPink" OnClick="PlanRiverServices" runat="server" Text="Next &gt;" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
