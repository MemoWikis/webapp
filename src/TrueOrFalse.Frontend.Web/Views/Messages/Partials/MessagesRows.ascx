<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<IList<MessageModelRow>>" %>

<% if (!Model.Any()) { %>
    <div class="alert alert-info">
        Du hast aktuell keine ungelesenen Nachrichten.
    </div>

<% return;
} %>

<% var index = 0;
    foreach (var msg in Model)
    {
        index++;%>
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

    <% if (index % 5 == 0)
       {
           Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");
       }  %>


<% } %>        