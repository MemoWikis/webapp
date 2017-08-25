<%@ Page Title="Über memucho" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
    Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/About/AboutMemucho.css" rel="stylesheet" />
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


<div class="row">
    <div class="col-xs-12">
        <div id="MainWrapper">

            <div id="pageTitleMemucho">
                <img src="/Images/Logo/LogoWordmark_w300.png"/>
                <h1>Schneller lernen, länger wissen</h1>
            </div>
            
            <div id="teamImgQuote">
                <img id="teamImg" src="/Images/Team/founderTeam_20161027.jpg"/>
                
                <div class="teamSquare hidden">
                    Ein Text bla bla Ein Text bla bla Ein Text bla bla Ein Text bla bla Ein Text bla bla Ein Text bla bla Ein Text bla bla 
                </div>

                <div id="teamQuoteCircle" class="">
                    <div class="circle">
                        <div class="circleInner">
                            <div class="circleWrapper">
                                <div class="circleContent">
                                    <i class="fa fa-quote-left quoteIcon">&nbsp;</i>
                                    <div id="teamQuote">
                                        Wir möchten, dass du optimiert lernen und dein Wissen organisieren kannst - und mehr Spaß hast!
                                        Dabei fördern wir offene Bildungsmaterialien.
                                    </div>
                                    <div id="teamQuoteNames">Christof, Robert und Jule</div>
                                    <div id="teamQuoteNamesSub">Gründerteam memucho</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            
            <div id="advantages">
                <h2>Deine Vorteile bei memucho</h2>
                
                <div class="advantage">
                    <div class="advantageIcon" style="color: #B13A48">
                        <i class="fa fa-heart-o">&nbsp;</i> <i class="fa fa-heart">&nbsp;</i>
                    </div>
                    <h3>Wunschwissen sammeln und nie vergessen</h3>
                    <p>
                        Stelle dir dein <i class="fa fa-heart" style="color: #B13A48"></i> Wunschwissen zusammen und entscheide, was du dir merken möchtest.
                        Damit hast du deinen Wissensstand immer im Blick. Wir erinnert dich, wenn du es wieder lernen musst, um es nicht zu vergessen.
                    </p>
                </div>

                <div class="advantage">
                    <div class="advantageIcon">
                        <i class="fa fa-check-circle"></i>
                    </div>
                    <h3>Optimierte Prüfungsvorbereitung</h3>
                    <p>
                        Du musst für eine Prüfung lernen? Lege einfach einen Termin an und wir erstellen dir einen persönlichen Lernplan. 
                        Intelligente Algorithmen sagen dir, wann du was am besten lernst und ob du deine Lernziele erreichen wirst.
                    </p>
                </div>

                <div class="advantage">
                    <div class="advantageIcon">
                        <i class="fa fa-lightbulb-o"></i>
                    </div>
                    <h3>Entdecke neues Wissen</h3>
                    <p>
                        Du musst für eine Prüfung lernen? Lege einfach einen Termin an und wir erstellen dir einen persönlichen Lernplan. 
                        Intelligente Algorithmen sagen dir, wann du was am besten lernst und ob du deine Lernziele erreichen wirst.
                    </p>
                </div>
                
                <div id="advantagesFooter">
                    <%  if (!false) { // Model.IsLoggedIn %>
                        <div class="" style="text-align: center; display: inline-block;">
                            <a href="<%= Url.Action(Links.RegisterAction, Links.RegisterController) %>" class="btn btn-lg btn-primary" role="button"><i class="fa fa-chevron-circle-right">&nbsp;</i> Jetzt kostenlos registrieren</a>
                        </div>
                    <%  } %>
                </div>

            </div>
            

            <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>
            
            
            <div id="keyNumbers">
                <div class="row">
                    <div class="col-xs-4 keyNumberCol">
                        <div class="keyNumber">
                            4.000.000 +
                        </div>
                        <div class="keyNumberExplanation">
                            Lernpunkte wurden bisher bei memucho errungen
                        </div>
                    </div>

                    <div class="col-xs-4 keyNumberCol">
                        <div class="keyNumber">
                            5.500
                        </div>
                        <div class="keyNumberExplanation">
                            Fragen gibt es zu beantworten
                        </div>
                    </div>

                    <div class="col-xs-4 keyNumberCol">
                        <div class="keyNumber">
                            28 %
                        </div>
                        <div class="keyNumberExplanation">
                            der Fragen werden von Nutzern falsch beantwortet
                        </div>
                    </div>
                </div>
            </div>
            
            
            <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>

            
            <div id="principles">
                <h2>Unsere Prinzipien</h2>
                <div class="advantage">
                    <div class="advantageIcon">
                        <i class="fa fa-bullhorn"></i>
                    </div>
                    <h3>Freie Bildungsinhalte</h3>
                    <div class="floatingImg">
                        <img src="/Images/LogosPartners/oer_logo_EN_2_w400.png"/><br/>
                        <p>
                            Jonathas Mello, <a target="_blank" href="https://creativecommons.org/licenses/by/3.0/">CC BY 3.0 Unported</a>
                        </p>
                    </div>
                    <p>
                        Alle Lerninhalte bei uns sind frei und rechtssicher lizenziert. 
                        Du kannst sie nutzen, weiterverwenden und ergänzen, wie es für dich am besten passst.
                        Wir nutzen dafür die offene Lizenz "Creative Commons Namensnennung" 
                        (kurz: <a href="https://creativecommons.org/licenses/by/4.0/deed.de" target="_blank">CC BY 4.0</a>).
                    </p>
                    <p>
                        Offene Bildungsinhalte ("Open Educational Resources", oder kurz "OER") demokratisieren Wissen und ermöglichen es Lehrenden und Lernenden, 
                        die Materialien an ihre spezifischen Bedürfnisse anzupassen. 
                        <a href="http://www.unesco.de/fileadmin/medien/Dokumente/Bildung/Was_sind_OER__cc.pdf" target="_blank">
                            Hier erfährst du mehr zu OER. <i class="fa fa-external-link"></i>
                        </a>
                    </p>
                </div>
                <div class="Clearfix"></div>
                <div class="advantage">
                    <div class="advantageIcon">
                        <i class="fa fa-leaf"></i>
                    </div>
                    <h3>Sozialunternehmen und Gemeinwohlorientierung</h3>
                    <p>
                        Wir möchten jedem Menschen den Zugang zu freien und hochwertigen Bildungsmaterialien ermöglichen. 
                        Und wir sind überzeugt, dass Unternehmen eine ethische, soziale und ökologische Verantwortung haben.
                        Deshalb möchten wir unser Unternehmen auf 
                        <a href="http://www.gemeinwohl-oekonomie.org/de" target="_blank;">gemeinwohlfördernden Werten <i class="fa fa-external-link"></i></a> 
                        aufbauen und werden dazu in Zukunft eine Gemeinwohlbilanz veröffentlichen.
                    </p>
                </div>

                <div class="advantage">
                    <div class="advantageIcon">
                        <i class="fa fa-search-plus"></i>
                    </div>
                    <h3>Datenschutz, Transparenz und Open-Source</h3>
                    <p>
                        Wir nutzen deine Daten, damit du besser lernen kannst und um memucho besser zu machen. 
                        Aber wir werden deine Daten niemals verkaufen. 
                        (<a class="helpLink" href="<%= Links.FAQItem("DataPrivacy") %>">Mehr zum <i class="fa fa-lock">&nbsp;</i>Datenschutz</a>)
                    </p>
                    <p>
                        Damit du überprüfen kannst, was wir versprechen und weil wir Open Source eine tolle Idee finden, 
                        sind auch die Quelltexte von memucho frei verfügbar. Du findest sie 
                        auf <a href="https://github.com/TrueOrFalse/TrueOrFalse" target="_blank"><i class="fa fa-github">&nbsp;</i>Github <i class="fa fa-external-link"></i></a>. 
                        In Zukunft möchten wir neben der Gemeinwohlbilanz auch wichtige Unternehmenszahlen regelmäßig veröffentlichen.
                    </p>
                </div>
                
            </div>


            <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>


            <div style="text-align: center; margin-bottom: 30px;">
                <iframe width="560" height="315" src="https://www.youtube.com/embed/qJW_V_q_3hs" frameborder="0" allowfullscreen></iframe>    
            </div>

        </div>
    </div>
