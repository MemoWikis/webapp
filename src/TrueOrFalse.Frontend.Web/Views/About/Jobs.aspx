<%@ Page Title="Jobs bei memucho" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/About/Jobs.css" rel="stylesheet" />
    <%= Scripts.Render("~/bundles/mailto") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="row">
    <div class="col-xs-12">
        <h1 class="PageHeader" style="margin-bottom: 25px; margin-top: 0px;"><span class="ColoredUnderline GeneralMemucho">Jobs &amp; Praktika bei memucho</span></h1>
    </div>
</div>
    
<div class="row">
    <div class="col-xs-12 col-md-8">
        <p>
            Bei memucho gibt es aktuell folgende Job-Angebote:    
        </p>
        <ul>
            <li><a href="#contentManager">Content-Manager (m/w)</a></li>
            <li><a href="#jobInhalte">Inhalte erstellen, pflegen und koordinieren (Nebenjob)</a></li>
        </ul>
    </div>
</div>

<div class="row">
    <div class="col-xs-12 col-md-8" style="margin-top: 20px;" id="contentManager">
        <div class="well">
            <h2 class="PageHeader">Content-Manager (m/w)</h2>
            <p>
                memucho ist ein innovatives Start-up und betreibt die Web-Anwendung memucho.de, einen digitalen Bildungsassistent. 
                Ziel von memucho ist, es Lernenden zu erleichtern, sich interessante und wissenswerte Dinge zu merken, ihr Wissen 
                zu organisieren und personalisiert zu lernen. Dabei setzen wir auf freie Bildungsinhalte und stellen alle unsere 
                Inhalte unter eine offene Lizenz. Die Anwendung, die wir entwickeln, ist zudem Open Source. 
                Das Unternehmen memucho selbst versteht sich als gemeinwohlorientiert.
            </p>
            <p>
                Wir suchen eine(n) Content-Manager/-in, als zentrale Ansprechpartnerin für die Erstellung und Optimierung 
                sämtlicher redaktioneller Inhalte auf der Webseite memucho.de. Die im weitesten Sinne redaktionellen Tätigkeiten 
                haben zum Ziel, die (interaktiven) Lerninhalte bei memucho nachhaltig auszubauen und damit für Lernende einen 
                konkreten Nutzen zu stiften.
            </p>
            <p class="listTitle">
                Zentrale Aufgabenfelder sind:
            </p>
            <ul>
                <li>
                    Gewinnung, Betreuung und Motivation von Nutzern bei der Erstellung von Inhalten sowie Identifikation, Gewinnung und Betreuung von Kooperationspartnern
                </li>
                <li>
                    Erstellung und Zusammenstellung von digitalen BIldungsinhalten und Weiterentwicklung der Nutzeransprache auf der Webseite
                </li>
                <li>
                    Prüfen von Lizenzen für Bilder und Texte
                </li>
                <li>
                    Aufbau und Betreuung unserer Twitter- und Facebook-Accounts
                </li>
                <li>
                    Vorstellung von memucho bei Bildungsveranstaltungen
                </li>
            </ul>
            <p class="listTitle">
                Unsere Anforderungen:
            </p>
            <ul>
                <li>
                    Abgeschlossenes Studium der Geistes- oder Sozialwissenschaften
                </li>
                <li>
                    Sehr sicherer Umgang mit der deutschen Sprache
                </li>
                <li>
                    Eigenständiges und eigenverantwortliches Arbeiten
                </li>
                <li>
                    Organisationstalent
                </li>
                <li>
                    Idealerweise erste Berufserfahrungen im Bildungsbereich und/oder der Öffentlichkeitsarbeit
                </li>
                <li>
                    Affinität zu digitalen Produkten
                </li>
            </ul>
            <p class="listTitle">
                Wir bieten dir
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
                    Ein faires Gehalt
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
</div>

    
<div class="row">
    <div class="col-xs-12 col-md-8" style="margin-top: 20px;" id="jobInhalte">
        <div class="well">
            <h2 class="PageHeader">Inhalte erstellen, pflegen und koordinieren (Nebenjob)</h2>
            <p>
                memucho ist ein junges Start-up. Wir wollen es Nutzern erleichtern, sich interessante und wissenswerte Dinge zu merken, ihr Wissen zu organisieren
                und personalisiert zu lernen. Dabei fördern wir freie Bildungsinhalte, denn unsere Inhalte stehen unter einer offenen Lizenz.
                Die Anwendung, die wir entwickeln, ist zudem Open Source 
                (<a href="https://github.com/TrueOrFalse/TrueOrFalse" target="_blank">Quelltext bei <i class="fa fa-github"></i> Github</a>).
            </p>
            <p>
                Wir sind gerade erst online gegangen. Jetzt brauchen wir dich, damit sich unsere Seite mit Inhalten füllt und wir zeigen können, 
                wie toll man damit lernen kann. Perspektivisch kannst du die Inhalte-Erstellung mit koordinieren und andere kommunikative Aufgaben
                mit übernehmen.
            </p>
            <p class="listTitle">
                Deine Aufgaben sind
            </p>
            <ul>
                <li>Erstellen von hochwertigen Lerninhalten bei memucho (<a href="<%= Links.SetsAll() %>">Beispiele</a>)</li>
                <li>Entwickeln eigener guter Ideen zu Lernsets</li>
                <li>Inhaltspflege vorhandener Inhalte (Rechtschreibung, Ergänzung von Themen, Quellen hinzufügen...)</li>
            </ul>
            <p class="listTitle">
                Dein perfektes Profil wäre:
            </p>
            <ul>
                <li>Du bist sehr sicher in deutscher Rechtschreibung</li>
                <li>Idealerweise hast du journalistisches Interesse und/oder Erfahrung in sozialen Netzwerken</li>
                <li>Du kannst selbständig und genau arbeiten und bist zuverlässig</li>
                <li>Du bist kreativ, hast eigene Ideen und kannst gutes Feedback geben</li>
                <li>Du hast Freude am Lernen</li>
            </ul>
            
            <p class="listTitle">
                Wir bieten dir
            </p>
            <ul>
                <li>Du arbeitest in einem jungen kleinen Team mit flachen Hierarchien</li>
                <li>Die schnelle Verwirklichung deiner guten Ideen</li>
                <li>Du arbeitest an Themen, die dich selbst interessieren und erweiterst dein Wissen</li>
                <li>Du machst die Welt ein bisschen besser, weil du offene Bildungsinhalte erstellst (Creative-Commons-Lizenz)</li>
                <li>Sehr flexible Arbeitszeiten</li>
                <li>Hauptsächlich Home-Office (oder Café-Office, oder Train-Office... wo immer du arbeiten möchtest)</li>
                <li>Faire Bezahlung auf Honorarbasis</li>
                <li>In den nächsten Monaten wird es bei uns viele neue spannende Aufgaben geben. Vielleicht auch für dich?</li>
            </ul>
            <h3 style="margin-top: 25px;">Interesse?</h3>
            <p>
                Schicke deine Kurzbewerbung per E-Mail an Christof (<span class="mailme">christof at memucho dot de</span>) und erzähle uns, 
                warum du auf diesen Job wie die perfekte Antwort auf eine Frage passt. 
                Erzähle uns dabei auch kurz, welche Interessen du hast und wie du zeitlich verfügbar bist. 
                Ein formaler Lebenslauf und ähnliches ist nicht nötig.
            </p>            
        </div>
    </div>
</div>

</asp:Content>
