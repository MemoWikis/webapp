<%@ Page Title="memucho: Schneller lernen, länger wissen" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" 
	Inherits="ViewPage<WelcomeModel>"%>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <link rel="canonical" href="<%= Settings.CanonicalHost %>">
    <meta name="description" content="memucho hilft dir beim Lernen. Du sparst Zeit, bist effizienter und es macht mehr Spaß! Entdecke neues Wissen oder füge dein eigenes hinzu.">
</asp:Content>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
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

    <script>
        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(function () { drawKnowledgeChart("chartWishKnowledge") });

        function drawKnowledgeChart(chartElementId) {
            if ($("#" + chartElementId).length === 0) {
                return;
            }

            var data = google.visualization.arrayToDataTable([
                    ['Wissenslevel', 'link', 'Anteil in %'],
                    ['Sicheres Wissen', '/Fragen/Wunschwissen/?filter=solid', <%= Model.KnowledgeSummary.Solid %>],
                    ['Solltest du festigen', '/Fragen/Wunschwissen/?filter=consolidate', <%= Model.KnowledgeSummary.NeedsConsolidation %>],
                    ['Solltest du lernen', '/Fragen/Wunschwissen/?filter=learn', <%= Model.KnowledgeSummary.NeedsLearning %>],
                    ['Noch nicht gelernt', '/Fragen/Wunschwissen/?filter=notLearned', <%= Model.KnowledgeSummary.NotLearned %>],
                ]);

            var options = {
                pieHole: 0.6,
                tooltip: { isHtml: true },
                legend: { position: 'labeled' },
                pieSliceText: 'none',
                chartArea: { 'width': '100%', height: '100%', top: 10 },
                slices: {
                    0: { color: '#afd534' },
                    1: { color: '#fdd648' },
                    2: { color: 'lightsalmon' },
                    3: { color: 'silver' }
                },
                pieStartAngle: 0
            };

            var view = new google.visualization.DataView(data);
            view.setColumns([0, 2]);

            var chart = new google.visualization.PieChart(document.getElementById(chartElementId));
            chart.draw(view, options);

            google.visualization.events.addListener(chart, 'select', selectHandler);

            function selectHandler(e) {
                var urlPart = data.getValue(chart.getSelection()[0].row, 1);
                location.href = urlPart;
            }
        }


    </script>

    <%= Scripts.Render("~/bundles/guidedTourScript") %>
    <%= Styles.Render("~/bundles/guidedTourStyle") %>
    <%= Scripts.Render("~/bundles/Welcome") %>
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    
<div class="row" id="welcomeContainer">
       
    <div class="col-md-12">

        <h1 id="titleFindYourContent">Finde deine Lerninhalte</h1>
        
        <div class="EduCategoryRow row">
            <div class="xxs-stack col-xs-6 col-sm-3">
                <a href="/Kategorien/Unterrichtsfaecher/682" class="EduCategory">
                    <span class="EduCategoryIcon">
                        <span class="fa-stack fa-lg">
                          <i class="fa fa-circle fa-stack-2x"></i>
                          <i class="fa fa-child fa-stack-1x fa-inverse IconForeground"></i>
                        </span>    
                    </span>
                    <span class="EduCategoryText">
                        Schule
                    </span>
                </a>
            </div>
            
            <div class="xxs-stack col-xs-6 col-sm-3">
                <a href="/Kategorien/Studienfaecher/687" class="EduCategory">
                    <span class="EduCategoryIcon">
                        <span class="fa-stack fa-lg">
                          <i class="fa fa-circle fa-stack-2x"></i>
                          <i class="fa fa-graduation-cap fa-stack-1x fa-inverse IconForeground"></i>
                        </span>    
                    </span>
                    <span class="EduCategoryText">
                        Studium
                    </span>
                </a>  
            </div>
            <div class="xxs-stack col-xs-6 col-sm-3">
                <a href="/Kategorien/Zertifikate/689" class="EduCategory show-tooltip" title="Sportbootführerscheine, Einbürgerungstest, Pferdebasispass etc.">
                    <span class="EduCategoryIcon">
                        <span class="fa-stack fa-lg">
                          <i class="fa fa-circle fa-stack-2x"></i>
                          <i class="fa fa-file-text-o fa-stack-1x fa-inverse IconForeground"></i>
                        </span>    
                    </span>
                    <span class="EduCategoryText">
                        Zertifikate
                    </span>
                </a>
            </div>
            <div class="xxs-stack col-xs-6 col-sm-3">
                <a href="/Kategorien/Allgemeinwissen/709" class="EduCategory">
                    <span class="EduCategoryIcon">
                        <span class="fa-stack fa-lg">
                          <i class="fa fa-circle fa-stack-2x"></i>
                          <i class="fa fa-lightbulb-o fa-stack-1x fa-inverse IconForeground"></i>
                        </span>    
                    </span>
                    <span class="EduCategoryText">
                        Allgemeinwissen
                    </span>
                </a>    
            </div>
        </div>

        <div id="WelcomeDashboard">
            <div class="row">
                <div class="col-sm-6" id="dashboardPoints">
                    <h2>Deine Lernpunkte</h2>
                    <div style="text-align: center; margin-bottom: 25px; margin-top: 15px;">
                        <span class="level-display">
                            <span style="display: inline-block; white-space: nowrap;">
                                <svg class="large">
                                    <circle cx="50%" cy="50%" r="50%" />
                                    <text class="level-count" x="50%" y="50%" dy = ".34em" ><%= Model.ActivityLevel %></text>
                                </svg>
                            </span>
                        </span>
                        <p style="margin-top: 10px;">
                            Mit <b><%= Model.ActivityPoints.ToString("N0") %> Lernpunkten</b> bist du <span style="white-space: nowrap"><b>Level <%= Model.ActivityLevel %></b>.</span>
                        </p>
                    </div>

                    <div class="NextLevelContainer">
                        <div class="ProgressBarContainer">
                            <div id="NextLevelProgressPercentageDone" class="ProgressBarSegment ProgressBarDone" style="width: <%= Model.ActivityPointsPercentageOfNextLevel %>%;">
                                <div class="ProgressBarSegment ProgressBarLegend">
                                    <span id="NextLevelProgressSpanPercentageDone"><%= Model.ActivityPointsPercentageOfNextLevel %> %</span>
                                </div>
                            </div>
                            <div class="ProgressBarSegment ProgressBarLeft" style="width: 100%;"></div>
            
                        </div>
                    </div>     
                    <div class="greyed" style="text-align: center; margin-bottom: 15px;">Noch <%= Model.ActivityPointsTillNextLevel %> Punkte bis Level <%= Model.ActivityLevel + 1 %></div>
                </div>

                <div class="col-sm-6" id="dashboardKnowledgeWheel">
                    <h2>Dein Wissensstand</h2>
                    <% if(Model.KnowledgeSummary.Total == 0) { %>
                        <div class="alert alert-info" style="min-height: 180px; margin-bottom: 54px;">
                            <p>
                                memucho kann deinen Wissensstand nicht zeigen, da du noch kein Wunschwissen hast.
                            </p>
                            <p>
                                Um dein Wunschwissen zu erweitern, suche dir interessante <a href="<%= Links.QuestionsAll() %>">Fragen</a>  
                                oder <a href="<%= Links.SetsAll() %>">Lernsets</a> aus und klicke dort auf das Herzsymbol:
                                <ul style="list-style-type: none">
                                    <li>
                                        <i class="fa fa-heart show-tooltip" style="color:#b13a48;" title="" data-original-title="In deinem Wunschwissen"></i>
                                        In deinem Wunschwissen
                                    </li>                                
                                    <li>
                                        <i class="fa fa-heart-o show-tooltip" style="color:#b13a48;" title="" data-original-title="Nicht Teil deines Wunschwissens."></i>
                                        <i>Nicht</i> in deinem Wunschwissen.
                                    </li>
                                </ul>
                            
                            </p>
                        </div>
                    <% }else { %>
                        <div id="chartWishKnowledge" style=""></div>
                    <% } %>
                </div>
            </div>
            
            <div class="separator">
            </div>

            <div id="dashboardFooter">
                <% if(Model.KnowledgeSummary.Total > 0) { %>
                    <a href="<%= Links.StartWishLearningSession() %>" class="btn btn-lg btn-primary show-tooltip" title="Startet eine persönliche Lernsitzung. Du wiederholst die Fragen aus deinem Wunschwissen, die am dringendsten zu lernen sind.">
                        <i class="fa fa-line-chart">&nbsp;</i>Jetzt Wunschwissen lernen
                    </a>
                <% } %>
                <span class="float-right-sm-up"><a class="btn btn-lg btn-link" href="<%= Links.Knowledge() %>">Mehr auf deiner<span style="text-decoration:none;">&nbsp;&nbsp;</span><i class="fa fa-heart" style="color:#b13a48;">&nbsp;</i>Wissenszentrale</a></span>
            </div>
        </div>



        <div id="memuchoInfo">
            <h2 id="memuchoInfoHeader">
                memucho ist dein Wissens-Assistent
            </h2>            
            <div id="memuchoInfoMain">
                <p>
                    <span class="fa-stack fa-2x numberCircleWrapper">
                        <i class="fa fa-circle fa-stack-2x numberCircle"></i>
                        <strong class="fa-stack-1x numberCircleText">1</strong>
                    </span>
                    <span class="memuchoInfoBenefit">Sammeln</span> <span class="memuchoInfoBenefitSub"><i class="fa fa-heart-o">&nbsp;</i>Entscheide, was du wissen möchtest.</span>
                </p>
                <p>
                    <span class="fa-stack fa-2x numberCircleWrapper">
                        <i class="fa fa-circle fa-stack-2x numberCircle"></i>
                        <strong class="fa-stack-1x numberCircleText">2</strong>
                    </span>
                    <span class="memuchoInfoBenefit">Lernen</span> <span class="memuchoInfoBenefitSub">Algorithmen helfen dir, zum idealen Zeitpunkt zu lernen.</span>
                </p>
                <p>
                    <span class="fa-stack fa-2x numberCircleWrapper">
                        <i class="fa fa-circle fa-stack-2x numberCircle"></i>
                        <strong class="fa-stack-1x numberCircleText">3</strong>
                    </span>
                    <span class="memuchoInfoBenefit">Nicht vergessen</span> <span class="memuchoInfoBenefitSub">Wir erinnern dich, bevor du vergisst.</span>
                </p>
            </div>
            <div class="separator"></div>
            <div id="memuchoInfoFooter">
                <% if (!Model.IsLoggedIn) { %>
                    <div style="text-align: center; display: inline-block;">
                        <a href="<%= Url.Action("Register", "Welcome") %>" class="btn btn-lg btn-primary" role="button"><i class="fa fa-chevron-circle-right">&nbsp;</i> Jetzt kostenlos registrieren</a> <br/>
                    </div>
                <% } %>
                <span class="float-right-sm-up"><a href="<%= Links.AboutMemucho() %>" class="btn btn-lg btn-link" style="float: right;">Erfahre mehr...</a></span>
            </div>
        </div>

        <% Html.RenderPartial("Partials/TopicOfWeek/TopicOfWeek_2017_30", new TopicOfWeek_2017_30Model(264)); // 264=cat. Psychologie Studium %>

        
        <div id="awards">
            <h2>Auszeichnungen</h2>
            <div class="row">
                <div class="col-sm-4">
                    <div class="logo-box">
                        <div>
                            <a href="https://www.land-der-ideen.de/ausgezeichnete-orte/preistraeger/memucho-online-plattform-zum-faktenlernen" target="_blank">
                                <img class="img-logo" src="/Images/LogosPartners/landderideen_ausgezeichnet-2017_w190c.jpg" alt="memucho ist ein ausgezeichneter Ort im Land der Ideen 2017"/>
                            </a>
                        </div>
                        <p>
                            memucho ist ein Ausgezeichneter Ort im Land der Ideen 2017.
                        </p>
                        <p class="logo-box-link">
                            <a href="https://www.land-der-ideen.de/ausgezeichnete-orte/preistraeger/memucho-online-plattform-zum-faktenlernen" target="_blank">
                                <span style="white-space: nowrap">Zum Wettbewerb <i class="fa fa-external-link"></i></span>
                            </a>
                        </p>
                    </div>        
                </div>
                <div class="col-sm-4">
                    <div class="logo-box">
                        <div>
                            <img class="img-logo" src="/Images/LogosPartners/innovationspreis-nominiertButton2016.png" alt="Nominiert 2016 für den Innovationspreis Berlin Brandenburg" width="170" height="110"/>
                        </div>
                        <p>
                            memucho wurde für den Innovationspreis Berlin Brandenburg nominiert.
                        </p>
                        <p class="logo-box-link">
                            <a href="http://www.innovationspreis.de/news/aktuelles/zehn-nominierungen-f%C3%BCr-den-innovationspreis-berlin-brandenburg-2016.html" target="_blank">
                                <span style="white-space: nowrap">Zur Jury-Entscheidung <i class="fa fa-external-link"></i></span>
                            </a>
                        </p>
                    </div>    
                </div>
                <div class="col-sm-4">
                    <div class="logo-box">
                        <div>
                            <img class="img-logo" src="/Images/LogosPartners/Logo-EXIST-eps.png" alt="" width="115" height="73"/>
                        </div>
                        <p>
                            memucho gewann über das EXIST-Programm eine Förderung vom BMWi und ESF.
                        </p>
                        <p class="logo-box-link">
                            <a href="http://www.fu-berlin.de/sites/profund/aktuelles/news/Memucho.html" target="_blank">
                                Zum Artikel <span style="white-space: nowrap">(FU Berlin) <i class="fa fa-external-link"></i></span>
                            </a>
                        </p>
                    </div>    
                </div>
            </div>
        </div>



        <div id="partner">
            <h2>Partner</h2>
            <div class="row">
                <div class="col-sm-4">
                    <div class="logo-box">
                        <div>
                            <a href="/Kategorien/Learning-Level-Up/722">
                                <img class="img-logo" src="/Images/LogosPartners/Logo_LearningLevelUp.png" alt="Learning Level Up und memucho kooperieren!" />
                            </a>
                        </div>
                        <p>
                            Learning Level Up bietet Animationen, Grafiken und Videos zum Lernen. Wir freuen uns über die Kooperation!
                        </p>
                        <p class="logo-box-link">
                            <a href="/Kategorien/Learning-Level-Up/722">Zur Themenseite</a> 
                        </p>
                    </div>        
                </div>
                <div class="col-sm-4">
                    <div class="logo-box">
                        <div>
                            <a href="https://www.tutory.de/" target="_blank">
                                <img class="img-logo" src="/Images/Sponsors/tutory.png" alt="tutory.de"/>
                            </a>
                        </div>
                        <p>
                            Mit tutory.de lassen sich im Handumdrehen tolle Arbeitsblätter direkt online erstellen
                        </p>
                        <p class="logo-box-link">
                            <a href="https://www.tutory.de/" target="_blank">
                                <span style="white-space: nowrap">tutory.de <i class="fa fa-external-link"></i></span>
                            </a>
                        </p>
                    </div>        
                </div>
                <div class="col-sm-4">
                    <div class="logo-box">
                        <div>
                            <a href="http://www.unesco.org/new/en/communication-and-information/access-to-knowledge/open-educational-resources/" target="_blank">
                                <img class="img-logo" src="/Images/LogosPartners/oer_logo_EN_2_w400.png" alt="Logo Open Educational Resources"/>
                            </a>
                        </div>
                        <p>
                            Offene Bildungsressourcen demokratisieren Bildung! Wir machen mit.
                        </p>
                        <p class="logo-box-link">
                            <a href="http://www.unesco.org/new/en/communication-and-information/access-to-knowledge/open-educational-resources/" target="_blank">
                                <span style="white-space: nowrap">Zur UNESCO-Seite <i class="fa fa-external-link"></i></span>
                            </a>
                        </p>
                    </div>        
                </div>

            </div>
        </div>

       
        <h1 class="welcomeContentSectionHeader">Studium</h1>
        <p class="welcomeContentSectionTarget">243[.] Lernsets, mit denen du für dein Studium lernen kannst</p>

        <div class="row CardsPortrait" style="padding-top: 0;">
            <% foreach (var categoryId in Model.CategoriesUniversity) { %>
                <div class="col-xs-6 col-md-3">
                    <% Html.RenderPartial("WelcomeCardMiniCategory", new WelcomeCardMiniCategoryModel(categoryId)); %>
                </div>
            <% } %>
