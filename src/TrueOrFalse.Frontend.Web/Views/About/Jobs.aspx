<%@ Page Title="Jobs bei memucho" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="System.Web.Mvc.ViewPage<BaseModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/About/Jobs.css" rel="stylesheet" />
    <%= Scripts.Render("~/bundles/mailto") %>

<% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Jobs", Url = "/Jobs", ToolTipText = "Jobs"});
   Model.TopNavMenu.IsCategoryBreadCrumb = false; %>
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
            Bei memucho gibt es aktuell folgende Praktika und Nebenjobs:    
        </p>
        <ul>
            <li><a href="#jobUXPraktikum">Praktikum UX-Design & Usability</a></li>
            <li><a href="#jobWiWi">BWL- & VWL-Studenten: Karteikarten erstellen zu deinen Lerninhalten (Nebenjob)</a></li>
            <li><a href="#jobInhalte">Inhalte erstellen, pflegen und koordinieren (Nebenjob)</a></li>
        </ul>
    </div>
</div>

<div class="row">
    <div class="col-xs-12 col-md-8" style="margin-top: 20px;" id="jobUXPraktikum">
        <div class="well">
            <h2 class="PageHeader">Praktikum UX-Design & Usability</h2>
            <p>
                memucho ist ein junges Start-up. Wir wollen es Nutzern erleichtern, sich interessante und wissenswerte Dinge zu merken, ihr Wissen zu organisieren
                und personalisiert zu lernen. Dabei fördern wir freie Bildungsinhalte, denn unsere Inhalte stehen unter einer offenen Lizenz.
                Die Anwendung, die wir entwickeln, ist zudem Open Source 
                (<a href="https://github.com/TrueOrFalse/TrueOrFalse" target="_blank">Quelltext bei <i class="fa fa-github"></i> Github</a>).
            </p>
            <p>
                Unsere Anwendung ist in der öffentlichen Beta-Phase. Uns geht es jetzt darum, zu verstehen, wie unsere Zielgruppe mit der Anwendung umgeht, wo die Hindernisse liegen 
                und wo wir die Funktionalität so anpassen können, dass ein hoher Nutzen entsteht. Es wäre toll, wenn du uns dabei helfen könntest.
            </p>
            <p class="listTitle">
                Deine Aufgaben sind
            </p>
            <ul>
                <li>Konzeption, Durchführung und Auswertung von Nutzertests, insbs. mit Schülern, Studenten und Lehrenden</li>
                <li>Erstellung und Weiterentwicklung von Prototypen zur Verbesserung einzelner Teile der vorhandenen Web-Anwendung</li>
                <li>Verbesserung von Navigations- und Interaktionselementen</li>
                <li>Weiterentwicklung der User Experience und des Designs</li>
            </ul>
            <p class="listTitle">
                Dein perfektes Profil wäre:
            </p>
            <ul>
                <li>Du studierst Kommunikationsdesign, Interaktionsdesign, Informationsdesign oder ähnliches mindestens im 3. Semester 
                    (oder hast einen vergleichbaren Hintergrund) und hast schon erste Erfahrungen in den oben genannten Arbeitsfeldern gemacht</li>
                <li>Du kannst mit Programmen zur Erstellung von Prototypen wie Invision oder Adobe Experience Design umgehen 
                    oder bist bereit, dir während des Praktikums die Benutzung selbständig beizubringen</li>
                <li>Du kannst dich an Layout-Frameworks wie Google Material Design einarbeiten und dich daran orientieren</li>
                <li>Du bist kreativ, hast eigene Ideen und kannst daran selbständig und genau arbeiten</li>
                <li>Du hast Freude am Lernen</li>
                <li>Du hast mindestens 3 Monate Zeit (Teilzeitmodelle möglich)</li>
            </ul>
            <p class="listTitle">
                Wir bieten dir
            </p>
            <ul>
                <li>Du arbeitest in einem jungen kleinen Team mit flachen Hierarchien und bist stark involviert</li>
                <li>Orientierung und Feedback von einem erfahrenen Grafiker, mit dem wir zusammenarbeiten</li>
                <li>Deine guten Ideen können schnell verwirklicht werden</li>
                <li>Du machst die Welt ein bisschen besser, weil du offene Bildungsinhalte und Open Source unterstützt</li>
                <li>Flexible Arbeitszeiten in einem schönen Büro in Berlin-Kreuzberg</li>
                <li>Home-Office (oder Café-Office, oder Train-Office... wo immer du arbeiten möchtest) nach Absprache möglich, bei mind. 3 Präsenztagen</li>
                <li>Teilzeit-Modelle gerne möglich</li>
                <li>Eine monatliche Aufwandsentschädigung</li>
            </ul>
            <h3 style="margin-top: 25px;">Interesse?</h3>
            <p>
                Das freut uns! Schaue dich auf unserer Seite memucho.de um, damit du einen Eindruck von deinem "Arbeitsgegenstand" gewinnst.
                Bei allen Fragen kannst du dich gerne an Christof wenden (Tel: +49-1577-6825707). Schicke dann deine Kurzbewerbung per E-Mail 
                an Christof (<span class="mailme">christof at memucho dot de</span>), gib uns einen kleinen Eindruck von deinen Erfahrungen und erzähle uns, 
                warum du auf dieses Praktikum wie die perfekte Antwort auf eine Frage passt. 
            </p>            
        </div>
    </div>
</div>
    
    
    
<%--
<div class="row">
    <div class="col-xs-12">
        <a href="#top" class="greyed"><i class="fa fa-caret-up">&nbsp;</i>Nach oben</a>
    </div>
</div>

