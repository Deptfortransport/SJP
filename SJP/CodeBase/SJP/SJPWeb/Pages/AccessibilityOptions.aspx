<%@ Page Title="" Language="C#" MasterPageFile="~/SJPWeb.Master" AutoEventWireup="true"
    CodeBehind="AccessibilityOptions.aspx.cs" Inherits="SJP.UserPortal.SJPWeb.Pages.AccessibilityOptions" %>

<asp:Content ID="Content4" ContentPlaceHolderID="contentMain" runat="server">

<div class="mainSection">

    <div id="messageSeprator" runat="server" visible="false">
        <div class="sjpSeparator" ></div>
    </div>
    <div class="mapInfo">
        <asp:Label ID="lblMapInfo" runat="server" EnableViewState="false"></asp:Label>
    </div>
    <div class="selectedJourney">
        <asp:Label ID="lblRequestJourney" runat="server" EnableViewState="false"></asp:Label>
        <div class="row journeyInfo">
            <asp:Label ID="lblFrom" CssClass="label" runat="server" EnableViewState="false"></asp:Label>
            <asp:Label ID="from" CssClass="options" runat="server" EnableViewState="false"></asp:Label>
        </div>
        <div class="row journeyInfo">
            <asp:Label ID="lblTo" CssClass="label" runat="server" EnableViewState="false"></asp:Label>
            <asp:Label ID="to" CssClass="options" runat="server" EnableViewState="false"></asp:Label>
        </div>
        <div class="row journeyInfo">
            <asp:Label ID="lblDateTime" CssClass="label" runat="server" EnableViewState="false"></asp:Label>
            <asp:Label ID="journeyDateTime" CssClass="options" runat="server" EnableViewState="false"></asp:Label>
        </div>
        <div class="row accessibilityMessage">
            <asp:Label ID="lblAccessibility" CssClass="accessibility" runat="server" EnableViewState="false"></asp:Label>
        </div>
        <div class="clearboth">
        </div>
    </div>
    <asp:Label ID="accessibilityPlanJourneyInfo" CssClass="accessiblityInfo" runat="server" EnableViewState="false"></asp:Label>

</div>

    <asp:UpdatePanel ID="commandUpdatePanel" runat="server" UpdateMode="Always">
        <ContentTemplate>
    <div class="box">
        <div class="bH"></div>
        <div class="bC">

            <div class="stopSelection">
                <div class="jssettings">
                    <asp:HiddenField ID="jsEnabled" runat="server" Value="false" />
                </div>
                <asp:Label ID="stopTypeSelect" CssClass="accissibleStopType" runat="server" EnableViewState="false"></asp:Label>
                <div class="stopTypes">
                    <asp:CheckBoxList ID="stopTypeList" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                        RepeatLayout="Flow" OnSelectedIndexChanged="StopTypeList_Changed" AutoPostBack="true">   
                    </asp:CheckBoxList>
                    <noscript>
                        <asp:Button ID="btnStopTypeListGo" CssClass="submit btnSmallerPink" runat="server" Text="Go" />
                    </noscript>
                </div>
                <asp:Label ID="stopSelectInfo" CssClass="stopSelectInfo" runat="server" EnableViewState="false"></asp:Label>
                <div class="stopSelectDrops">
                    <div class="row">
                        <div class="label">
                            <asp:Label ID="lblCountry" runat="server" EnableViewState="false"></asp:Label>
                        </div>
                        <div class="options">
                            <asp:DropDownList ID="countryList" runat="server" CssClass="dropList" AutoPostBack="true" OnSelectedIndexChanged="CountryList_Changed"></asp:DropDownList>
                            <noscript>
                                <asp:Button ID="btnCountryListGo" CssClass="submit btnSmallerPink" runat="server" OnClick="CountryList_Changed" Text="Go" />
                            </noscript>
                        </div>
                    </div>

                    <div class="row">
                        <div class="label">
                            <asp:Label ID="lblAdminArea" runat="server" EnableViewState="false"></asp:Label>
                        </div>
                        <div class="options">
                            <asp:DropDownList ID="adminAreaList"  CssClass="dropList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="AdminAreaList_Changed"></asp:DropDownList>
                            <noscript>
                                <asp:Button ID="btnAdminAreaListGo" CssClass="submit btnSmallerPink" runat="server" OnClick="BtnAdminAreaListGo_Click" Text="Go" />
                            </noscript>
                        </div>
                    </div>
                    <div id="districtRow" runat="server" class="row" visible="false">
                        <div class="label">
                            <asp:Label ID="lblDistrict" runat="server" EnableViewState="false"></asp:Label>
                        </div>
                        <div class="options">
                            <asp:DropDownList ID="districtList" CssClass="dropList" runat="server" AutoPostBack="true" Visible="false" OnSelectedIndexChanged="DistrictList_Changed"></asp:DropDownList>
                            <noscript>
                                <asp:Button ID="btnDistrictListGo" CssClass="submit btnSmallerPink" runat="server" OnClick="BtnDistrictListGo_Click" Text="Go" />
                            </noscript>
                        </div>
                    </div>
                    <div class="row">
                        <div class="label">
                            <asp:Label ID="journeyFrom" runat="server" EnableViewState="false"></asp:Label>
                        </div>
                        <div class="options">
                            <asp:DropDownList ID="gnatList" runat="server" CssClass="GNATList dropList" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="GNATList_Changed"></asp:DropDownList>
                            <noscript>
                                <asp:Button ID="btnGNATListGo" CssClass="submit btnSmallerPink" runat="server" OnClick="BtnGNATListGo_Click" Text="Go" />
                            </noscript>
                        </div>
                    </div>
                    <div class="clearboth">
                    </div>
                </div>
            </div>
            <div class="journeySubmit">
                <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" CssClass="submit btnSmallPink floatleft" EnableViewState="false" />
                <asp:Button ID="btnPlanJourney" runat="server" OnClick="btnPlanJourney_Click" CssClass="submit btnLargePink floatright" Enabled="false" />
            </div>
        
        </div>
        <div class="bF"></div>
    </div>
            <div id="debugInfoDiv" runat="server" visible="false" class="debugInfoDiv">
                <asp:Repeater ID="debugGnatListRptr" runat="server" OnItemDataBound="GnatListRptr_DataBound">
                    <ItemTemplate>
                        <div id= "debugGnatStation" runat="server" class="debugGnatStation"></div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
