<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<WelcomeBoxTopSetsModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<p style="padding-left: 15px; margin-bottom: 0px;">
    <% foreach (var topSet in Model.TopSets){%>
            <p style="margin-bottom: 0px; padding-left: 15px; padding-bottom: 5px; line-height: 12px;">
                <a href="<%= Links.SetDetail(Url,topSet.Name,topSet.SetId) %>"><span class="label label-set"><%: topSet.Name %></span></a> 
                <% if (topSet.QCount == 1) {%> (1&nbsp;Frage)
                <% } else {%>(<%: topSet.QCount %>&nbsp;Fragen)<%} %>
            </p>
    <%} %>
</p>

