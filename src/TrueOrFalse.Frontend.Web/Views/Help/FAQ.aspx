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
              Woher kommt der Name memucho?
            </a>
          </h4>
        </div>
        <div id="FaqTextWhyNameMemucho" class="panel-collapse collapse" role="tabpanel" aria-labelledby="FaqHeadingWhyNameMemucho">
          <div class="panel-body">
              memucho ist ein Kunstwort setzt sich zusammen aus MEMO und MUCHO. "MEMO" kommt von "memorieren", 
              stammt von dem Lateinischen Wort "memorare" ab und bedeutet "merken".<br/>
              "Mucho" ist das spanisches Wort für "viel" und wird wie "mutscho" ausgesprochen. <br/>
              memucho heißt also: "Viel merken" - denn darum geht es hier!
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
                  Für uns sind Datensicherheit und Datenschutz entscheidende Themen. Gerade weil wir viele Daten sammeln, sehen wir eine besondere 
                  Verantwortung darin, Daten zu schützen und transparent zu sein. Das kann jeder prüfen, denn der Programm-Quelltext für memucho ist
                  als Open Source-Software öffentlich und auf <a href="https://github.com/TrueOrFalse/TrueOrFalse"><i class="fa fa-github"></i>Github</a> einsehbar.
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
                  nicht mehr zu sehen sein sollten. Die privaten Fragen von dir bleiben aber privat.
              </p>
          </div>
        </div>
      </div>

      <div class="panel panel-default">
        <div class="panel-heading" role="tab" id="FaqHeadingWhichDataWhy">
          <h4 class="panel-title">
            <a class="collapsed" data-toggle="collapse" data-parent="#FaqAccordion" href="#FaqTextWhichDataWhy" aria-expanded="false" aria-controls="FaqTextWhichDataWhy">
              Welche Daten werden von euch erfasst? Warum?
            </a>
          </h4>
        </div>
        <div id="FaqTextWhichDataWhy" class="panel-collapse collapse" role="tabpanel" aria-labelledby="FaqHeadingWhichDataWhy">
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
                  <a href="#FaqTextDataPrivacy">Datenschutz</a> ist bei uns sehr streng. Wir fragen dich deshalb nicht nach persönlichen Daten, die wir nicht für die 
                  Seite benötigen. Die wenigen persönlichen Daten, die wir von dir haben, sind sehr gut gesichert und wir werden sie auch niemals verkaufen.
              </p>
          </div>
        </div>
      </div>

      <div class="panel panel-default">
        <div class="panel-heading" role="tab" id="FaqHeadingHowMemuchoWorks">
          <h4 class="panel-title">
            <a data-toggle="collapse" data-parent="#FaqAccordion" href="#FaqTextHowMemuchoWorks" aria-expanded="true" aria-controls="FaqTextHowMemuchoWorks">
              Wie funktioniert memucho?
            </a>
          </h4>
        </div>
        <div id="FaqTextHowMemuchoWorks" class="panel-collapse collapse" role="tabpanel" aria-labelledby="FaqHeadingHowMemuchoWorks">
          <div class="panel-body">
              <p>
                  memucho ist eine vernetzte Lern- und Wissensplattform. Hier wird Wissen in Frage-Antwort-Form gespeichert, organisiert und 
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
        <div class="panel-heading" role="tab" id="FaqHeadingWhatIsKnowledge">
          <h4 class="panel-title">
            <a data-toggle="collapse" data-parent="#FaqAccordion" href="#FaqTextWhatIsKnowledge" aria-expanded="true" aria-controls="FaqTextWhatIsKnowledge">
              Was ist "Wunschwissen"?
            </a>
          </h4>
        </div>
        <div id="FaqTextWhatIsKnowledge" class="panel-collapse collapse" role="tabpanel" aria-labelledby="FaqHeadingWhatIsKnowledge">
          <div class="panel-body">
              <p>
                  In deinem Wunschwissen legst du fest, was du gerne wissen möchtest. So behälst du besser den Überblick. Du kannst jede einzelne 
                  Frage oder ganze Fragesätze zu deinem Wunschwissen hinzufügen. Klicke dazu einfach auf das Herz-Symbol bei einer Frage oder 
                  einem Fragesatz. Ist das Herz komplett rot ausgefüllt, ist die Frage bzw. der Fragesatz in deinem  Wunschwissen, ansonsten 
                  ist das Herz weiß. Du kannst Fragen auch wieder aus deinem Wunschwissen entfernen, wenn du wieder auf das Herz bei der 
                  entsprechenden Frage klickst.
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
                  memucho befindet sich in der Beta-Phase. Das bedeutet, dass die Seite noch nicht ganz fertig ist. Einige Funktionen fehlen noch 
                  (zum Beispiel das Lernen zu einem bestimmten Termin), andere Dinge müssen wir noch verbessern und benutzerfreundlicher machen 
                  (zum Beispiel den Lernalgorithmus). Außerdem haben wir noch sooo viele Ideen, die auf Verwirklichung warten. Wir arbeiten daran! 
              </p>
              <p>
                  Aber schon jetzt kannst du memucho nutzen - und uns als Beta-Nutzer wichtige Hinweise geben, wo etwas nicht funktioniert und 
                  welche Funktionen dir besonders dringend fehlen. Am besten wirst du schon jetzt Fördermitglied!
              </p>
          </div>
        </div>
      </div>

      <div class="panel panel-default">
        <div class="panel-heading" role="tab" id="FaqHeadingContact">
          <h4 class="panel-title">
            <a class="collapsed" data-toggle="collapse" data-parent="#FaqAccordion" href="#FaqTextContact" aria-expanded="false" aria-controls="FaqTextContact">
              Wie erreiche ich euch?
            </a>
          </h4>
        </div>
        <div id="FaqTextContact" class="panel-collapse collapse" role="tabpanel" aria-labelledby="FaqHeadingContact">
          <div class="panel-body">
              <p>
                  Per Email: <a href="mailto:kontakt@memucho.de">kontakt@memucho.de</a><br/> 
                  Robert erreichst du auch über Skype ("robert-mischke") oder per Telefon (+49-178-18 668 48).
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

    </div>
    
</asp:Content>


