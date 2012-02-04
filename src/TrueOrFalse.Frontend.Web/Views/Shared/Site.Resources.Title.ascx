<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>        

<div id="header"  class="span-12">
    <div class="container">
        <div class="pull-left">
            <a href="/"><h1>Richtig oder Falsch</h1></a>
        </div>
        
        
        <div class="pull-right" style="padding-top: 15px; ">
            <% Html.RenderPartial(UserControls.Logon); %>
            
            <% if (HttpContext.Current.Request.IsLocal){ %>
                <%: Html.ActionLink("Exportieren", "AllToLocalFile", "Export") %>
            <% } %>
        </div>
        
        </h1>
        

    </div>
</div>