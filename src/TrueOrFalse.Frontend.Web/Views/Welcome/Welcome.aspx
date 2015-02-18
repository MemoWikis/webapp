<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
	Inherits="ViewPage<WelcomeModel>"%>
<%@ Import Namespace="TrueOrFalse.Web.Context" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <title>MEMuchO</title>
    <link href="/Views/Welcome/Welcome.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">

<div class="row">
       
    <div class="col-md-8">
            
        <div class="well">
            <a href="#teaserWhatIsMemucho">
                <h1 style="margin-top: 0; margin-bottom: 7px; font-size: 24px;">MEMuchO ist eine Lern- und Wissensplattform</h1>
            </a>            
            <ul style="margin-top: 0; margin-bottom: 0; padding-top: 3px; ">                
                <li><a href="#teaserWhatIsMemucho" style="font-size: 15px;">Wie hilft dir MEMuchO? &nbsp; <i class="fa fa-arrow-right" style="" ></i></a></li>
                <li><a href="#teaserPrinciples" style="font-size: 15px;">Wikipedia-Prinzip, Vernetzung und Gemeinwohlorientierung &nbsp; <i class="fa fa-arrow-right" style="" ></i></a></li>
                <li><a href="#teaserWhoWeAre" style="font-size: 15px;">Wer sind wir? &nbsp; <i class="fa fa-arrow-right" style="" ></i></a></li>
            </ul>
        </div>
            
        <div class="row ThumbnailRow" style="padding-top: 0px;">
            <div class="ThumbnailColumn">
                <div class="thumbnail">
                    <img src="http://fillmurray.com/200/200">
                    <div class="caption">
                        <h4>Politik</h4>
                        <p>Wer war der 1. deutsche Bundeskanzler?</p>
                        <a href="#" class="btn btn-primary" role="button">beantworten</a>
                    </div>
                </div>
            </div>
            <div class="ThumbnailColumn">
                <div class="thumbnail">
                    <img src="http://placecage.com/200/200">
                    <div class="caption">
                        <h4>Geschichte</h4>
                        <p>...</p>
                        <a href="#" class="btn btn-primary" role="button">beantworten</a>
                    </div>
                </div>
            </div>
            <div class="ThumbnailColumn">
                <div class="thumbnail">
                    <img src="http://placecage.com/g/200/200">
                    <div class="caption">
                        <h4>Zitate</h4>
                        <p>...</p>
                        <a href="#" class="btn btn-primary" role="button">beantworten</a>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel panel-default">
            <div class="media panel-body" >
                <a class="pull-left" href="#">
                    <img class="media-object" src="http://placebear.com/120/120 " alt="...">
                </a>
                <div class="media-body">
                    <h4 class="media-heading">Streitbare Zitate:</h4>
                    Wer sagte:
                    "Wenn ich über die steuer- und erbrechtliche Anerkennung von homosexuellen Paaren diskutiere, kann ich gleich über Teufelsanbetung diskutieren."
                    <a href="#" class="" role="button" style="display: block; padding-top: 20px;">beantworten</a>
                </div>
            </div>
        </div>
            <div class="panel panel-default">
                <div class="media panel-body" >
                    <a class="pull-left" href="#">
                    <img class="media-object" src="http://lorempixel.com/120/120/sports/" alt="...">
                </a>
                <div class="media-body">
                    <h4 class="media-heading">Das kann Leben retten:</h4>
                    Ersthelferfragen und Selbstdiagnose. 
                    <a href="#" class="" role="button" style="display: block; padding-top: 20px;">beantworten</a>
                </div>
           </div>
        </div>
        <div class="well">
            <h3><a name="teaserWhatIsMemucho">Was ist MEMuchO?</a></h3>
            <p>
                MEMuchO ist eine vernetzte Lern- und Wissensplattform. <br/>
                MEMuchO hilft dir...
            </p>
            <ul>
                <li>
                    <b>...schneller zu lernen.</b>
                    <p>
                        MEMuchO analysiert dein Lernverhalten und wiederholt schwierige 
                        Fragen zum optimalen Zeitpunkt. So brauchst du weniger Zeit zum Lernen.
                    </p>
                </li>
                <li>
                    <b>...dein Allgemein- und Spezialwissen zu erweitern.</b>
                    <p>
                        Du möchtest gerne mehr über Politik oder über die verschiedenen 
                        Spurbreiten von Modelleisenbahnen erfahren? Finde die passenden 
                        Fragesätze und stelle dir dein Wunschwissen zusammen!
                    </p>
                </li>
                <li>
                    <b>...zu einem bestimmten Termin zu lernen.</b>
                    <p>
                        Eine Klassenarbeit, eine Prüfung oder ein wichtiges Gespräch steht an? 
                        Lege einen Termin an und bestimme, was du bis dahin wissen musst. 
                        Mit MEMuchO weißt du immer, was du schon sicher kannst und wo du noch 
                        weiter üben musst.
                    </p>
                </li>
                <li>
                    <b>...zu überblicken, was du weißt und was du wissen möchtest.</b>
                    <p>
                        Du möchtest dir gerne 50, 500, 5000 (oder mehr) Fakten merken? 
                        Kein Problem, mit MEMuchO behältst du den Überblick. 
                    </p>
                </li>
                <li>
                    <b>...dein Wissen mit anderen zu teilen und gemeinsam zu lernen.</b>
                    <p>
                        MEMuchO ist ein offenes Netzwerk, wo du dein Wissen teilen, auf das 
                        Wissen anderer zurückgreifen und mit Freunden gemeinsam lernen kannst. 
                        Denn Wissen wird mehr, wenn man es teilt! 
                    </p>
                </li>
            </ul>
            <p>
                Du willst es ausprobieren? <a href="/Registrieren">Registriere dich</a> und lege los! <br/>
                Du findest das eine tolle Idee, möchtest mitmachen und uns unterstützen?
                Werde Fördermitglied der ersten Stunde!
            </p>
        </div>
        <div class="well">

            <h3><a name="teaserPrinciples">Unsere Prinzipien</a></h3>
            <ul>
                <li>
                    <b>Wikipedia-Prinzip</b>
                    <p>
                        In MEMuchO unterliegen öffentliche Inhalte einer Creative Commons Lizenz, 
                        genau wie die Mehrheit der Einträge auf Wikipedia. Öffentliche MEMuchO-Inhalte 
                        können also von jedem kostenfrei und ohne Einschränkungen verwendet werden 
                        (<a rel="license" href="http://creativecommons.org/licenses/by/4.0/deed.de">Hier 
                        erfährst du genaueres zur Lizenz CC BY 4.0.</a>). Private Inhalte sind aber privat.
                    </p>
                </li>
                <li>
                    <b>Gemeinwohlorientierung</b><br/>
                    <p>
                        Wir möchten unser Unternehmen auf gemeinwohlfördernden Werten aufbauen. 
                        Als Teil der <a href="http://www.gemeinwohl-oekonomie.org/de">Gemeinwohlökonomie</a> 
                        sind wir davon überzeugt, dass Unternehmen der Gemeinschaft dienen müssen und deshalb 
                        eine ethische, soziale und ökologische Verantwortung haben. Daher werden wir in Zukunft 
                        eine Gemeinwohlbilanz veröffentlichen.
                    </p>
                </li>
                <li>
                    <b>Mitwirkung und Vernetzung</b>
                    <p>
                        Wir glauben, dass Wissen vernetzt sein muss. Der Vernetzungsgedanke spielt bei uns eine 
                        große Rolle. In der aktuellen Beta-Phase ist uns euer Feedback ganz besonders wichtig, 
                        aber auch später werden Mitglieder mitentscheiden, welche Funktionen wir als nächstes umsetzen 
                        und aktive Mitglieder können Inhalte mitmoderieren.
                    </p>
                </li>
                <li>
                    <b>Datenschutz ist uns sehr sehr wichtig</b>
                    <p>
                        Wir nutzen deine Daten, damit du besser lernen kannst und um MEMuchO besser zu machen. 
                        Aber wir werden deine Daten niemals verkaufen. (Erfahre mehr über unseren Datenschutz.)
                    </p>
                </li>
                <li>
                    <b>Open-Source und Transparenz</b>
                    <p>
                        Die Software, mit der MEMuchO läuft, steht unter einer Open-Source-Lizenz. Die Quelltexte 
                        sind frei verfügbar und können von allen frei verwendet werden. Du findest sie 
                        auf <a href="https://github.com/TrueOrFalse/TrueOrFalse"><i class="fa fa-github"></i>Github</a>. 
                        In Zukunft möchten wir neben der Gemeinwohlbilanz auch unsere Unternehmenszahlen veröffentlichen.
                    </p> 
                </li>        
            </ul>
            <p>
                Du willst es ausprobieren? <a href="/Registrieren">Registriere dich</a> und lege los! <br/>
                Du findest das eine tolle Idee, möchtest mitmachen und uns unterstützen?
                Werde Fördermitglied der ersten Stunde!
            </p>
        </div>
        <div class="well Founder">
            <h3><a name="teaserWhoWeAre">Team</a></h3>
            <div class="row">
                
                <div class="col-xs-6 ImageColumn">
                    <img src="http://www.gravatar.com/avatar/b937ba0e44b611a418f38cb24a8e18ea?s=128"/>
                        <br/> <b>Robert</b> (Gründer) <br/>

                </div>
                
                <div class="col-xs-6 ImageColumn">
                    <img src="/Images/no-profile-picture-128.png"/>  
                    <br/> <b>Jule </b>(Gründerin) <br/> 
                </div>
                <div class="col-xs-12" style="margin-top: 10px;">
                    <p>
                        Eine spannende Reise: Wir möchten nicht nur <a href="#teaserWhatIsMemucho">das Lernen von Faktenwissen vereinfachen</a>, 
                        sondern auch ein stabiles <a href="#teaserPrinciples">gemeinwohlorientiertes Unternehmen</a> aufbauen. 
                    </p>
                    <p>
                        Wir konzipieren, programmieren und gestalten.
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
            
            
        <div class="row">
            <div class="col-md-12"><h4 class="media-heading">Kennst du das aktuelle Kabinett?</h4></div>
        </div>
        <div class="row">
            <div class="col-md-6"><img class="media-object" src="http://placekitten.com/120/120" alt="..."></div>
            <div class="col-md-6"><img class="media-object" src="http://placesheen.com/120/120" alt="..."></div>
        </div>
            

        <div class="row" style="padding-top: 10px;">
            <div class="col-md-6">
                <h4 class="media-heading">Studienfächer</h4>
                <ul>
                    <li><a href="">Psychologie</a></li>
                    <li><a href="">Philosophie</a></li>
                    <li><a href="">Informatik</a></li>
                    <li><a href="">[mehr]</a></li>
                </ul>
            </div>

            <div class="col-md-6">
                <h4 class="media-heading">Schulfächer</h4>
                <ul>
                    <li><a href="">Deutsch</a></li>
                    <li><a href="">Mathe</a></li>
                    <li><a href="">Geschichte</a></li>
                    <li><a href="">[mehr]</a></li>
                </ul>
            </div>
        </div>
        
        <div class="row" style="padding-top: 10px;">
            <div class="col-md-12">
                <h4 class="media-heading">Visual Studio</h4>
                <ul>
                    <li><a href="">Shortcut um in Visual-Studio zum nächsten Fehler zu springen.</a></li>
                    <li><a href="">Shortcut um den Build abzubrechen ("Build.Cancel")</a></li>
                </ul>
            </div>
        </div>
        
        <div class="row" style="padding-top: 10px;">
            <div class="col-md-12">
                <h4 class="media-heading">Krise in der Ukraine</h4>
                <ul>
                    <li></li>
                </ul>
            </div>
        </div>
    
        <div class="row" style="padding-top: 10px;">
            <div class="col-md-12"><h4 class="media-heading">Erkennst du diese Vögel?</h4></div>
        </div>
        <div class="row">
            <div class="col-md-6"><img class="media-object" src="http://lorempixel.com/120/120/transport" alt="..."></div>
            <div class="col-md-6"><img class="media-object" src="http://placekitten.com/g/120/120" alt="..."></div>
        </div>
        
        <div class="row" style="padding-top: 10px;">
            <div class="col-md-12"><h4 class="media-heading">Was ist das für ein Baum?</h4></div>
        </div>
        <div class="row">
            <div class="col-md-6"><img class="media-object" src="http://lorempixel.com/120/120/transport" alt="..."></div>
            <div class="col-md-6"><img class="media-object" src="http://placekitten.com/g/120/120" alt="..."></div>
        </div>        

        <div class="row" style="padding-top: 10px;">
            <div class="col-md-12"><h4 class="media-heading">Wirtschaftskrise</h4></div>
            <div class="col-md-12"><img class="media-object" src="http://lorempixel.com/260/120/g/transport/" alt="...">
        </div>
    </div>
</div>
</div>

</asp:Content>