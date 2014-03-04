<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<MessageModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/message") %>
    <%= Scripts.Render("~/bundles/js/Messages") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <input type="hidden" id="urlMessageSetRead" value="<%= Links.MessageSetRead(Url) %>"/>
    <input type="hidden" id="urlMessageSetUnread" value="<%= Links.MessageSetUnread(Url) %>"/>
    

    <div class="col-md-6">
        
        <h2 style="margin-top: 0px;">Nachrichten</h2>    

        <% foreach(var msg in Model.Rows){ %>
            <div class="row msgRow rowBase <%: msg.IsRead ? "isRead" : "" %>" data-messageId="<%: msg.MessageId %>">
                <div class="col-xs-12 header">
                    <h4><%: msg.Subject %></h4>
                </div>

                <div class="col-xs-12 body">
                    <%= msg.Body %>
                </div>

                <div class="col-xs-5 footer">
                    <span class="show-tooltip" title="<%: msg.WhenDatetime %>">vor <%: msg.When %></span>
                </div>
                <div class="col-xs-7  footer">
                    <span class="pull-right" style="<%: msg.IsRead ? "display: none" : "" %>">
                        <a href="#" class="markAsRead">
                            als gelesen makieren
                        </a>
                        &nbsp; <i class="fa fa-square-o show-tooltip" style="color:sandybrown;" title="Die Frage ist ungelesen"></i>
                    </span>
                    
                    <span class="pull-right" style="<%: msg.IsRead ? "" : "display: none" %>">
                        <a href="#" class="markAsUnRead">
                            als ungelesen makieren 
                        </a>
                        &nbsp; <i class="fa fa-check-square-o show-tooltip" style="color:green" title="Die Frage ist gelesen"></i>
                    </span>
                </div>
            </div>

        <% } %>        

    </div>
    <div class="col-md-3"></div>

</asp:Content>