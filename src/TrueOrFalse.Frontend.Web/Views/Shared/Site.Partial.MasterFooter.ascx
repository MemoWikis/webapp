<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="col-md-12">
    
    <div class="row">
        <div class="col-md-6 col-xs-12">        
            <a href="#"><i class="fa fa-bar-chart"></i> Algorithmus-Einblick</a>
        </div>

        <div class="col-md-6 col-xs-12" class="pull-right">
            <%= Html.ActionLink("Gemeinwohlökonomie", Links.WelfareCompany, Links.VariousController)%> | 
            <%= Html.ActionLink("Impressum", Links.Impressum, Links.VariousController)%>
        </div>
    </div>
    <div class="Clearfix"></div>
</div>


<div class="col-md-12" style="margin-top: 2px;">
    
    <span style="display: inline;">Auf:</span>
    <ul id="footerOn" style="margin-left: -15px;">
        <li>
            
            <a href="http://teamcity.richtig-oder-falsch.de:8080/project.html?projectId=TrueOrFalse&guest=1">
                <i class="fa fa-cogs"></i> 
                Teamcity (Build:<%= Assembly.Load("TrueOrFalse").GetName().Version.Major %>)
            </a>
        </li>
        <li><a class="TextLinkWithIcon" href="https://github.com/TrueOrFalse/TrueOrFalse"><i class="fa fa-github"></i> Github</a></li>
        <li>
            <a href="http://teamcity.richtig-oder-falsch.de:8080/project.html?projectId=TrueOrFalse&guest=1">
                
            </a>    
        </li>
    </ul>

    <div class="pull-right">
        <% if(ViewBag.BetaBackgroundLicenceUrl != null){ %>
            <a href="<%= ViewBag.BetaBackgroundLicenceUrl %>">Lizenz Hintergrundbild</a>
        <% } %>        
    </div>
    
</div>