<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="col-md-12">

    <div style="float:left; padding-left: 0px;">
        <a href="http://teamcity.richtig-oder-falsch.de:8080/project.html?projectId=TrueOrFalse&guest=1">
            Build: <%= Assembly.Load("TrueOrFalse").GetName().Version.Major %> </a>
    </div>


    <div class="pull-right">
        <%= Html.ActionLink("Gemeinwohlunternehmen", Links.WelfareCompany, Links.VariousController)%> | 
        <%= Html.ActionLink("Impressum", Links.Impressum, Links.VariousController)%>
    </div>
    <div class="Clearfix"></div>
</div>


<div class="col-md-12 ">
    
    <span style="display: inline;">Auf:</span>
    <ul id="footerOn">
        <li><a href="http://teamcity.richtig-oder-falsch.de:8080/project.html?projectId=TrueOrFalse&guest=1">Teamcity</a></li>
        <li><a class="TextLinkWithIcon" href="https://github.com/TrueOrFalse/TrueOrFalse"><i class="fa fa-github"></i> Github</a></li>
    </ul>
    
</div>