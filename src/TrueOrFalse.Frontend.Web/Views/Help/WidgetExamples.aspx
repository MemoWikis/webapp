<%@ Page Title="Widget: Beispiele für Fragen und Lernsets (Quiz)" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Help/Widget.css" rel="stylesheet" />
    
    <%= Scripts.Render("~/bundles/mailto") %>
    
    <%= Scripts.Render("~/bundles/js/Help") %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <% Html.RenderPartial("~/Views/Help/WidgetMenu.ascx", new WidgetMenuModel{CurrentIsExample = true});  %>

    <div class="row">
        <div class="col-xs-12">
            <div class="well">

                <h1 class="PageHeader"><span class="ColoredUnderline GeneralMemucho">Beispiele für Widgets</span></h1>
                
                <p>
                    Die Inhalte und Lernfunktionen von memucho können als Widget nahtlos in bestehende Seiten eingebettet werden. 
                    Möglich ist die Einbettung einzelner Fragen unterschiedlichen Typs, ganzer Lernsets als Quiz (mit oder ohne Video) 
                    und perspektivisch weiterer Lerntools. Hier zeigen wir dir Beispiele, damit du siehst, wie das bei dir aussehen würde.
                </p>

                <div class="row" style="margin-top: -40px;">
                    <div class="col-sm-4">
                        <h2>Einzelfrage</h2>
                        <ul class="nav nav-pills nav-stacked">
                            <li><a href="#singleChoice">Single Choice</a></li>
                            <li><a href="#multipleChoice">Multiple Choice</a></li>
                            <li><a href="#dragAndDrop">Zuordnen (Drag and Drop)</a></li>
                            <li><a href="#flip">Umdrehen (Flip)</a></li>
                            <li><a href="#text">Text-Antwort</a></li>
                        </ul>
                    </div>
                    <div class="col-sm-4">
                        <h2>Lernset (Quiz)</h2>
                        <ul class="nav nav-pills nav-stacked">
                            <li><a href="#setDefault">Standard-Lernset (Quiz)</a></li>
                            <li><a href="#setVideo"><i class="fa fa-youtube-play" aria-hidden="true"></i> Video-Lernset</a></li>
                        </ul>
                    </div>
                    <div class="col-sm-4">
                        <h2>Lehrer & Lerntools</h2>
                        <ul class="nav nav-pills nav-stacked">
                            <li><a href="#pdfExport">PDF-Export Lernset</a></li>
                            <li><a href="#dashboard">Wissenszentrale (Dashboard)</a></li>
                            <li><a href="#moreTools">Weitere auf Anfrage</a></li>
                        </ul>
                    </div>
                </div>
                
                <hr style="margin-bottom: -10px;"/>

                <div class="row features" style="margin-bottom: 30px;">
                    <div class="col-md-6 col-xs-12">
                        <h3>Konfiguration</h3>
                        <ul class="nav nav-pills nav-stacked">
                            <li><i class="fa fa-check" aria-hidden="true"></i> Breite flexibel einstellbar</li>
                            <li><i class="fa fa-check" aria-hidden="true"></i> Optionales Branding</li>
                            <li><i class="fa fa-check" aria-hidden="true"></i> Optionaler Wunschwissen-Button</li>
                        </ul>
                    </div>
                    <div class="col-md-6 col-xs-12">
                        <h3>Eigenschaften</h3>
                        <ul class="nav nav-pills nav-stacked">
                            <li><i class="fa fa-check" aria-hidden="true"></i> Responsive Design</li>
                            <li><i class="fa fa-check" aria-hidden="true"></i> Kleiner Payload (async geladen)</li>
                            <li><i class="fa fa-check" aria-hidden="true"></i> Vielfältige Fragetypen und Video-Unterstützung</li>
                        </ul>
                    </div>
                </div>
                
                <p>
                    Technische Hilfe zur Einbettung findest du <a href="<%= Links.HelpWidget()%>">hier</a>, auch speziell für die Systeme 
                    <a href="<%= Links.HelpWidgetWordpress() %>">Wordpress</a>, <a href="<%= Links.HelpWidgetMoodle() %>">Moodle</a> und 
                    <a href="<%= Links.HelpWidgetBlackboard() %>">Blackboard</a>.
                </p>
            </div>
        </div>
    </div>
    
    <div class="row">
        <div class="col-xs-12">
            <div class="well widgetExamples">
                
                <h1 class="PageHeader"><span class="ColoredUnderline GeneralMemucho">Einzelfragen</span></h1>

                <p>
                    Einzelne Fragen können eingebettet werden, um andere Inhalte wie längere Texte aufzulockern. Sie animieren die Leser zur Interaktion.
                    Das erhöht die Konzentration und den Spaß mit deinen Inhalten. Zur Auswahl stehen verschiedene Frage-Antwort-Typen, die für 
                    unterschiedliche Inhalte geeignet sein können.
                </p>
                    
                <h3 id="singleChoice">Eine richtige Antwort (Single Choice)</h3>
                <p>
                    Bei diesem Aufgabentyp gibt es genau eine richtige Antwort. Das Bild ist optional. 
                </p>
                <p>
                    Nach einem Bericht über den Pilz-Fund beim letzten Waldausflug fragen wir dich also:
                </p>
                <script src="https://memucho.de/views/widgets/w.js" data-t="question" data-id="3629" data-width="100%" data-maxWidth="100%" data-hideKnowledgeBtn="true"></script>
                    
                <h3 id="multipleChoice">Multiple Choice</h3>
                <p>
                    Beim "echten" Multiple Choice können keine oder mehrere Antworten richtig sein. 
                    Das erhöht den Schwierigkeitsgrad, macht aber auch die Fragen oft interessanter.
                </p>
                <script src="https://memucho.de/views/widgets/w.js" data-t="question" data-id="3485" data-width="100%" data-maxWidth="100%" data-hideKnowledgeBtn="true"></script>
                    
                <h3 id="dragAndDrop">Zuordnen (Drag and Drop)</h3>
                <p>
                    Hier müssen Elemente zugeordnet werden, die zueinander passen. Der Einsatz ist sehr flexibel und eignet sich 
                    zum Beispiel auch dazu, Arbeitsschritte in die richtige Reihenfolge zu bringen.
                </p>
                <p>
                    Bei einer Kursseite zur Prozentrechnung, direkt hinter dem Absatz zu Grundwert/Prozentwert fragen wir dich also:
                </p>
                <script src="https://memucho.de/views/widgets/w.js" data-t="question" data-id="3623" data-width="100%" data-maxWidth="100%" data-hideKnowledgeBtn="true"></script>
                    
                <h3 id="flip">Umdrehen (Flip)</h3>
                <p>
                    In Anlehnung an die klassische Karteikarte können hier Antworten durch Umdrehen einer Karte aufgedeckt werden. 
                </p>
                <script src="https://memucho.de/views/widgets/w.js" data-t="question" data-id="4261" data-width="100%" data-maxWidth="100%" data-hideKnowledgeBtn="true"></script>
                    
                <h3 id="text">Freie Textantwort</h3>
                <p>
                    Zur Prüfung von aktivem Wissen eignet sich dieser Typ besonders gut. Möglich ist auch die Eingabe einer Zahl oder eines Datums.
                </p>
                <p>
                    Am Ende unseres kleinen Kurses zur Prozentrechnung sollst du also nochmal Kopfrechnen:
                </p>
                <script src="https://memucho.de/views/widgets/w.js" data-t="question" data-id="3638" data-width="100%" data-maxWidth="100%" data-hideKnowledgeBtn="true"></script>

            </div>
        </div>
    </div>
    
    <div class="row">
        <div class="col-xs-12">
            <div class="well widgetExamples">
                
                <h1 class="PageHeader"><span class="ColoredUnderline GeneralMemucho">Lernset (Quiz)</span></h1>
                <p>
                    Eine Sammlung an Fragen zu einem Thema kann als Lernset-Widget eingebunden werden. 
                    Die oben vorgestellten Fragetypen sind dabei alle verwend- und kombinierbar.
                    Der Nutzer erhält bei jeder Frage wieder eine Rückmeldung mit Erklärung. 
                    Nach einer bestimmten Anzahl an Fragen wird eine Auswertung als Rückmeldung zum Wissensstand angezeigt. 
                </p>

                <h3 id="setDefault">Standard-Lernset (Quiz)</h3>
                <p>
                    Für ein klassisches Quiz kann das Lernset-Widget eingebunden werden. 
                </p>
                <p>
                    Hier haben unsere Schüler*innen auf dem Schulblog einen Bericht über ihre letzte Klassenfahrt nach Italien geschrieben - 
                    oder eine kleine Zeitung berichtet in einem Feature über die italienischen Restaurants der Region. 
                    In beiden Fällen wollen wir jetzt dein Nudelwissen testen:
                </p>
                <script src="https://memucho.de/views/widgets/w.js" data-t="set" data-id="22" data-width="100%" data-hideKnowledgeBtn="true"></script>

                <h3 id="setVideo">Video-Lernset</h3>
                <p>
                    Lernsets können auch auch mit youtube-Videos verknüpft werden. Im Video-Widget erscheint dann beides auf deiner Seite. 
                    Wird das Video abgespielt, hält es immer, sobald eine Frage beantwortet werden kann. 
                    Umgedreht kann zur Stelle im Video gesprungen werden, wo die Frage beantwortet wird.
                    Die Fragen können aber auch ohne das Video beantwortet werden.
                </p>
                <p>
                    Hier haben wir ein tolles Lernvideo vom <a href="https://www.youtube.com/user/MrWissen2go" target="_blank">youtuber MrWissen2go</a>, 
                    welches wir unseren Schülern in Fach Geschichte in unserem Schul-LMS zur Einführung und Wiederholung empfehlen.
                </p>
                <script src="https://memucho.de/views/widgets/w.js" data-t="setVideo" data-id="95" data-width="100%" data-maxWidth="100%" data-hideKnowledgeBtn="true"></script>
            </div>
        </div>
    </div>
    
    <div class="row">
        <div class="col-xs-12">
            <div class="well widgetExamples">
                
                <h1 class="PageHeader"><span class="ColoredUnderline GeneralMemucho">Lehrer & Lerntools</span></h1>
                <p>
                    Als Widget sind auch weitere Funktionen implementierbar, die Lehrende und Bildungsinstitutionen unterstützen. 
                    Wir stellen diese Funktionen bei Interesse gerne vor, schicken Sie uns einfach eine kurze <a href="#contact">Nachricht</a>.
                </p>
                
                <h3 id="pdfExport">PDF-Export Lernset</h3>
                <p>
                    Ein Lernset kann als PDF exportiert werden, um die Lernfragen in verschiedenen Formen in der Klasse einzusetzen: 
                    Als klassisches Arbeitsblatt (oder Teil davon), als kleiner Test, als Grundlage für ein interaktives Quiz in Gruppenarbeit etc.
                </p>
                
                <h3 id="dashboard">Wissenszentrale (Dashboard)</h3>
                <p>
                    Auf der <a href="<%= Links.Knowledge() %>">Wissenszentrale</a> von memucho bekommen Lernende einen Überblick über ihr Lernverhalten und ihren Wissensstand. 
                    Dieses Dashboard (oder Teile davon) kann als Widget in bestehende LMS oder andere Arten der Lernsysteme integriert werden.
                </p>

                <h3 id="moreTools">Weitere auf Anfrage</h3>
                <p>
                    Hast du spezifische Wünsche, um Lernfunktionen, Wissensstand-Evaluierungen oder Autorentools in dein System zu integrieren? 
                    <a href="#contact">Sprich uns an</a>, wir helfen dir gerne weiter.
                </p>
                
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-xs-12" id="contact">
            <div class="well" style="margin-top: 25px;">

                <h2 class="PageHeader">
                    <span class="ColoredUnderline GeneralMemucho">Kontakt</span>
                </h2>

                <div class="row">
                    <div class="col-xs-4 col-md-3 TeamPic">
                        <img src="/Images/Team/team_christof_20170404_P3312344_155.jpg" alt="Foto Christof"/>
                    </div>
                    <div class="col-xs-8 col-md-9">
                        <p>
                            Du hast Fragen? Du hättest gerne ein angepasstes Angebot? Sprich uns einfach an, wir freuen uns über deine Nachricht.
                            Dein Ansprechpartner für alle Fragen zum Widget ist:<br/>
                        </p>
                        <p>
                            <strong>Christof Mauersberger</strong><br/>
                            E-Mail: <span class="mailme">christof at memucho dot de</span><br/>
                            Telefon: 01577-6825707<br/>
                        </p>
                        
                    </div>
                </div>

            </div>
        </div>
    </div>

</asp:Content>