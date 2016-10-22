<%@ Page Title="Deine Nachrichten" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<MessageModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <link rel="canonical" href="<%= Settings.CanonicalHost %><%= Links.Messages(Url) %>" />
    <%= Styles.Render("~/bundles/message") %>
    <%= Scripts.Render("~/bundles/js/Messages") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <input type="hidden" id="urlMessageSetRead" value="<%= Links.MessageSetRead(Url) %>"/>
    <input type="hidden" id="urlMessageSetUnread" value="<%= Links.MessageSetUnread(Url) %>"/>
    
    <div class="row">
        <div class="col-md-9">
        
            <h1 style="margin-top: 0; margin-bottom: 20px;">
                <span class="ColoredUnderline Message" style="padding-right: 3px;">Nachrichten</span>
            </h1>    

            <% foreach(var msg in Model.Rows){ %>
                <div class="row msgRow rowBase <%: msg.IsRead ? "isRead" : "" %>" data-messageId="<%: msg.MessageId %>">
                    <div class="msg">
                        <div class="col-xs-12 header">
                            <h4><%: msg.Subject %></h4>
                        </div>

                        <div class="col-xs-12 body">
                            <%= msg.Body %>
                        </div>

                        <div class="col-sm-5 footer">
                            <span class="show-tooltip" title="<%: msg.WhenDatetime %>">vor <%: msg.When %></span>
                        </div>
                        <div class="col-sm-7  footer">
                            <%if(msg.MessageId != 0){ %>
                                <span class="pull-right" style="<%: msg.IsRead ? "display: none" : "" %>">
                                    <a href="#" class="TextLinkWithIcon markAsRead">
                                        <span class="TextSpan">als gelesen makieren</span> 
                                        &nbsp; <i class="fa fa-square-o show-tooltip" style="color:sandybrown;" title="Die Frage ist ungelesen"></i>

                                    </a>
                                </span>
                    
                                <span class="pull-right" style="<%: msg.IsRead ? "" : "display: none" %>">
                                    <a href="#" class="TextLinkWithIcon markAsUnRead">
                                        <span class="TextSpan">als ungelesen makieren</span> 
                                        &nbsp; <i class="fa fa-check-square-o show-tooltip" style="color:green" title="Die Frage ist gelesen"></i>
                                    </a>
                                </span>
                            <% } %>
                        </div>
                    </div>
                </div>

            <% } %>        

        </div>
        <div class="col-md-3"></div>
    </div>
</asp:Content>