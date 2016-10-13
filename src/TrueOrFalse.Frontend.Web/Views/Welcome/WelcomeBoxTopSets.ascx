<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<WelcomeBoxTopSetsModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% foreach (var topSet in Model.TopSets){%>
    <div class="LabelItem LabelItem-Set">
        <a href="<%= Links.SetDetail(Url,topSet.Name,topSet.SetId) %>"><span class=""><%: topSet.Name %></span></a> 
        <% if (topSet.QCount == 1) {%> 
            (1&nbsp;Frage)
        <% } else {%>
            (<%: topSet.QCount %>&nbsp;Fragen)<%} 
        %>
    </div>
<%} %>

