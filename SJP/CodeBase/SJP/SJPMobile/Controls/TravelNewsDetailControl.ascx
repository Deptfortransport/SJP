<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TravelNewsDetailControl.ascx.cs" Inherits="SJP.UserPortal.SJPMobile.Controls.TravelNewsDetailControl" %>

<div data-role="page" class="newsDetail collapsed">
    <asp:HiddenField ID="newsDetailId" runat="server" />

    <div class="headerBack headerBackBlue jshide">
        <h2 ID="detailNewsTitle" runat="server"></h2>
        <asp:LinkButton ID="closeNewsItem" CssClass="topNavLeft" runat="server" data-icon="delete"></asp:LinkButton>
    </div>
    
    <h3 id="detailNewsHeadlineLbl" runat="server"></h3>

    <div class="newsDetails">
        <div class="dates">
            <div class="startDate">
                <asp:Label ID="startDateLbl" class="newsDetailLabel" runat="server" />
                <asp:Label ID="startDateContent" runat="server" />
            </div>
            <div class="statusDate">
                <asp:Label ID="statusDateLbl" class="newsDetailLabel" runat="server" />
                <asp:Label ID="statusDateContent" runat="server" />
            </div>
        </div>

        <div class="operator">
            <asp:Label ID="operatorLbl" class="newsDetailLabel" runat="server" />
            <asp:Label ID="operatorContent" runat="server" />
        </div>
        <div class="severity">
            <asp:Label ID="severityLbl" class="newsDetailLabel" runat="server" />
            <asp:Label ID="severityContent" runat="server" />
        </div>
        <div class="incidentType">
            <asp:Label ID="incidentTypeLbl" class="newsDetailLabel" runat="server" />
            <asp:Label ID="incidentTypeContent" runat="server" />
        </div>
        <div class="location">
            <asp:Label ID="locationTextLbl" class="newsDetailLabel" runat="server" />
            <asp:Label ID="locationTextContent" runat="server" />
        </div>
        <div class="venueAffected">
            <asp:Label ID="venuesAffectedLbl" class="newsDetailLabel" runat="server" />
            <asp:Label ID="venuesAffectedContent" runat="server" />
        </div>
                
        <div class="detail">
            <asp:Label ID="detailTextLbl" class="newsDetailLabel" runat="server" />
            <asp:Label ID="detailTextContent" runat="server" />
        </div>
        
        <div class="advice">
            <asp:Label ID="travelAdviceLbl" class="newsDetailLabel" runat="server" />
            <asp:Label ID="travelAdviceContent" runat="server" />
        </div>

    </div>
</div>
