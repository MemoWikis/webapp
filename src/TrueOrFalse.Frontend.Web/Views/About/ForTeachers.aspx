<%@ Page Title="memucho für Lehrende" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<ForTeachersModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
        <link href="/Views/About/ForTeachers.css" rel="stylesheet" />
    </asp:Content>

    <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


        <div class="row">
            <div class="col-xs-12">

                <div class="well">

                    <h1 class="PageHeader" style="margin-bottom: 15px; margin-top: 0;"><span class="ColoredUnderline GeneralMemucho">memucho als Lehrender sinnvoll nutzen</span></h1>
                    <p style="font-size: 1.3em; line-height: 1.4em;">
                        memucho unterstützt Lernende dabei, sich Grundlagenwissen einzuprägen und komplexe Zusammenhänge leichter rekonstruieren zu können.
                        Gleichzeitig erhöht memucho deutlich die Motivation der Schüler.
                    </p>
                    <p style="font-size: 1.3em; line-height: 1.4em;">
                        Egal, wie du deinen Unterricht gestaltest, es gibt verschiedene Möglichkeiten, memucho sinnvoll einzubinden.
                        Das tolle ist: memucho ist für dich und für die Schüler kostenlos.
                    </p>
                    <!--Selber Schülern zur Verfügung stellen, als Lerntool empfehlen, in eigene Blogs oder Schulseiten einbinden, ..?-->
                    <!--Unsere Lernauswertungen, die Möglichkeit zum Quiz-Spiel gegen memucho oder in Echtzeit gegen Freunde, und das Sammeln-->
                    
                    <div class="row" style="margin-top: 30px;">
                        <div class="col-xs-12 col-md-6">
                            <div class="row">
                                <div class="col-xs-2 overviewIcon">
                                    <i class="fa fa-desktop" style="color: #afd534">&nbsp;</i>
                                </div>
                                <div class="col-xs-10">
                                    <p class="overviewHeader">Integration in eigene Seite</p>
                                    <p class="overviewSubtext">Einzelne Fragen oder Lernfunktion für ganze Fragesätze direkt in deiner Webseite</p>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12 col-md-6">
                            <div class="row">
                                <div class="col-xs-2 overviewIcon">
                                    <i class="fa fa-puzzle-piece" style="color: #afd534">&nbsp;</i>
                                </div>
                                <div class="col-xs-10">
                                    <p class="overviewHeader">Inhalte leicht zusammenstellen</p>
                                    <p class="overviewSubtext">Vorhandene Inhalte übernehmen, neu zusammenstellen und mit eigenen Inhalten ergänzen</pclass="overviewSubtext">
                                </div>
                            </div>
                        </div>
                    </div>
                    

                    
                    <h2 style="margin-bottom: 15px;">
                        <i class="fa fa-desktop" style="color: #afd534">&nbsp;&nbsp;</i><span class="ColoredUnderline GeneralMemucho">memucho in deine Webseite einbinden (Widget)</span>
                    </h2>
                    <p class="subheader">
                        Einzelne Fragen oder Lernfunktion für ganze Fragesätze direkt in deiner Webseite
                    </p>

                    <p>
                        Du kannst deinen Schülern direkt den Link zu einzelnen Fragesätzen oder ganzen Themengebieten schicken.
                        Wenn du einen eigenen Blog oder eine eigene Webseite hast, dann kannst du die Lernfunktion für Fragesätze direkt bei dir einbinden. 
                        (Oft klappt das auch bei Learning-Management-Systemen (LMS), wenn die an deiner Schule eingesetzt werden.) Das geht ganz einfach per Copy'n'Paste.
                    </p>
                    <p>
                        Bei jeder einzelnen Frage findest du unten einen Link "<i class="fa fa-code" aria-hidden="true">&nbsp;</i>Einbetten".
                        Dort findest du eine kleine Code-Zeile, die ungefähr so aussieht:
                    </p>
                    <p>
                        <code>
                            &lt;script src="https://memucho.de/views/widgets/question.js" questionId="1183" width="560" height="315"&gt; &lt;/script&gt;
                        </code>
                    </p>
                    <p>
                        Diese kannst du einfach an die Stelle kopieren, wo die Frage in deiner Webseite erscheinen soll.
                        Du musst lediglich beachten, dass du dich in einem Modus befindest, wo du HTML-Code einfügen darfst. Einige Beispiele:
                    </p>
                    <ul>
                        <li>Wordpress: "Text" und nicht "Visuell" <img src="/Images/Screenshots/wordpress-textmode-small.png"/></li>
                        <li>
                            Moodle: Material hinzufügen: Textfeld. Dann im Textfeld das "Mehr Symbole anzeigen" <img src="/Images/Screenshots/moodle-1-mehrFelder-small.png"/> aktivieren 
                            und dann den HTML-Modus einschalten <img src="/Images/Screenshots/moodle-2-HTMLmode-small.png"/>
                        </li>
                        <li>Blackboard: Bei Kursmaterial ein Element erstellen, dann im Editierfenster auf den Button "HTML" klicken <img src="/Images/Screenshots/blackboard-html-mode-small.png"/></li>
                    </ul>




                    <h2 style="margin-bottom: 15px;">
                        <i class="fa fa-puzzle-piece" style="color: #afd534">&nbsp;&nbsp;</i><span class="ColoredUnderline GeneralMemucho">Inhalte passend zu deiner Lerneinheit</span>
                    </h2>
                    <p class="subheader">
                        Vorhandene Inhalte übernehmen, neu zusammenstellen und mit eigenen Inhalten ergänzen
                    </p>
                    <p>
                        Bei memucho gibt es bereits <a href="<%= Links.QuestionsAll()%>"><%= Model.TotalPublicQuestionCount.ToString("N0") %> öffentliche Fragen</a> und
                        <a href="<%= Links.SetsAll()%>"><%= Model.TotalSetCount.ToString("N0") %> zusammengestellte Fragesätze</a> (jeder Fragesatz bündelt mehrere Fragen zu einem Thema),
                        eingeordnet in <%= Model.TotalCategoryCount.ToString("N0") %> Themen und Unterthemen.
                        Du kannst die vorhandenen Fragesätze einfach nutzen und zum Beispiel deinen Schülern zum Lernen empfehlen.
                    </p>
                    <img src="/Images/Screenshots/fragen-zusammenstellen.png" class="screenshot" width="525" height="379"/>
                    <p>
                        Die Fragen in den Fragesätzen passen nicht genau auf den behandelten Stoff? Kein Problem, du kannst die Zusammenstellung ganz leicht ändern und bei Bedarf
                        mit eigene Fragen ergänzen. Dabei musst du das Rad nicht neu erfinden. So geht's:
                    </p>
                    <ol>
                        <li>
                            Erstelle einen <a href="<%= Links.SetCreate() %>">neuen leeren Fragesatz</a>
                            (Menü <i class="fa fa-arrow-circle-o-right "></i> Fragesätze <i class="fa fa-arrow-circle-o-right "></i> Fragesatz erstellen)
                        </li>
                        <li>
                            Suche alle passenden Fragen, die du gerne übernehmen würdest. Gehe dazu zu <a href="<%= Links.QuestionsAll() %>">Fragen</a> und nutze die Filterfunktionen.
                            Du kannst dir zum Beispiel nur die Fragen zu einem Thema anzeigen lassen. (Vgl. Screenshot)
                        </li>
                        <li>Markiere die gewünschten Fragen (Klick auf die Checkbox in der oberen Hälfte des Bildes)</li>
                        <li>Klick auf die Schaltfläche "Zum Fragesatz hinzufügen". Wähle hier den von dir angelegten Fragesatz aus.</li>
                    </ol>
                    <p>
                        Bei <a href="<%= Links.SetsMine() %>">"Meine Fragesätze"</a> findest du deine Fragesätze jederzeit wieder und kannst sie bearbeiten.
                        Füge ein passendes Bild hinzu, eine gute Beschreibung und passe gegebenenfalls die Reihenfolge der Fragen an.
                        Wenn andere deinen Fragesatz nützlich finden und damit lernen, erhältst du Reputationspunkte!
                    </p>
                    <p>
                        Wenn etwas fehlt, kannst du einfach <a href="<%= Links.CreateQuestion() %>">eigene Fragen erstellen</a>
                        (Menü <i class="fa fa-arrow-circle-o-right "></i> Fragen <i class="fa fa-arrow-circle-o-right "></i> Frage erstellen) 
                        und zu deinem Fragesatz hinzufügen. Deinen eigenen Fragesatz kann niemand außer dir verändern.
                    </p>                            
                    
                    

                </div>

            </div>
        </div>


    </asp:Content>
