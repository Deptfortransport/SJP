<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccessibleStopsControl.ascx.cs" Inherits="SJP.UserPortal.SJPMobile.Controls.AccessibleStopsControl" %>

<div id="accessibility">

    <h3 id="accessibleStopsHeading" runat="server" enableviewstate="false" />

<div id="accessibilityStops" runat="server" class="accessibilityStops">
    
    <div class="accessibleStopsInfoDiv">
        <asp:Label ID="accessibleStopsInfo" runat="server" EnableViewState="false"></asp:Label>
    </div>

    <div class="clear"></div>
    
    <div class="accessibleCheck">
        <fieldset>
            <h4 id="stopTypeHeading" runat="server" enableviewstate="false" />
            <div class="ui-grid-a">
                <div class="ui-block-a">
                    <asp:CheckBoxList ID="stopTypeListLeft" runat="server" 
                        OnSelectedIndexChanged="StopTypeList_Changed"
                        RepeatLayout="Flow" repeatdirection="horizontal" AutoPostBack="true">   
                    </asp:CheckBoxList>
                </div>
                <div class="ui-block-b">
                     <asp:CheckBoxList ID="stopTypeListRight" runat="server" 
                        OnSelectedIndexChanged="StopTypeList_Changed" 
                        RepeatLayout="Flow" repeatdirection="horizontal" AutoPostBack="true">   
                    </asp:CheckBoxList>
                </div>
            </div>
      </fieldset> 
    </div> 
    
    <div id="country">
        <fieldset data-role="controlgroup">
            <h4 id="countryHeading" runat="server" enableviewstate="false" />
            <asp:DropDownList ID="countryList" runat="server" OnSelectedIndexChanged="Country_Click" AutoPostBack="true" CssClass="accessibleStopsDrop" />
        </fieldset>
    </div>

    <div id="county">
        <fieldset data-role="controlgroup">
            <h4 id="countyHeading" runat="server" enableviewstate="false" />
            <asp:DropDownList ID="countyList" runat="server" OnSelectedIndexChanged="County_Click" AutoPostBack="true" CssClass="accessibleStopsDrop" />
        </fieldset>
    </div>

    <div id="district" runat="server" visible="false">
        <fieldset data-role="controlgroup">
            <h4 id="districtHeading" runat="server" enableviewstate="false" />
            <asp:DropDownList ID="districtList" runat="server" OnSelectedIndexChanged="District_Click" AutoPostBack="true" CssClass="accessibleStopsDrop" />
        </fieldset>
    </div>
    
    <div class="hide jsshow">
        <noscript>
            <asp:Button ID="updateBtnNonJS" runat="server" OnClick="updateBtn_Click" EnableViewState="false" CssClass="updateBtnNonJS" ></asp:Button>
        </noscript>
    </div>

    <div id="stop">
        <fieldset data-role="controlgroup">
            <h4 id="stopHeading" runat="server" enableviewstate="false" />
            <asp:DropDownList ID="stopList" runat="server" OnSelectedIndexChanged="Stop_Click" Enabled="false" CssClass="accessibleStopsDrop" />
        </fieldset>
    </div>
</div>
</div>

<div class="clear"></div>

<div class="submittab">
    <div class="jshide">
        <asp:LinkButton ID="planJourneyBtn" runat="server" OnClick="planJourneyBtn_Click" EnableViewState="false" CssClass="buttonMag" ></asp:LinkButton>
    </div>
    <noscript>
        <asp:Button ID="planJourneyBtnNonJS" runat="server" OnClick="planJourneyBtn_Click" EnableViewState="false" CssClass="buttonMag buttonMagNonJS" ></asp:Button>
    </noscript>
</div>
