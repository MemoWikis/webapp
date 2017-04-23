<%@ Page Title="memucho für Lehrende" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<ForTeachersModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/About/ForTeachers.css" rel="stylesheet" />
    <%= Scripts.Render("~/bundles/mailto") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="row">
        <div class="col-xs-12">

            <div class="well">

                <h1 class="PageHeader" style="margin-bottom: 15px; margin-top: 0;"><span class="ColoredUnderline GeneralMemucho">memucho als Lehrer und Dozent sinnvoll nutzen</span></h1>
                <p class="teaserText">
                    memucho unterstützt Lernende dabei, sich Grundlagenwissen einzuprägen und komplexe Zusammenhänge leichter rekonstruieren zu können.
                    Gleichzeitig erhöht memucho deutlich die Motivation der Schüler und Studenten.
                </p>
                <p class="teaserText">
                    Egal, wie du deinen Unterricht gestaltest, es gibt verschiedene Möglichkeiten, memucho sinnvoll einzubinden.
                    Das Tolle ist: memucho ist für dich und für die Schüler und Studenten kostenlos.
                </p>
                <!--Selber Schülern zur Verfügung stellen, als Lerntool empfehlen, in eigene Blogs oder Schulseiten einbinden, ..?-->
                <!--Unsere Lernauswertungen, die Möglichkeit zum Quiz-Spiel gegen memucho oder in Echtzeit gegen Freunde, und das Sammeln-->
                    
                <div class="row">
                    <div class="col-xs-12 col-sm-6">
                        <div class="row overviewBlock">
                            <div class="col-xs-2 overviewIcon">
                                <i class="fa fa-puzzle-piece" style="color: #afd534">&nbsp;</i>
                            </div>
                            <div class="col-xs-10">
                                <p class="overviewHeader">Inhalte passend zum Lernstoff</p>
                                <p class="overviewSubtext">Vorhandene Inhalte übernehmen, neu zusammenstellen und mit eigenen Fragen ergänzen. So passen die Inhalte genau zum behandelten Stoff.</p>
                            </div>
                            <div class="col-xs-12">
                                <p class="overviewMore">
                                    <a href="#CompileContentMore" class="btn btn-primary">Mehr erfahren</a>
                                </p>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6">
                        <div class="row overviewBlock">
                            <div class="col-xs-2 overviewIcon">
                                <i class="fa fa-desktop" style="color: #afd534">&nbsp;</i>
                            </div>
                            <div class="col-xs-10">
                                <p class="overviewHeader">Integration in Kurswebseite</p>
                                <p class="overviewSubtext">
                                    Einzelne Fragen oder die Lernfunktion für ganze Fragesätze direkt in die Kursseite, das Schul-LMS oder deinen Blog integrieren.
                                </p>
                            </div>
                            <div class="col-xs-12">
                                <p class="overviewMore">
                                    <a href="#WidgetMore" class="btn btn-primary">Mehr erfahren</a>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-xs-12 col-sm-6">
                        <div class="row overviewBlock">
                            <div class="col-xs-2 overviewIcon">
                                <i class="fa fa-lightbulb-o" style="color: #afd534">&nbsp;</i>
                            </div>
                            <div class="col-xs-10">
                                <p class="overviewHeader">Schüler erstellen Lerninhalte</p>
                                <p class="overviewSubtext">
                                    Schüler können als Gruppenarbeit oder Hausaufgabe den Lernstoff bei memucho selbst anzulegen. Davon profitieren alle.
                                </p>
                            </div>
                            <div class="col-xs-12">
                                <p class="overviewMore">
                                    <a href="#StudentContentMore" class="btn btn-primary">Mehr erfahren</a>
                                </p>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6">
                        <div class="row overviewBlock">
                            <div class="col-xs-2 overviewIcon">
                                <i class="fa fa-trophy" style="color: #afd534">&nbsp;</i>
                            </div>
                            <div class="col-xs-10">
                                <p class="overviewHeader">Echtzeit-Quiz und Lernauswertung</p>
                                <p class="overviewSubtext">Jeder erhält eine individuelle Lernauswertung und die ganze Klasse kann im Echtzeit-Quiz gegeneinander antreten.</p>
                            </div>
                            <div class="col-xs-12">
                                <p class="overviewMore">
                                    <a href="#QuizMore" class="btn btn-primary">Mehr erfahren</a>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>




    <div class="row">
        <div class="col-xs-12">
            <div class="well">
                <h2 style="margin-bottom: 15px;" class="PageHeader" id="CompileContentMore">
                    <span class="ColoredUnderline GeneralMemucho"><i class="fa fa-puzzle-piece" style="color: #afd534">&nbsp;&nbsp;</i>Inhalte passend zum Lernstoff</span>
                </h2>
                <p class="subheader">
                    Vorhandene Inhalte übernehmen, neu zusammenstellen und mit eigenen Inhalten ergänzen
                </p>
                <p>
                    Bei memucho gibt es bereits <a href="<%= Links.QuestionsAll()%>"><%= Model.TotalPublicQuestionCount.ToString("N0") %> öffentliche Fragen</a> und
                    <a href="<%= Links.SetsAll()%>"><%= Model.TotalSetCount.ToString("N0") %> zusammengestellte Fragesätze</a> (jeder Fragesatz bündelt mehrere Fragen zu einem Thema),
                    eingeordnet in <a href="<%= Links.CategoriesAll()%>"><%= Model.TotalCategoryCount.ToString("N0") %> Themen und Unterthemen</a>. 
                    Alle Inhalte bei memucho sind frei und rechtssicher verwendbar, sie stehen unter der Creative-Commons-Lizenz "CC-BY 4.0". 
                    Du kannst die vorhandenen Fragesätze einfach nutzen und zum Beispiel deinen Schülern zum Lernen empfehlen.
                </p>
                <p>
                    Die Fragen in den Fragesätzen passen nicht genau auf den behandelten Stoff? Kein Problem, du kannst die Zusammenstellung ganz leicht ändern und bei Bedarf
                    mit eigenen Fragen ergänzen. Dabei musst du das Rad nicht neu erfinden.
                    Du legst einfach einen eigenen Fragesatz an und fügst alle Fragen, die es bei memucho schon gibt und die für dich relevant sind, hinzu.
                    Zu den Aspekten, die bei memucho noch fehlen, erstellst du einfach eigene Fragen und fügst sie deinem eigenen Fragesatz hinzu.
                    So passen die Fragen ganz genau zum behandelten Lernstoff.<br />
                </p>
                <p>
                    Übrigens: Eine tolle Alternative ist es, deine <a href="#StudentContentMore">Schüler oder Studenten in das Erstellen der Lerninhalte</a> einzubeziehen.
                </p>
                <p>
                    Du willst genauer wissen, wie du Fragesätze neu zusammenstellst und ergänzt?
                    <button class="btn btn-secondary" data-toggle="collapse" data-target="#CompileContentDetails">Schritt-für-Schritt-Anleitung anzeigen</button>
                </p>

                <div id="CompileContentDetails" class="collapse">
                    <h4 style="margin-top: 25px;">
                        Schritt-für-Schritt-Anleitung zum Anpassen und Ergänzen von Fragesätzen
                    </h4>
                    <ol>
                        <li>
                            Erstelle einen <a href="<%= Links.SetCreate() %>">neuen leeren Fragesatz</a>
                            (Menü <i class="fa fa-arrow-circle-o-right"></i> Fragesätze <i class="fa fa-arrow-circle-o-right"></i> Fragesatz erstellen)
                        </li>
                        <img src="/Images/Screenshots/fragen-zusammenstellen.png" class="screenshot" width="525" height="379" />
                        <li>
                            Füge alle passenden Fragen, die du gerne übernehmen würdest, zu deinem Fragesatz hinzu (vgl. Screenshot). Das geht so:
                            <ul>
                                <li>
                                    Gehe zu <a href="<%= Links.QuestionsAll() %>">Fragen</a> und nutze die Filterfunktionen, um die gesuchten Fragen anzuzeigen.
                                    Du kannst dir zum Beispiel nur die Fragen zu einem Thema anzeigen lassen.
                                </li>
                                <li>Markiere die gewünschten Fragen (Klick auf die Checkbox in der oberen Hälfte des Bildes).</li>
                                <li>Klick auf die Schaltfläche "Zum Fragesatz hinzufügen". Wähle hier den von dir angelegten Fragesatz aus.</li>
                            </ul>
                        </li>
                        <li>
                            Bei <a href="<%= Links.SetsMine() %>">"Meine Fragesätze"</a> 
                            (Menü <i class="fa fa-arrow-circle-o-right"></i> Fragesätze <i class="fa fa-arrow-circle-o-right"></i> Karteireiter Meine Fragesätze)
                            findest du deine Fragesätze jederzeit wieder und kannst sie bearbeiten.
                            Füge ein passendes Bild hinzu, eine gute Beschreibung und passe gegebenenfalls die Reihenfolge der Fragen an.
                            Wenn andere deinen Fragesatz nützlich finden und damit lernen, erhältst du Reputationspunkte!
                        </li>
                        <li>
                            Wenn dir im Fragesatz bestimmte Aspekte fehlen, kannst du jederzeit <a href="<%= Links.CreateQuestion() %>">eigene Fragen erstellen</a>
                            (Menü <i class="fa fa-arrow-circle-o-right"></i> Fragen <i class="fa fa-arrow-circle-o-right"></i> Frage erstellen).
                        </li>
                        <li>
                            Füge nun noch die eigenen Fragen zu dem Fragesatz hinzu. Du findest die von dir erstellten Fragen unter <a href="<%=Links.QuestionsMine()%>">Meine Fragen</a> 
                            (Menü <i class="fa fa-arrow-circle-o-right"></i> Fragen <i class="fa fa-arrow-circle-o-right"></i> Karteireiter Meine Fragen). 
                            Wähle sie aus und füge sie deinem Fragesatz hinzu (wie oben).
                        </li>
                    </ol>
                </div>
            </div>
        </div>
    </div>
    

    <div class="row">
        <div class="col-xs-12">
            <div class="well">

                <h2 style="margin-bottom: 15px;" class="PageHeader" id="WidgetMore">
                    <span class="ColoredUnderline GeneralMemucho"><i class="fa fa-desktop" style="color: #afd534">&nbsp;&nbsp;</i>Integration in Kurs- oder Schulwebseite (per Widget)</span>
                </h2>
                <p class="subheader">
                    Lernfunktion direkt in die Kursseite, das Schul-LMS oder deinen Blog integrieren
                </p>
                
                <p>
                    Du möchtest, dass deine Schüler oder Studenten direkt auf der Kursseite lernen? Du möchtest Fragen oder Fragesätze und 
                    das dazugehörige Video auf deinem eigenen Blog oder der Schulwebseite zeigen?
                    Mit den memucho-Widgets ist das problemlos möglich. Du brauchst keine Programmier-Kenntnisse und in der Regel auch keine besonderen Rechte.
                    Das funktioniert bei eigenen Seiten und bei verschiedenen Plattformen wie Wordpress, Moodle, Blackboard usw.
                </p>
                <p>
                    Wenn du ein memucho-Widget einbindest, erscheint der Quiz (Fragesatz-Widget), das Video mit den Lernfragen (Video-Widget) oder die einzelne Lernfrage (Frage-Widget)
                    direkt dort, wo du es einbindest. 
                    Du musst nur eine Zeile Code von memucho per Copy'n'Paste bei dir einfügen, so wie du zum Beispiel auch youtube-Videos einbetten kannst. 
                    Dann können deine Schüler direkt auf der Kurs- oder Projektwebseite ihr Wissen testen und die Inhalte lernen.
                </p>
                <p>
                    Du willst genauer wissen, wie du memucho-Widgets einbettest? 
                    <a href="<%= Links.HelpWidget() %>">Hier zeigen wir dir eine Übersicht über die drei Widgets mit einer Anleitung</a>, wie du sie einbetten kannst.
                    <%--<button class="btn btn-secondary" data-toggle="collapse" data-target="#WidgetDetails">Schritt-für-Schritt-Anleitung anzeigen</button>--%>
                </p>

