<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SideBarRightControl.ascx.cs" Inherits="SJP.UserPortal.SJPWeb.Controls.SideBarRightControl" %>
<%@ Register src="JourneyPlannerWidget.ascx" tagname="JourneyPlannerWidget" tagprefix="uc1" %>
<%@ Register src="VenueMapsWidget.ascx" tagname="VenueMapsWidget" tagprefix="uc1" %>
<%@ Register src="TravelNewsWidget.ascx" tagname="TravelNewsWidget" tagprefix="uc1" %>
<%@ Register src="~/Controls/TopTipsWidget.ascx" tagname="TopTipsWidget" tagprefix="uc1" %>
<%@ Register src="~/Controls/GenericPromoWidget.ascx" tagname="GenericPromoWidget" tagprefix="uc1" %>
<%@ Register src="~/Controls/GNATMapsWidget.ascx" tagname="GNATMapsWidget" tagprefix="uc1" %>
<%@ Register src="~/Controls/TravelNewsInfoWidget.ascx" tagname="TravelNewsInfoWidget" tagprefix="uc1" %>

<uc1:TopTipsWidget ID="topTipsWidget" runat="server" Visible="true" />
<uc1:GenericPromoWidget ID="faqWidget" runat="server" PromoType="FAQ" Visible="false" />
<uc1:JourneyPlannerWidget ID="journeyPlannerWidget" runat="server" visible="false"/>
<uc1:VenueMapsWidget ID="venueMapsWidget" runat="server" visible="false"/>
<uc1:TravelNewsWidget ID="travelNewsWidget" runat="server" visible="false"/>
<uc1:GenericPromoWidget ID="walkingWidget" runat="server" visible="false" PromoType="Walking" />
<uc1:GenericPromoWidget ID="gamesTravelCardWidget" runat="server" visible="false" PromoType="GamesTravelCard" />
<uc1:GenericPromoWidget ID="accessibleTravelWidget" runat="server" visible="false" PromoType="AccessibleTravel" />
<uc1:GNATMapsWidget ID="gbGNATMapWidget" runat="server" visible="false" PromoType="GBGNATMap" />
<uc1:GNATMapsWidget ID="seGNATMapWidget" runat="server" visible="false" PromoType="SEGNATMap" />
<uc1:GNATMapsWidget ID="londonGNATMapWidget" runat="server" visible="false" PromoType="LondonGNATMap" />
<uc1:TravelNewsInfoWidget ID="travelNewsInfoWidget" runat="server" visible="false" />
