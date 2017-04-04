<%@ Page Title="Widget-Integration von memucho" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Help/Widget.css" rel="stylesheet" />
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

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="row">
        <div class="col-xs-12">

            <div class="well">

                <h1 class="PageHeader" style="margin-bottom: 15px; margin-top: 0;"><span class="ColoredUnderline GeneralMemucho">Lerninhalte in die eigene Webseite einbinden</span></h1>
                <p class="teaserText">
                    Die Lerntechnologie und die Lerninhalte von memucho können als Widget leicht in bestehende Webseiten integriert werden. 
                    Egal ob auf dem privaten Blog, der Vereins- oder Unternehmensseite, oder dem Schul- oder Lernmanagementsystem: Nötig ist eine Zeile HTML-Code, 
                    die du einfach von memucho kopieren kannst.
                </p>
                   
            </div>
        </div>
    </div>



    <div class="row">
        <div class="col-xs-12">
            <div class="well">

                <h3 style="margin-bottom: 15px;" class="PageHeader">
                    <span class="ColoredUnderline GeneralMemucho">Quiz auf deiner Webseite: Wissenstest mit Auswertung einbinden</span>
                </h3>
                <p>
                    Du kannst einen Quiz zu einem ganzen Fragesatz auf deiner Webseite einbinden. So können deine Webseitenbesucher ihr Wissen testen - und das macht vielen Spaß.
                    Und du hast eine einfache Möglichkeit, schnell gute Inhalte auf deine Seite zu bringen.
                </p>
                <p>
                    Alle Inhalte bei memucho sind dafür nutzbar. <strong>Du kannst vorhandene Inhalte frei verwenden, neu zusammenstellen und bei Bedarf mit eigenen ergänzen.</strong>
                    Wenn du keinen perfekt passenden Fragesatz findest, erstelle einfach selbst einen und füge vorhandene oder neue Fragen von dir hinzu.
                    So geht's:
                </p>
                <h4>1. Fragesatz finden und Einbetten-Dialog öffnen</h4>
                <p class="screenshotExplanation">
                    Suche dir den passenden <a href="<%= Links.SetsAll() %>">Fragesatz</a> aus oder <a href="<%= Links.SetCreate() %>">erstelle dir selbst einen</a>.
                    Auf der Fragesatzseite findest du den Link <code><i class="fa fa-code fa-code">&nbsp;</i>Einbetten</code> (vgl. Bild).
                    Klicke darauf.
                </p>
                <p class="screenshot">
                    <img src="/Images/Screenshots/SetEmbedLink.png" />
                </p>

                <h4>2. HTML-Codezeile kopieren</h4>
                <p class="screenshotExplanation">
                    Es öffnet sich das Dialogfenster zur Einbettung. Oben findest du die HTML-Codezeile (vgl. Bild). 
                    Markiere sie und kopiere sie an die Stelle deiner Webseite, wo der Quiz bei dir erscheinen soll.<br/>
                </p>
                <p class="screenshotExplanation">
                    Wenn du magst, kannst du bei den "Einstellungen" noch ein paar Dinge verändern.
                    Unten im Dialogfenster siehst du gleich eine Vorschau, wie das Quiz-Widget bei dir aussehen wird.
                </p>
                <p class="screenshot">
                    <img src="/Images/Screenshots/SetEmbedModal.png" />
                </p>

                <h4>3. Code in die eigene Webseite einfügen</h4>                    
                <p class="screenshotExplanation">
                    Die Code-Zeile aus dem Dialog kannst du direkt an die Stelle deiner Webseite kopieren, wo der Quiz erscheinen soll.
                    Achte darauf, dass du dich in einem Modus befindest, wo du HTML-Code einfügen darfst (als Klartext, ohne Formatierungen). Oft gibt es dafür einen Umschalter. 
                    Wo genau, das zeigen wir dir hier für die folgenden Systeme: 
                </p>
                <ul class="screenshotExplanation">
                    <li>Wordpress</li>
                    <li>Moodle</li>
                    <li>Blackboard</li>
                </ul>
                <h3>Fragen oder Probleme?</h3>
                <p>
                    Funktioniert es nicht? Sind noch Fragen offen? Kein Problem, melde dich einfach bei uns, wir helfen dir gerne weiter. Christof erreichst du unter 01577-6825707 
                    oder per E-Mail an <span class="mailme">christof at memucho dot de</span>.
                </p>
            </div>
        </div>
    </div>
    
    
    <div class="row">
        <div class="col-xs-12">
            <div class="well">

                <h2 style="margin-bottom: 15px;" class="PageHeader">
                    <span class="ColoredUnderline GeneralMemucho">Video-Fragesatz mit Video und Fragen [...]</span>
                </h2>
                <p class="subheader">
                    [...]
                </p>
                <p>
                    [Bild: So sieht's aus]
                </p>
                
                <p>
                    Du willst genauer wissen, wie du einen Fragesatz mit Video einbettest?
                    <button class="btn btn-secondary" data-toggle="collapse" data-target="#WidgetSetVideoDetails">Schritt-für-Schritt-Anleitung anzeigen</button>
                </p>

                <div id="WidgetSetVideoDetails" class="collapse">
                    <h4 style="margin-top: 25px;">
                        Schritt-für-Schritt-Anleitung zur Einbettung von Fragesätzen
                    </h4>
                    <p>
                        [bla...]
                    </p>

                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-xs-12">
            <div class="well">
                <h2 style="margin-bottom: 15px;" class="PageHeader">
                    <span class="ColoredUnderline GeneralMemucho">Einzelne Fragen per Widget einbinden</span>
                </h2>
                <p>
                    [beim Blogbeitrag Text auflockern, Nutzer zu Interaktion animieren...]
                </p>
                <p>
                    [Bild: So sieht's aus]
                </p>
                <p>
                    Du willst genauer wissen, wie ...
                    <button class="btn btn-secondary" data-toggle="collapse" data-target="#WidgetQuestionDetails">Schritt-für-Schritt-Anleitung anzeigen</button>
                </p>

                <div id="WidgetQuestionDetails" class="collapse">
                    <h4 style="margin-top: 25px;">
                        Schritt-für-Schritt-Anleitung [...]
                    </h4>
                    <ol>
                        <li>[bla]</li>
                        <li>[bla]</li>
                    </ol>
                </div>
            </div>
        </div>
    </div>
    


</asp:Content>