<div class="row">
    <div class="col-xs-12 col-md-8" style="margin-top: 20px;" id="jobUXPraktikum">
        <div class="well">
            <h2 class="PageHeader">Du lernst mit Karteikarten? Teste memucho und belohne dich mit Pizza!</h2>
            <p>
                Du bist Schüler oder Schülerin in der Sek II, studierst gerade oder machst eine Ausbildung? 
                Um dich besser auf Prüfungen vorzubereiten erstellst du dir selber Karteikarten (egal ob auf Papier oder digital)? 
                Das finden wir gut! Wir wollen das Lernen mit Karteikarten noch besser machen und deshalb brauchen wir dich.
            </p>
            <p>
                Du kannst uns nämlich helfen, memucho besser zu machen. Und zwar so: Du kommst für 1-2 Stunden zu uns ins Büro (in Kreuzberg), 
                testest unsere Anwendung und sagst uns, was du gut findest und was nicht. Dafür bekommst du von uns Pizza und Getränke.
                Außerdem solltest du deine Karteikarten mitbringen, damit wir sehen, wie du gerade lernst. Vielleicht können wir sie auch übertragen?
            </p>
            <h3 style="margin-top: 25px;">Interesse?</h3>
            <p>
                Das freut uns! Schicke eine kurze E-Mail an Christof (<span class="mailme">christof at memucho dot de</span>) 
                und schreibe, wofür du gerade lernst, wofür du demnächst lernen wirst und wann du am besten Zeit hast, 
                um memucho bei uns zu testen (Welche Tage? Vormittags/nachmittags?). 
                Bei Fragen kannst du dich gerne bei Christof melden (Tel: +49-1577-6825707). 
            </p>
        </div>
    </div>
</div>--%>

<div class="row">
    <div class="col-xs-12">
        <a href="#top" class="greyed"><i class="fa fa-caret-up">&nbsp;</i>Nach oben</a>
    </div>
</div>

    
<div class="row">
    <div class="col-xs-12 col-md-8" style="margin-top: 20px;" id="jobWiWi">
        <div class="well">
            <h2 class="PageHeader">BWL- & VWL-Studenten: Karteikarten erstellen zu deinen Lerninhalten (Nebenjob)</h2>
            <p>
                memucho ist ein junges Start-up. Wir wollen es Nutzern erleichtern, sich interessante und wissenswerte Dinge zu merken, ihr Wissen zu organisieren
                und personalisiert zu lernen. Dabei fördern wir freie Bildungsinhalte, denn unsere Inhalte stehen unter einer offenen Lizenz.
                Die Anwendung, die wir entwickeln, ist zudem Open Source 
                (<a href="https://github.com/TrueOrFalse/TrueOrFalse" target="_blank">Quelltext bei <i class="fa fa-github"></i> Github</a>).
            </p>
            <p>
                Wir sind gerade erst online gegangen. Jetzt brauchen wir dich, damit auch alle BWL und VWL-Studenten besser mit memucho lernen können. 
                Wir wollen Lernkarten erstellen für Vorlesungsinhalte in wirtschaftswissenschaftlichen Fächern und dafür brauchen wir dich. 
                Perspektivisch kannst du die Inhalte-Erstellung mit koordinieren und andere kommunikative Aufgaben mit übernehmen.
            </p>
            <p class="listTitle">
                Deine Aufgaben sind:
            </p>
            <ul>
                <li>Erstellen von hochwertigen Lerninhalten bei memucho v.a. zu Einführungsthemen der BWL und VWL (<a href="<%= Links.SetsAll() %>">Beispiele für Lernsets</a>)</li>
                <li>Entwickeln eigener guter Ideen zu Lernsets</li>
                <li>Inhaltspflege vorhandener Inhalte (Rechtschreibung, Ergänzung von Themen, Quellen hinzufügen...)</li>
            </ul>
            <p class="listTitle">
                Dein perfektes Profil wäre:
            </p>
            <ul>
                <li>Du studierst BWL oder VWL mindestens im 3. Semester und hast mindestens gute Leistungen, vielleicht hast du sogar schon deinen Bachelor abgeschlossen</li>
                <li>Du bist sehr sicher in deutscher Rechtschreibung</li>
                <li>Du kannst selbständig und genau arbeiten und bist zuverlässig</li>
                <li>Du hast Freude am Lernen, auch mit digitalen Werkzeugen</li>
            </ul>
            
            <p class="listTitle">
                Wir bieten dir:
            </p>
            <ul>
                <li>Du arbeitest in einem jungen kleinen Team mit flachen Hierarchien</li>
                <li>Die schnelle Verwirklichung deiner guten Ideen</li>
                <li>Du arbeitest an Themen, die dich selbst interessieren und erweiterst dein Wissen</li>
                <li>Du machst die Welt ein bisschen besser, weil du offene Bildungsinhalte erstellst (Creative-Commons-Lizenz)</li>
                <li>Sehr flexible Arbeitszeiten</li>
                <li>Hauptsächlich Home-Office (oder Café-Office, oder Train-Office... wo immer du arbeiten möchtest)</li>
                <li>Faire Bezahlung nach Stunden</li>
                <li>In den nächsten Monaten wird es bei uns viele neue spannende Aufgaben geben. Vielleicht auch für dich?</li>
            </ul>
            <h3 style="margin-top: 25px;">Interesse?</h3>
            <p>
                Schicke deine Kurzbewerbung per E-Mail an Christof (<span class="mailme">christof at memucho dot de</span>) und erzähle uns, 
                was und wo du studierst, wo du im Studium stehst, wie/womit du bisher für Prüfungen lernst und wie viele Stunden in der Woche
                du Zeit hättest. Ein formaler Lebenslauf und ähnliches ist nicht nötig, gib aber auf jeden Fall eine Telefonnummer an, 
                damit wir dich zurückrufen können. Wir freuen uns auf dich!
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
