﻿<%@ Page Title="Masterarbeit Informatik bei memucho" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/About/Jobs.css" rel="stylesheet" />

<script type="text/javascript" >

    $(function () {
        $("span.mailme")
            .each(function() {
                var spt = this.innerHTML;
                var at = / at /;
                var dot = / dot /g;
                var addr = spt.replace(at, "@").replace(dot, ".");
                $(this).after('<a href="mailto:' + addr + '" title="Schreibe eine E-Mail">' + addr + '</a>');
                $(this).remove();
            });
    });
</script>    
    
<style>

    h3 {
        padding-bottom: 13px;
    }
            
    h4 {
        padding-top: 13px;
        padding-bottom: 13px;
    }
</style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


<div class="row">
    <div class="col-xs-12">
        <h1 class="PageHeader" style="margin-bottom: 25px; margin-top: 0px;"><span class="ColoredUnderline GeneralMemucho">Masterarbeit Informatik bei memucho</span></h1>
    </div>
</div>
    
<div class="row">
    <div class="col-xs-12" style="margin-top: 20px;" id="jobUXPraktikum">
        <div class="well">
            <p>
                memucho ist ein junges Start-up. Wir wollen es Nutzern erleichtern, sich interessante und wissenswerte Dinge zu merken, ihr Wissen zu organisieren
                und personalisiert zu lernen. Dabei fördern wir freie Bildungsinhalte, denn unsere Inhalte stehen unter einer offenen Lizenz (CC BY 4.0).
                Die Anwendung, die wir entwickeln, ist zudem Open Source 
                (<a href="https://github.com/TrueOrFalse/TrueOrFalse" target="_blank">Quelltext bei <i class="fa fa-github"></i> Github</a>). 
                Wir sind eine Ausgründung der Freien Universität Berlin, Prof. Raúl Rojas ist im Rahmen des EXIST-Gründerstipendiums unser Mentor.
            </p>
            <h3>Lerntechnologie: Wettbewerb von Vergessensmodellen</h3>
            <p>
                memucho bietet personalisiertes Lernen an und prognostiziert dem Nutzer, wann er was am besten lernt - und wieviel Zeit 
                er benötigt, um eine bestimmte Menge an Informationen zu lernen. Dafür haben wir eine Umgebung entwickelt, 
                bei der verschiedene Vorhersagemodelle in einem Wettbewerb gegeneinander antreten.
                Jedes Modell leistet eine zentrale Vorhersage: Kann ein bestimmter Nutzer ein bestimmtes Lernobjekt zu
                einem bestimmten Zeitpunkt korrekt beantworten? Trainiert und bewertet werden die Memorierungsmodelle anhand der vorliegenden historischen Lerndaten,
                und zwar für jede Lernsituation (= gebildet aus den Features zu den Antworten, den Fragen und perspektivisch den Nutzern) separat.
            </p>
            <p>
                Wir haben zu Testzwecken bereits drei einfache Referenz-Algorithmen entwickelt, die in diesem Wettbewerbssystem gegeneinander antreten.
                Die Ergebnisse (inkl. der bereits implementierten Features) sind <a href="<%= Links.AlgoInsightForecast() %>">hier öffentlich sichtbar</a>.
            </p>

            <h3>Vorschläge für Masterarbeiten</h3>
            
            <h4>Masterarbeit 1: "Verbesserung der Vorhersage-Qualität, durch dynamische Selektion des leistungsfähigesten Vorhersage-Modells zur Laufzeit"</h4>
            <p>
                Belegt wird die These, dass typischerweise ein Vorhersage-Modell-Mix zu einem besseren Ergebnis führt, 
                als ein einzelnes Vorhersage-Modell, da in den meisten Situationen einzelnen Vorhersagen auf sehr unterschiedlichen Datenmengen beruhen.
                Beschrieben wird die Vorauswahl von Vorhersage-Modellen aufgrund ihrer Eigenschaften und 
                ein Vorgehen für die dynamische Auswahl zur Laufzeit eines Programmes. 
            </p>
            <h4>Masterarbeit 2: "Vergleich verschiedener Vorhersage-Modelle (Predective Modells), in unterschiedlichen Datensituationen"</h4>
            <p>
                Gegenstand der Masterarbeit ist der "Vergleich verschiedener Vorhersage-Modelle (Predective Modells), in unterschiedlichen Datensituationen".
                Hierbei werden die Vorhersageeigenschaften miteinander Verglichen. 
                Unsere Software bietet bereits eine Plattform, um die Vorhersageeigenschaften 
                von Algorithmen zu bewerten und zu vergleichen (ähnlich <a href="http://kaggle.com">Kaggle</a>). 
            </p>
            
            <h4>Masterarbeit 3: Entwicklung eines Vergessensmodells auf Basis von historischen Daten.</h4>
            <p>
                Gegenstand der Masterarbeit ist die Entwicklung eines leistungsfähigen Vergessensmodell, 
                das im Wettbewerb bisherige Modelle deutlich überbietet.
            </p>
            <h4>Masterarbeit 4: Deine Idee </h4>
            <p>
                Wir treffen uns und überlegen welches Thema zu Dir und zu uns passen kann. 
            </p>
            
            <h4>Die Vorteile für dich sind:</h4>    
            <ul>
                <li>Du arbeitest mit netten Leuten</li>
                <li>Deine Ergebnisse werden direkt umgesetzt und verbessern die vorhandene Anwendung</li>
                <li>Du kannst mit einem interessanten Datensatz arbeiten und lernst viel darüber, wie Menschen lernen</li>
                <li>Du machst die Welt ein bisschen besser, weil du offene Bildungsinhalte und Open Source unterstützt</li>
                <li>Es besteht die Möglichkeit, eine Aufwandsentschädigung zu erhalten</li>
            </ul>
            <h3>Fragen? Interesse?</h3>
            <p>
                Wenn du Interesse hast und/oder vielleicht noch einzelne Fragen offen sind, kannst du dich gerne bei Robert 
                melden (+49-178-1866848, <span class="mailme">robert at memucho dot de</span>). 
                Er kann dir auch mehr über die Technologie und die Implementierung erzählen. 
            </p>
        </div>
    </div>
</div>


</asp:Content>
