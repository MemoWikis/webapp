<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="col-xs-12">
    <div class="row">
        <div class="col-xs-12">
            <div id="partnersAndSponsors">
                <img class="partnerImage" src="/Images/LogosPartners/BMWi-Logo-t.png"/>
                <img class="partnerImage" src="/Images/LogosPartners/Logo-EXIST-eps.png"/>
                <img class="partnerImage" src="/Images/LogosPartners/Logo-ESF-rgb-gif.png"/>
                <img class="partnerImage" src="/Images/LogosPartners/Logo-EU-cmyk-eps.png"/>
                <img class="partnerImage" src="/Images/LogosPartners/Claim-ESF-cmyk-eps.png"/>
                <img class="partnerImage" src="/Images/LogosPartners/profund-innovation-logo-t.png"/>
                <p>
                    memucho wird im Rahmen des EXIST-Programms durch das Bundesministerium für Wirtschaft und Energie und den Europäischen Sozialfonds gefördert.
                </p>
            </div>
        </div>
    </div>
    
    <div class="row">
        <div class="col-sm-6 col-xs-12">        
            <%--<a href="#"><i class="fa fa-bar-chart"></i>--%>
                <%--<%= Html.ActionLink("Algorithmus-Einblick", "Forecast", "AlgoInsight")  %>--%>
                <%= Html.ActionLink("Gemeinwohlökonomie", Links.WelfareCompany, Links.VariousController)%>
            </a>
        </div>

        <div class="col-sm-6 col-xs-12 text-align-right-md">
            <%= Html.ActionLink("Nutzungsbedingungen (AGBs)", Links.TermsAndConditions, Links.VariousController)%> | 
            <%= Html.ActionLink("Impressum", Links.Impressum, Links.VariousController)%>
        </div>
    </div>
    <div class="Clearfix"></div>
</div>


<div class="col-xs-12" style="margin-top: 2px;">
    
    <div class="row">
        <div class="col-sm-6 col-xs-12">        
            <a href="https://github.com/TrueOrFalse/TrueOrFalse"><i class="fa fa-github">&nbsp;</i>Github</a>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <a href="http://teamcity.richtig-oder-falsch.de:8080/project.html?projectId=TrueOrFalse&guest=1">
                <i class="fa fa-cogs">&nbsp;</i>Teamcity
                <% var assembly = Assembly.Load("TrueOrFalse"); %>
                <span style="color:darkgray">
                    (Build: <%= assembly.GetName().Version.Major %> am
                    <%= Html.Raw(AssemblyLinkerTimestamp.Get(assembly).ToString("dd.MM.yyyy 'um' HH:mm")) %>)
                </span>
            </a>
        </div>
        <div class="col-sm-6 col-xs-12 text-align-right-md">
            
        </div>
    </div>
    <div class="pull-right">
        <% if(ViewBag.BetaBackgroundLicenceUrl != null){ %>
            <a href="<%= ViewBag.BetaBackgroundLicenceUrl %>">Lizenz Hintergrundbild</a>
        <% } %>        
    </div>
    
</div>