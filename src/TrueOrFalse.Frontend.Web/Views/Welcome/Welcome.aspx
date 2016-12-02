<%@ Page Title="memucho: Schneller lernen, länger wissen" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
	Inherits="ViewPage<WelcomeModel>"%>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <link rel="canonical" href="<%= Settings.CanonicalHost %>">
    <meta name="description" content="memucho hilft dir beim Lernen. Du sparst Zeit, bist effizienter und es macht mehr Spaß! Entdecke neues Wissen oder füge dein eigenes hinzu.">
</asp:Content>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <link href="/Views/Welcome/Welcome.css" rel="stylesheet" />
    
    <script type="text/javascript" >

        /* Source http://www.html-advisor.com/javascript/hide-email-with-javascript-jquery/ */
        $(function () {
            var spt = $('span.mailme');
            var at = / at /;
            var dot = / dot /g;
            var addr = $(spt).text().replace(at, "@").replace(dot, ".");
            $(spt).after('<a href="mailto:' + addr + '" title="Send an email">' + addr + '</a>').hover(function () { window.status = "Send a letter!"; }, function () { window.status = ""; });
            $(spt).remove();
        });
    </script>

    <%= Scripts.Render("~/bundles/guidedTourScript") %>
    <%= Styles.Render("~/bundles/guidedTourStyle") %>
    <%= Scripts.Render("~/bundles/Welcome") %>
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    
<div class="row">
       
    <div class="col-md-9">
            
        <div class="well" style="padding: 13px; padding-bottom: 10px;">
            <h1 style="margin-top: 0; margin-bottom: 15px; font-size: 24px; text-align: center;"><span style="white-space: nowrap;">Schneller lernen,</span> <span style="white-space: nowrap;">länger wissen</span></h1>
            <div class="row">
                <div class="col-xs-4 xxs-stack" style="text-align: center; font-size: 100%; padding: 5px;">
                    <p>
                        <i class="fa fa-2x fa-lightbulb-o" style="color: #2C5FB2;"></i>
                    </p>
                    <p>
                        Wir optimieren dein Lernen mit einem <a href="#teaserWhatIsMemucho" data-click-log="StartPage,Click,Lernplaene">persönlichen Übungsplan!</a><br/>
                    </p>
                </div>
                <div class="col-xs-4 xxs-stack" style="text-align: center; font-size: 100%; padding: 5px;">
                    <p>
                        <i class="fa fa-2x fa fa-clock-o" style="color: #2C5FB2;"></i>
                    </p>
                    <p>
                        <a href="#teaserWhatIsMemucho" data-click-log="StartPage,Click,Zeitsparen">Du sparst Zeit</a> und siehst, wie viel Zeit du noch zum Lernen brauchst.
                    </p>
                </div>
                <div class="col-xs-4 xxs-stack" style="text-align: center; font-size: 100%; padding: 5px;">
                    <p>
                        <i class="fa fa-2x fa-line-chart" style="color: #2C5FB2;"></i>
                    </p>
                    <p>
                        <a href="#teaserWhatIsMemucho" data-click-log="StartPage,Click,Algos">Optimierte Algorithmen</a> erinnern dich ans Lernen, bevor du vergisst.
                    </p>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12" style="text-align: left; margin-top: 15px;">
                        <a href="#" class="btn btn-link btn-sm ButtonOnHover" id="btnStartWelcomeTour" data-click-log="WelcomeTour,Click,Start"><i class="fa fa-map-signs">&nbsp;</i>Lerne memucho kennen in 6 Schritten</a>
                        <a href="<%= Links.AboutMemucho() %>" class="btn btn-link ButtonOnHover pull-right">ERFAHRE MEHR...</a><br />
                    <p>
