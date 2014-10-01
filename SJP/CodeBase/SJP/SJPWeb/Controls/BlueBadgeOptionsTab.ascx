<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BlueBadgeOptionsTab.ascx.cs" Inherits="SJP.UserPortal.SJPWeb.Controls.BlueBadgeOptionsTab" %>
<div class="tabContent">
     <div class="tabHeader">
        <asp:Image ID="blueBadgeOptions" runat="server" />
    </div>
    <div class="updateContent">
        <asp:UpdateProgress ID="updateProgress" runat="server" DynamicLayout="true" Visible="false">
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
                <asp:PostBackTrigger ControlID="planBlueBadge" />
            </Triggers>
            <ContentTemplate>
                <div class="content" id="venueContent" runat="server"></div>
                <div class="submittab">
                    <asp:Button ID="planBlueBadge" CssClass="submit btnSmallPink" runat="server" Text="Next &gt;" OnClick="PlanBlueBadge"  />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
     
</div>