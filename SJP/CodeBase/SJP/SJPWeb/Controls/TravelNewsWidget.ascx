<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TravelNewsWidget.ascx.cs"
    Inherits="SJP.UserPortal.SJPWeb.Controls.TravelNewsWidget" EnableViewState="true" %>

<div class="row">
    <div class="box  m newsList travelNewsWidget">
        <div class="bH">
            <h3>
                <asp:HyperLink ID="widgetHeading" runat="server" />
                <div id="tnPaging" runat="server" class="navigation floatright">
                    <asp:ImageButton ID="prev" CssClass="prev floatleft" runat="server" OnClick="Prev_Click" />
                    <div class="navigation-totals floatleft">
                        <asp:Label ID="currentPage" CssClass="current" runat="server">1</asp:Label>
                        /
                        <asp:Label ID="pageTotal" CssClass="total" runat="server">3</asp:Label>
                    </div>
                    <asp:ImageButton ID="next" CssClass="next floatleft" runat="server" OnClick="Next_Click" />
                </div>
            </h3>
        </div>
        <div class="bC">

            <div class="tnHeadlinesContainer">
                <asp:Label ID="lblNoIncidents" runat="server" EnableViewState="false" Visible="false" CssClass="noincidents"></asp:Label>                
                <asp:Repeater ID="travelNewsHeadlines" runat="server" OnItemDataBound="TravelNewsHeadlines_DataBound">
                    <HeaderTemplate>
                        <ul class="PagerContainer">
                    </HeaderTemplate>
                    <ItemTemplate>
                         <li class="pageItem" id="pageItem" runat="server">
                            <asp:Repeater ID="travelNewsHeadlineItems" runat="server" OnItemDataBound="TravelNewsHeadlineItems_DataBound">
                                <HeaderTemplate>
                                    <div class="hfeed">
                                        <ul class=" newsList">
                                </HeaderTemplate>
                                <ItemTemplate>
                                        <li class="odd">
                                            <div class="newsTeaser">
                                                <div class="newsWrap">
												    <div class="entry-title">
													    <asp:HyperLink ID="tnItemLink" runat="server" />	
													</div>
											    </div>
										    </div>
                                        </li>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                        <li class="even">
                                            <div class="newsTeaser">
                                                <div class="newsWrap">
												    <div class="entry-title">
													    <asp:HyperLink ID="tnItemLink" runat="server" />	
													</div>
											    </div>
										    </div>
                                        </li>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                        </ul>
                                    </div>
                                </FooterTemplate>
                            </asp:Repeater>
                        </li>
                    </ItemTemplate>
                     <FooterTemplate>
                        </ul>
                    </FooterTemplate>
                </asp:Repeater>
            </div>

        </div>
        <div class="bF"> 
		    <div class="more">
			    <asp:HyperLink ID="tnMoreLink" runat="server"></asp:HyperLink>    
			</div>
        </div>
    </div>
</div>