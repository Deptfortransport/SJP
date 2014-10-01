<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopTipsWidget.ascx.cs"
    Inherits="SJP.UserPortal.SJPWeb.Controls.TopTipsWidget" %>

<div class="row">
    <div id="top-tips" class="box">
        
        <div class="bH">
            <div class="snippet-header">
                <h3>
                    <asp:label ID="topTipsHeading" Text="Top tips" runat="server"  EnableViewState="false"/>
                    <div class="navigation floatright">
                        <asp:HiddenField ID="currentTopTip" runat="server" />
                        <a href="#" class="prev floatleft" style="display:none;">
                            <asp:Label id="navigationPrev" runat="server" CssClass="hide-content" Text="Prev"  EnableViewState="false"/>
                        </a>
                        <asp:ImageButton ID="prevButton" class="navButton floatleft" runat="server" OnClick="PrevButton_Click" />
                        <div class="navigation-totals floatleft">
                            <asp:Label id="currentTipNo" runat="server" CssClass="current" Text="1" EnableViewState="false"/>
                             / 
                             <asp:Label id="totalTipNo" runat="server" CssClass="total" Text="4"  EnableViewState="false"/>
                        </div>
                        <a href="#" class="next floatleft" style="display:none;">
                            <asp:Label id="navigationNext" runat="server" CssClass="hide-content" Text="Next"  EnableViewState="false"/>
                        </a>
                        <asp:ImageButton ID="nextButton" class="navButton floatleft" runat="server" OnClick="NextButton_Click" />
                    </div>
                 </h3>
            </div>
        </div>

        <div class="bC">
            <div class="slider">
                <asp:Repeater ID="topTips" runat="server" OnItemDataBound="TopTips_ItemDataBound" EnableViewState="true">
                    <ItemTemplate>
                        <div class="content" runat="server" id="tipContainer">
                            <%# Container.DataItem %>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>

        <div class="bF"> </div>
    </div>
</div>
