<%@ Page Title="Jobs bei memucho" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">

<script type="text/javascript" >

    /* Source http://www.html-advisor.com/javascript/hide-email-with-javascript-jquery/ */
    $(function () {
        var spt = $('span.mailme');
        var at = / at /;
        var dot = / dot /g;
        var addr = $(spt).text().replace(at, "@").replace(dot, ".");
        $(spt).after('<a href="mailto:' + addr + '" title="Schreibe eine E-Mail">' + addr + '</a>').hover(function () { window.status = "Schreibe eine E-Mail!"; }, function () { window.status = ""; });
        $(spt).remove();
    });
</script>    
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


<div class="row">
    <div class="col-xs-12">
        <h1 class="PageHeader" style="margin-bottom: 15px; margin-top: 0px;"><span class="ColoredUnderline GeneralMemucho">Jobs &amp; Praktika bei memucho</span></h1>
    </div>
</div>
<div class="row">
    <div class="col-xs-12 col-sm-8 rowBase" style="margin-top: 20px;" id="jobUXPraktikum">
        <h2>Praktikum UX-Design & Usability</h2>
        <p>
            memucho ist ein junges Start-up. Wir wollen es Nutzern erleichern, sich interessante und wissenswerte Dinge zu merken, ihr Wissen zu organisieren
            und personalisiert zu lernen. Dabei fördern wir freie Bildungsinhalte, denn unsere Inhalte stehen unter einer offenen Lizenz.
            Die Anwendung, die wir entwickeln, ist zudem Open Source.
        </p>
        <p>
            Unsere Anwendung ist in der öffentlichen Beta-Phase. Uns geht es jetzt darum, zu verstehen, wie unsere Zielgruppe mit der Anwendung umgeht, wo die Hindernisse liegen 
            und wo wir die Funktionalität so anpassen können, dass ein "Suchtpotenzial" entsteht. Es wäre toll, wenn du uns dabei helfen könntest.
        </p>
        <p>
            <strong>Deine Aufgaben sind</strong>
            <ul>
                <li>Konzeption, Durchführung und Auswertung von Nutzertests, insbs. mit Schülern und Lehrenden</li>
                <li>Erstellung und Weiterentwicklung von Prototypen zur Verbesserung einzelner Teile der vorhandenen Web-Anwendung</li>
                <li>Verbesserung von Navigations- und Interaktionselementen</li>
                <li>Weiterentwicklung der User Experience und des Designs</li>
            </ul>
            <strong>Dein perfektes Profil wäre:</strong>
            <ul>
                <li>Du studierst Kommunikationsdesign, Interaktionsdesign, Kommunikationsdesign, Informationsdesign oder ähnliches mindestens im 3. Semester 
                    (oder hast einen vergleichbaren Hintergrund) und hast schon erste Erfahrungen in den oben genannten Arbeitsfeldern gemacht</li>
                <li>Du kannst mit Programmen zur Erstellung von Prototypen wie Invision oder Adobe Experience Design umgehen 
                    oder bist bereit, dir während des Praktikums die Benutzung selbständig beizubringen</li>
                <li>Du kannst mit Layout-Frameworks wie Google Material Design umgehen</li>
                <li>Du bist kreativ, hast eigene Ideen und kannst daran selbständig und genau arbeiten</li>
                <li>Du hast Freude am Lernen</li>
                <li>Du hast mindestens 3 Monate Zeit (Teilzeitmodelle möglich)</li>
            </ul>
            
            <strong>Wir bieten dir</strong>
            <ul>
                <li>Du arbeitest in einem jungen kleinen Team ohne Hierarchien und bist stark involviert</li>
                <li>Orientierung und Feedback von einem erfahrenen Grafiker</li>
                <li>Deine guten Ideen können schnell verwirklicht werden</li>
                <li>Du machst die Welt ein bisschen besser, weil du offene Bildungsinhalte und Open Source unterstützt</li>
                <li>Flexible Arbeitszeiten in einem schönen Büro in Berlin-Kreuzberg</li>
                <li>Home-Office (oder Café-Office, oder Train-Office... wo immer du arbeiten möchtest) nach Absprache möglich, bei mind. 3 Präsenztage</li>
                <li>Teilzeit-Modelle gerne möglich</li>
                <li>Eine monatliche Aufwandsentschädigung</li>
            </ul>
        </p>
        <h3 style="margin-top: 25px;">Interesse?</h3>
        <p>
            Das freut uns! Schaue dich auf unserer Seite memucho.de um, damit du einen Eindruck von deinem "Arbeitsgegenstand" gewinnst.
            Bei allen Fragen kannst du dich gerne an Christof wenden (Tel: +49-1577-6825707). Schicke dann deine Kurzbewerbung per E-Mail 
            an Christof (<span class="mailme">christof at memucho dot de</span>), gib uns einen kleinen Eindruck von deinen Erfahrungen und erzähle uns, 
            warum du auf dieses Praktikum wie die perfekte Antwort auf eine Frage passt. 
        </p>
    </div>
    <div class="col-xs-12 col-sm-4" style="text-align: center;">

    </div>
