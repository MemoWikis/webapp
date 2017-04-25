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
                            <li><a href="#dragAndDrop">Drag and Drop</a></li>
                            <li><a href="#flip">Flip</a></li>
                            <li><a href="#text">Text-Antwort</a></li>
                        </ul>
                    </div>
                    <div class="col-lg-4">
                        <h2>Fragesatz</h2>
                        <ul class="nav nav-pills nav-stacked">
                            <li><a href="#setDefault">Default</a></li>
                            <li><a href="#setVideo"><i class="fa fa-youtube-play" aria-hidden="true"></i> Video</a></li>
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
                    <div class="col-lg-4">
                        <h2>Konfiguration</h2>
                        <ul class="">
                            <li><i class="fa fa-check" aria-hidden="true"></i> Breite flexibel einstellbar</li>
                            <li><i class="fa fa-check" aria-hidden="true"></i> optionales Branding</li>
                            <li><i class="fa fa-check" aria-hidden="true"></i> optionaler Wunschwissenbutton</li>
                        </ul>
                    </div>
                    <div class="col-lg-4">
                        <h2>Eigenschaften</h2>
                        <ul class="nav nav-pills nav-stacked">
                            <li><i class="fa fa-check" aria-hidden="true"></i> Resonsive Design</li>
                            <li><i class="fa fa-check" aria-hidden="true"></i> Kleiner Payload (async geladen)</li>
                            <li><i class="fa fa-check" aria-hidden="true"></i> optionaler Wunschwissenbutton</li>
                        </ul>
                    </div>
                </div>
                
            </div>
        </div>
    </div>
    
    <div class="row">
        <div class="col-xs-12">
            <div class="well">
                
                <h1 class="PageHeader"><span class="ColoredUnderline GeneralMemucho">Einzelfragen</span></h1>
                    
                <h3><a name="singleChoice">Single Choice</a></h3>
                <script src="https://memucho.de/views/widgets/w.js" t="question" id="3629" width="100%" maxWidth="100%" hideKnowledgeBtn="true"></script>
                    
                <h3><a name="multipleChoice">Multiple Choice</a></h3>
                <script src="https://memucho.de/views/widgets/w.js" t="question" id="3485" width="100%" maxWidth="100%" hideKnowledgeBtn="true"></script>
                    
                <h3><a name="dragAndDrop">Drag and Drop</a></h3>
                <script src="https://memucho.de/views/widgets/w.js" t="question" id="3623" width="100%" maxWidth="100%" hideKnowledgeBtn="true"></script>
                    
                <h3><a name="flip">Flip</a></h3>
                <p>In Arbeit</p>
                    
                <h3><a name="text">Text</a></h3>
                <script src="https://memucho.de/views/widgets/w.js" t="question" id="3638" width="100%" maxWidth="100%" hideKnowledgeBtn="true"></script>

            </div>
        </div>
    </div>
    
    <div class="row">
        <div class="col-xs-12">
            <div class="well">
                
                <h1 class="PageHeader"><span class="ColoredUnderline GeneralMemucho">Fragesatz</span></h1>
                
                <h3><a name="setDefault">Default</a></h3>
                <script src="https://memucho.de/views/widgets/w.js" t="set" id="22" width="100%" maxWidth="100%" hideKnowledgeBtn="true"></script>
                
                <h3><a name="setVideo">Video</a></h3>
                <script src="https://memucho.de/views/widgets/w.js" t="setVideo" id="95" width="100%" maxWidth="100%" hideKnowledgeBtn="true"></script>
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