</div>
        
<div class="row">
    <div class="col-xs-12">        

        <div class="well">
            <h1 class="PageHeader" id="Vorteile">
                <span class="ColoredUnderline Question">Deine Vorteile bei memucho</span>
            </h1>
            
            <div class="row aboutRow">
                <div class="col-xs-3 xxs-stack aboutImg <%--col-xs-push-9--%>">
                    <div class="aboutImgInner">
                        <i class="fa fa-heart fa-3x" style="/*color:#b13a48*/"></i>
                    </div>
                </div>
                <div class="col-xs-9 xxs-stack aboutText <%--col-xs-pull-3--%>">
                    <h3>Wunschwissen sammeln und nie vergessen</h3>
                    <p>
                        Bei memucho kannst du dein <i class="fa fa-heart" style="color:#b13a48;"></i> Wunschwissen zusammenstellen. 
                        Entscheide, was du dir merken möchtest und memucho erinnert dich rechtzeitig daran, 
                        wenn du es wieder lernen musst, um es nicht zu vergessen.
                    </p>
                    <p>
                        Wo? Überall, wo du ein kleines <i class="fa fa-heart-o" style="color:#b13a48;"></i> findest, kannst du draufklicken, 
                        um ein Lernset, ein ganzes Thema oder eine einzelne Frage zu deinem Wunschwissen hinzuzufügen.
                        In deiner <a href="<%= Links.Knowledge() %>">Wissenszentrale</a> bekommst du jederzeit einen Überblick zu deinem Wunschwissen.
                    </p>
                </div>
            </div>

            <div class="row aboutRow">
                <div class="col-xs-3 xxs-stack aboutImg <%--col-xs-push-9--%>">
                    <div class="aboutImgInner">
                        <i class="fa fa-check-circle fa-3x"></i>
                    </div>
                </div>
                <div class="col-xs-9 xxs-stack aboutText <%--col-xs-pull-3--%>">
                    <h3>Sicherheit gewinnen</h3>
                    <p>
                        Mit memucho weißt du immer, wie viel Zeit du noch zum Lernen brauchst und ob du deine Lernziele erreichst. 
                        Du liegst im Plan? Dann kannst du mit gutem Gewissen ins Kino gehen.
                    </p>
                    <p>
                        Wo? Im Moment musst du dafür die einen Prüfungstermin anlegen (vergleiche den nächsten Punkt), aber ganz bald 
                        kannst du auch dein Wunschwissen optimiert lernen - also all das, was du dir immer merken möchtest.
                    </p>
                </div>
            </div>

            <div class="row aboutRow">
                <div class="col-xs-3 xxs-stack aboutImg">
                    <div class="aboutImgInner">
                        <i class="fa fa-line-chart fa-3x"></i>
                    </div>
                </div>
                <div class="col-xs-9 xxs-stack aboutText">
                    <h3>Optimiert für eine Prüfung lernen</h3>
                    <p>
                        memucho erstellt dir einen persönlichen Lernplan, wenn du dich auf eine Prüfung vorbereitest. 
                        Intelligente Algorithmen sagen dir, wann du was am besten lernst und ob du deine Lernziele erreichen wirst. 
                        memucho ist dein persönlicher Coach beim Auswendiglernen.
                    </p>
                    <p>
                        Wie das geht? Gehe zu <a href="<%= Links.Dates() %>">Termine</a>, lege einen neuen Termin an und wähle die
                        zu lernenden Lernsets aus. memucho erstellt dir dann automatisch einen persönlichen Lernplan.
                    </p>
                </div>
            </div>
            
            <div class="row aboutRow">
                <div class="col-xs-3 aboutImg xxs-stack">
                    <div class="aboutImgInner">
                        <i class="fa fa-clock-o fa-3x"></i>
                    </div>
                </div>
                <div class="col-xs-9 xxs-stack aboutText">
                    <h3>Zeit sparen</h3>
                    <p>
                        memucho weiß, welche Fragen du am dringendsten lernen musst und welche (noch) nicht.
                        So lernst du immer das Richtige und musst nie wieder umsonst lernen. Dadurch sparst du Zeit.
                    </p>
                    <p>
                        Wie wir das machen? Wir analysieren, wie und wofür du lernst und wie andere Nutzer die Inhalte gelernt haben. 
                        Dadurch können wir vorhersagen, wie gut du etwas (noch) weißt. Nutze dafür unsere Lernen-Funktion. 
                        Sie ist nur verfügbar, wenn du <a href="#" data-btn-login="true">angemeldet bist</a>.
                        <%--(<a href="<%= Links.AlgoInsightForecast() %>">Hier erfährst du mehr über unsere Technologie.</a>)--%>
                    </p>
                </div>
            </div>

            <div class="row aboutRow">
                <div class="col-xs-3 xxs-stack aboutImg">
                    <div class="aboutImgInner">
                        <i class="fa fa-cogs fa-3x"></i>
                    </div>
                </div>
                <div class="col-xs-9 xxs-stack aboutText">
                    <h3>Und vieles mehr...</h3>
                    <p>
                        Mit memucho behältst du immer den Überblick, egal wie viel Wissen du dir merken möchtest oder wieviele Prüfungen du hast. 
                        memucho zeigt dir ausführliche <a href="<%= Links.Knowledge() %>">Lernauswertungen</a> und Statistiken 
                        und du kannst <a href="<%= Links.Users() %>">deinen Freunden folgen</a>, 
                        mit ihnen lernen und im <a href="<%= Links.Games() %>">Quiz-Spiel</a> gegen sie antreten.
                    </p>
                </div>
            </div>
            <div class="row aboutRow">
                <%  var isLoggedIn = Sl.R<SessionUser>().IsLoggedIn;
                    if (!isLoggedIn) { %>
                    <div class="col-xs-12" style="margin-top: 10px; text-align: center">
                        <a id="btnRegisterAboutFeatures" href="<%= Url.Action(Links.RegisterAction, Links.RegisterController) %>" class="btn btn-success" role="button"><i class="fa fa-chevron-circle-right">&nbsp;</i> Jetzt Registrieren</a> <br/>
                        <div class="" style="margin-top: 3px; font-style: italic">*memucho ist kostenlos.</div>
                    </div>
                <%  } %>
            </div>

        </div>
    </div>
