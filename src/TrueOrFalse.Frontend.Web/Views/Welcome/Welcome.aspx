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
                <h1 style="margin-top: 0px; margin-bottom: 7px; font-size: 24px;">MEMuchO ist eine Lern- und Wissensplattform</h1>
            </a>            
            <ul style="margin-top: 0px; margin-bottom: 0px; padding-top: 3px; ">                
                <li><a href="#teaser1" style="font-size: 15px;">Wie hilft dir MEMuchO? &nbsp; <i class="fa fa-arrow-right" style="" ></i></a></li>
                <li><a href="#teaser2" style="font-size: 15px;">Wikipedia Prinzip und Open und Source &nbsp; <i class="fa fa-arrow-right" style="" ></i></a></li>
                <li><a href="#teaser3" style="font-size: 15px;">Wer sind wir? &nbsp; <i class="fa fa-arrow-right" style="" ></i></a></li>
            </ul>
        </div>
            
        <div class="row" style="padding-top: 0px;">
            <div class="col-sm-6 col-md-4">
                <div class="thumbnail">
                    <img src="http://fillmurray.com/200/200">
                    <div class="caption">
                        <h4>Politik</h4>
                        <p>Wer war der 1. deutsche Bundeskanzler?</p>
                        <p><a href="#" class="btn btn-primary" role="button">beantworten</a></p>
                    </div>
                </div>
            </div>
            <div class="col-sm-6 col-md-4">
                <div class="thumbnail">
                    <img src="http://placecage.com/200/200">
                    <div class="caption">
                        <h4>Geschichte</h4>
                        <p>...</p>
                        <p><a href="#" class="btn btn-primary" role="button">beantworten</a></p>
                    </div>
                </div>
            </div>
            <div class="col-sm-6 col-md-4">
                <div class="thumbnail">
                    <img src="http://placecage.com/g/200/200">
                    <div class="caption">
                        <h4>Zitate</h4>
                        <p>...</p>
                        <p><a href="#" class="btn btn-primary" role="button">beantworten</a></p>
                    </div>
                </div>
            </div>
        </div>
            
        <div class="media" style="padding-top: 0px; margin-top: 0px">
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
            
        <div class="media">
            <a class="pull-left" href="#">
                <img class="media-object" src="http://lorempixel.com/120/120/sports/" alt="...">
            </a>
            <div class="media-body">
                <h4 class="media-heading">Das kann Leben retten:</h4>
                Ersthelferfragen und Selbstdiagnose. 
                <a href="#" class="" role="button" style="display: block; padding-top: 20px;">beantworten</a>
            </div>
        </div>

        <h3><a name="teaser1">Wie hilft dir MEMuchO?</a></h3>
        <p>
            MEMuchO hilft dir dabei: 
            <ul>
                <li>
                    <b>Schneller zu lernen:</b>
                    <p>
                        MEMuchO analysiert Dein Lernverhalten und 
                        versucht den optimalen Zeitpunkt für eine 
                        benötigte Wiederholung von Wissen zu ermitteln. 
                        So kannst du schneller lernen.
                    </p>
                    <p>
                        (Die Analyse von Lernverhalten funktioniert nur, wenn wir viele Daten sammeln: 
                        Zu diesem Thema mehr hier: <a href="/Hilfe/DatenSicherheit">Hilfe Datenansicht</a>)
                    </p>
                </li>
                <li>
                    <b>Zu überblicken Was du weist und was du wissen möchtest.</b>
                    <p>
                        Du möchtest dir 50, 500, 5000 (oder mehr) Fakten merken. 
                        Wir helfen dir dabei den Überblick zu behalten. 
                    </p>
                </li>
                <li>
                    <b>Zu einen genauern Termin zu lernen:</b>
                    <p>
                        Eine Klassenarbeit, eine Prüfung, ein wichtiges Gespräch steht an? 
                        Du kannst Termine anlegen und bestimmen, was du zu diesem Termin
                        als Faktenwissen abrufen möchtest.
                    </p>
                    <p>
                        Dabei hast du genau im Blick, welches Wissen du schon sicher kannst und
                        wo du noch weiter üben musst. 
                    </p> 
                    <p>
                        MEMuchO erinnert Dich an Termine und 
                        informiert Dich darüber, was es noch zu lernen gibt. 
                    </p>
                </li>
            </ul>
        </p>

        <h3><a name="teaser2">Unsere Prinzipien</a></h3>
        <ul>
            <li>
                <b>Wikipedia Prinzip</b>
                <p>
                    In MEMuchO unterliegen öffentlichen Inhalte einer
                    Creative Commons Lizenz. Genau wie die Mehrheit der Einträge auf Wikipedia.
                            
                    Öffentliche MEMuchO-Inhalte können also von jedermann 
                    kostenfrei und ohne Einschränkungen verwendet werden. 
                
                    <a rel="license" href="http://creativecommons.org/licenses/by/3.0/deed.de">Zum Lizenztext: 
                        <img alt="Creative Commons Lizenzvertrag" style="border-width:0" src="http://i.creativecommons.org/l/by/3.0/80x15.png" />
                    </a>
                </p>
            </li>
            <li>
                <b>Wir werden Deine Daten niemals verkaufen.</b>
                <p>Wir Nutzen Daten dafür, um Richtig-oder-Falsch besser zu machen.</p>
            </li>
            <li>
                <b>Gemeinwohlorientierung</b>
            </li>
            <li>
                <b>Transparenz!</b>
            </li>
            <li>
                <b>Open-Source</b>
                <p>
                    Die Software mit der MEMuchO läuft, steht unter einer Open Source Lizenz.
                    Die Quelltexte findest du auf <a href="https://github.com/TrueOrFalse/TrueOrFalse"><i class="fa fa-github"></i> Github</a>. 
                </p>
            </li>        
        </ul>
        <div style="height: 10px;"></div>        
                
        <h3><a name="teaser3">Team</a></h3>
        <div style="width: 280px;">
                    
                
            <div style="width: 128px; float: left; margin: 0px 20px 0 0px; ">
                <img src="http://www.gravatar.com/avatar/b937ba0e44b611a418f38cb24a8e18ea?s=128"/>
                    <br/> <b>Robert</b> (Gründer) <br/>

            </div>
                
            <div style="width: 128px; float: left; ">
                <img src="/Images/no-profile-picture-128.png"/>  
                <br/> <b>Jule </b>(Gründerin) <br/> 
            </div>
            <div style="clear:both"></div>
            <div style="margin-top: 10px;">
                <p>
                    Eine spannende Reise: Wir möchten nicht nur <a href="#teaser1">das Lernen von Faktenwissen vereinfachen</a>, 
                    sondern auch ein stabiles <a href="#teaser2">gemeinwohlorientiertes Unternehmen</a> aufbauen. 
                </p>
                <p>
                    Wir konzepieren, programmieren und gestalten.
                </p>
            </div>
        </div>
                
    </div>
            
    <div class="col-md-4">
        <%
            var userSession = new SessionUser();
            if (!userSession.IsLoggedIn){
        %>
            <div class="box" style="padding: 20px; ">
                <a href="<%= Url.Action("Login") %>" class="btn btn-success btn-lg" style="width: 100%" role="button">Anmelden</a>
                <br/><br/>
                <a href="<%= Url.Action("Register") %>" class="btn btn-primary btn-lg" style="width: 100%; margins-top: 5px;" role="button">Registrieren</a>
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
            <div class="col-md-12"><h4 class="media-heading">Fakten zur Ukraine</h4></div>
        </div>
        <div class="row">
            <div class="col-md-6"><img class="media-object" src="http://lorempixel.com/120/120/transport" alt="..."></div>
            <div class="col-md-6"><img class="media-object" src="http://placekitten.com/g/120/120" alt="..."></div>
        </div>
            
        <div class="row" style="padding-top: 10px;">
            <div class="col-md-12"><h4 class="media-heading">Wirtschaftskrise</h4></div>
        </div>
        <div class="row">
            <div class="col-md-6"><img class="media-object" src="http://lorempixel.com/120/120/g/transport/" alt="...">
            </div>
            <div class="col-md-6"><img class="media-object" src="http://lorempixel.com/120/120/abstract" alt="..."></div>
        </div>
    </div>
</div>

</asp:Content>