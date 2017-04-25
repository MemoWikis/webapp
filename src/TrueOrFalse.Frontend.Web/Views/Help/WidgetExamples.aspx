<%@ Page Title="Widget: Angebote und Preise" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Help/Widget.css" rel="stylesheet" />
    
    <%= Scripts.Render("~/bundles/mailto") %>
    <script type="text/javascript" >
        $(function () {
            $("a.mailmeMore")
                .each(function () {
                    var hrefOrg = this.getAttribute('href');
                    var spt = hrefOrg.substr(0, hrefOrg.indexOf("?") - 1);
                    //console.log("hrefOrg: " + hrefOrg);
                    //console.log("spt: " + spt);
                    var at = / at /;
                    var dot = / dot /g;
                    var addr = spt.replace(at, "@").replace(dot, ".");
                    var hrefNew = "mailto:" + addr + hrefOrg.substr(hrefOrg.indexOf("?"));
                    //console.log("hrefNew: " + hrefNew)
                    this.setAttribute('href', hrefNew);
                });
        });
    </script>    
    
    <%= Scripts.Render("~/bundles/js/Help") %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <% Html.RenderPartial("~/Views/Help/WidgetMenu.ascx", new WidgetMenuModel{CurrentIsExample = true});  %>

    <div class="row">
        <div class="col-xs-12">
            <div class="well">

                <h1 class="PageHeader"><span class="ColoredUnderline GeneralMemucho">Beispiele</span></h1>
                
                <div class="row" style="margin-top: -40px;">
                    <div class="col-lg-4">
                        <h2>Einzelfrage</h2>
                        <ul class="nav nav-pills nav-stacked">
                            <li><a href="#singleChoice">Single Choice</a></li>
                            <li><a href="#multipleChoice">Multiple Choice</a></li>
                            <li><a href="#dragAndDrop">Zuordnen (Drag and Drop)</a></li>
                            <li><a href="#flip">Flip</a></li>
                            <li><a href="#text">Text-Antwort</a></li>
                        </ul>
                    </div>
                    <div class="col-lg-4">
                        <h2>Fragesatz (Quiz)</h2>
                        <ul class="nav nav-pills nav-stacked">
                            <li><a href="#setDefault">Standard-Fragesatz (Quiz)</a></li>
                            <li><a href="#setVideo"><i class="fa fa-youtube-play" aria-hidden="true"></i> Video-Fragesatz</a></li>
                        </ul>
                    </div>
                    <div class="col-lg-4">
                        <h2>Lehrer & Lerntools</h2>
                        <ul class="nav nav-pills nav-stacked">
                            <li><a href="#">PDF-Export Fragesatz</a></li>
                            <li><a href="#">Dashboard</a></li>
                            <li><a href="#">mehr auf Anfrage</a></li>
                        </ul>
                    </div>
                </div>
                
                <hr style="margin-bottom: -10px;"/>

                <div class="row features" style="margin-bottom: 30px;">
                    <div class="col-md-6 col-xs-12">
                        <h3>Konfiguration</h3>
                        <ul class="nav nav-pills nav-stacked">
                            <li><i class="fa fa-check" aria-hidden="true"></i> Breite flexibel einstellbar</li>
                            <li><i class="fa fa-check" aria-hidden="true"></i> optionales Branding</li>
                            <li><i class="fa fa-check" aria-hidden="true"></i> optionaler Wunschwissenbutton</li>
                        </ul>
                    </div>
                    <div class="col-md-6 col-xs-12">
                        <h3>Eigenschaften</h3>
                        <ul class="nav nav-pills nav-stacked">
                            <li><i class="fa fa-check" aria-hidden="true"></i> Resonsive Design</li>
                            <li><i class="fa fa-check" aria-hidden="true"></i> Kleiner Payload (async geladen)</li>
                            <li><i class="fa fa-check" aria-hidden="true"></i> Vielfältige Fragetypen und Video-Unterstützung</li>
                        </ul>
                    </div>
                </div>
                
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
                    Bei diesem Aufgabentyp gibt es genau eine richtige Antwort. Das Bild ist optional. <br />
                    Nach einem Bericht über den Pilz-Fund beim letzten Waldausflug fragen wir dich also:
                </p>
                    
                <h3 id="multipleChoice">Multiple Choice</h3>
                <p>
                    Beim "echten" Multiple Choice können keine oder mehrere Antworten richtig sein. 
                    Das erhöht den Schwierigkeitsgrad, macht aber auch die Fragen oft interessanter.
                </p>
                    
                <h3 id="dragAndDrop">Zuordnen (Drag and Drop)</h3>
                <p>
                    Hier müssen Elemente zugeordnet werden, die zueinander passen. Der Einsatz ist sehr flexibel und eignet sich 
                    zum Beispiel auch dazu, Arbeitsschritte in die richtige Reihenfolge zu bringen. <br/>
                    Bei einer Kursseite zur Prozentrechnung, direkt hinter dem Absatz zu Grundwert/Prozentwert fragen wir dich also:
                </p>
                    
                <h3 id="flip">Flip</h3>
                <p>
                    In Anlehnung an die klassische Karteikarte können hier Antworten durch Umdrehen einer Karte aufgedeckt werden. 
                    Dieser Frage-Antwort-Typ ist in Arbeit und folgt demnächst.
                </p>
                    
                <h3 id="text">Freie Textantwort</h3>
                <p>
                    Zur Prüfung von aktivem Wissen eignet sich dieser Typ besonders gut. Möglich ist auch die Eingabe einer Zahl oder eines Datums. <br/>
                    Am Ende unseres kleinen Kurses zur Prozentrechnung sollst du also nochmal Kopfrechnen:
                </p>

            </div>
        </div>
    </div>
    
    <div class="row">
        <div class="col-xs-12">
            <div class="well">
                
                <h1 class="PageHeader"><span class="ColoredUnderline GeneralMemucho">Fragesatz</span></h1>
                
                <p>
                    
                </p>
                
                <h3 id="setDefault">Standard-Fragesatz (Quiz)</h3>
                
                <h3 id="setVideo">Video-Fragesatz</h3>
                <script src="https://memucho.de/views/widgets/w.js" data-t="setVideo" data-id="95" data-width="100%" data-maxdata-width="100%" data-hideKnowledgeBtn="true"></script>
            </div>
        </div>
    </div>
    
    <div class="row">
        <div class="col-xs-12">
            <div class="well">
                
                <h1 class="PageHeader"><span class="ColoredUnderline GeneralMemucho">Lehrer & Lerntools</span></h1>
                <p>Dokumentation folgt.</p>
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