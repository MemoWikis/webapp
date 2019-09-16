<%@ Page Title="Jobs bei memucho" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="System.Web.Mvc.ViewPage<BaseModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/About/Jobs.css" rel="stylesheet" />
    <%= Scripts.Render("~/bundles/mailto") %>

<% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Jobs", Url = Links.Jobs(), ToolTipText = "Jobs"});
   Model.TopNavMenu.IsCategoryBreadCrumb = false; %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="row">
    <div class="col-xs-12">
        <h1 class="PageHeader" style="margin-bottom: 25px; margin-top: 0px;">Jobs &amp; Praktika bei memucho</h1>
    </div>
</div>
    
<div class="row">
    <div class="col-xs-12 col-md-8">
        <p>
            Bei memucho gibt es aktuell keine Job-Angebote
        </p>
 <%--       <ul>
            <li><a href="#contentmanager">Contentmanager (m/w)</a></li>
         
        </ul>--%>
    </div>
</div>
<%--        	  ⦁	 Stellenbeginn: Ab sofort. Umfang: 30h/Woche. Arbeitsort: Schönes Büro in Wildau, nähe Bahnhof und Uni. Die Stelle wird gefördert durch das Programm Gründung innovativ der ILB mit Geldern aus dem Europäischen Fond für regionale Entwicklung (EFRE). Kontakt: Robert Mischke<span class="mailme"> robert at memucho dot de</span>
<div class="row">
    <div class="col-xs-12 col-md-8" style="margin-top: 20px;" id="contentmanager">
        <div class="well">
            <h2 class="PageHeader">Content-Manager (m/w)</h2>
            <p>
                memucho ist ein neues Lerntool, mit dem wir es Lernenden erleichtern, 
                sich interessante und wissenswerte Dinge zu merken, ihr Wissen zu organisieren und personalisiert zu lernen. 
                Dabei setzen wir auf freie Bildungsinhalte und stellen alle unsere Inhalte unter eine offene Lizenz. 
                Die Anwendung, die wir entwickeln, ist zudem Open Source. Das Unternehmen memucho selbst versteht sich als gemeinwohlorientiert.
            </p>
            <p>
                Wir suchen nun eine(n)
                Content-Manager/-in, als zentrale/n Ansprechpartner/in für die Erstellung und Optimierung sämtlicher
                redaktioneller Inhalte auf der Webseite memucho.de.
                Die im weitesten Sinne redaktionellen Tätigkeiten haben zum Ziel, die (interaktiven) Lerninhalte bei memucho nachhaltig auszubauen
                und damit für Lernende einen konkreten Nutzen zu stiften.
            </p>
            <p class="listTitle">
                Zentrale Aufgabenfelder sind:
            </p>
            <ul>
                <li>
                    Gewinnung, Betreuung und Motivation von Nutzer/innen bei der Erstellung von Inhalten sowie Identifikation, Gewinnung und Betreuung von Kooperationspartnern
                </li>
                <li>
                    Erstellung und Zusammenstellung von digitalen BIldungsinhalten und Weiterentwicklung der Nutzer/innenansprache auf der Webseite
                </li>
                <li>
                    Prüfen von Lizenzen für Bilder und Texte
                </li>
                <li>
                    Aufbau und Betreuung unserer Twitter- und Facebook-Accounts sowie eines Blogs
                </li>
                <li>
                    Vorstellung von memucho bei Bildungsveranstaltungen/Netzwerkarbeit
                </li>
                
            </ul>
            <p class="listTitle">
                Unsere Anforderungen:
            </p>
            <ul>
                <li>
                    Sehr sicherer Umgang mit der deutschen Sprache
                </li>
                <li>
                    Eigenständiges und eigenverantwortliches Arbeiten
                </li>
                <li>
                    Affinität zu digitalen Produkten
                </li>
            </ul>
            <p class="listTitle">
                Wir bieten:
            </p>
            <ul>
                <li>
                    Ein kleines, aber feines Team mit sehr flachen Hierarchien
                </li>
                <li>
                    Abwechslungsreiche Tätigkeit, bei der Ihre Talente zur Geltung kommen
                </li>
                <li>
                    Die Möglichkeit, etwas Gutes zu tun bei der Digitalisierung von Bildung und der Förderung freier Bildungsmaterialien
                </li>
                <li>
                    Möglichkeit für Home Office
                </li>
            </ul>
            <p>
                Stellenbeginn: Ab sofort<br/>
                Umfang: 30h/Woche<br />
                Arbeitsort: Schönes Büro in Wildau, nähe Bahnhof und Uni
            </p>
            <p>
                Die Stelle wird gefördert durch das Programm Gründung innovativ der ILB mit Geldern aus dem Europäischen Fond für regionale Entwicklung (EFRE).
            </p>
            <p>
                Kontakt: Robert Mischke (<span class="mailme">robert at memucho dot de</span>)
            </p>            
        </div>
    </div>
</div>


<div class="row">
    <div class="col-xs-12">
        <a href="#top" class="greyed"><i class="fa fa-caret-up">&nbsp;</i>Nach oben</a>
    </div>
</div>--%>

</asp:Content>