<%--            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(64)); //Sozialstaat Deutschland %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(57)); //UN %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(25)); //Hauptstädte Flächenbundesländer %>
            <% Html.RenderPartial("WelcomeBoxSingleSet", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(25)); //Hauptstädte Flächenbundesländer %>--%>
        </div>
<%--        <% Html.RenderPartial("WelcomeBoxSetTxtQ", WelcomeBoxSetTxtQModel.GetWelcomeBoxSetTxtQModel(27, new int[] { 749, 635, 630 })); //Einbürgerungstest %>
        <% Html.RenderPartial("WelcomeBoxCategoryTxtQ", WelcomeBoxCategoryTxtQModel.GetWelcomeBoxCategoryTxtQModel(205, new int[] { 381, 379, 384 }, "Du möchtest dir eine fundierte Meinung zur Flüchtlingspolitik bilden? Erweitere dein Hintergrundwissen mit Fakten!")); %>--%>


     
        


        <div class="well">
            <h3>
                <a id="teaserWhatIsMemucho"></a>
                Was ist memucho?
            </h3>
            <p>
                memucho hilft dir, Interessantes zu lernen, nie wieder zu vergessen und dein Wissen zu organisieren.
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
                        Finde die passenden Lernsets und stelle dir dein Wunschwissen zusammen!
                    </p>
                </div>
                <div class="clearfix"></div>
                <div class="col-xs-6 xxs-stack" style="text-align: center; font-size: 100%; padding: 5px 5px 10px;">
                  <i class="fa fa-calendar-o fa-2x show-tooltip" style="color: #2C5FB2"></i><br/>
                    <b>Zu einem bestimmten Termin lernen</b>
                    <p>
                        Eine Klassenarbeit oder eine Prüfung steht an? Mit Terminen in memucho weißt du immer, 
                        was du schon sicher kannst und wo du weiter lernen musst.
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
                <% if (!Model.IsLoggedIn) { %>
                    <div class="col-xs-12" style="margin-top: 10px; text-align: center">
                        <a href="<%= Url.Action("Register", "Welcome") %>" class="btn btn-lg btn-primary" role="button"><i class="fa fa-chevron-circle-right">&nbsp;</i> Jetzt kostenlos registrieren</a> <br/>
                    </div>
                <% } %>
                <div class="col-xs-12" style="margin-top: 10px; text-align: right;">
                    <a id="btnMoreFeatures" href="<%= Links.AboutMemucho() %>" class="btn btn-link ButtonOnHover">ERFAHRE MEHR...</a><br />
                </div>
            </div>
        </div>

        <div class="well">
            <h3>
                <a id="teaserPrinciples"></a>
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
            <div class="row">
                <% if (!Model.IsLoggedIn) { %>
                    <div class="col-xs-12" style="margin-top: 10px; text-align: center">
                        <a href="<%= Url.Action("Register", "Welcome") %>" class="btn btn-lg btn-primary" role="button"><i class="fa fa-chevron-circle-right">&nbsp;</i> Jetzt kostenlos registrieren</a> <br/>
                    </div>
                <% } %>
                <div class="col-xs-12" style="margin-top: 10px; text-align: right;">
                    <a id="btnMorePrinciples" href="<%= Links.AboutMemucho() %>" class="btn btn-link ButtonOnHover">ERFAHRE MEHR...</a><br />
                </div>
            </div>
        </div>
        <div class="well Team">
            <h3>
                <a name="teaserWhoWeAre"></a>
                Team
            </h3>
            <div class="row">
                
                <div class="col-xs-4 TeamPic">
                    <img src="/Images/Team/team_robert201509_155.jpg"/>
                        <br/> <b>Robert</b> (Gründer) <br/>
                </div>
                
                <div class="col-xs-4 TeamPic">
                    <img src="/Images/Team/team_jule201509-2_155.jpg"/>  
                    <br/> <b>Jule</b> (Gründerin) <br/> 
                </div>

                <div class="col-xs-4 TeamPic">
                    <img src="/Images/Team/team_christof_20170404_P3312344_155.jpg"/>  
                    <br/> <b>Christof</b> (Gründer) <br/> 
                </div>
                <div class="col-xs-4 TeamPic">
                    <img src="/Images/Team/team_lisa_sq_155.jpg"/>  
                    <br/> <b>Lisa</b> (Kommunikation) <br/> 
                </div>
                <div class="col-xs-4 TeamPic">
                    <img src="/Images/Team/team_julian20170404_P3312327_155.jpg"/>  
                    <br/> <b>Julian</b> (Entwicklung) <br/> 
                </div>
                <div class="col-xs-12" style="margin-top: 10px;">
                    <p>
                        Wir möchten den Zugang zu freien Bildungsinhalten verbessern und dass Faktenlernen einfacher wird und mehr Spaß macht. 
                        Und wir möchten dabei ein stabiles <a href="#teaserPrinciples">gemeinwohlorientiertes Unternehmen</a> aufbauen. 
                        Wir konzipieren, gestalten und programmieren memucho gemeinsam.
                    </p>
                    <p>
                        Wenn du Fragen oder Anregungen hast, schreibe uns eine E-Mail an <span class="mailme">team at memucho dot de</span> oder rufe uns an: +49 - 30 - 616 566 26.
                    </p>
                </div>
            </div>
        </div>  
    </div>
        
    <div class="row">
        <div class="col-xs-6 col-md-3">
            <h4>Neueste Lernsets</h4>
            <div class="LabelList">
                <% Html.RenderPartial("WelcomeBoxTopSets", WelcomeBoxTopSetsModel.CreateMostRecent(5)); %>
            </div>
        </div>
        <div class="col-xs-6 col-md-3">
            <h4>Neueste Themen:</h4>
            <div class="LabelList">
                <% Html.RenderPartial("WelcomeBoxTopCategories", WelcomeBoxTopCategoriesModel.CreateMostRecent(5)); %>
            </div>
        </div>
        <div class="col-xs-6 col-md-3">
            <h4>Neueste Fragen</h4>
            <div class="LabelList">
                <% Html.RenderPartial("WelcomeBoxTopQuestions", WelcomeBoxTopQuestionsModel.CreateMostRecent(8)); %>
            </div>
        </div>
        <div class="col-xs-6 col-md-3">
            <h4>Top-Themen nach Anzahl Fragen</h4>
            <div class="LabelList">
                <% Html.RenderPartial("WelcomeBoxTopCategories", WelcomeBoxTopCategoriesModel.CreateTopCategories(5)); %>
            </div>
        </div>
    </div>

<%--    <div class="col-md-12">
        <% if (!Model.IsLoggedIn)
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
    </div>--%>


</div>

</asp:Content>