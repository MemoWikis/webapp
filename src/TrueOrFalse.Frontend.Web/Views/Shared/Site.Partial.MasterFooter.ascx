<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="col-xs-12">
    <div class="row">
        <div class="col-xs-12">
            <div class="partnersAndSponsors">
                <div class="row">
                    <div class="col-xs-4 col-md-2" style="text-align: left;">
                        <img class="partnerImage" src="/Images/LogosPartners/BMWi-Logo-t.png" width="142" height="120"/>
                    </div>
                    <div class="col-xs-4 col-md-2">
                        <img class="partnerImage" src="/Images/LogosPartners/Logo-EXIST-eps.png" width="115" height="73" style="margin-top: 23px"/>
                    </div>
                    <div class="col-xs-4 col-md-2 alignRightUntillMD">
                        <img class="partnerImage" src="/Images/LogosPartners/Logo-ESF-rgb-gif.png" width="115" height="57" style="margin-top: 31px"/>
                    </div>
                    <div class="clearfix visible-xs visible-sm"></div>
                    <div class="col-xs-4 col-md-2 alignLeftUntillMD">
                        <img class="partnerImage" src="/Images/LogosPartners/Logo-EU-cmyk-eps.png" width="62" height="69" style="margin-top: 30px"/>
                    </div>
                    <div class="col-xs-4 col-md-2">
                        <img class="partnerImage" src="/Images/LogosPartners/Claim-ESF-cmyk-eps.png" width="115" height="48" style="margin-top: 33px"/>
                    </div>
                    <div class="col-xs-4 col-md-2" style="text-align: right;">
                        <img class="partnerImage" src="/Images/LogosPartners/profund-innovation-logo-t.png" width="160" height="60" style="margin-top: 30px"/>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <div class="row">
        <div class="col-sm-6 col-xs-12">        
            <%--<%= Html.ActionLink("Algorithmus-Einblick", "Forecast", "AlgoInsight")  %>--%>
            <%= Html.ActionLink("Gemeinwohlökonomie", Links.WelfareCompany, Links.VariousController)%> <br/>
            <a href="https://github.com/TrueOrFalse/TrueOrFalse"><i class="fa fa-github">&nbsp;</i>Github</a> <br/>
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
            <a href="<%=Links.Jobs() %>">Jobs bei memucho</a><br/>
            <%= Html.ActionLink("Nutzungsbedingungen (AGBs)", Links.TermsAndConditions, Links.VariousController)%> <br/>
            <%= Html.ActionLink("Impressum", Links.Impressum, Links.VariousController)%> <br />
            <a href="<%=Links.FAQItem("Contact") %>">Kontakt</a>
        </div>
    </div>
    <div class="Clearfix"></div>
</div>


<div class="col-xs-12" style="margin-top: 2px;">
   
    <div class="pull-right">
        <% if(ViewBag.BetaBackgroundLicenceUrl != null){ %>
            <a href="<%= ViewBag.BetaBackgroundLicenceUrl %>">Lizenz Hintergrundbild</a>
        <% } %>        
    </div>
    
</div>