<%--                        <a href="#teaserWhatIsMemucho" class="" data-click-log="StartPage,Click,WasKannMemucho"><i class="fa fa-info-circle" style="">&nbsp;</i>Erfahre, wie memucho dir beim Lernen hilft</a><br/>--%>
                    </p>
                    <p>
                    </p>    
                </div>
            </div>
        </div>

        <div class="row CardsPortrait" style="padding-top: 0;">
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(42)); //Abitur Weimarer Republik %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(45)); //EU-Integration %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(49)); //Abitur Franz. Rev %>
        </div>

        <div class="row CardsPortrait" style="padding-top: 0;">
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(40)); //Mutterschutz Rechtliche Grundlagen %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(47)); //Binnenschein-Basisfragen %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(27)); //Einbürgerungstest %>
        </div>

        <% Html.RenderPartial("WelcomeBoxSetImgQ", WelcomeBoxSetImgQModel.GetWelcomeBoxSetImgQModel(19, new[] { 468, 464, 460 })); // EU-Länder erkennen %>

        <div class="row CardsPortrait" style="padding-top: 0;">
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(44)); //EU-Organe %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(20, "Kennst du die Hauptstädte aller 28 Länder der Europäischen Union? Finde es heraus!")); %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(46)); //Pferdehaltung %>
        </div>

        <div class="row CardsPortrait" style="padding-top: 0;">
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(30)); //Literatur Klassik %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(35)); //Literatur Barock %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(29)); //Literatur Zitate %>
        </div>

        <% Html.RenderPartial("WelcomeBoxCategoryTxtQ", WelcomeBoxCategoryTxtQModel.GetWelcomeBoxCategoryTxtQModel(205, new int[] { 381, 379, 384 }, "Du möchtest dir eine fundierte Meinung zur Flüchtlingspolitik bilden? Erweitere dein Hintergrundwissen mit Fakten!")); %>
        
        <div class="row CardsPortrait" style="padding-top: 0;">
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(22, "Farfalle, Penne oder Rigatoni? Weißt du, wie diese Nudelsorten heißen?")); %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(17)); //Sehenswürdigkeiten %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(14)); //Laubbäume %>
            <%--<% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(37)); //ARD-Intendanten %>--%>
        </div>

        <% Html.RenderPartial("WelcomeBoxCategoryTxtQ", WelcomeBoxCategoryTxtQModel.GetWelcomeBoxCategoryTxtQModel(336, new int[] { 973, 965, 962 }, "Knifflige Fragen! Wer erfand den Champagner? Der Mönch Dom Pérignon 1670?")); %>

        <% Html.RenderPartial("WelcomeBoxCategoryTxtQ", WelcomeBoxCategoryTxtQModel.GetWelcomeBoxCategoryTxtQModel(14, new int[] { 404, 405, 406 }, "Du verstehst den Wirtschafts-Teil der Zeitung nicht? Teste und erweitere dein Grundlagen-Wissen zu Wirtschaftsthemen!")); %>

        <%--<% Html.RenderPartial("WelcomeBoxSetImgQ", WelcomeBoxSetImgQModel.GetWelcomeBoxSetImgQModel(17, new[] { 373, 360, 367 })); %>--%> <%-- Sehenswürdigkeiten--%>

        <div class="row CardsPortrait" style="padding-top: 0;">
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(18)); //Sieger Fußbal-WM Herren %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(13)); //Scherzfragen %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(7)); //Bundeskanzler erkennen %>
        </div>

        <%--<% Html.RenderPartial("WelcomeBoxSetImgQ", WelcomeBoxSetImgQModel.GetWelcomeBoxSetImgQModel(14, new[] { 348, 341, 344 })); // Laubbäume %>--%>

        <% Html.RenderPartial("WelcomeBoxSetTxtQ", WelcomeBoxSetTxtQModel.GetWelcomeBoxSetTxtQModel(12, new[] { 303, 288, 289 }, "Der berühmteste Agent im Dienste Ihrer Majestät: Kennst du die wichtigsten Fakten zu den James Bond-Filmen?")); %>


        <div class="well">
            <h3>
                <a name="teaserWhatIsMemucho"></a>
                Was ist memucho?
            </h3>
            <p>
                memucho ist eine vernetzte Lern- und Wissensplattform. Damit kannst du:
            </p>
            
            <div class="row">
                <div class="col-xs-6 xxs-stack" style="text-align: center; font-size: 100%; padding: 5px 5px 10px;">
                  <i class="fa fa-clock-o fa-2x show-tooltip" style="color: #2C5FB2"></i><br/>
                    <b>Schneller lernen</b>
                    <p>
                        memucho analysiert dein Lernverhalten und wiederholt schwierige Fragen zum optimalen Zeitpunkt. 
                        So brauchst du weniger Zeit zum Lernen.
                    </p>
                </div>
                <div class="col-xs-6 xxs-stack" style="text-align: center; font-size: 100%; padding: 5px 5px 10px;">
                  <i class="fa fa-book fa-2x show-tooltip" style="color: #2C5FB2"></i><br/>
                    <b>Wissen erweitern</b>
                    <p>
                        Du möchtest gerne mehr über Politik, die Griechenland-Krise oder über James Bond-Filme wissen? 
                        Finde die passenden Fragesätze und stelle dir dein Wunschwissen zusammen!
                    </p>
                </div>
                <div class="clearfix"></div>
                <div class="col-xs-6 xxs-stack" style="text-align: center; font-size: 100%; padding: 5px 5px 10px;">
                  <i class="fa fa-calendar-o fa-2x show-tooltip" style="color: #2C5FB2"></i><br/>
                    <b>Zu einem bestimmten Termin lernen</b>
                    <p>
                        Eine Klassenarbeit oder eine Prüfung steht an? Mit Terminen in memucho weißt du immer, 
                        was du schon sicher kannst und wo du weiter üben musst.
                    </p>
                </div>
                <div class="col-xs-6 xxs-stack" style="text-align: center; font-size: 100%; padding: 5px 5px 10px;">
                  <i class="fa fa-pie-chart fa-2x show-tooltip" style="color: #2C5FB2"></i><br/>
                    <b>Überblick behalten</b>
                    <p>
                        Du möchtest dir gerne 50, 500, 5000 (oder mehr) Fakten merken? Kein Problem, mit memucho behältst du den Überblick.
                    </p>
                </div>
                <div class="clearfix"></div>
                <div class="col-xs-6 xxs-stack" style="text-align: center; font-size: 100%; padding: 5px 5px 10px;">
                  <i class="fa fa-share-alt fa-2x show-tooltip" style="color: #2C5FB2"></i><br/>
                    <b>Wissen teilen</b>
                    <p>
                        memucho ist ein offenes Netzwerk, wo du dein Wissen teilen und das Wissen anderer nutzen kannst. 
                        Denn Wissen wird mehr, wenn man es teilt!
                    </p>
                </div>
                <div class="col-xs-6 xxs-stack" style="text-align: center; font-size: 100%; padding: 5px 3px 20px;">
                  <i class="fa fa-users fa-2x show-tooltip" style="color: #2C5FB2"></i><br/>
                    <b>Gemeinsam lernen</b>
                    <p>
                        Lerne gemeinsam mit Freunden und verabrede dich zum Quizduell, um dich auf die Klassenarbeit vorzubereiten.
                    </p>
                </div>
                <div class="clearfix visible-xs"></div>
                <div class="col-xs-12">
                    <p>
                        Ausprobieren? <a href="<%= Url.Action("Register", "Welcome") %>">Registriere dich</a> und lege los! <br/>
                        Tolle Idee? Unterstütze uns und werde <a id="SupportUs" class="helpLink TextLinkWithIcon" href="<%= Url.Action(Links.Membership, Links.AccountController) %>">
                        <i class="fa fa-thumbs-up"></i>&nbsp;Fördermitglied</a> der ersten Stunde!
                    </p>                
                </div>
            </div>
        </div>

        <div class="well">
            <h3>
                <a name="teaserPrinciples"></a>
                Unsere Prinzipien
            </h3>
            <ul class="fa-ul">
                <li><i class="fa fa-li fa-book"></i>
                    <b>Freie Bildungsinhalte ("Open Educational Resources")</b>
                    <p>
                        Wir sind Teil der Bewegung zur Förderung frei zugänglicher Bildungsmaterialien.
                        In memucho unterliegen öffentliche Inhalte einer Creative Commons-Lizenz, 
                        genau wie fast alle Einträge auf Wikipedia. Öffentliche memucho-Inhalte 
                        können also von jedem kostenfrei und ohne Einschränkungen verwendet werden 
                        (<a rel="license" href="http://creativecommons.org/licenses/by/4.0/deed.de">Hier 
                        erfährst du genaueres zur Lizenz CC BY 4.0.</a>). Private Inhalte sind aber privat.
                    </p>
                </li>
                <li><i class="fa fa-li fa-tree"></i>
                    <b>Gemeinwohlorientierung</b><br/>
                    <p>
                        Wir möchten unser Unternehmen auf gemeinwohlfördernden Werten aufbauen. 
                        Als Teil der <a href="http://www.gemeinwohl-oekonomie.org/de">Gemeinwohlökonomie</a> 
                        sind wir davon überzeugt, dass Unternehmen der Gemeinschaft dienen müssen und deshalb 
                        eine ethische, soziale und ökologische Verantwortung haben. Daher werden wir in Zukunft 
                        eine Gemeinwohlbilanz veröffentlichen.
                    </p>
                </li>
                <li><i class="fa fa-li fa-exchange"></i>
                    <b>Mitwirkung und Vernetzung</b>
                    <p>
                        Wir glauben, dass Wissen vernetzt sein muss. Der Vernetzungsgedanke spielt bei uns eine 
                        große Rolle. In der aktuellen Beta-Phase ist uns euer Feedback ganz besonders wichtig, 
                        aber auch später werden Mitglieder mitentscheiden, welche Funktionen wir als nächstes umsetzen 
                        und aktive Mitglieder können Inhalte mitmoderieren.
                    </p>
                </li>
                <li><i class="fa fa-li fa-lock"></i>
                    <b>Datenschutz ist uns sehr sehr wichtig</b>
                    <p>
                        Wir nutzen deine Daten, damit du besser lernen kannst und um memucho besser zu machen. 
                        Aber wir werden deine Daten niemals verkaufen. (<a class="helpLink" href="<%= Links.FAQItem("DataPrivacy") %>">Erfahre mehr</a> über unseren Datenschutz.)
                    </p>
                </li>
                <li><i class="fa fa-li fa-github"></i>
                    <b>Open-Source und Transparenz</b>
                    <p>
                        Die Software, mit der memucho läuft, steht unter einer Open-Source-Lizenz. Die Quelltexte 
                        sind frei verfügbar und können von allen frei verwendet werden. Du findest sie 
                        auf <a href="https://github.com/TrueOrFalse/TrueOrFalse" target="_blank"><i class="fa fa-github">&nbsp;</i>Github</a>. 
                        In Zukunft möchten wir neben der Gemeinwohlbilanz auch unsere Unternehmenszahlen veröffentlichen.
                    </p> 
                </li>        
            </ul>
            <p>
                Ausprobieren? <a href="<%= Url.Action("Register", "Welcome") %>">Registriere dich</a> und lege los! <br/>
                Tolle Idee? Unterstütze uns und werde <a id="SupportUs" class="helpLink TextLinkWithIcon" href="<%= Url.Action(Links.Membership, Links.AccountController) %>">
                <i class="fa fa-thumbs-up"></i>&nbsp;Fördermitglied</a> der ersten Stunde!
            </p>                
        </div>
        <div class="well Founder">
            <h3>
                <a name="teaserWhoWeAre"></a>
                Team
            </h3>
            <div class="row">
                
                <div class="col-xs-4 ImageColumn">
                    <img src="/Images/Team/team_robert201509_155.jpg"/>
                        <br/> <b>Robert</b> (Gründer) <br/>
                </div>
                
                <div class="col-xs-4 ImageColumn">
                    <img src="/Images/Team/team_jule201509-2_155.jpg"/>  
                    <br/> <b>Jule</b> (Gründerin) <br/> 
                </div>

                <div class="col-xs-4 ImageColumn">
                    <img src="/Images/Team/team_christof201509_155.jpg"/>  
                    <br/> <b>Christof</b> (Gründer) <br/> 
                </div>


                <div class="col-xs-12" style="margin-top: 10px;">
                    <p>
                        Wir möchten den Zugang zu freien Bildungsinhalten verbessern und dass Faktenlernen einfacher wird und mehr Spaß macht. 
                        Und wir möchten dabei ein stabiles <a href="#teaserPrinciples">gemeinwohlorientiertes Unternehmen</a> aufbauen. 
                        Als Gründungsteam konzipieren, gestalten und programmieren wir memucho gemeinsam.
                    </p>
                    <p>
                        Wenn du Fragen oder Anregungen hast, schreibe uns eine E-Mail an <span class="mailme">team at memucho dot de</span> oder rufe Christof an: +49-1577-6825707.
                    </p>
                </div>
            </div>
        </div>  
    </div>
            
    <div class="col-md-3">
        <%
            var userSession = new SessionUser();
            if (!userSession.IsLoggedIn){
        %>
            <div class="well" id="boxLoginOrRegister" style="padding: 20px; ">
                <a href="<%= Url.Action("Register", "Welcome") %>" class="btn btn-success" style="width: 100%;" role="button"><i class="fa fa-chevron-circle-right">&nbsp;</i> Jetzt Registrieren</a>
                <div style="margin-top: 3px; font-style: italic">*memucho ist kostenlos.</div>
            </div>
        <% } %>

        <div class="well" id="nominationInnopreis" style="padding: 20px; ">
            <img src="/Images/LogosPartners/innovationspreis-Logo-quer-positiv-web-HGw.png" height="44" width="159" style="margin-bottom: 10px;"/>
            <p>
                memucho ist nominiert für den Innovationspreis Berlin Brandenburg!
            </p>
            <p>
                <a href="http://www.innovationspreis.de/news/aktuelles/zehn-nominierungen-f%C3%BCr-den-innovationspreis-berlin-brandenburg-2016.html" target="_blank">
                    <span style="white-space: nowrap">Zur Jury-Entscheidung <i class="fa fa-external-link"></i></span>
                </a>
            </p>
            <p>
                <a href="http://www.inforadio.de/programm/schema/sendungen/wirtschaft_aktuell/201611/73133.html" target="_blank">
                    Bericht <span style="white-space: nowrap">rbb Inforadio <i class="fa fa-external-link"></i></span>
                </a>
            </p>
        </div>

        <div class="well">
            <h4>Neueste Fragesätze</h4>
            <div class="LabelList">
                <% Html.RenderPartial("WelcomeBoxTopSets", WelcomeBoxTopSetsModel.CreateMostRecent(5)); %>
            </div>
        </div>
        
        <div class="well">
            <h4>Neueste Fragen</h4>
            <div class="LabelList">
                <% Html.RenderPartial("WelcomeBoxTopQuestions", WelcomeBoxTopQuestionsModel.CreateMostRecent(8)); %>
            </div>
        </div>
        
        <div class="well">
            <h4>Top-Kategorien nach Fragen</h4>
                <div class="LabelList">
                    <% Html.RenderPartial("WelcomeBoxTopCategories", WelcomeBoxTopCategoriesModel.CreateTopCategories(5)); %>
                </div>
        </div>
        
        <div class="well">
            <h4>Neueste Kategorien:</h4>
            <div class="LabelList">
                <% Html.RenderPartial("WelcomeBoxTopCategories", WelcomeBoxTopCategoriesModel.CreateMostRecent(5)); %>
            </div>
        </div>

        <div class="well">
            <h4>Umfangreichste Fragesätze</h4>
            <div class="LabelList">
                <% Html.RenderPartial("WelcomeBoxTopSets", WelcomeBoxTopSetsModel.CreateMostQuestions(5)); %>
            </div>
        </div>
        
        <% if (!userSession.IsLoggedIn)
           { %>
            <div class="well" id="newsletterSignUp" style="padding: 20px; ">
                <h4>Newsletter</h4>
                <p>Du möchtest bei wichtigen Neuigkeiten benachrichtigt werden? Melde dich hier an:</p>
                <div class="alert alert-danger" role="alert" id="msgInvalidEmail" style="display:none">
                    Keine gültige E-Mail-Adresse.
                </div>
            
                <div class="alert alert-success" role="alert" id="msgEmailSend" style="display:none">
                    Deine Adresse wurde für den Newsletter angemeldet.
                </div>

                <form class="form-inline" style="color: white;">
                    <div class="">
                        <input type="email" class="form-control" id="txtNewsletterRequesterEmail" placeholder="deine@email.de" style="width: 100%;">
                    </div>
                    <div class="" style="text-align: center; margin-top: 10px;">
                        <a class="btn btn-primary" href="#" id="btnNewsletterRequest" style="">
                            <i class="fa fa-envelope-o">&nbsp;</i>Anmelden
                        </a>
                    </div>
                </form> 
            </div>
        <% } %>
        <%--<div class="row" style="padding-top: 10px;">
            <div class="col-md-12"><h3 class="media-heading">memucho-Netzwerk</h3></div>
        </div>
        
        <div class="panel panel-default" style="padding-top: 15px; opacity: 0.4;">
            <div class="panel-heading">Nutzer-Ranking nach Reputation</div>
            <div class="panel-body" style="padding-top: 12px;">
                <p style="padding-left: 5px;"><img src="/favicon-32x32.png" height="25" width="25" style="float: left; vertical-align: text-bottom"/>&nbsp;Pauli (130 Punkte)</p>
                <p style="padding-left: 5px;"><img src="/favicon-32x32.png" height="25" width="25" style="float: left; vertical-align: bottom"/>&nbsp;Robert (120 Punkte)</p>
                <p style="padding-left: 5px;"><img src="/favicon-32x32.png" height="25" width="25" style="float: left; vertical-align: middle"/>&nbsp;Christof (112 Punkte)</p>
            </div>
        </div>--%>

        <%--<div class="row" style="padding-top: 10px;">
            <div class="col-md-6">
                <h4 class="media-heading">Studienfächer</h4>
                <ul>
                    <li><a href="#">Psychologie</a></li>
                    <li><a href="#">Philosophie</a></li>
                    <li><a href="#">Informatik</a></li>
                    <li><a href="#">[mehr]</a></li>
                </ul>
            </div>

            <div class="col-md-6">
                <h4 class="media-heading">Schulfächer</h4>
                <ul>
                    <li><a href="#">Deutsch</a></li>
                    <li><a href="#">Mathe</a></li>
                    <li><a href="#">Geschichte</a></li>
                    <li><a href="#">[mehr]</a></li>
                </ul>
            </div>
        </div>--%>

    </div>
</div>

</asp:Content>