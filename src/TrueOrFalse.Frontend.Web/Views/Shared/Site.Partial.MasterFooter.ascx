﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<%= Scripts.Render("~/bundles/js/stickySearch") %>
<%  var userSession = new SessionUser();
    if (!userSession.IsLoggedIn) { %>
    <%= Scripts.Render("~/bundles/js/headerSearch") %>
<%} %>
<div class="row Promoter">
    <div class="col-xs-12">
    </div>
</div>

<div class="row footer-links-memucho">

    <div class="FooterCol xxs-stack col-xs-6 col-md-4">
        <b>memucho</b><br />
        <a href="<%=Links.Team() %>">Team</a><br />
        <a href="<%=Links.AboutMemucho() %>">Über memucho</a><br />
        <a href="<%=Links.ForTeachers() %>">memucho für Lehrer/Dozenten</a><br />
        <a href="<%= Links.Users() %>">Alle Nutzer</a><br />
            &nbsp;<br />
        <a href="<%=Links.WelfareCompany() %>">Gemeinwohlökonomie</a><br />
        <a href="<%=Links.Jobs() %>">Jobs</a><br />
        <a href="<%=Links.Contact %>">Kontakt & Anfahrt</a><br />
    </div>

    <div class="FooterCol xxs-stack col-xs-6 col-md-4">

        <b>Software</b><br />
        <a href="https://github.com/TrueOrFalse/TrueOrFalse" target="_blank"><i class="fa fa-github">&nbsp;</i>Github</a>
        <br />
        <a href="http://teamcity.memucho.de:8080/project.html?projectId=TrueOrFalse&guest=1" target="_blank">
            <i class="fa fa-cogs">&nbsp;</i>Teamcity
        </a>
        <br />
        <% if (Request.IsLocal)
            { %>
        <%= Html.ActionLink("Algorithmus-Einblick", "Forecast", "AlgoInsight")  %><br />
        <% } %>
        <% var assembly = Assembly.Load("TrueOrFalse"); %>
        <span style="color: darkgray">(Build: <%= assembly.GetName().Version.Major %> am
                <%= Html.Raw(AssemblyLinkerTimestamp.Get(assembly).ToString("dd.MM.yyyy 'um' HH:mm")) %>)
        </span>
        <br />
        &nbsp;
            <br />
        <b>Rechtliches</b><br />
        <a href="<%=Links.TermsAndConditions %>">Nutzungsbedingungen (AGBs)</a><br />
        <a href="<%=Links.Imprint %>">Impressum & Datenschutzerklärung</a><br />
    </div>
    <div class="visible-xs visible-sm" style="clear: both"></div>

    <div class="FooterCol xxs-stack col-xs-6 col-md-2">
        <b>Hilfe</b><br />
        <a href="<%=Links.HelpFAQ() %>">FAQ</a><br />
        Robert: <span style="white-space: nowrap;">+49-0178-1866848</span>
    </div>

    <div class="FooterCol xxs-stack col-xs-6 col-md-2">
        <b>Mehr</b><br />
        <br />
        <a href="https://www.facebook.com/MemuchoWissen" target="_blank"><i class="fa fa-facebook-official" aria-hidden="true">&nbsp;</i>auf Facebook</a><br />
        <a href="https://twitter.com/memuchoWissen" target="_blank"><i class="fa fa-twitter" aria-hidden="true">&nbsp;</i>auf Twitter</a><br />
    </div>
</div>
<div class="Clearfix"></div>