<%--                <div id="WidgetDetails" class="collapse">
                    <h4 style="margin-top: 25px;">
                        Schritt-für-Schritt-Anleitung zur Einbettung von Fragen und Fragesätzen (memucho-Widget)
                    </h4>
                    <p>
                        Bei memucho findest du bei jeder einzelnen Frage (im Beantworten-Modus) unten einen Link "<strong><i class="fa fa-code" aria-hidden="true">&nbsp;</i>Einbetten</strong>".
                        Dort findest du eine kleine Code-Zeile, die ungefähr so aussieht:
                    </p>
                    <p>
                        <code>
                            &lt;script src="https://memucho.de/views/widgets/question.js" questionId="1183" width="560" height="315"&gt; &lt;/script&gt;
                        </code>
                    </p>
                    <p>
                        Diese kannst du einfach an die Stelle kopieren, wo die Frage in deiner Webseite erscheinen soll.
                        Du musst lediglich beachten, dass du dich in einem Modus befindest, wo du HTML-Code einfügen darfst. Oft gibt es dafür einen Umschalter. Einige Beispiele:
                    </p>
                    <ul>
                        <li>Bei Wordpress: "Text" und nicht "Visuell" <img src="/Images/Screenshots/widget-wordpress-textmode-small.png" /></li>
                        <li>
                            Bei Moodle: Material hinzufügen: Textfeld. Dann im Textfeld das "Mehr Symbole anzeigen" <img src="/Images/Screenshots/moodle-1-mehrFelder-small.png" /> aktivieren
                            und dann den HTML-Modus einschalten <img src="/Images/Screenshots/moodle-2-HTMLmode-small.png" />
                        </li>
                        <li>Bei Blackboard: Bei Kursmaterial ein Element erstellen, dann im Editierfenster auf den Button "HTML" klicken <img src="/Images/Screenshots/widget-blackboard-html-mode-small.png" /></li>
                    </ul>
                    <p>
                        <strong>Das Widget für die Lernfunktion ganzer Fragesätze folgt in Kürze, wir arbeiten daran!</strong>
                    </p>
                </div>--%>

            </div>
        </div>
    </div>

    
    <div class="row">
        <div class="col-xs-12">
            <div class="well">
                <h2 style="margin-bottom: 15px;" class="PageHeader" id="StudentContentMore">
                    <span class="ColoredUnderline GeneralMemucho"><i class="fa fa-lightbulb-o" style="color: #afd534">&nbsp;&nbsp;</i>Schüler/Studenten erstellen Lerninhalte</span>
                </h2>
                <p class="subheader">
                    Schüler und Studenten können Lerninhalte selbst erstellen. Davon profitieren alle!
                </p>
                <p>
                    Den Lernstoff selbst aufzubereiten ist eine sehr effektive Art zu Lernen. 
                    Daher kannst du als Lehrer oder Dozent eine Gruppenarbeit oder eine Hausaufgabe vergeben, bei der deine Schüler bzw. Studenten 
                    die passenden Fragen zum behandelten Lernstoff selbst erstellen. 
                    Dabei kannst du Themen gut aufteilen, denn die einzelnen Fragen von unterschiedlichen Schülern können leicht zu 
                    einheitlichen Fragesätzen zusammengefasst werden, die für die ganze Klasse relevant sind.
                </p>
                <p>
                    Deine Schüler und Studenten lernen dadurch nicht nur selber viel, sie tun auch etwas Gutes: Sie erstellen offene Bildungsinhalte! 
                    Davon profitieren direkt die Mitschüler oder Kommilitonen, aber auch alle anderen, die damit lernen möchten. 
                    Angst, dass die Qualität nicht stimmt? Für gute Fragen, mit denen andere lernen wollen, erhalten die Ersteller Reputationspunkte. 
                    Das motiviert sie und gibt ein gutes Gefühl. Außerdem werden Fehler schnell erkannt, wenn die eigenen Mitschüler damit lernen. 
                    Über die Kommentarfunktion kann auf Fehler hingewiesen werden und Verbesserungsvorschläge gemacht werden. 
                    Wir achten darauf, dass die Kommentare fair und konstruktiv sind. Fehler sind kein Problem, denn sie können leicht korrigiert werden.
                </p>
                <p>
                    Hat eine Klasse oder ein Kurs Fragen erstellt, können diese über die <a href="#WidgetMore">Widget-Funktion</a> auch direkt in die Kursseite
                    oder - zum Beispiel als Abschluss eines Projektes - sogar auf die Schulwebseite eingebunden werden. 
                    Eine tolle und sehr einfache Präsentation der Aktivitäten in der Schule. Bestimmt macht es auch Eltern Spaß, ihr Wissen zu testen oder sogar
                    im Quiz gegen die eigenen Kinder anzutreten.
                    <!-- Beispiel liefern?: Der Biokurs behandelt gerade Vererbungslehre? Der von Schülern erstellte passende Fragesatz als Ergebnis auf der Schulwebseite kann mit Eltern geteilt werden... -->
                </p>
            </div>
        </div>
    </div>


    <div class="row">
        <div class="col-xs-12">
            <div class="well">
                <h2 style="margin-bottom: 15px;" class="PageHeader" id="QuizMore">
                    <span class="ColoredUnderline GeneralMemucho"><i class="fa fa-trophy" style="color: #afd534">&nbsp;&nbsp;</i>Echtzeit-Quiz und Lernauswertung steigern die Motivation</span>
                </h2>
                <p class="subheader">
                    Echtzeit-Quiz mit bis zu 30 Mitspielern und eine individuelle Lernauswertung steigern die Motivation
                </p>
                <p>
                    Den Klassensieger in der Vererbungslehre oder den EU-Institutionen in einem spannenden Spiel bestimmen?
                    Auf Basis der Lerninhalte können bis zu 30 Spieler in einem Echtzeit-Quiz gegeneinander antreten. 
                    (Der Echtzeit-Quiz befindet sich zur Zeit noch in der Testphase.) 
                    Für jede Frage gibt es 15 Sekunden Zeit, wer am Ende die meisten Fragen richtig beantwortet hat, hat gewonnen. 
                    Spielen kann jeder auf seinem eigenen Handy oder auf Schulgeräten. Benötigt wird nur Internet und ein aktueller Browser.
                </p>
                <p>
                    Jeder, der mit memucho lernt, erhält persönliche Lernauswertungen. In der eigenen Wissenszentrale sieht man den eigenen Wissensstand und die eigenen Lernaktivitäten. 
                    Wie viele Lerntage hatte ich bisher? Wie viele Tage in Folge? Hier können eigene Bestmarken gesetzt und immer wieder überboten werden. 
                    Auch bei einzelnen Lernsitzungen erhalten die Lernenden immer ein Feedback und sehen, wie sich ihr Wissensstand entwickelt.
                    Diese Rückmeldungen motivieren zum Lernen und zur Auseinandersetzung mit dem Lernstoff.
                </p>

            </div>
        </div>
    </div>
    
    <div class="row">
        <div class="col-xs-12">
            <div class="well" style="margin-top: 25px;">
                <h2 class="PageHeader">
                    <span class="ColoredUnderline GeneralMemucho">Kontakt</span>
                </h2>

                <div class="row">
                    <div class="col-xs-4 col-md-3 TeamPic">
                        <img src="/Images/Team/team_lisa_sq_155.jpg"/>  
                    </div>
                    <div class="col-xs-8 col-md-9">
                        <p>
                            Du hast Fragen? Du hast selber tolle Ideen? Du kannst uns von deinen Erfahrungen beim Einsatz digitaler Medien im Unterricht bzw. in der Lehre erzählen?
                            Sprich uns einfach an, wir freuen uns über deine Nachricht! Dein Ansprechpartner ist:<br/>
                        </p>
                        <p>
                            <strong>Lisa</strong><br/>
                            E-Mail: <span class="mailme">lisa at memucho dot de</span><br/>
                            Telefon: 0151 - 265 033 70<br/>
                        </p>
                        
                    </div>
                </div>

            </div>
        </div>
    </div>



</asp:Content>