<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
	Inherits="ViewPage<WelcomeModel>"%>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <title>memucho</title>
    <link href="/Views/Welcome/Welcome.css" rel="stylesheet" />
<%--    <%= Scripts.Render("~/bundles/guidedTourScript") %>
    <%= Scripts.Render("~/bundles/Welcome") %>
    <%= Styles.Render("~/bundles/guidedTourStyle") %>--%>
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    
<div class="row">
       
    <div class="col-md-8">
            
        <div class="alert alert-success alert-dismissable" style="padding: 13px; padding-bottom: 0px; background-color: #c8e276;">
            <h1 style="margin-top: 0; margin-bottom: 12px; font-size: 24px;">memucho: Besser Lernen mit mehr Spaß</h1>
            <div class="row">
                <div class="col-xs-4" style="text-align: center; font-size: 100%; padding: 5px;">
                    <p>
                        <i class="fa fa-2x fa-lightbulb-o" style="color: #2C5FB2;"></i><br/>
                        memucho hilft dir beim Lernen. Du sparst Zeit und es macht mehr Spaß!<br/>
                    </p>
                    <!--p>
                        <a href="#" class="btn" id="btnStartWelcomeTour"><i class="fa fa-map-signs"></i> Tour starten</a>
                    </p-->
                </div>
                <div class="col-xs-4" style="text-align: center; font-size: 100%; padding: 5px;">
                    <p>
                        <i class="fa fa-2x fa-book" style="color: #2C5FB2;"></i><br/>
                        Unsere Prinzipien: Freie Bildungsinhalte, Gemeinwohlorientierung und Transparenz.
                    </p>
                </div>
                <div class="col-xs-4" style="text-align: center; font-size: 100%; padding: 5px;">
                    <p>
                        <i class="fa fa-2x fa-users" style="color: #2C5FB2;"></i><br/>
                        Robert, Jule und Christof entwickeln memucho zusammen. Wir haben uns viel vorgenommen.
                    </p>
                </div>
                <p style="text-align: center;">
                    <a href="#teaserWhatIsMemucho"><i class="fa fa-caret-down" style=""></i>&nbsp;Erfahre mehr</a>
                </p>
            </div>
        </div>

        <div class="row ThumbnailRow" style="padding-top: 0;">
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(18)); %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(20, "Kennst du die Hauptstädte aller 28 Länder der Europäischen Union? Finde es heraus!")); %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(22, "Farfalle, Penne oder Rigatoni? Weißt du wie diese Nudelsorten heißen?")); %>
        </div>

        <div class="panel panel-default">
            <% Html.RenderPartial("WelcomeBoxCategoryTxtQ", WelcomeBoxCategoryTxtQModel.GetWelcomeBoxCategoryTxtQModel(205, new int[] { 381, 379, 384 }, "Du möchtest dir eine fundierte Meinung zur Flüchtlingspolitik bilden? Erweitere dein Hintergrundwissen mit Fakten!")); %>
        </div>

        <div class="panel panel-default">
            <% Html.RenderPartial("WelcomeBoxCategoryTxtQ", WelcomeBoxCategoryTxtQModel.GetWelcomeBoxCategoryTxtQModel(14, new int[] { 404, 405, 406 }, "Du verstehst den Wirtschafts-Teil der Zeitung nicht? Du möchtest die Griechenland-Verhandlungen einschätzen können? Erweitere dein Wissen zu Wirtschaftsthemen!")); %>
        </div>

        <div class="panel panel-default">
            <% Html.RenderPartial("WelcomeBoxSetImgQ", WelcomeBoxSetImgQModel.GetWelcomeBoxSetImgQModel(17, new[] { 373, 360, 367 }, "Weißt du, wo diese weltweit bekannten Sehenswürdigkeiten stehen?")); %>
        </div>

        <div class="row ThumbnailRow" style="padding-top: 0;">
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(12)); %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(13, "Kleine Scherze versüßen denn Alltag!")); %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(7)); %>
        </div>

        <div class="panel panel-default">
            <% Html.RenderPartial("WelcomeBoxSetImgQ", WelcomeBoxSetImgQModel.GetWelcomeBoxSetImgQModel(19, new[] { 468, 464, 460 })); %>
        </div>

        <div class="panel panel-default">
            <% Html.RenderPartial("WelcomeBoxSetImgQ", WelcomeBoxSetImgQModel.GetWelcomeBoxSetImgQModel(14, new[] { 348, 341, 344 })); %>
        </div>

        <div class="panel panel-default">
            <% Html.RenderPartial("WelcomeBoxSetTxtQ", WelcomeBoxSetTxtQModel.GetWelcomeBoxSetTxtQModel(12, new[] { 303, 288, 289 }, "Der berühmteste Agent im Dienste Ihrer Majestät: Kennst du die wichtigsten Fakten zu den James Bond-Filmen?")); %>
        </div>

