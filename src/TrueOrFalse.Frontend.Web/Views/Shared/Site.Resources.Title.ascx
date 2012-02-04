<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>        

<div id="header"  class="span24 last">
    <div id="title">
        <h1>Richtig oder Falsch</h1>
    </div>
              
    <div id="logindisplay" style="float:right">
        <% Html.RenderPartial(UserControls.Logon); %>
            
        <% if (HttpContext.Current.Request.IsLocal)
            { %>
            <%: Html.ActionLink("Exportieren", "AllToLocalFile", "Export") %>
        <% } %>
    </div>
</div>