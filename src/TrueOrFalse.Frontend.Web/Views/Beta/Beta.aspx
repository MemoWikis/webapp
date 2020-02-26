<%@ Page Title="memucho in Beta" Language="C#" MasterPageFile="~/Views/Shared/Site.Beta.Master" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Web.Optimization" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Beta/Beta.css" rel="stylesheet" />
    <%= Scripts.Render("~/bundles/beta") %>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">    
    
    <div class="row">
        <div class="col-md-8 col-md-offset-2">
            <h1 style="margin-top: 30px; margin-bottom: 20px;" class="animated">
                Private Beta
            </h1>
    
            <div class="alert alert-danger" role="alert" id="msgInvalidBetaCode" style="display:none">
                Kein gültiger Beta-Code.
                Weiter unten kannst du dich registrieren.
            </div>
            
            <div class="row">
                <form class="form-inline" style="font-size: 25px; line-height: 20px;">
                    <div class="col-md-offset-1 col-sm-3" style="line-height: 33px;">
                        <p class="form-control-static">Zugang:</p>
                    </div>                        
                    <div class="col-sm-5 col-md-4 form-group">
                        <input type="password" class="form-control col-sm-6"
                            id="txtBetaCode" placeholder="Beta-Code" 
                            style="font-size: 18px; width: 100% !important; display: block; margin-bottom: 5px;">
                        <a href="#txtEmailRequester" style="font-size: 12px; display: block; text-align: left; margin-left: 5px;">Zugang anfragen</a>
                    </div>
                    <div class="col-sm-2">
                        <a class="btn btn-success shake" href="#" style="font-size: 18px;" id="btnEnter">
                            <i class="fa fa-sign-in"></i> Eintreten
                        </a>                        
                    </div>
                </form>                
            </div>

        </div>
    </div>
    
    <div class="row">
        <div class="well col-md-8 col-md-offset-2" style="margin-top: 35px; background-color: whitesmoke; ">
            <h3>Was wird memucho?</h3>
            <p>
                memucho wird eine vernetzte Lern- und Wissensplattform. <br/>
                Mit memucho kannst du...
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
                    <b>Allgemein- und Spezialwissen erweitern</b>
                    <p>
                        Du möchtest gerne mehr über Politik, die Griechenland-Krise oder über James Bond-Filme wissen? 
                        Finde die passenden Themen und stelle dir dein Wunschwissen zusammen!
                    </p>
                </div>
                <div class="clearfix"></div>
                <div class="col-xs-6" style="text-align: center; font-size: 100%; padding: 5px 5px 10px;">
                  <i class="fa fa-calendar-o fa-2x show-tooltip" style="color: #2C5FB2"></i><br/>
                    <b>Zu einem bestimmten Termin lernen</b>
                    <p>
                        Eine Klassenarbeit oder eine Prüfung steht an? Mit Terminen in memucho weißt du immer, 
                        was du schon sicher kannst und wo du noch weiter lernen musst.
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
                        memucho ist ein offenes Netzwerk, wo du dein Wissen teilen und auf das Wissen anderer zurückgreifen kannst. 
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
            </div>
            

        </div>
    </div>
    
    <div class="row">
        <div class="Survey well col-md-8 col-md-offset-2">
            <p class="" style="font-weight: bold">Mach mit bei unserer kleinen Umfrage!</p>
            <p>
                Es dauert nur 5 Minuten und du hilfst uns dabei, memucho besser zu machen.<br/>
                <a class="btn btn-md btn-info" href="http://www.memucho.de/umfrage/LRyB9p" target="_blank" style="margin-top: 15px; margin-bottom: -5px;">
                    <i class="fa fa-arrow-right" style="margin-right: 5px;"></i>Umfrage starten
                </a>
            </p>
        </div>
    </div>

    <div class="row">
        <div class="well col-md-8 col-md-offset-2" style="background-color: whitesmoke; ">
            <p style="font-weight: bold">Open Educational Resources</p>
            <p>
                memucho ist Teil der Bewegung zur Förderung freier Bildungsinhalte. <br/>
                Alle Inhalte bei memucho werden frei zugänglich sein!
            </p>
        </div>
    </div>
    
    <div class="row" style="margin-bottom: 100px;">
        <div class="col-md-6 col-md-offset-3">
            <h1>
                Betatester werden
            </h1>
            
            <p>
                Wenn du Betatester werden möchtest, schicke uns eine Anfrage.
            </p>
            
            <div class="alert alert-danger" role="alert" id="msgInvalidEmail" style="display:none">
                Keine gültige E-Mail.
            </div>
            
            <div class="alert alert-success" role="alert" id="msgEmailSend" style="display:none">
                Deine Betatester-Anfrage wurde versendet. Wir melden uns in den nächsten Tagen bei dir.
            </div>
            
            <form class="form-inline" style="color: white;">
                <div class="form-group">
                    <div class="col-sm-6">
                        <input type="email" class="form-control" id="txtEmailRequester" placeholder="deine@email.de" style="font-size: 18px;">
                    </div>    
                </div>
                <a class="btn btn-info shake" href="#" id="btnBetaRequest" style="font-size: 18px;">
                    <i class="fa fa-envelope-o"></i> Zugang anfragen
                </a>
            </form> 
        </div>
    </div>

</asp:Content>