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
            <a href="#teaser1">
                <h1 style="margin-top: 0; margin-bottom: 7px; font-size: 24px;">MEMuchO ist eine Lern- und Wissensplattform</h1>
            </a>            
            <ul style="margin-top: 0; margin-bottom: 0; padding-top: 3px; ">                
                <li><a href="#teaser1" style="font-size: 15px;">Wie hilft dir MEMuchO? &nbsp; <i class="fa fa-arrow-right" style="" ></i></a></li>
                <li><a href="#teaser2" style="font-size: 15px;">Wikipedia-Prinzip und Open Source &nbsp; <i class="fa fa-arrow-right" style="" ></i></a></li>
                <li><a href="#teaser3" style="font-size: 15px;">Wer sind wir? &nbsp; <i class="fa fa-arrow-right" style="" ></i></a></li>
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
            <h3><a name="teaser1">MEMuchO hilft dir ...</a></h3>
            <ul>
                <li>
                    <b>...schneller zu lernen.</b>
                    <p>
                        MEMuchO analysiert dein Lernverhalten und 
                        versucht den optimalen Zeitpunkt für eine 
                        benötigte Wiederholung von Wissen zu ermitteln. 
                        So kannst du schneller lernen.
                    </p>
                    <p>
                        (Die Analyse von Lernverhalten funktioniert nur wenn wir viele Daten sammeln: 
                        Zu diesem Thema mehr hier: <a href="/Hilfe/DatenSicherheit">Hilfe Datenansicht</a>)
                    </p>
                </li>
                <li>
                    <b>...zu überblicken, was du weißt und was du wissen möchtest.</b>
                    <p>
                        Du möchtest dir 50, 500, 5000 (oder mehr) Fakten merken. 
                        Wir helfen dir dabei den Überblick zu behalten. 
                    </p>
                </li>
                <li>
                    <b>...zu einem bestimmten Termin zu lernen.</b>
                    <p>
                        Eine Klassenarbeit, eine Prüfung oder ein wichtiges Gespräch steht an? 
                        Du kannst Termine anlegen und bestimmen, was du zu diesem Termin
                        als Faktenwissen abrufen möchtest.
                    </p>
                    <p>
                        Dabei hast du genau im Blick, welches Wissen du schon sicher kannst und
                        wo du noch weiter üben musst. 
                    </p> 
                    <p>
                        MEMuchO erinnert dich an Termine und 
                        informiert dich darüber, was es noch zu lernen gibt. 
                    </p>
                </li>
            </ul>
        </div>
        <div class="well">

            <h3><a name="teaser2">Unsere Prinzipien</a></h3>
            <ul>
                <li>
                    <b>Wikipedia-Prinzip</b>
                    <p>
                        In MEMuchO unterliegen öffentliche Inhalte einer
                        Creative Commons Lizenz. Genau wie die Mehrheit der Einträge auf Wikipedia.
                            
                        Öffentliche MEMuchO-Inhalte können also von jedermann 
                        kostenfrei und ohne Einschränkungen verwendet werden.
                
                        <a rel="license" href="http://creativecommons.org/licenses/by/3.0/deed.de">Zum Lizenztext: 
                            <img alt="Creative Commons Lizenzvertrag" style="border-width:0" src="http://i.creativecommons.org/l/by/3.0/80x15.png" />
                        </a>
                    </p>
                </li>
                <li>
                    <b>Wir werden deine Daten niemals verkaufen.</b>
                    <p>Wir nutzen Daten dafür, um MEMuchO besser zu machen.</p>
                </li>
                <li>
                    <b>Gemeinwohlorientierung</b><br/>
                    <p>
                        Wir möchten unser Unternehmen auf  
                        <a href="http://www.gemeinwohl-oekonomie.org/de">gemeinwohlfördernden Werten</a> aufbauen.
                    </p>
                    <p>
                        Zu den Ideen der <a href="http://www.gemeinwohl-oekonomie.org/de">Gemeinwohlökonomie</a> gehört zum Beispiel: 
                        Unternehmen sollten kooperativ und <i>nicht</i> konkurenzorientiert agieren. 
                        Daher sollten Information und Know-How geteilt werden.
                    </p>
                </li>
                <li>
                    <b>Transparenz!</b>
                </li>
                <li>
                    <b>Open-Source</b>
                    <p>
                        Die Software, mit der MEMuchO läuft, steht unter einer Open-Source-Lizenz.
                        Die Quelltexte findest du auf <a href="https://github.com/TrueOrFalse/TrueOrFalse"><i class="fa fa-github"></i> Github</a>. 
                    </p> 
                </li>        
            </ul>
        </div>
        <div class="well Founder">
            <h3><a name="teaser3">Team</a></h3>
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
                        Eine spannende Reise: Wir möchten nicht nur <a href="#teaser1">das Lernen von Faktenwissen vereinfachen</a>, 
                        sondern auch ein stabiles <a href="#teaser2">gemeinwohlorientiertes Unternehmen</a> aufbauen. 
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