</div>
    

<div class="row">
    <div class="col-xs-12">
        <div class="well">
            <h1 class="PageHeader" id="Team">
                <span class="ColoredUnderline Question">Das Team</span>
            </h1>
            <p>
                Wir möchten den Zugang zu freien Bildungsinhalten verbessern und mit memucho ein soziales Wissensnetzwerk aufbauen, mit dem
                Faktenlernen effizienter wird und mehr Spaß macht. Wir konzipieren, gestalten und programmieren memucho gemeinsam.
            </p>

            <div class="row" style="margin-top: 30px;">
                <div class="col-xs-4 TeamPic">
                    <img src="/Images/Team/team_robert201509_155.jpg"/>
                        <br/> <b>Robert</b><br/>(Gründer) 
                </div>
                
                <div class="col-xs-4 TeamPic">
                    <img src="/Images/Team/team_jule201509-2_155.jpg"/>  
                    <br/> <b>Jule</b><br/>(Gründerin)
                </div>

                <div class="col-xs-4 TeamPic">
                    <img src="/Images/Team/team_christof_20170404_P3312344_155.jpg"/>  
                    <br/> <b>Christof</b><br/>(Gründer)
                </div>
                <div class="col-xs-4 TeamPic">
                    <img src="/Images/Team/team_lisa_sq_155.jpg"/>  
                    <br/> <b>Lisa</b><br/>(Kommunikation)
                </div>
                <div class="col-xs-4 TeamPic">
                    <img src="/Images/Team/team_julian20170404_P3312327_155.jpg"/>  
                    <br/> <b>Julian</b><br/>(Entwicklung)
                </div>
            </div>
        </div>
    </div>
</div>

   
<div class="row">
    <div class="col-xs-12">
        <div class="well">
            <h1 class="PageHeader" id="Partner">
                <span class="ColoredUnderline Question">Unsere Partner</span>
            </h1>
            <p class="partnerLogos">
                <a href="https://learning-levelup.de/" target="_blank"><img src="/Images/LogosPartners/Logo_LearningLevelUp.png"/></a>
                <a href="https://www.tutory.de/" target="_blank"><img src="/Images/LogosPartners/Logo_tutory_250px.png"/></a>
                <a href="http://www.lernen-mit-spass.ch/" target="_blank"><img src="/Images/LogosPartners/Logo_lernenmitspass_w200.gif"/></a>
                <%--<a href="http://www.geschichtsinfos.de/" target="_blank"><img src="/Images/LogosPartners/Logo_Geschichtsinfos_de_w200.jpg"/></a>--%>
            </p>
        </div>
    </div>
</div>

</asp:Content>
