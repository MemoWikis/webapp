<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="row" style="border-top: 1px solid #e5e5e5; margin-top: 60px;">
    <div class="clearfix" style="margin-bottom: 10px;"></div>

    <div style="float:left; padding-left: 0px;">
        <a href="http://teamcity.richtig-oder-falsch.de:8080/viewType.html?buildTypeId=TrueOrFalse_Default&guest=1">
            Build: <%= Assembly.Load("TrueOrFalse").GetName().Version.Major %> </a>
    </div>


    <div style="float:right; margin-left: -20px; margin-right: 20px;">
        <%= Html.ActionLink("Gemeinwohlunternehmen", Links.WelfareCompany, Links.VariousController)%> | 
        <%= Html.ActionLink("Impressum", Links.Impressum, Links.VariousController)%>
    </div>    
</div>


<div class="row" style="margin-top: 40px; margin-bottom: 40px;">
    
    Auf:
    <ul>
        <li><a href="http://teamcity.richtig-oder-falsch.de:8080/viewType.html?buildTypeId=TrueOrFalse_Default&guest=1">Teamcity</a></li>
        <li><a href="https://github.com/TrueOrFalse/TrueOrFalse"><i class="fa fa-github"></i> Github</a></li>
    </ul>
    
</div>