﻿<%@ Page Title="Hilfe & FAQ" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<HelpModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
<script type="text/javascript" >

    /* Source http://www.html-advisor.com/javascript/hide-email-with-javascript-jquery/ */
    $(function () {
        var spt = $('span.mailme');
        var at = / at /;
        var dot = / dot /g;
        var addr = $(spt).text().replace(at, "@").replace(dot, ".");
        $(spt).after('<a href="mailto:' + addr + '" title="Send an email">' + addr + '</a>').hover(function () { window.status = "Send a letter!"; }, function () { window.status = ""; });
        $(spt).remove();
        $(document).ready(function () { //http://stackoverflow.com/questions/12008389/linking-to-a-section-of-an-accordion-from-another-page#answer-12008992
            location.hash && $(location.hash + '.collapse').collapse('show');
        });
    });
</script>    
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="PageHeader">FAQ - Häufig gestellte Fragen</h2>
    
    <div class="panel-group" id="FaqAccordion" role="tablist" aria-multiselectable="true">
            <% FAQAccordeonItem currentFaqItem;%>
        
      <div class="panel panel-default">
        <% currentFaqItem = new FAQAccordeonItem("WhyNameMemucho"); %>
          
        <div class="panel-heading" role="tab" id="<%= currentFaqItem.ItemHtmlIdHeading %>">
          <h4 class="panel-title">
            <a data-toggle="collapse" data-parent="#FaqAccordion" href="#<%= currentFaqItem.ItemHtmlIdText %>" aria-expanded="true" aria-controls="<%= currentFaqItem.ItemHtmlIdText %>">
              Woher kommt der Name memucho?
            </a>
          </h4>
        </div>
        <div id="<%= currentFaqItem.ItemHtmlIdText %>" class="panel-collapse collapse" role="tabpanel" aria-labelledby="<%= currentFaqItem.ItemHtmlIdHeading %>">
          <div class="panel-body">
              memucho ist ein Kunstwort setzt sich zusammen aus MEMO und MUCHO. "MEMO" kommt von "memorieren", 
              stammt von dem Lateinischen Wort "memorare" ab und bedeutet "merken".<br/>
              "Mucho" ist das spanisches Wort für "viel" und wird wie "mutscho" ausgesprochen. <br/>
              memucho heißt also: "Viel merken" - denn darum geht es hier!
          </div>
        </div>
      </div>

      <div class="panel panel-default">
        <% currentFaqItem = new FAQAccordeonItem("DataPrivacy"); %>
        <div class="panel-heading" role="tab" id="<%= currentFaqItem.ItemHtmlIdHeading %>">
          <h4 class="panel-title">
            <a class="collapsed" data-toggle="collapse" data-parent="#FaqAccordion" href="#<%= currentFaqItem.ItemHtmlIdText %>" aria-expanded="false" aria-controls="<%= currentFaqItem.ItemHtmlIdText %>">
              Wie sieht es bei euch genau mit dem Datenschutz aus?
            </a>
          </h4>
        </div>
        <div id="<%= currentFaqItem.ItemHtmlIdText %>" class="panel-collapse collapse" role="tabpanel" aria-labelledby="<%= currentFaqItem.ItemHtmlIdHeading %>">
          <div class="panel-body">
              <p>
                  Für uns sind Datensicherheit und Datenschutz wichtige Themen. Gerade weil wir viele Daten sammeln, sehen wir eine besondere 
                  Verantwortung darin, Daten zu schützen und transparent zu sein. Das kann jeder prüfen, denn der Programm-Quelltext für memucho ist
                  als Open Source-Software öffentlich und auf <a href="https://github.com/TrueOrFalse/TrueOrFalse"><i class="fa fa-github">&nbsp;</i>Github</a> einsehbar.
              </p>
              <p>
                  Der Datenschutz ist bei uns sehr streng. Wir fragen dich deshalb nicht nach persönlichen Daten, die wir nicht für die Seite
                  benötigen. Die wenigen persönlichen Daten, die wir von dir haben, sind sehr gut gesichert und wir werden sie auch niemals verkaufen.
                  Beispielsweise gibt für uns keine Möglichkeiten, dein Passwort zu ermitteln. Dein geheimes Passwort ist also bei uns sicher.
                  (Technisch: Passworte werden gehashed und mit einem SALT abgelegt gespeichert.)
              </p>
              <p>
                  Allerdings sind bei memucho das Wikipedia-Prinzip und der Netzwerk-Gedanke sehr wichtig. Deshalb stehen die öffentlichen 
                  Fragen unter einer Creative Commons-Lizenz (<a rel="license" href="http://creativecommons.org/licenses/by/4.0/deed.de">CC BY 4.0.</a>).
                  Jeder andere Nutzer kann die Fragen also (unter bestimmten Bedingungen) für seine Zwecke nutzen. So ist das auch bei Wikipedia, 
                  deswegen können wir zum Beispiel viele Bilder von Wikipedia verwenden und sie auch weiterhin nutzen, wenn sie bei Wikipedia selbst 
                  nicht mehr zu sehen sein sollten. Die privaten Fragen von dir bleiben aber privat und sind nicht für andere sichtbar.
              </p>
          </div>
        </div>
      </div>

      <div class="panel panel-default">
        <% currentFaqItem = new FAQAccordeonItem("WhichDataWhy"); %>
        <div class="panel-heading" role="tab" id="<%= currentFaqItem.ItemHtmlIdHeading %>">
          <h4 class="panel-title">
            <a class="collapsed" data-toggle="collapse" data-parent="#FaqAccordion" href="#<%= currentFaqItem.ItemHtmlIdText %>" aria-expanded="false" aria-controls="<%= currentFaqItem.ItemHtmlIdText %>">
              Welche Daten werden von euch erfasst? Warum?
            </a>
          </h4>
        </div>
        <div id="<%= currentFaqItem.ItemHtmlIdText %>" class="panel-collapse collapse" role="tabpanel" aria-labelledby="<%= currentFaqItem.ItemHtmlIdHeading %>">
          <div class="panel-body">
              <p>
                   Wir versuchen, möglichst alle deine Interaktionen mit Fragen und Antworten zu sammeln. Wie oft wurde eine Frage angesehen, wie oft und von wem
                   wurde sie zum Wunschwissen hinzugefügt, wie oft wurde die Frage beantwort und zu welchem Zeitpunkt, welche Antwort wurde gegeben.
                   Wir sammeln, wenn möglich, diese Daten personifziert, also nicht anonymisiert.
              </p>
              <p>
                  Wir sammeln diese Daten nur, weil wir dich so am besten beim Lernen unterstützen können. Wenn du zu einem bestimmten Termin lernst oder ein bestimmtes
                  Wissen niemals vergessen möchtest, dann möchte dir memucho die Fragen zum optimalen Zeitpunkt zum Lernen vorlegen. Damit du genug, aber auch nicht zu 
                  viel lernst. Dafür brauchen wir aber diese Daten, denn je mehr wir über das Lernverhalten von möglichst vielen Nutzern wissen, desto genauer können
                  unsere Algorithmen arbeiten. (Technisch: Wir setzen so genannte Machine Learning-Algorithmen ein, die selbstständig Lernmuster erkennen und Prognosen
                  abgeben können.)
              </p>
              <p>
                  Wir sammeln diese Daten nur, um memucho zu einem möglichst nützlichem Werkzeug zu machen und nicht, um Nutzerdaten zu verkaufen. Der 
                  Datenschutz ist bei uns sehr streng. Wir fragen dich deshalb nicht nach persönlichen Daten, die wir nicht für die 
                  Seite benötigen. Die wenigen persönlichen Daten, die wir von dir haben, sind sehr gut gesichert und wir werden sie auch niemals verkaufen.
              </p>
          </div>
        </div>
      </div>

      <div class="panel panel-default">
        <% currentFaqItem = new FAQAccordeonItem("HowMemuchoWorks"); %>
        <div class="panel-heading" role="tab" id="<%= currentFaqItem.ItemHtmlIdHeading %>">
          <h4 class="panel-title">
            <a data-toggle="collapse" data-parent="#FaqAccordion" href="#<%= currentFaqItem.ItemHtmlIdText %>" aria-expanded="true" aria-controls="<%= currentFaqItem.ItemHtmlIdText %>">
              Wie funktioniert memucho?
            </a>
          </h4>
        </div>
        <div id="<%= currentFaqItem.ItemHtmlIdText %>" class="panel-collapse collapse" role="tabpanel" aria-labelledby="<%= currentFaqItem.ItemHtmlIdHeading %>">
          <div class="panel-body">
              <p>
                  memucho ist eine vernetzte Lern- und Wissensplattform. Hier wird Wissen in Frage-Antwort-Form gespeichert, organisiert und 
                  zwischen Nutzern geteilt. Angepasste Lernfunktionen erlauben es dir, schneller und zu bestimmten Terminen 
                  (z.B. Klassenarbeiten, Prüfungen) zu lernen.
              </p>
              <p>
                  Und das funktioniert so: In deinem Wunschwissen legst du fest, was du gerne wissen möchtest. Du kannst einzelne Fragen oder 
                  ganze Fragesätze zu deinem Wunschwissen hinzufügen. Nutze das Wissen, was andere Nutzer erstellt haben und erstelle selbst neue
                  Fragen und Fragesätze.
                  
                  Jeder kann Fragen (Zum Beispiel: "Wie heißt der höchste Berg der Erde?") mit der richtigen Antwort 
                  (im Beispiel: "Mount Everest") erstellen. Mehrere Fragen können zu einem Fragesatz zusammengefasst werden.
              </p>
          </div>
        </div>
      </div>

      <div class="panel panel-default">
        <% currentFaqItem = new FAQAccordeonItem("WhatIsKnowledge"); %>
        <div class="panel-heading" role="tab" id="<%= currentFaqItem.ItemHtmlIdHeading %>">
          <h4 class="panel-title">
            <a data-toggle="collapse" data-parent="#FaqAccordion" href="#<%= currentFaqItem.ItemHtmlIdText %>" aria-expanded="true" aria-controls="<%= currentFaqItem.ItemHtmlIdText %>">
              Was ist "Wunschwissen"?
            </a>
          </h4>
        </div>
        <div id="<%= currentFaqItem.ItemHtmlIdText %>" class="panel-collapse collapse" role="tabpanel" aria-labelledby="<%= currentFaqItem.ItemHtmlIdHeading %>">
          <div class="panel-body">
              <p>
                  In deinem Wunschwissen legst du fest, was du gern langfristig wissen möchtest. So behältst du besser den Überblick. Du kannst jede einzelne 
                  Frage oder ganze Fragesätze zu deinem Wunschwissen hinzufügen. Klicke dazu einfach auf das Herz-Symbol bei einer Frage oder 
                  einem Fragesatz. Ist das Herz komplett rot ausgefüllt, ist die Frage bzw. der Fragesatz in deinem  Wunschwissen, ansonsten 
                  ist das Herz weiß. Du kannst Fragen auch wieder aus deinem Wunschwissen entfernen, indem du wieder auf das Herz bei der 
                  entsprechenden Frage klickst.
              </p>
          </div>
        </div>
      </div>

      <div class="panel panel-default">
        <% currentFaqItem = new FAQAccordeonItem("WhatIsBeta"); %>
        <div class="panel-heading" role="tab" id="<%= currentFaqItem.ItemHtmlIdHeading %>">
          <h4 class="panel-title">
            <a class="collapsed" data-toggle="collapse" data-parent="#FaqAccordion" href="#<%= currentFaqItem.ItemHtmlIdText %>" aria-expanded="false" aria-controls="<%= currentFaqItem.ItemHtmlIdText %>">
              Was bedeutet BETA-Phase?
            </a>
          </h4>
        </div>
        <div id="<%= currentFaqItem.ItemHtmlIdText %>" class="panel-collapse collapse" role="tabpanel" aria-labelledby="<%= currentFaqItem.ItemHtmlIdHeading %>">
          <div class="panel-body">
              <p>
                  memucho befindet sich in der Beta-Phase. Das bedeutet, dass die Seite noch nicht ganz fertig ist. Einige Funktionen fehlen noch 
                  (zum Beispiel das Sammeln von Trophäen), andere Dinge müssen wir noch verbessern und benutzerfreundlicher machen 
                  (zum Beispiel den Lernalgorithmus). Außerdem haben wir noch sooo viele Ideen, die auf Verwirklichung warten. Wir arbeiten daran! 
              </p>
              <p>
                  Aber schon jetzt kannst du memucho nutzen - und uns als Beta-Nutzer wichtige Hinweise geben, wo etwas nicht funktioniert und 
                  welche Funktionen dir besonders dringend fehlen. Am besten wirst du schon jetzt <a href="<%= Url.Action(Links.Membership, Links.AccountController) %>">Fördermitglied</a>!
              </p>
          </div>
        </div>
      </div>

      <div class="panel panel-default">
            <% currentFaqItem = new FAQAccordeonItem("Contact"); %>
          
        <div class="panel-heading" role="tab" id="<%= currentFaqItem.ItemHtmlIdHeading %>">
          <h4 class="panel-title">
            <a class="collapsed" data-toggle="collapse" data-parent="#FaqAccordion" href="#<%= currentFaqItem.ItemHtmlIdText %>" aria-expanded="false" aria-controls="<%= currentFaqItem.ItemHtmlIdText %>">
              Wie erreiche ich euch?
            </a>
          </h4>
        </div>
        <div id="<%= currentFaqItem.ItemHtmlIdText %>" class="panel-collapse collapse" role="tabpanel" aria-labelledby="<%= currentFaqItem.ItemHtmlIdHeading %>">
          <div class="panel-body">
              <p>
                  Per E-Mail: <span class="mailme">team at memucho dot de</span><br/> 
                  Christof erreichst du auch über Skype ("cmauersberger") oder per Telefon (+49-1577-6825707).
              </p>
              <p>
                  Oder du schreibst uns eine Postkarte oder kommst persönlich bei uns vorbei (am besten mit Anmeldung)!<br/>
                  Team memucho<br/>
                  c/o "Raecke Schreiber GbR"<br/>
                  Erkelenzdamm 59/61<br/>
                  D-10999 Berlin<br/>
                  (Aufgang: Portal 3A, 4.OG)
              </p>
          </div>
        </div>
      </div>
    <div class="panel panel-default">
        <% currentFaqItem = new FAQAccordeonItem("QuestionLicense"); %>
        <div class="panel-heading" role="tab" id="<%= currentFaqItem.ItemHtmlIdHeading %>">
          <h4 class="panel-title">
            <a data-toggle="collapse" data-parent="#FaqAccordion" href="#<%= currentFaqItem.ItemHtmlIdText %>" aria-expanded="true" aria-controls="<%= currentFaqItem.ItemHtmlIdText %>">
              Unter welcher Lizenz stehen die Fragen bei memucho und (wie) können sie weiterverwendet werden?
            </a>
          </h4>
        </div>
        <div id="<%= currentFaqItem.ItemHtmlIdText %>" class="panel-collapse collapse" role="tabpanel" aria-labelledby="<%= currentFaqItem.ItemHtmlIdHeading %>">
          <div class="panel-body">
              In der Detailansicht steht bei jeder Frage die Lizenz, unter der sie veröffentlicht wurde.
              Die nutzererstellten Fragen bei memucho stehen alle unter der Lizenz "Creative Commons - Namensnennung 4.0 International" 
              (<a rel="license" href="http://creativecommons.org/licenses/by/4.0/deed.de">CC BY 4.0.</a>).
              Diese Fragen können bei angemessener Nennung des Urhebers (gib am besten die Url der Fragedetailseite und den Nutzernamen des Erstellers an) 
              und der Lizenz frei weiterverwendet und dabei auch verändert werden.
              Einige von uns eingestellte Fragen stehen unter anderen Lizenzen (zum Beispiel amtliche Fragesammlungen wie Führerscheinfragen). 
              Ist bei diesen Fragen die Weiterverwendung eingeschränkt, findest du einen entsprechenden Hinweis darauf bei der Lizenzangabe.
          </div>
        </div>
      </div>
    </div>
    
</asp:Content>