<%--        <div class="row ThumbnailRow" style="padding-top: 0px;">
            <% Html.RenderPartial("WelcomeBoxSingleQuestion", WelcomeBoxSingleQuestionModel.GetWelcomeBoxQuestionVModel(questionId: 385)); %>
            <% Html.RenderPartial("WelcomeBoxSingleQuestion", WelcomeBoxSingleQuestionModel.GetWelcomeBoxQuestionVModel(questionId: 337)); %>
            <% Html.RenderPartial("WelcomeBoxSingleQuestion", WelcomeBoxSingleQuestionModel.GetWelcomeBoxQuestionVModel(questionId: 233)); %>
        </div>--%>

<%--        <div class="panel panel-default">
            <% Html.RenderPartial("WelcomeBoxSetImgQ", WelcomeBoxSetImgQModel.GetWelcomeBoxSetImgQModel(211, new[] { 394, 395, 390 }, "Farfalle, Penne oder Rigatoni? Weißt du wie diese Nudelsorten heißen?")); %>
        </div>--%>
        <div class="well">
            <h3><a name="teaserWhatIsMemucho"></a>Was ist memucho?</h3>
            <p>
                memucho ist eine vernetzte Lern- und Wissensplattform. Damit kannst du:
            </p>
            
            <div class="row">
                <div class="col-xs-6" style="text-align: center; font-size: 100%; padding: 5px 5px 10px;">
                  <i class="fa fa-clock-o fa-2x show-tooltip" style="color: #2C5FB2"></i><br/>
                    <b>Schneller lernen</b>
                    <p>
                        memucho analysiert dein Lernverhalten und wiederholt schwierige Fragen zum optimalen Zeitpunkt. 
                        So brauchst du weniger Zeit zum Lernen.
                    </p>
                </div>
                <div class="col-xs-6" style="text-align: center; font-size: 100%; padding: 5px 5px 10px;">
                  <i class="fa fa-book fa-2x show-tooltip" style="color: #2C5FB2"></i><br/>
                    <b>Wissen erweitern</b>
                    <p>
                        Du möchtest gerne mehr über Politik, die Griechenland-Krise oder über James Bond-Filme wissen? 
                        Finde die passenden Fragesätze und stelle dir dein Wunschwissen zusammen!
                    </p>
                </div>
                <div class="clearfix"></div>
                <div class="col-xs-6" style="text-align: center; font-size: 100%; padding: 5px 5px 10px;">
                  <i class="fa fa-calendar-o fa-2x show-tooltip" style="color: #2C5FB2"></i><br/>
                    <b>Zu einem bestimmten Termin lernen</b>
                    <p>
                        Eine Klassenarbeit oder eine Prüfung steht an? Mit Terminen in memucho weißt du immer, 
                        was du schon sicher kannst und wo du weiter üben musst.
                    </p>
                </div>
                <div class="col-xs-6" style="text-align: center; font-size: 100%; padding: 5px 5px 10px;">
                  <i class="fa fa-pie-chart fa-2x show-tooltip" style="color: #2C5FB2"></i><br/>
                    <b>Überblick behalten</b>
                    <p>
                        Du möchtest dir gerne 50, 500, 5000 (oder mehr) Fakten merken? Kein Problem, mit memucho behältst du den Überblick.
                    </p>
                </div>
                <div class="clearfix"></div>
                <div class="col-xs-6" style="text-align: center; font-size: 100%; padding: 5px 5px 10px;">
                  <i class="fa fa-share-alt fa-2x show-tooltip" style="color: #2C5FB2"></i><br/>
                    <b>Wissen teilen</b>
                    <p>
                        memucho ist ein offenes Netzwerk, wo du dein Wissen teilen und das Wissen anderer nutzen kannst. 
                        Denn Wissen wird mehr, wenn man es teilt!
                    </p>
                </div>
                <div class="col-xs-6" style="text-align: center; font-size: 100%; padding: 5px 3px 20px;">
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

         <%--   <div class="row">
                <div class="col-xs-6 col-md-4" style="text-align: center; font-size: 100%; padding: 5px 3px 20px;">
                  <i class="fa fa-clock-o fa-2x show-tooltip" style="color: #2C5FB2" title="memucho analysiert dein Lernverhalten und wiederholt schwierige Fragen zum optimalen Zeitpunkt. So brauchst du weniger Zeit zum Lernen."></i><br/>
                    <b>Schneller lernen</b>
                </div>
                <div class="col-xs-6 col-md-4" style="text-align: center; font-size: 100%; padding: 5px 3px 20px;">
                  <i class="fa fa-book fa-2x show-tooltip" style="color: #2C5FB2" title="Du möchtest gerne mehr über Politik, die Griechenland-Krise oder über James Bond-Filme wissen? Finde die passenden Fragesätze und stelle dir dein Wunschwissen zusammen!"></i><br/>
                    <b>Allgemein- und Spezialwissen erweitern</b>
                </div>
                <div class="clearfix visible-xs"></div>
                <div class="col-xs-6 col-md-4" style="text-align: center; font-size: 100%; padding: 5px 3px 20px;">
                  <i class="fa fa-calendar-o fa-2x show-tooltip" style="color: #2C5FB2" title="Eine Klassenarbeit, eine Prüfung oder ein wichtiges Gespräch steht an? Lege einen Termin an und bestimme, was du bis dahin wissen musst. Mit memucho weißt du immer, was du schon sicher kannst und wo du noch weiter üben musst."></i><br/>
                    <b>Zu einem bestimmten Termin lernen</b>
                </div>
                <div class="clearfix visible-md"></div>
                <div class="col-xs-6 col-md-4" style="text-align: center; font-size: 100%; padding: 5px 3px 20px;">
                  <i class="fa fa-pie-chart fa-2x show-tooltip" style="color: #2C5FB2" title="Du möchtest dir gerne 50, 500, 5000 (oder mehr) Fakten merken? Kein Problem, mit memucho behältst du den Überblick."></i><br/>
                    <b>Überblick behalten</b>
                </div>
                <div class="clearfix visible-xs"></div>
                <div class="col-xs-6 col-md-4" style="text-align: center; font-size: 100%; padding: 5px 3px 20px;">
                  <i class="fa fa-share-alt fa-2x show-tooltip" style="color: #2C5FB2" title="memucho ist ein offenes Netzwerk, wo du dein Wissen teilen und auf das Wissen anderer zurückgreifen kannst. Denn Wissen wird mehr, wenn man es teilt!"></i><br/>
                    <b>Wissen teilen</b>
                </div>
                <div class="col-xs-6 col-md-4" style="text-align: center; font-size: 100%; padding: 5px 3px 20px;">
                  <i class="fa fa-users fa-2x show-tooltip" style="color: #2C5FB2" title="Lerne gemeinsam mit Freunden und verabrede dich zum Quizduell, um dich auf die Klassenarbeit vorzubereiten."></i><br/>
                    <b>Gemeinsam lernen</b>
                </div>
                <div class="clearfix visible-xs"></div>
                <div class="col-xs-12">
                    <p>
                        Ausprobieren? <a href="<%= Url.Action("Register", "Welcome") %>">Registriere dich</a> und lege los! <br/>
                        Tolle Idee? Unterstütze uns und werde <a id="SupportUs" class="helpLink TextLinkWithIcon" href="<%= Url.Action(Links.Membership, Links.AccountController) %>">
                        <i class="fa fa-thumbs-up"></i>&nbsp;Fördermitglied</a> der ersten Stunde!
                    </p>                
                </div>
            </div>--%>
        </div>

        <div class="well">
            <h3><a name="teaserPrinciples">Unsere Prinzipien</a></h3>
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
                        Aber wir werden deine Daten niemals verkaufen. (<a class="helpLink" href="<%= Url.Action(Links.HelpFAQ, Links.HelpController) %>">Erfahre mehr</a> über unseren Datenschutz.)
                    </p>
                </li>
                <li><i class="fa fa-li fa-github"></i>
                    <b>Open-Source und Transparenz</b>
                    <p>
                        Die Software, mit der memucho läuft, steht unter einer Open-Source-Lizenz. Die Quelltexte 
                        sind frei verfügbar und können von allen frei verwendet werden. Du findest sie 
                        auf <a href="https://github.com/TrueOrFalse/TrueOrFalse"><i class="fa fa-github"></i> Github</a>. 
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
            <h3><a name="teaserWhoWeAre">Team</a></h3>
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
                        Wenn du Fragen oder Anregungen hast, schreibe uns eine Email an <a href="mailto:kontakt@memucho.de">kontakt@memucho.de</a> oder rufe Christof an: 01577-6825707.
                    </p>
                </div>
            </div>
        </div>  
    </div>
            
    <div class="col-md-4">
        <%
            var userSession = new SessionUser();
            if (!userSession.IsLoggedIn){
        %>
            <div class="box" style="padding: 20px; ">
                <a href="<%= Url.Action("Login", "Welcome") %>" class="btn btn-success btn-lg" style="width: 100%" role="button">Anmelden</a>
                <br/><br/>
                <a href="<%= Url.Action("Register", "Welcome") %>" class="btn btn-primary btn-lg" style="width: 100%;" role="button">Registrieren</a>
            </div>
        <% } %>
            
        <div class="panel panel-default">
            <div class="panel-heading">Top-Kategorien nach Fragen:</div>
            <div class="panel-body">
                <% Html.RenderPartial("WelcomeBoxTopCategories", WelcomeBoxTopCategoriesModel.CreateTopCategories(5)); %>
            </div>
        </div>
        
        <div class="panel panel-default">
            <div class="panel-heading">Neueste Fragesätze:</div>
            <div class="panel-body">
                <% Html.RenderPartial("WelcomeBoxTopSets", WelcomeBoxTopSetsModel.CreateMostRecent(5)); %>
            </div>
        </div>
        
        <div class="panel panel-default">
            <div class="panel-heading">Umfangreichste Fragesätze:</div>
            <div class="panel-body">
                <% Html.RenderPartial("WelcomeBoxTopSets", WelcomeBoxTopSetsModel.CreateMostQuestions(5)); %>
            </div>
        </div>
        
        <div class="panel panel-default">
            <div class="panel-heading">Neueste Fragen:</div>
            <div class="panel-body" style="padding-top: 12px;">
                <% Html.RenderPartial("WelcomeBoxTopQuestions", WelcomeBoxTopQuestionsModel.CreateMostRecent(8)); %>
            </div>
        </div>
        
        <div class="panel panel-default">
            <div class="panel-heading">Neueste Kategorien:</div>
            <div class="panel-body" style="padding-top: 12px;">
                <% Html.RenderPartial("WelcomeBoxTopCategories", WelcomeBoxTopCategoriesModel.CreateMostRecent(5)); %>
            </div>
        </div>

        <div class="row" style="padding-top: 10px;">
            <div class="col-md-12"><h3 class="media-heading">memucho-Netzwerk</h3></div>
        </div>
        
        <div class="panel panel-default" style="padding-top: 15px; opacity: 0.4;">
            <div class="panel-heading">Nutzer-Ranking nach Reputation</div>
            <div class="panel-body" style="padding-top: 12px;">
                <p style="padding-left: 5px;"><img src="/favicon-32x32.png" height="25" width="25" style="float: left; vertical-align: text-bottom"/>&nbsp;Pauli (130 Punkte)</p>
                <p style="padding-left: 5px;"><img src="/favicon-32x32.png" height="25" width="25" style="float: left; vertical-align: bottom"/>&nbsp;Robert (120 Punkte)</p>
                <p style="padding-left: 5px;"><img src="/favicon-32x32.png" height="25" width="25" style="float: left; vertical-align: middle"/>&nbsp;Christof (112 Punkte)</p>
            </div>
        </div>

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