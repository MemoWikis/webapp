<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<WelcomeBoxTopSetsModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<p style="padding-left: 15px; margin-bottom: 0px;">
    <% foreach (var set in Model.Sets) {%>
            <p style="margin-bottom: 0px; padding-left: 15px; padding-bottom: 5px; line-height: 12px;">
                <a href="<%= Links.SetDetail(Url,set) %>"><span class="label label-set"><%: set.Name %></span></a> 
                <% if (set.QuestionsInSet.Count == 1) {%> (1&nbsp;Frage)
                <% } else {%>(<%: set.QuestionsInSet.Count %>&nbsp;Fragen)<%} %>
            </p>
    <%} %>
</p>

