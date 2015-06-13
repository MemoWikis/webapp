<%@ Page Title="Hilfe & FAQ" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<HelpModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="PageHeader">FAQ - Häufig gestellte Fragen</h2>
    
    <div class="panel-group" id="FaqAccordion" role="tablist" aria-multiselectable="true">
        
      <div class="panel panel-default">
        <div class="panel-heading" role="tab" id="FaqHeadingWhyNameMemucho">
          <h4 class="panel-title">
            <a data-toggle="collapse" data-parent="#FaqAccordion" href="#FaqTextWhyNameMemucho" aria-expanded="true" aria-controls="FaqTextWhyNameMemucho">
              Woher kommt der Name MEMuchO?
            </a>
          </h4>
        </div>
        <div id="FaqTextWhyNameMemucho" class="panel-collapse collapse" role="tabpanel" aria-labelledby="FaqHeadingWhyNameMemucho">
          <div class="panel-body">
              MEMuchO ist ein Kunstwort setzt sich zusammen aus MEMO und MUCHO. "MEMO" kommt von "memorieren", 
              stammt von dem Lateinischen Wort "memorare" ab und bedeutet "merken".<br/>
              "Mucho" ist das spanisches Wort für "viel" und wird wie "mutscho" ausgesprochen. <br/>
              MEMuchO heißt also: "Viel merken" - denn darum geht es ja hier!
          </div>
        </div>
      </div>

      <div class="panel panel-default">
        <div class="panel-heading" role="tab" id="FaqHeadingHowMemuchoWorks">
          <h4 class="panel-title">
            <a data-toggle="collapse" data-parent="#FaqAccordion" href="#FaqTextHowMemuchoWorks" aria-expanded="true" aria-controls="FaqTextHowMemuchoWorks">
              Wie funktioniert MEMuchO?
            </a>
          </h4>
        </div>
        <div id="FaqTextHowMemuchoWorks" class="panel-collapse collapse" role="tabpanel" aria-labelledby="FaqHeadingHowMemuchoWorks">
          <div class="panel-body">
              <p>
                  MEMuchO ist eine vernetzte Lern- und Wissensplattform. Hier wird Wissen in Frage-Antwort-Form gespeichert, organisiert und 
                  zwischen Nutzern geteilt. Angepasste Lernfunktionen erlauben es dir, schneller und zu bestimmten Terminen 
                  (z.B. Klassenarbeiten, Prüfungen) zu lernen.
              </p>
              <p>
                  Und das funktioniert so: In deinem Wunschwissen legst du fest, was du gerne wissen möchtest. Du kannst einzelne Fragen oder 
                  ganze Fragesätze zu deinem Wunschwissen hinzufügen. Nutze das Wissen, was andere Nutzer erstellt haben und erstelle neue  deine eigenen 
                  Fragen hinzu.
                  
                  Jeder kann <b>Fragen</b> erstellen (Zum Beispiel: "Wie heißt der höchste Berg der Erde?") mit der richtigen Antwort 
                  (im Beispiel: "Mount Everest"). Mehrere Fragen können zu einem Fragesatz zusammengef
              </p>
          </div>
        </div>
      </div>

      <div class="panel panel-default">
        <div class="panel-heading" role="tab" id="FaqHeadingDataPrivacy">
          <h4 class="panel-title">
            <a class="collapsed" data-toggle="collapse" data-parent="#FaqAccordion" href="#FaqTextDataPrivacy" aria-expanded="false" aria-controls="FaqTextDataPrivacy">
              Wie sieht es bei euch genau mit dem Datenschutz aus?
            </a>
          </h4>
        </div>
        <div id="FaqTextDataPrivacy" class="panel-collapse collapse" role="tabpanel" aria-labelledby="FaqHeadingDataPrivacy">
          <div class="panel-body">
              <p>
                  Der Datenschutz ist bei uns sehr streng. Wir fragen dich deshalb nicht nach persönlichen Daten, die wir nicht für die Seite
                  benötigen. Die wenigen persönlichen Daten, die wir von dir haben, sind sehr gut gesichert und wir werden sie auch niemals verkaufen.
              </p>
              <p>
                  Allerdings sind bei MEMuchO das Wikipedia-Prinzip und der Netzwerk-Gedanke sehr wichtig. Deshalb stehen die öffentlichen 
                  Fragen unter einer Creative Commons-Lizenz (<a rel="license" href="http://creativecommons.org/licenses/by/4.0/deed.de">CC BY 4.0.</a>).
                  Jeder andere Nutzer kann die Fragen also (unter bestimmten Bedingungen) für seine Zwecke nutzen. So ist das auch bei Wikipedia, 
                  deswegen können wir zum Beispiel viele Bilder von Wikipedia verwenden und sie auch weiterhin nutzen, wenn sie bei Wikipedia selbst 
                  nicht mehr zu sehen sein sollten. Die privaten Fragen von dir bleiben aber privat.
              </p>
          </div>
        </div>
      </div>

      <div class="panel panel-default">
        <div class="panel-heading" role="tab" id="FaqHeadingWhatIsBeta">
          <h4 class="panel-title">
            <a class="collapsed" data-toggle="collapse" data-parent="#FaqAccordion" href="#FaqTextWhatIsBeta" aria-expanded="false" aria-controls="FaqTextWhatIsBeta">
              Was bedeutet BETA-Phase?
            </a>
          </h4>
        </div>
        <div id="FaqTextWhatIsBeta" class="panel-collapse collapse" role="tabpanel" aria-labelledby="FaqHeadingWhatIsBeta">
          <div class="panel-body">
              <p>
                  MEMuchO befindet sich in der Beta-Phase. Das bedeutet, dass die Seite noch nicht ganz fertig ist. Einige Funktionen fehlen noch 
                  (zum Beispiel das Lernen zu einem bestimmten Termin), andere Dinge müssen wir noch verbessern und benutzerfreundlicher machen 
                  (zum Beispiel den Lernalgorithmus). Außerdem haben wir noch sooo viele Ideen, die auf Verwirklichung warten. Wir arbeiten daran! 
              </p>
              <p>
                  Aber schon jetzt kannst du MEMuchO nutzen - und uns als Beta-Nutzer wichtige Hinweise geben, wo etwas nicht funktioniert und 
                  welche Funktionen dir besonders dringend fehlen. Am besten wirst du schon jetzt Fördermitglied!
              </p>
          </div>
        </div>
      </div>

    </div>
    
</asp:Content>


