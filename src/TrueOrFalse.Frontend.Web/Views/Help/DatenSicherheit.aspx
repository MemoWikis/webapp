<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<HelpModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="box box-main">
    <h2><a href="<%= Url.Action(Links.HelpWillkommen, Links.HelpController) %>">Hilfe</a> - Datensicherheit & Datenschutz</h2>
    <div class="box-content" style="min-height: 120px; clear: both; padding-top: 10px;">
        <h4>Überblick</h4>
            
        <p>
            Für uns sind Datensicherheit und Datenschutz entscheidende Themen. 
            Gerade weil wir viele  Daten sammeln, sehen wir eine besondere Verantwortung darin Daten zu schützen und transparent zu sein. 

            Die folgenden Abschnitte gehen auf unterschiedliche Aspkete von Datenschutz und Datensicherheit ein. 
        </p>
            
        <p>
            Prüft uns!
            Da die RIOFA-Software Open-Source ist, also der Quelltext 
            von jederman eingesehen werden kann, kann vieles was wir dokumentieren 
            direkt geprüft werden. 
            Ihr könnt uns auch jeden Montag-Nachmittag ab 15Uhr im <a href="<%= Url.Action("Willkommen", "Help") %>">Büro</a> besuchen. 
            Klassisch erreicht Ihr uns auch per <a href="<%= Url.Action("Willkommen", "Help") %>">Email und Telefon.</a>
        </p>
            
            
        Themen<br/>
        <ul>
            <li><a href="#welcheDatenWerdenGesammelt">Welche Daten werden gesammelt und warum</a></li>
            <li><a href="#wasTunWirMitDenDaten">Was tun wir mit den Daten (und was nicht)</a></li>
            <li><a href="#openSourceUndSicherheit">Open Source und Sicherheit</a></li>
            <li><a href="#wieSicherSindMeineDaten">Wie Sicher sind meine Daten</a></li>
            <li><a href="#passwortSicherheit">Passwort Sicherheit</a></li>
            <li><a href="#teamVonRioafZugriff">Team von RIOFA Zugriff auf Nutzerkonto erlauben</a></li>
        </ul>
        <br/>
        <h5><a name="welcheDatenWerdenGesammelt">Welche Daten werden gesammelt und warum</a></h5>

        <p>
            Wir probieren möglichst alle Interaktionen mit Fragen und Antworten zu sammeln. 
            Wie oft wurde eine Frage angesehen, wie oft gemerkt, wie oft wurde eine Frage beantwort und zu welchem Zeitpunkt,
            wir sammeln sogar die konkrete Antwort. Wir sammeln, wenn möglich, Daten personifziert, also 
            nicht anonymisiert.
        </p>
        <p>
            Wir versprechen uns davon Lernen zu verbessern. Zentral dabei ist, das wir den idealen 
            Zeitpunkt für eine Lern-Wiederholung ermittlen können. Und je mehr wir über das 
            Lernverhalten von möglichst vielen Benutzern an Daten gesammelt haben, 
            desto besser können unsere Algorithmen arbeiten.                
        </p>
        <p>
            Wir sammeln Nutzerdaten, um RIOFA zu einem möglichst 
            nützlichem Werkzeug zu machen und nicht um  Nutzerdaten zu verkaufen. 
        </p>
            
        <h5><a name="wasTunWirMitDenDaten">Was tun wir mit den Daten (und was nicht)</a></h5>
        <p>
            Wir nutzen Daten zum Beispeich für unsere Lernalalgorithmen oder um Wissen nach Qualität zu gewichten, also 
            Programmfunktionen.
        </p>
        <p>
            Zukünftig möchten wir statistische Daten aufbereiten und
            jedermann als freies Wissen zur Verfügung stellen.
            Bei der statistischen Aufbereitung achten wir auf Anonymität.
        </p>
        <p>
            Alles was wir mit Daten tun, tun wir transparent. 
            <b>Wir werden keine Nutzer- und Nutzungsdaten an Dritte verkaufen.</b>
            Öffentliche Inhalte die von Nutzern erstellt werden, unterliegen einer Creative Commons Lizenz
            und können von jedermann kostenfrei und ohne Einschränkung genutzt werden. 
            Fragen und Fragesätze werden komfortabel hertunterladbar sein. 
            Private Fragen sind tabu und bleiben geheim. 
        </p>
            
        <h5><a name="openSourceUndSicherheit">Open Source und Sicherheit</a></h5>
            

        <h5><a name="wieSicherSindMeineDaten">Wie Sicher sind meine Daten</a></h5>

        <h5><a name="passwortSicherheit">Passwort Sicherheit</a></h5>
        <p>
            Es gibt für uns keine Möglichkeiten Dein Passwort zu ermitteln. 
            Dein geheimes Passwort ist also bei uns sicher. 
        </p>
        <p>
            Technisch: Passworte werden gehashed und mit einem SALT abgelegt gespeichert. 
            Die Implementierung kannst Du Dir auf Github ansehen: 
            <a href="https://github.com/TrueOrFalse/TrueOrFalse/tree/master/src/TrueOrFalse/Domain/User/Registration">
                Link auf Github
            </a>
        </p>

        <h5><a name="teamVonRioafZugriff">Team von RIOFA Zugriff auf Nutzerkonto erlauben</a></h5>
        <p>
            Um Fehler zu finden oder Probleme nachzuvollziehen, 
            kann es nützlich sein, das wir uns in Deinem Konto einloggen.
            <b>Das geschieht jedoch ausschliesslich mit Deiner Zustimmung.</b> 
            Solange es keinen konkreten Anlass gibt, wird der Zugriff nicht benötigt und es gibt 
            keinen Grund das Häkchen zu setzen. Sollte es jedoch mal ein Problem geben, 
            kannst Du uns nach Rücksprache Zugang gewähren - ohne das wir Dein Passwort erfahren. 
            Dafür gibt es unter Deinen Einstellungen eine Checkbox, mit der Du uns den Zugang erlauben kannst. 
        </p>
    </div>
</div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
