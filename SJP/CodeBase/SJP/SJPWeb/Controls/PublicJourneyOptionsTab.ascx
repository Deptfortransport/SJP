<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PublicJourneyOptionsTab.ascx.cs"
    Inherits="SJP.UserPortal.SJPWeb.Controls.PublicJourneyOptionsTab" %>
<div class="tabContent">
    <div class="tabHeader">
        <asp:Image ID="publicJourneyOptions" runat="server" />
    </div>
    <div class="content">
        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="pjContent" id="venueContent" runat="server">
                        
                </div>
                <div id="accessibilityOptions" class="accessibilityOptions" runat="server" enableviewstate="false">
                    <div class="pjOptionsHead">
                        <div class="arrow_btn">
                            <asp:ImageButton ID="additionalMobilityNeeds" CssClass="closed" runat="server" OnClick="AdditionalMobilityNeeds_Click" />
                        </div>
                        <asp:Label ID="mobilityNeedsLabel" runat="server" />
                    </div>
                    <div class="pjOptions collapsed" id="mobilityOptions" runat="server">
                        <p class="nestedOption floatleft">
                            <asp:RadioButton ID="stepFree" runat="server" GroupName="mobilityOptions" CssClass="accessibleOption"/>
                        </p>
                        <a class="tooltip_information1" href="#" title=""  onclick="return false;" id="tooltipInfo_StepFree" runat="server" enableviewstate="false">
                            <asp:Image ID="tooltipInfoImg_StepFree" CssClass="information" runat="server" />
                        </a>
                        <div class="clearboth"></div>

                        <p class="nestedOption floatleft">
                            <asp:RadioButton ID="stepFreeAndAssistance" runat="server" GroupName="mobilityOptions" CssClass="accessibleOption"/>
                        </p>
                        <a class="tooltip_information1" href="#" title=""  onclick="return false;" id="tooltipInfo_StepFreeAndAssistance" runat="server" enableviewstate="false">
                            <asp:Image ID="tooltipInfoImg_StepFreeAndAssistance" CssClass="information" runat="server" />
                        </a>
                        <div class="clearboth"></div>

                        <p class="nestedOption floatleft">
                            <asp:RadioButton ID="assistance" runat="server" GroupName="mobilityOptions" CssClass="accessibleOption"/>
                        </p>
                        <a class="tooltip_information1" href="#" title=""  onclick="return false;" id="tooltipInfo_Assistance" runat="server" enableviewstate="false">
                            <asp:Image ID="tooltipInfoImg_Assistance" CssClass="information" runat="server" />
                        </a>
                        <div class="clearboth"></div>

                        <p class="nestedOption floatleft">
                            <asp:RadioButton ID="excludeUnderground" runat="server" GroupName="mobilityOptions" CssClass="accessibleOption" />
                        </p>
                        <a class="tooltip_information1" href="#" title=""  onclick="return false;" id="tooltipInfo_ExcludeUnderground" runat="server" enableviewstate="false">
                            <asp:Image ID="tooltipInfoImg_ExcludeUnderground" CssClass="information" runat="server" />
                        </a>
                        <div class="clearboth"></div>

                        <p class="nestedOption noMobilityNeedsRadio floatleft">
                            <asp:RadioButton ID="noMobilityNeeds" runat="server" GroupName="mobilityOptions" CssClass="accessibleOption" />
                        </p>
                        <a class="tooltip_information1" href="#" title=""  onclick="return false;" id="tooltipInfo_NoMobilityNeeds" runat="server" enableviewstate="false">
                            <asp:Image ID="tooltipInfoImg_NoMobilityNeeds" CssClass="information" runat="server" />
                        </a>
                        <div class="clearboth"></div>

                        <p class="nestedOption fewerInterchangeCheck">
                            <asp:CheckBox ID="fewerInterchanges" runat="server" CssClass="accessibleOptionCheckbox"/>
                        </p>
                        <div class="clearboth"></div>

                        <strong><asp:HyperLink ID="lnkAccessibleTravel" runat="server" EnableViewState="false"></asp:HyperLink></strong>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="clearboth">
    </div>
    <div class="submittab">
        <asp:Button ID="PlanPublicJourney" CssClass="submit btnLargePink" runat="server"
            Text="Plan my journey &gt;" OnClick="PublicJourneyPlanned" />
    </div>
</div>