</div>


<div class="row">
    <div class="col-xs-12 col-sm-8 rowBase" style="margin-top: 20px;" id="jobInhalte">
        <h3>Inhalte erstellen, pflegen und koordinieren (Nebenjob)</h3>
        <p>
            memucho ist ein junges Start-up. Wir wollen Lernen personalisieren, damit alle effizienter und mit mehr Spaß lernen können. 
            Dabei fördern wir freie Bildungsinhalte, denn unsere Inhalte stehen unter einer offenen Lizenz.
            Die Anwendung, die wir entwickeln, ist zudem Open Source.
        </p>
        <p>
            Wir sind gerade erst online gegangen. Jetzt brauchen wir dich, damit sich unsere Seite mit Inhalten füllt und wir zeigen können, 
            wie toll man damit lernen kann. Perspektivisch kannst du die Inhalte-Erstellung mit koordinieren und andere kommunikative Aufgaben
            mit übernehmen.
        </p>
        <p>
            Deine Aufgaben sind
            <ul>
                <li>Erstellen von hochwertigen Fragesätzen bei memucho zu Themen der Allgemeinbildung <a href="<%= Links.SetsAll() %>">(Beispiele)</a></li>
                <li>Entwickeln eigener guter Ideen zu Fragesätzen</li>
                <li>Inhaltspflege vorhandener Inhalte (Rechtschreibung, Ergänzung von Themen, Quellen hinzufügen...)</li>
            </ul>
            Dein perfektes Profil wäre:
            <ul>
                <li>Du bist sehr sicher in deutscher Rechtschreibung</li>
                <li>Idealerweise hast du journalistisches Interesse und/oder Erfahrung in sozialen Netzwerken</li>
                <li>Du kannst selbständig und genau arbeiten und bist zuverlässig</li>
                <li>Du bist kreativ und hast eigene Ideen</li>
                <li>Du hast Freude am Lernen</li>
            </ul>
            
            Wir bieten dir
            <ul>
                <li>Du arbeitest in einem jungen kleinen Team ohne Hierarchien</li>
                <li>Die schnelle Verwirklichung deiner guten Ideen</li>
                <li>Du arbeitest an Themen, die dich selbst interessieren und erweiterst dein Wissen</li>
                <li>Du machst die Welt ein bisschen besser, weil du offene Inhalte erstellst (Creative-Commons-Lizenz)</li>
                <li>Sehr flexible Arbeitszeiten</li>
                <li>Hauptsächlich Home-Office (oder Café-Office, oder Train-Office... wo immer du arbeiten möchtest)</li>
                <li>Faire Bezahlung auf Honorarbasis</li>
                <li>In den nächsten Monaten wird es bei uns viele neue spannende Aufgaben geben. Vielleicht auch für dich?</li>
            </ul>
        </p>
        <h4>Interesse?</h4>
        <p>
            Schicke deine Kurzbewerbung per E-Mail an Christof (<span class="mailme">christof at memucho dot de</span>) und erzähle uns, 
            warum du auf diesen Job wie die perfekte Antwort auf eine Frage passt. 
            Erzähle uns dabei auch kurz, welche Interessen du hast und wie du zeitlich verfügbar bist. 
            Ein formaler Lebenslauf und ähnliches ist nicht nötig.
        </p>
    </div>
    <div class="col-xs-12 col-sm-4" style="text-align: center;">

    </div>
</div>

</asp:Content>
